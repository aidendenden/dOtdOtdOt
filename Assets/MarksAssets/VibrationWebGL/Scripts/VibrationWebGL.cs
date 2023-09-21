using System.Runtime.InteropServices;

namespace MarksAssets.VibrationWebGL {
    public class VibrationWebGL  {
        [DllImport("__Internal", EntryPoint="VibrateArray_VibrationWebGL")]
        private static extern bool VibrateArray_VibrationWebGL(uint[] array, int size);

        #if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal", EntryPoint="Vibrate_VibrationWebGL")]
        public static extern bool Vibrate(uint value);
        #else
        public static bool Vibrate(uint value) {return false;}
        #endif

        #if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal", EntryPoint="isSupported_VibrationWebGL")]
        public static extern bool isSupported();
        #else
        public static bool isSupported() {return false;}
        #endif

        public static bool Vibrate(uint[] array = null) {
            #if UNITY_WEBGL && !UNITY_EDITOR
            return VibrateArray_VibrationWebGL(array != null ? array : new uint[] {100}, array != null ? array.Length : 1);
            #else
            return false;
            #endif
        }
    }
}