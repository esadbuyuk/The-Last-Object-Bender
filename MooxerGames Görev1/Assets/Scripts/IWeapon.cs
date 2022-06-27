using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    void FlyToPlayer();
    void Pick();
    void Hit();
    void Throw();

    Vector3 PickRotation { get; }
    Vector3 PickPosition { get; }
}
