#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace MORT.Service.Overlay
{
    internal enum OverlayPerformanceStage
    {
        CaptureArea, WgcReadback, ComparePixels, RestoreBackground, SaveComparison,
        PaintTotal, SizeAndLocation, ClearCanvas, LayoutAndDraw, BackgroundDraw,
        BackgroundBitmapUpload, TextDraw, GetHbitmap, UpdateLayeredWindow, SurfaceFlush
    }

    //프로파일링이 꺼져 있으면 타임스탬프/할당/파일 접근 없이 빈 scope만 반환한다.
    internal static class OverlayPerformanceProfiler
    {
        private static Session? _current;
        internal static string? LastDirectory { get; private set; }
        internal static string? LastError { get; private set; }

        internal static void Start(string settings)
        {
            Stop();
            LastError = null;
            var session = new Session(settings);
            LastDirectory = session.DirectoryPath;
            Volatile.Write(ref _current, session);
            session.Start();
        }

        internal static void Stop() => Interlocked.Exchange(ref _current, null)?.Dispose();

        internal static Scope Measure(OverlayPerformanceStage stage, int width = 0, int height = 0)
        {
            var session = Volatile.Read(ref _current);
            return session == null ? default : new Scope(session, stage, width, height);
        }

        internal readonly struct Scope : IDisposable
        {
            private readonly Session? _session;
            private readonly OverlayPerformanceStage _stage;
            private readonly long _start;
            private readonly int _width, _height;
            internal Scope(Session session, OverlayPerformanceStage stage, int width, int height)
            {
                _session = session; _stage = stage; _width = width; _height = height;
                _start = Stopwatch.GetTimestamp();
            }
            public void Dispose()
            {
                if(_session != null) _session.Record(_stage, Stopwatch.GetElapsedTime(_start).TotalMilliseconds, _width, _height);
            }
        }

        internal sealed class Session : IDisposable
        {
            private readonly object _gate = new(), _flushGate = new();
            private readonly Dictionary<OverlayPerformanceStage, Bucket> _buckets = new();
            private readonly Stopwatch _clock = Stopwatch.StartNew();
            private readonly Process _process = Process.GetCurrentProcess();
            private readonly StreamWriter _stages, _usage;
            private Timer? _timer;
            private bool _stopped;
            private double _lastTime;
            private TimeSpan _lastCpu;
            private long _lastAllocation;
            private readonly int[] _lastGc = new int[3];
            internal string DirectoryPath { get; }

            internal Session(string settings)
            {
                DirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserData", "Debug", "OverlayPerformance",
                    DateTime.UtcNow.ToString("yyyyMMdd-HHmmss-fff", CultureInfo.InvariantCulture) + "-UTC-" + Guid.NewGuid().ToString("N")[..6]);
                Directory.CreateDirectory(DirectoryPath);
                File.WriteAllText(Path.Combine(DirectoryPath, "settings.txt"), settings + Environment.NewLine
                    + $"LogicalProcessors={Environment.ProcessorCount}" + Environment.NewLine
                    + "Times are elapsed wall time, not per-stage CPU. PaintTotal includes LayoutAndDraw and presentation. LayoutAndDraw includes BackgroundDraw and TextDraw. BackgroundDraw includes BackgroundBitmapUpload. Do not add nested stages. WgcReadback includes async GPU wait. CPU percent is normalized by logical processor count. Allocated MB is allocation during the interval, not retained RAM. p95 uses up to 8192 samples per stage/interval. No screen pixels or OCR/translation text are recorded.");
                _stages = new StreamWriter(Path.Combine(DirectoryPath, "stages.csv"));
                try { _usage = new StreamWriter(Path.Combine(DirectoryPath, "process.csv")); }
                catch { _stages.Dispose(); _process.Dispose(); throw; }
                _stages.WriteLine("elapsed_s,interval_s,stage,count,total_ms,mean_ms,p95_ms,max_ms,width,height,samples_dropped");
                _usage.WriteLine("elapsed_s,interval_s,cpu_percent,cpu_ms,working_set_mb,private_mb,allocated_mb,gen0,gen1,gen2");
                _lastCpu = _process.TotalProcessorTime;
                _lastAllocation = GC.GetTotalAllocatedBytes(false);
                for(int i = 0; i < 3; i++) _lastGc[i] = GC.CollectionCount(i);
            }

            internal void Start() => _timer = new Timer(_ => FlushSafely(), null, 1000, 1000);
            internal void Record(OverlayPerformanceStage stage, double ms, int width, int height)
            {
                lock(_gate)
                {
                    if(_stopped) return;
                    if(!_buckets.TryGetValue(stage, out var b)) _buckets[stage] = b = new Bucket();
                    b.Count++; b.Total += ms; b.Max = Math.Max(b.Max, ms); b.Width = width; b.Height = height;
                    if(b.Samples.Count < 8192) b.Samples.Add(ms);
                }
            }

            private void FlushSafely()
            {
                lock(_flushGate)
                {
                    if(_stopped) return;
                    try { Flush(); }
                    catch(Exception ex)
                    {
                        LastError = ex.Message;
                        Interlocked.CompareExchange(ref _current, null, this);
                        Close();
                    }
                }
            }

            private void Flush()
            {
                KeyValuePair<OverlayPerformanceStage, Bucket>[] snapshot;
                lock(_gate) { snapshot = _buckets.ToArray(); _buckets.Clear(); }
                double now = _clock.Elapsed.TotalSeconds, interval = now - _lastTime;
                if(interval <= 0) return;
                foreach(var (stage, b) in snapshot)
                {
                    b.Samples.Sort();
                    double p95 = b.Samples[(int)Math.Ceiling(b.Samples.Count * .95) - 1];
                    _stages.WriteLine(FormattableString.Invariant($"{now:F3},{interval:F3},{stage},{b.Count},{b.Total:F3},{b.Total / b.Count:F3},{p95:F3},{b.Max:F3},{b.Width},{b.Height},{b.Count - b.Samples.Count}"));
                }
                _process.Refresh();
                var cpu = _process.TotalProcessorTime;
                double cpuMs = (cpu - _lastCpu).TotalMilliseconds;
                long allocation = GC.GetTotalAllocatedBytes(false);
                int[] gc = Enumerable.Range(0, 3).Select(GC.CollectionCount).ToArray();
                _usage.WriteLine(FormattableString.Invariant($"{now:F3},{interval:F3},{cpuMs / (interval * 1000 * Environment.ProcessorCount) * 100:F2},{cpuMs:F3},{_process.WorkingSet64 / 1048576.0:F2},{_process.PrivateMemorySize64 / 1048576.0:F2},{(allocation - _lastAllocation) / 1048576.0:F2},{gc[0] - _lastGc[0]},{gc[1] - _lastGc[1]},{gc[2] - _lastGc[2]}"));
                _lastTime = now; _lastCpu = cpu; _lastAllocation = allocation; Array.Copy(gc, _lastGc, 3);
                _stages.Flush(); _usage.Flush();
            }

            public void Dispose()
            {
                lock(_flushGate)
                {
                    if(_stopped) return;
                    try { Flush(); }
                    catch(Exception ex) { LastError = ex.Message; }
                    finally { Close(); }
                }
            }
            private void Close()
            {
                lock(_gate) _stopped = true;
                _timer?.Dispose();
                try { _stages.Dispose(); } catch { }
                try { _usage.Dispose(); } catch { }
                _process.Dispose();
            }
            private sealed class Bucket
            {
                internal int Count, Width, Height;
                internal double Total, Max;
                internal readonly List<double> Samples = new();
            }
        }
    }
}
