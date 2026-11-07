using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace MORT.TransAPI
{
    /// <summary>ExternDll의 x64 hdTrans 엔진을 지연 로드한다. 입력과 반환 버퍼는 UTF-8이다.</summary>
    public sealed class HdTransAPI : IDisposable
    {
        private readonly object _sync = new object();
        private IntPtr _library;
        private bool _initialized;
        private bool _disposed;
        private InitializeDelegate? _initialize;
        private TranslateDelegate? _translate;
        private FreeMemDelegate? _freeMem;
        private TerminateDelegate? _terminate;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int InitializeDelegate(IntPtr directories);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int TranslateDelegate(byte[] text, UIntPtr length, out IntPtr output, out UIntPtr outputLength);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void FreeMemDelegate(IntPtr buffer);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void TerminateDelegate();

        public static string UnavailableMessage => LocalizeManager.LocalizeManager.GetLocalizeString("HdTrans Unavailable");

        public bool TryInitialize()
        {
            lock (_sync)
            {
                try
                {
                    ObjectDisposedException.ThrowIf(_disposed, this);
                    EnsureInitialized();
                    return true;
                }
                catch (Exception e)
                {
                    //화면에는 공통 안내만 보여 주고 상세 원인은 로그에 남긴다.
                    Util.ShowLog("[hdTrans] " + e);
                    return false;
                }
            }
        }

        public string GetResult(string original, ref bool isError)
        {
            //초기화·번역·종료를 같은 잠금으로 보호하여 실행 중인 DLL이 해제되지 않게 한다.
            lock (_sync)
            {
                isError = false;
                try
                {
                    if (!TryInitialize())
                    {
                        isError = true;
                        return UnavailableMessage;
                    }
                    if (string.IsNullOrWhiteSpace(original)) return original;
                    byte[] input = Encoding.UTF8.GetBytes(original);
                    IntPtr output = IntPtr.Zero;
                    try
                    {
                        int result = _translate!(input, (UIntPtr)input.Length, out output, out UIntPtr length);
                        if (result != 0 || output == IntPtr.Zero)
                        {
                            throw new InvalidOperationException(string.Format(
                                LocalizeManager.LocalizeManager.GetLocalizeString("HdTrans Translation Failed"), result));
                        }
                        return Marshal.PtrToStringUTF8(output, checked((int)length.ToUInt64())) ?? "";
                    }
                    finally
                    {
                        if (output != IntPtr.Zero) _freeMem!(output);
                    }
                }
                catch (Exception e)
                {
                    isError = true;
                    return LocalizeManager.LocalizeManager.GetLocalizeString("HdTrans Name") + ": " + e.Message;
                }
            }
        }

        private void EnsureInitialized()
        {
            if (_initialized) return;
            string path = Path.Combine(AppContext.BaseDirectory, "ExternDll", "hdTransEngine.dll");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(LocalizeManager.LocalizeManager.GetLocalizeString("HdTrans Engine Missing"), path);
            }
            try
            {
                _library = NativeLibrary.Load(path);
                _initialize = LoadFunction<InitializeDelegate>("hdTrans_Initialize");
                _translate = LoadFunction<TranslateDelegate>("hdTrans_Translate");
                _freeMem = LoadFunction<FreeMemDelegate>("hdTrans_FreeMem");
                _terminate = LoadFunction<TerminateDelegate>("hdTrans_Terminate");
                //NULL이면 DLL 옆 hdtrans-data 폴더에서 사전과 사용자 설정을 읽는다.
                if (_initialize(IntPtr.Zero) != 0)
                {
                    _terminate();
                    throw new InvalidOperationException(LocalizeManager.LocalizeManager.GetLocalizeString("HdTrans Initialize Failed"));
                }
                _initialized = true;
            }
            catch
            {
                if (_library != IntPtr.Zero) NativeLibrary.Free(_library);
                _library = IntPtr.Zero;
                throw;
            }
        }

        private T LoadFunction<T>(string name) where T : Delegate
        {
            return Marshal.GetDelegateForFunctionPointer<T>(NativeLibrary.GetExport(_library, name));
        }

        public void Dispose()
        {
            lock (_sync)
            {
                if (_disposed) return;
                _disposed = true;
                try
                {
                    if (_initialized) _terminate!();
                }
                finally
                {
                    _initialized = false;
                    if (_library != IntPtr.Zero) NativeLibrary.Free(_library);
                    _library = IntPtr.Zero;
                }
            }
        }
    }
}
