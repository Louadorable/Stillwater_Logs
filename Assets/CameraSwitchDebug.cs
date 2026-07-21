using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Debug toggle: F7 switches between Main Camera and RadarCam.
/// Keeps the Screen Space - Camera canvas pointed at the active camera.
/// </summary>
public class CameraSwitchDebug : MonoBehaviour
{
    public Camera mainCamera;
    public Camera radarCamera;
    public Canvas canvas;

    private bool showingRadar;

    void Start()
    {
        ShowMain();
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.f7Key.wasPressedThisFrame)
        {
            if (showingRadar)
                ShowMain();
            else
                ShowRadar();
        }
    }

    void ShowMain()
    {
        showingRadar = false;
        SetCameraActive(mainCamera, true);
        SetCameraActive(radarCamera, false);

        if (canvas != null && mainCamera != null)
            canvas.worldCamera = mainCamera;
    }

    void ShowRadar()
    {
        showingRadar = true;
        SetCameraActive(mainCamera, false);
        SetCameraActive(radarCamera, true);

        if (canvas != null && radarCamera != null)
            canvas.worldCamera = radarCamera;
    }

    static void SetCameraActive(Camera cam, bool active)
    {
        if (cam == null) return;
        cam.gameObject.SetActive(active);
    }
}
