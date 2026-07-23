using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Only lets this Screen Space - Camera canvas receive UI clicks
/// while its assigned camera GameObject is active. Prevents an
/// inactive-camera canvas from stealing clicks from another menu.
/// </summary>
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(GraphicRaycaster))]
public class CanvasEventsWhenCameraActive : MonoBehaviour
{
    Canvas canvas;
    GraphicRaycaster raycaster;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        raycaster = GetComponent<GraphicRaycaster>();
        Apply();
    }

    void LateUpdate()
    {
        Apply();
    }

    void Apply()
    {
        bool camActive = canvas.worldCamera != null
            && canvas.worldCamera.gameObject.activeInHierarchy;

        if (raycaster != null && raycaster.enabled != camActive)
            raycaster.enabled = camActive;
    }
}
