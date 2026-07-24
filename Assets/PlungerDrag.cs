using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Click-and-drag the plunger vertically while MedicalCam is active.
/// The plunger bottom cannot go below the syringe bottom, and can rise
/// as far as the High notch. Fluid fills from the syringe bottom up to
/// the plunger bottom. At the Mid (level 2) notch, the Inject button appears.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class PlungerDrag : MonoBehaviour
{
    [Tooltip("Syringe transform used as the lower height reference.")]
    public Transform syringe;

    [Tooltip("Mid notch mark (level 2). Inject button shows when plunger bottom reaches this.")]
    public Transform midNotch;

    [Tooltip("High notch mark. The plunger bottom can rise up to this height.")]
    public Transform highNotch;

    [Tooltip("Fluid sprite that fills the syringe up to the plunger bottom.")]
    public Transform fluid;

    [Tooltip("Pot container that bounds the pot fluid.")]
    public Transform pot;

    [Tooltip("Fluid in the pot. Lowers as syringe fluid rises, and rises as syringe fluid lowers.")]
    public Transform potFluid;

    [Tooltip("Shown when the plunger bottom is at or above the Mid notch.")]
    public GameObject injectButton;

    [Tooltip("Enemy AI reset when Inject is pressed at level 2.")]
    public Entity entity;

    [Tooltip("Triggers game over UI (used for level 3 overdose).")]
    public CameraSwitchDebug cameraSwitch;

    [Tooltip("Camera used for pointer raycasts (MedicalCam).")]
    public Camera medicalCamera;

    [Header("Inject Sleep Sequence")]
    [Tooltip("Full-screen blur that fades in after a successful inject.")]
    public Image drugBlur;

    [Tooltip("Shown after Drug Blur reaches full alpha.")]
    public GameObject sleepyScreen;

    [Tooltip("Updated with a random sleep duration (HowLongAleep TMP).")]
    public TextMeshProUGUI howLongAsleepText;

    [Tooltip("Seconds for Drug Blur alpha to reach max.")]
    public float drugBlurFadeDuration = 1.5f;

    [Tooltip("Pause on Sleepy Screen before returning to Main Cam.")]
    public float sleepyScreenPause = 3f;

    [Tooltip("Optional extra lower limit as an offset from the syringe's local Y. The syringe bottom is always respected.")]
    public float minOffsetFromSyringe = -12f;

    [Tooltip("How close the plunger bottom must be to Mid (or above) to count as level 2.")]
    public float midNotchTolerance = 0.15f;

    [Tooltip("How close the plunger bottom must be to High to count as level 3.")]
    public float highNotchTolerance = 0.15f;

    public bool IsDragging { get; private set; }

    Collider2D plungerCollider;
    float dragOffsetLocalY;
    bool injectSequenceRunning;

    void Awake()
    {
        plungerCollider = GetComponent<Collider2D>();

        if (entity == null)
            entity = FindFirstObjectByType<Entity>();
        if (cameraSwitch == null)
            cameraSwitch = FindFirstObjectByType<CameraSwitchDebug>();

        if (injectButton != null)
        {
            injectButton.SetActive(false);
            var button = injectButton.GetComponent<Button>();
            if (button != null)
                button.onClick.AddListener(OnInjectPressed);
        }

        ResetSleepSequenceVisuals();
    }

    void Start()
    {
        SyncFluidToPlunger();
        UpdateInjectButtonVisibility();
    }

    void Update()
    {
        if (medicalCamera == null || !medicalCamera.gameObject.activeInHierarchy)
        {
            IsDragging = false;
            return;
        }

        if (injectSequenceRunning) return;

        var mouse = Mouse.current;
        if (mouse == null) return;

        if (mouse.leftButton.wasPressedThisFrame)
            TryBeginDrag(mouse.position.ReadValue());

        if (IsDragging && mouse.leftButton.isPressed)
            DragTo(mouse.position.ReadValue());

        if (mouse.leftButton.wasReleasedThisFrame)
            IsDragging = false;
    }

    void LateUpdate()
    {
        SyncFluidToPlunger();
        UpdateInjectButtonVisibility();
    }

    void TryBeginDrag(Vector2 screenPos)
    {
        Vector3 world = medicalCamera.ScreenToWorldPoint(screenPos);
        Vector2 world2D = world;

        if (plungerCollider == null || !plungerCollider.OverlapPoint(world2D))
            return;

        IsDragging = true;
        float localMouseY = ScreenToParentLocalY(screenPos);
        dragOffsetLocalY = transform.localPosition.y - localMouseY;
    }

    void DragTo(Vector2 screenPos)
    {
        if (syringe == null) return;

        float localMouseY = ScreenToParentLocalY(screenPos);
        float targetY = localMouseY + dragOffsetLocalY;

        float minY = GetMinLocalY();
        float maxY = GetMaxLocalY();
        float clampedY = Mathf.Clamp(targetY, minY, Mathf.Max(minY, maxY));

        Vector3 pos = transform.localPosition;
        pos.y = clampedY;
        transform.localPosition = pos;
    }

    void OnInjectPressed()
    {
        if (injectSequenceRunning) return;

        if (IsPlungerAtOrAboveHigh())
        {
            if (cameraSwitch == null)
                cameraSwitch = FindFirstObjectByType<CameraSwitchDebug>();
            if (cameraSwitch != null)
                cameraSwitch.ShowDeath("You have overdosed.");
            return;
        }

        if (entity == null)
            entity = FindFirstObjectByType<Entity>();
        if (entity != null)
            entity.ResetOutsideRadar();

        StartCoroutine(SuccessfulInjectSequence());
    }

    IEnumerator SuccessfulInjectSequence()
    {
        injectSequenceRunning = true;
        IsDragging = false;

        if (injectButton != null)
            injectButton.SetActive(false);

        // Fade Drug Blur from 0 -> 1.
        if (drugBlur != null)
        {
            drugBlur.gameObject.SetActive(true);
            drugBlur.raycastTarget = true;
            SetImageAlpha(drugBlur, 0f);

            float duration = Mathf.Max(0.01f, drugBlurFadeDuration);
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                SetImageAlpha(drugBlur, Mathf.Clamp01(elapsed / duration));
                yield return null;
            }

            SetImageAlpha(drugBlur, 1f);
        }

        // Show Sleepy Screen with a random sleep duration.
        if (howLongAsleepText != null)
            howLongAsleepText.text = FormatSleepDuration(Random.Range(20, 121));

        if (sleepyScreen != null)
            sleepyScreen.SetActive(true);

        yield return new WaitForSeconds(sleepyScreenPause);

        ResetSleepSequenceVisuals();

        if (cameraSwitch == null)
            cameraSwitch = FindFirstObjectByType<CameraSwitchDebug>();
        if (cameraSwitch != null)
            cameraSwitch.ShowMain();

        injectSequenceRunning = false;
        UpdateInjectButtonVisibility();
    }

    void ResetSleepSequenceVisuals()
    {
        if (drugBlur != null)
        {
            SetImageAlpha(drugBlur, 0f);
            drugBlur.raycastTarget = false;
            drugBlur.gameObject.SetActive(false);
        }

        if (sleepyScreen != null)
            sleepyScreen.SetActive(false);
    }

    static void SetImageAlpha(Image image, float alpha)
    {
        Color c = image.color;
        c.a = alpha;
        image.color = c;
    }

    static string FormatSleepDuration(int totalMinutes)
    {
        int hours = totalMinutes / 60;
        int minutes = totalMinutes % 60;

        if (hours <= 0)
            return minutes == 1 ? "1 minute" : $"{minutes} minutes";

        string hourPart = hours == 1 ? "1 hour" : $"{hours} hours";
        if (minutes <= 0)
            return hourPart;

        string minutePart = minutes == 1 ? "1 minute" : $"{minutes} minutes";
        return $"{hourPart} {minutePart}";
    }

    void UpdateInjectButtonVisibility()
    {
        if (injectButton == null || injectSequenceRunning) return;

        bool atLevel2 = IsPlungerAtOrAboveMid();
        if (injectButton.activeSelf != atLevel2)
            injectButton.SetActive(atLevel2);
    }

    bool IsPlungerAtOrAboveMid()
    {
        if (midNotch == null) return false;

        Transform space = transform.parent != null ? transform.parent : transform;
        float plungerBottom = GetBoundsMinLocalY(transform, space);
        float midY = space.InverseTransformPoint(midNotch.position).y;
        return plungerBottom >= midY - midNotchTolerance;
    }

    bool IsPlungerAtOrAboveHigh()
    {
        if (highNotch == null) return false;

        Transform space = transform.parent != null ? transform.parent : transform;
        float plungerBottom = GetBoundsMinLocalY(transform, space);
        float highY = space.InverseTransformPoint(highNotch.position).y;
        return plungerBottom >= highY - highNotchTolerance;
    }

    void SyncFluidToPlunger()
    {
        if (fluid == null || syringe == null) return;

        Transform space = transform.parent != null ? transform.parent : transform;
        float syringeBottom = GetBoundsMinLocalY(syringe, space);
        float syringeTop = GetBoundsMaxLocalY(syringe, space);
        float plungerBottom = GetBoundsMinLocalY(transform, space);

        // Fluid top follows the plunger bottom, clipped to the syringe square.
        float fluidTop = Mathf.Clamp(plungerBottom, syringeBottom, syringeTop);
        float fluidBottom = syringeBottom;
        float height = Mathf.Max(0.001f, fluidTop - fluidBottom);
        float centerY = (fluidTop + fluidBottom) * 0.5f;

        Vector3 pos = fluid.localPosition;
        pos.x = syringe.localPosition.x;
        pos.y = centerY;
        fluid.localPosition = pos;

        // 1x1 center-pivot square: scale matches syringe width and fill height.
        Vector3 scale = fluid.localScale;
        scale.x = syringe.localScale.x;
        scale.y = height;
        fluid.localScale = scale;

        float syringeCapacity = Mathf.Max(0.001f, syringeTop - syringeBottom);
        float syringeFill = Mathf.Clamp01(height / syringeCapacity);
        SyncPotFluid(space, syringeFill);
    }

    void SyncPotFluid(Transform space, float syringeFill)
    {
        if (pot == null || potFluid == null) return;

        float potBottom = GetBoundsMinLocalY(pot, space);
        float potTop = GetBoundsMaxLocalY(pot, space);
        float potCapacity = Mathf.Max(0.001f, potTop - potBottom);

        // Inverse of syringe fill: more in the syringe means less in the pot.
        float potHeight = Mathf.Max(0.001f, potCapacity * (1f - syringeFill));
        float potFluidTop = potBottom + potHeight;
        float centerY = (potFluidTop + potBottom) * 0.5f;

        Vector3 pos = potFluid.localPosition;
        pos.x = pot.localPosition.x;
        pos.y = centerY;
        potFluid.localPosition = pos;

        Vector3 scale = potFluid.localScale;
        scale.x = pot.localScale.x;
        scale.y = potHeight;
        potFluid.localScale = scale;
    }

    float GetPlungerBottomOffset(Transform space)
    {
        return GetBoundsMinLocalY(transform, space) - transform.localPosition.y;
    }

    float GetMinLocalY()
    {
        Transform space = transform.parent != null ? transform.parent : transform;
        float syringeBottom = GetBoundsMinLocalY(syringe, space);
        float plungerBottomOffset = GetPlungerBottomOffset(space);

        float boundsMinY = syringeBottom - plungerBottomOffset;
        float offsetMinY = syringe.localPosition.y + minOffsetFromSyringe;
        return Mathf.Max(boundsMinY, offsetMinY);
    }

    float GetMaxLocalY()
    {
        Transform space = transform.parent != null ? transform.parent : transform;
        float plungerBottomOffset = GetPlungerBottomOffset(space);

        if (highNotch != null)
        {
            float highLocalY = space.InverseTransformPoint(highNotch.position).y;
            return highLocalY - plungerBottomOffset;
        }

        return transform.localPosition.y;
    }

    static float GetBoundsMinLocalY(Transform root, Transform space)
    {
        var renderers = root.GetComponentsInChildren<Renderer>();
        if (renderers == null || renderers.Length == 0)
            return space.InverseTransformPoint(root.position).y;

        float minY = float.PositiveInfinity;
        foreach (var r in renderers)
        {
            Bounds b = r.bounds;
            Vector3 worldBottom = new Vector3(b.center.x, b.min.y, b.center.z);
            float localY = space.InverseTransformPoint(worldBottom).y;
            if (localY < minY)
                minY = localY;
        }

        return minY;
    }

    static float GetBoundsMaxLocalY(Transform root, Transform space)
    {
        var renderers = root.GetComponentsInChildren<Renderer>();
        if (renderers == null || renderers.Length == 0)
            return space.InverseTransformPoint(root.position).y;

        float maxY = float.NegativeInfinity;
        foreach (var r in renderers)
        {
            Bounds b = r.bounds;
            Vector3 worldTop = new Vector3(b.center.x, b.max.y, b.center.z);
            float localY = space.InverseTransformPoint(worldTop).y;
            if (localY > maxY)
                maxY = localY;
        }

        return maxY;
    }

    float ScreenToParentLocalY(Vector2 screenPos)
    {
        Vector3 world = medicalCamera.ScreenToWorldPoint(screenPos);
        if (transform.parent == null)
            return world.y;
        return transform.parent.InverseTransformPoint(world).y;
    }
}
