// Moves the camera around when the player's cursor is at the edge of the screen
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraScrolling : MonoBehaviour {
    [SerializeField] float speed = 13;
    [SerializeField] float sensitiveArea = 0.2f;
    [SerializeField] float xMin = 12;
    [SerializeField] float xMax =  140;
    [SerializeField] float zMin = -7;
    [SerializeField] float zMax =  140;

    void Awake() {
        Cursor.lockState = CursorLockMode.Confined;
    }

    void LateUpdate() {
        // only if the window is focused
        if (!DetectFocus.focused) return;

        // note: we allow scrolling while the mouse is over UI elements. the
        // solution is to keep the scroll area small, it gives the best feeling.
        var dir = Vector3.zero;

        // clamp the mouse position within screen size, so we don't scroll ultra
        // fast when leaving the window
        var pos = new Vector2(Mathf.Clamp(Input.mousePosition.x, 0, Screen.width),
                              Mathf.Clamp(Input.mousePosition.y, 0, Screen.height));

        // top? then scroll smoothly based on distance
        if (pos.y > Screen.height * (1-sensitiveArea)) {
            // mouse ratio in screen (between 80% and 100% etc.)
            float ratio = pos.y / Screen.height;
            // relative ratio in top area
            // (80%..100% becomes 0%..20% and then 0%..100%)
            dir.z = (ratio - (1-sensitiveArea)) / sensitiveArea;
        }

        // bottom? then scroll smoothly based on distance
        if (pos.y < Screen.height * sensitiveArea) {
            // mouse ratio in screen (between 80% and 100% etc.)
            float ratio = pos.y / Screen.height;
            // relative ratio in top area
            // (20%..100% becomes 0%..100% and then inversed for bottom)
            dir.z = -(1 - (ratio / sensitiveArea));
        }
        
        // right? then scroll smoothly based on distance
        if (pos.x > Screen.width * (1-sensitiveArea)) {
            // mouse ratio in screen (between 80% and 100% etc.)
            float ratio = pos.x / Screen.width;
            // relative ratio in top area
            // (80%..100% becomes 0%..20% and then 0%..100%)
            dir.x = (ratio - (1-sensitiveArea)) / sensitiveArea;
        }

        // left? then scroll smoothly based on distance
        if (pos.x < Screen.width * sensitiveArea) {
            // mouse ratio in screen (between 80% and 100% etc.)
            float ratio = pos.x / Screen.width;
            // relative ratio in top area
            // (20%..100% becomes 0%..100% and then inversed for bottom)
            dir.x = -(1 - (ratio / sensitiveArea));
        }

        // if x and z are set, then scrolling would be too fast. set length
        // to the longest value instead (might be negative too, hence abs)
        if (dir.x != 0 && dir.z != 0)
            dir = dir.normalized * Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.z));

        // move
        transform.position += dir * speed * Time.deltaTime;

        // keep in bounds
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, xMin, xMax),
            transform.position.y,
            Mathf.Clamp(transform.position.z, zMin, zMax)
        );
    }

    // position the camera so that it is centered on a target
    public void FocusOn(Vector3 pos) {
        // decrease forward*distance from pos. good enough for now.
        float height = transform.position.y;
        transform.position = pos - (transform.rotation * Vector3.forward * height);

        // the previous calculation is not 100% exact, which often causes us to
        // zoom in a bit too far. make sure to keep initial height.
        transform.position = new Vector3(transform.position.x, height, transform.position.z);
    }
}
