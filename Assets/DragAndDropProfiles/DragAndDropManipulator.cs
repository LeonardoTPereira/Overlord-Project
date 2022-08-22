using UnityEngine;
using UnityEngine.UIElements;

public class DragAndDropManipulator : PointerManipulator
{
    public DragAndDropManipulator(VisualElement target)
    {
        Debug.Log("Created manipulator");
        this.target = target;
        Root = target.parent;
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerDownEvent>(PointerDownHandler);
        target.RegisterCallback<PointerMoveEvent>(PointerMoveHandler);
        target.RegisterCallback<PointerUpEvent>(PointerUpHandler);
        target.RegisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerDownEvent>(PointerDownHandler);
        target.UnregisterCallback<PointerMoveEvent>(PointerMoveHandler);
        target.UnregisterCallback<PointerUpEvent>(PointerUpHandler);
        target.UnregisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
    }

    private Vector2 TargetStartPosition { get; set; }

    private Vector3 PointerStartPosition { get; set; }

    private bool Enabled { get; set; }

    private VisualElement Root { get; }

    private void PointerDownHandler(PointerDownEvent evt)
    {
        Debug.Log("Pointer Down");
        TargetStartPosition = target.transform.position;
        PointerStartPosition = evt.position;
        target.CapturePointer(evt.pointerId);
        Enabled = true;
    }

    private void PointerMoveHandler(PointerMoveEvent evt)
    {
        if (!Enabled || !target.HasPointerCapture(evt.pointerId)) return;
        Debug.Log("Pointer Move");

        var pointerDelta = evt.position - PointerStartPosition;

        target.transform.position = new Vector2(
            Mathf.Clamp(TargetStartPosition.x + pointerDelta.x, 0, target.panel.visualTree.worldBound.width),
            Mathf.Clamp(TargetStartPosition.y + pointerDelta.y, 0, target.panel.visualTree.worldBound.height));
    }

    private void PointerUpHandler(PointerUpEvent evt)
    {
        if (Enabled && target.HasPointerCapture(evt.pointerId))
        {
            Debug.Log("Pointer Up");
            target.ReleasePointer(evt.pointerId);
        }
    }

    private void PointerCaptureOutHandler(PointerCaptureOutEvent evt)
    {
        if (!Enabled) return;
        Debug.Log("Pointer Capture Out");
        var slotsContainer = Root.Q<VisualElement>("slots");
        var allSlots = slotsContainer.Query<VisualElement>(className: "slot");
        var overlappingSlots = allSlots.Where(OverlapsTarget);
        var closestOverlappingSlot = FindClosestSlot(overlappingSlots);
        var closestPos = Vector3.zero;
        if (closestOverlappingSlot != null)
        {
            closestPos = RootSpaceOfSlot(closestOverlappingSlot);
            closestPos = new Vector2(closestPos.x, closestPos.y);
        }
        
        target.transform.position = closestOverlappingSlot != null ? closestPos - new Vector3(target.layout.center.x, target.layout.center.y, 0): TargetStartPosition;

        Enabled = false;
    }

    private bool OverlapsTarget(VisualElement slot)
    {
        return target.worldBound.Overlaps(slot.worldBound);
    }

    private VisualElement FindClosestSlot(UQueryBuilder<VisualElement> slots)
    {
        var slotsList = slots.ToList();
        var bestDistanceSq = float.MaxValue;
        VisualElement closest = null;
        foreach (var slot in slotsList)
        {
            var displacement =
                RootSpaceOfSlot(slot) - target.transform.position;
            var distanceSq = displacement.sqrMagnitude;
            if (!(distanceSq < bestDistanceSq)) continue;
            bestDistanceSq = distanceSq;
            closest = slot;
        }
        return closest;
    }

    private Vector3 RootSpaceOfSlot(VisualElement slot)
    {
        var slotWorldSpace = slot.parent.LocalToWorld(slot.layout.center);
        return Root.WorldToLocal(slotWorldSpace);
    }
}