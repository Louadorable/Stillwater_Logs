using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Click-and-drag the plunger vertically while MedicalCam is active.
/// The plunger bottom cannot go below the syringe bottom, and can rise
/// as far as the High notch.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class PlungerDrag : MonoBehaviour
{
    [Tooltip("Syringe transform used as the lower height reference.")]
    public Transform syringe;

    [Tooltip("High notch mark. The plunger bottom can rise up to this height.")]
    public Transform highNotch;

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

    float GetPlungerBottomOffset(Transform space)
    {
        return GetBoundsMinLocalY(transform, space) - transform.localPosition.y;
    }

    float GetMinLocalY()
    {
        Transform space = transform.parent != null ? transform.parent : transform;
        float syringeBottom = GetBoundsMinLocalY(syringe, space);
        float plungerBottomOffset = GetPlungerBottomOffset(space);

        // Keep the plunger's visual bottom from going under the syringe's visual bottom.
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
            // Allow the plunger bottom to rise up to the High notch.
            return highLocalY - plungerBottomOffset;
        }

        // Fallback if High notch isn't assigned.
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

    float ScreenToParentLocalY(Vector2 screenPos)
    {
        Vector3 world = medicalCamera.ScreenToWorldPoint(screenPos);
        if (transform.parent == null)
            return world.y;
        return transform.parent.InverseTransformPoint(world).y;
    }
}
