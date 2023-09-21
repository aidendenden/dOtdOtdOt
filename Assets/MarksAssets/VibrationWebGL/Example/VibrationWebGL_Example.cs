using UnityEngine;
using MarksAssets.VibrationWebGL;

public class VibrationWebGL_Example : MonoBehaviour {
    public void Vibrate() {
        VibrationWebGL.Vibrate(500);//vibrate for 500ms;
    }

    public void VibrateSequence() {
        VibrationWebGL.Vibrate(new uint[] {200, 500, 200});//vibrate for 200ms, stop for 500ms, vibrate for 200ms again.
    }

    public void Stop() {
        VibrationWebGL.Vibrate(0);//interrupt vibration. Same as calling Vibrate with an empty array: VibrationWebGL.Vibrate(new uint[] {})
    }
}
