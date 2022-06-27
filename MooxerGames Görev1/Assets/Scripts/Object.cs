using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour, IHoldable
{    
    private GameObject player1;
    private Player player;
    private GameObject cpu1;
    private Cpu cpu;

    private Rigidbody objectRb;

    public Vector3 pickPosition;
    public Vector3 pickRotation;

    public Vector3 PickRotation { get; private set; }

    public Vector3 PickPosition { get; private set; }

    public void AssignAsHoldable()
    {
        player.AddToHoldableObjects(gameObject);
        cpu.AddToHoldableObjects(gameObject);
    }


    public void Drop()
    {
        transform.SetParent(null);
        objectRb.isKinematic = false;
    }

    public void Pick(GameObject hand)
    {
        objectRb.isKinematic = true;
        transform.SetParent(hand.transform);
        transform.localPosition = PickPosition;
        transform.localEulerAngles = PickRotation;
    }

    void Awake()
    {       
        PickPosition = pickPosition;
        PickRotation = pickRotation;
        
        player1 = GameObject.Find("Player");
        cpu1 = GameObject.Find("CPU");
        objectRb = GetComponent<Rigidbody>();
        player = player1.GetComponent<Player>();
        cpu = cpu1.GetComponent<Cpu>();
    }   

    private void OnCollisionEnter(Collision collision)
    {
        if (player.canBreak)
        {
            if (collision.gameObject.CompareTag("weapon"))
            {
                gameObject.SetActive(false); // ıf you want create new weapons with different damage effects you can use IWeapon.Hit() function.
            }
        }
    }

    void Start()
    {
        AssignAsHoldable();
    }
}
