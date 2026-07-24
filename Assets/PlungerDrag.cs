using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Collider2D))]
public class PlungerDrag : MonoBehaviour
{
    [Tooltip("Syringe transform used as the lower height reference.")]
    public Transform syringe;

    [Tooltip("High notch mark. The plunger bottom can rise up to this height.")]
    public Transform highNotch;

    [Tooltip("Fluid sprite that fills the syringe up to the plunger bottom.")]
    public Transform fluid;

    [Tooltip("Camera used for pointer raycasts (MedicalCam).")]
    public Camera medicalCamera;

    [Tooltip("Optional extra lower limit as an offset from the syringe's local Y. The syringe bottom is always respected.")]
    public float minOffsetFromSyringe = -12f;

    public bool IsDragging { get; private set; }

    Collider2D plungerCollider;
    float dragOffsetLocalY;

    void Awake()
    {
        plungerCollider = GetComponent<Collider2D>();
    }

    void Start()
    {
        SyncFluidToPlunger();
    }

    void Update()
    {
        if (medicalCamera == null || !medicalCamera.gameObject.activeInHierarchy)
        {
            IsDragging = false;
            return;
        }

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
