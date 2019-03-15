// Add this script to the camera (or similar) to detect game window focus.
using UnityEngine;

public class DetectFocus : MonoBehaviour {
    // true by default so that scripts that depend on it still work even if we
    // forget to add this script to the camera.
    public static bool focused = true;

    void OnApplicationFocus(bool focusStatus) {
        focused = focusStatus;
    }
}
