using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// F7 toggles RadarCam (press again to return to Main).
/// F8 toggles MedicalCam (press again to return to Main).
/// F6 always returns to Main Camera.
/// ShowDeath() switches to the game-over DeathCam / death UI.
/// </summary>
public class CameraSwitchDebug : MonoBehaviour
{
    public Camera mainCamera;
    public Camera radarCamera;
    public Camera medicalCamera;
    public Camera deathCamera;
    public Canvas canvas;
    public Canvas deathCanvas;
    public GameObject deathScreen;
    [Tooltip("Optional subtitle under GAME OVER (e.g. overdose). Created at runtime if unset.")]
    public TextMeshProUGUI deathReasonText;

    private bool showingRadar;
    private bool showingMedical;
    private bool gameOver;

    void Awake()
    {
        EnsureDeathReasonText();
    }

    void Start()
    {
        ShowMain();
    }

    void Update()
    {
        if (gameOver) return;

        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.f7Key.wasPressedThisFrame)
        {
            if (showingRadar)
                ShowMain();
            else
                ShowRadar();
        }

        if (keyboard.f8Key.wasPressedThisFrame)
        {
            if (showingMedical)
                ShowMain();
            else
                ShowMedical();
        }

        if (keyboard.f6Key.wasPressedThisFrame)
            ShowMain();
    }

    public void ShowDeath(string reason = null)
    {
        gameOver = true;
        showingRadar = false;
        showingMedical = false;

        var entity = FindFirstObjectByType<Entity>();
        if (entity != null)
            entity.StopForGameOver();

        SetCameraActive(mainCamera, false);
        SetCameraActive(radarCamera, false);
        SetCameraActive(medicalCamera, false);
        SetCameraActive(deathCamera, true);

        if (canvas != null)
            canvas.gameObject.SetActive(false);

        if (deathCanvas != null)
        {
            deathCanvas.gameObject.SetActive(true);
            if (deathCamera != null)
                deathCanvas.worldCamera = deathCamera;
        }

        if (deathScreen != null)
            deathScreen.SetActive(true);

        EnsureDeathReasonText();
        if (deathReasonText != null)
        {
            bool hasReason = !string.IsNullOrEmpty(reason);
            deathReasonText.text = hasReason ? reason : "";
            deathReasonText.gameObject.SetActive(hasReason);
        }
    }

    void ShowMain()
    {
        showingRadar = false;
        showingMedical = false;
        SetCameraActive(mainCamera, true);
        SetCameraActive(radarCamera, false);
        SetCameraActive(medicalCamera, false);
        SetCameraActive(deathCamera, false);

        if (canvas != null)
        {
            canvas.gameObject.SetActive(true);
            if (mainCamera != null)
                canvas.worldCamera = mainCamera;
        }

        HideDeathUi();
    }

    void ShowRadar()
    {
        showingRadar = true;
        showingMedical = false;
        SetCameraActive(mainCamera, false);
        SetCameraActive(radarCamera, true);
        SetCameraActive(medicalCamera, false);
        SetCameraActive(deathCamera, false);

        if (canvas != null)
        {
            canvas.gameObject.SetActive(true);
            if (radarCamera != null)
                canvas.worldCamera = radarCamera;
        }

        HideDeathUi();
    }

    void ShowMedical()
    {
        showingRadar = false;
        showingMedical = true;
        SetCameraActive(mainCamera, false);
        SetCameraActive(radarCamera, false);
        SetCameraActive(medicalCamera, true);
        SetCameraActive(deathCamera, false);

        if (canvas != null)
        {
            canvas.gameObject.SetActive(true);
            if (medicalCamera != null)
                canvas.worldCamera = medicalCamera;
        }

        HideDeathUi();
    }

    void HideDeathUi()
    {
        if (deathCanvas != null)
            deathCanvas.gameObject.SetActive(false);

        if (deathScreen != null)
            deathScreen.SetActive(false);
    }

    void EnsureDeathReasonText()
    {
        if (deathReasonText != null || deathCanvas == null)
            return;

        TextMeshProUGUI title = null;
        foreach (var tmp in deathCanvas.GetComponentsInChildren<TextMeshProUGUI>(true))
        {
            if (tmp.text != null
                && tmp.text.IndexOf("GAME OVER", System.StringComparison.OrdinalIgnoreCase) >= 0)
            {
                title = tmp;
                break;
            }
        }

        if (title == null)
            return;

        var go = new GameObject("DeathReasonText", typeof(RectTransform));
        go.transform.SetParent(title.transform.parent, false);
        deathReasonText = go.AddComponent<TextMeshProUGUI>();
        deathReasonText.font = title.font;
        deathReasonText.fontSharedMaterial = title.fontSharedMaterial;
        deathReasonText.color = title.color;
        deathReasonText.fontSize = 80f;
        deathReasonText.alignment = TextAlignmentOptions.Center;
        deathReasonText.enableWordWrapping = true;
        deathReasonText.text = "";
        deathReasonText.gameObject.SetActive(false);

        var rt = deathReasonText.rectTransform;
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = title.rectTransform.anchoredPosition + new Vector2(0f, -140f);
        rt.sizeDelta = new Vector2(1400f, 120f);
    }

    static void SetCameraActive(Camera cam, bool active)
    {
        if (cam == null) return;
        cam.gameObject.SetActive(active);
    }
}
