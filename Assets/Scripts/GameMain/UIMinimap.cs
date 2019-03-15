// This script should be attached to the minimap RawImage.
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIMinimap : MonoBehaviour, IPointerDownHandler, IDragHandler {
    [SerializeField] Camera minimapCamera;
    
    // focus camera to where we click on minimap
    public void OnPointerDown(PointerEventData d) {
        // get mouse position
        var mouse = Input.mousePosition;

        // get corners in world coordinates (botleft, topleft, topright, botright)
        var corners = new Vector3[4];
        GetComponent<RectTransform>().GetWorldCorners(corners);

        // calculate relative position
        // (0,0 is at the bottom left of the screen)
        var relative = mouse - corners[0];

        // scale between 0 and 1
        float width = GetComponent<RectTransform>().rect.width;
        float height = GetComponent<RectTransform>().rect.height;
        var ratio = new Vector2(relative.x / width, relative.y / height);

        // calculate relative based on render texture size
        // (it might be 128x128 while the UI image is 150x150 etc.)
        var relativeRenderTex = new Vector2(ratio.x * GetComponent<RawImage>().texture.width,
                                            ratio.y * GetComponent<RawImage>().texture.height);

        // raycast through minimap camera. this way it will always work, no
        // matter where the terrain is or how it is rotated etc.
        Ray ray = minimapCamera.ScreenPointToRay(relativeRenderTex);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit)) {
            // left click = focus camera on that position
            if (d.button == PointerEventData.InputButton.Left) {
                Camera.main.GetComponent<CameraScrolling>().FocusOn(hit.point);
            // right click = move hero there
            } else if (d.button == PointerEventData.InputButton.Right) {
                // set indicator and navigate
                //player.SetIndicatorViaPosition(hit.point, hit.normal);
            }
        }
    }

    // dragging should keep focusing the camera on that point
    public void OnDrag(PointerEventData d) {
        OnPointerDown(d);
    }
}
