using UnityEngine;

public interface IHoldable
{
    void Pick(GameObject hand);
    void Drop();
    void AssignAsHoldable();

    Vector3 PickRotation { get; }
    Vector3 PickPosition { get; }
}
