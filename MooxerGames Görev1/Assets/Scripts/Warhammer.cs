using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Warhammer : MonoBehaviour, IWeapon
{
    public Vector3 pickPosition;
    public Vector3 pickRotation;

    public Vector3 PickRotation { get; private set; }
    public Vector3 PickPosition { get; private set; }

    [SerializeField]
    private GameObject weapon;
    private Rigidbody weaponRb;
    [SerializeField]
    private GameObject hand;

    

    public void FlyToPlayer()
    {       
        weaponRb.AddForce((hand.transform.position - weapon.transform.position).normalized * 10, ForceMode.Acceleration);
    }

    public void Hit() // ıf you want create new weapons with different damage effects you can use IWeapon.Hit() function.
    {
        throw new System.NotImplementedException();
    }

    public void Pick()
    {        
        weaponRb.isKinematic = true;

        weapon.transform.SetParent(hand.transform);
        weapon.transform.localPosition = PickPosition;
        weapon.transform.localEulerAngles = PickRotation;

        Debug.Log("weapon picked.");
    }

    public void Throw()
    {
        weapon.transform.SetParent(null);
        weaponRb.isKinematic = false;
        
        weaponRb.AddForce((transform.forward) * 50, ForceMode.Impulse);
    }

    private void Awake()
    {
        PickPosition = pickPosition;
        PickRotation = pickRotation;

        weaponRb = weapon.GetComponent<Rigidbody>();
    }    
}
