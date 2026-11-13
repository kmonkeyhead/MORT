#nullable enable
using System;
using System.Drawing;
using System.Threading;

namespace MORT.Service.Overlay
{
    //복원 작업, 게시된 결과, UI 읽기가 모두 끝난 배열만 다시 쓴다.
    //UI가 늦으면 새 배열을 계속 만들지 않고 해당 프레임의 복원을 건너뛴다.
    internal sealed class OverlayFrameBufferPool : IDisposable
    {
        private readonly object _gate = new();
        private readonly Slot?[] _slots = new Slot[3];
        private bool _disposed;

        internal Lease? Rent(int length, int maximumLength)
        {
            lock(_gate)
            {
                if(_disposed) return null;
                int index = -1;
                for(int i = 0; i < _slots.Length; i++)
                {
                    var candidate = _slots[i];
                    if(candidate?.InUse == true) continue;
                    if(index < 0) index = i;
                    if(candidate != null && candidate.Pixels.Length >= length) { index = i; break; }
                }
                if(index < 0) return null;
                var slot = _slots[index] ??= new Slot();
                if(slot.Pixels.Length < length)
                {
                    int capacity = (int)Math.Max(length, Math.Min(maximumLength, (long)slot.Pixels.Length * 2));
                    slot.Pixels = new byte[capacity];
                }
                else
                {
                    //이전 출력이 쓴 행만 지운다. 여유 용량이나 투명한 빈 공간 전체는 초기화하지 않는다.
                    for(int i = 0; i < slot.DirtyCount; i++)
                    {
                        var rect = slot.Dirty[i];
                        for(int y = rect.Top; y < rect.Bottom; y++)
                            slot.Pixels.AsSpan((y * slot.Stride + rect.Left) * 4, rect.Width * 4).Clear();
                    }
                }
                slot.DirtyCount = 0;
                slot.InUse = true;
                return new Lease(this, slot);
            }
        }

        internal void Reset()
        {
            lock(_gate) Array.Clear(_slots, 0, _slots.Length);
            //아직 읽고 있는 이전 크기의 배열은 해당 Lease가 반환될 때까지 유효하다.
        }

        public void Dispose()
        {
            lock(_gate)
            {
                _disposed = true;
                Array.Clear(_slots, 0, _slots.Length);
            }
        }

        private void Return(Slot slot)
        {
            lock(_gate) slot.InUse = false;
        }

        internal sealed class Slot
        {
            internal byte[] Pixels = Array.Empty<byte>();
            internal Rectangle[] Dirty = Array.Empty<Rectangle>();
            internal int DirtyCount, Stride;
            internal bool InUse;
        }

        internal sealed class Lease
        {
            private readonly OverlayFrameBufferPool _pool;
            private readonly Slot _slot;
            private int _references = 1;
            internal byte[] Pixels => _slot.Pixels;

            internal Lease(OverlayFrameBufferPool pool, Slot slot) { _pool = pool; _slot = slot; }

            internal void SetStride(int width) => _slot.Stride = width;
            internal void AddDirty(Rectangle rect)
            {
                if(rect.Width <= 0 || rect.Height <= 0) return;
                if(_slot.DirtyCount == _slot.Dirty.Length)
                    Array.Resize(ref _slot.Dirty, Math.Max(4, _slot.DirtyCount * 2));
                _slot.Dirty[_slot.DirtyCount++] = rect;
            }

            internal void Retain()
            {
                int count;
                do
                {
                    count = Volatile.Read(ref _references);
                    if(count <= 0) throw new ObjectDisposedException(nameof(Lease));
                } while(Interlocked.CompareExchange(ref _references, count + 1, count) != count);
            }

            internal void Release()
            {
                int count = Interlocked.Decrement(ref _references);
                if(count == 0) _pool.Return(_slot);
                else if(count < 0) throw new InvalidOperationException("Restoration buffer released twice.");
            }
        }
    }
}
