using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject hand;

    [SerializeField] private PlayerSettings playerSettings;

    private IPlayerInput playerInput;
    private PlayerController playerController;

    private IWeapon weapon;
    private IHoldable holdableObject;

    public List<GameObject> HoldableObjects { get; private set; } = new List<GameObject>(); // put all objects which has IHoldable interface!

    private bool leftHandFull = false;
    private bool rightHandFull = true;

    public GameObject pickedObject { get; private set; }

    private bool weaponİsComing = false;

    private Animator playerAnim;

    public bool canBreak { get; private set; }


    private void Awake()
    {
        playerInput = playerSettings.UseAi ? new AiInput() as IPlayerInput : new ControllerInput();
        playerController = new PlayerController(playerInput, transform, playerSettings);

        weapon = GetComponent<IWeapon>();

        playerAnim = GetComponent<Animator>();
    }
    
    void Update()
    {
        playerInput.ReadInput();
        playerController.Move();

        KeepPlayerInBound();

        if (playerInput.Thrust == 0)
        {
            playerAnim.SetBool("isMooving", false);
        }
        else
        {
            playerAnim.SetBool("isMooving", true);
        }
        
        if (Input.GetKeyDown(KeyCode.R) && !rightHandFull)
        {
            ThrowOrCallWeapon();
        }

        if (weaponİsComing == true)
        {
            MakeWeaponFly();
        }

        if (Input.GetKeyDown(KeyCode.E) && rightHandFull)
        {
            BreakObject();
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            PickNearestObject();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            DropObject();
        }
    }

    public void ThrowOrCallWeapon()
    {
        if (rightHandFull)
        {
            ThrowWeaponAnim();
        }
        else
        {
            CallWeapon();
        }
    }

    public void DropObject()
    {
        if (leftHandFull)
        {
            pickedObject.GetComponent<IHoldable>().Drop();
            leftHandFull = false;
        }        
    }

    public void MakeWeaponFly() // I called this with animation event.
    {
        weapon.FlyToPlayer();        
    }

    public void CallWeapon()
    {
        weaponİsComing = true;
        playerAnim.SetTrigger("Break"); // this one looks like calling hammer when hammer is throwed.
    }

    public void PickWeapon()
    {
        weapon.Pick();
        rightHandFull = true;
    }

    public void SendWeapon() // I called this with animation event.
    {
        weapon.Throw();
        rightHandFull = false;
    }

    public void ThrowWeaponAnim()
    {
        playerAnim.SetTrigger("Throw Weapon");
    }

    public void BreakObject()
    {
        if (rightHandFull)
        {
            playerAnim.SetTrigger("Break");
            canBreak = true;
        }
    }

    public void CantBreakAnymore() // I called this with animation event.
    {
        canBreak = false;
    }

    public void PickNearestObject()
    {
        GameObject obj = NearestObject();

        if (!leftHandFull)
        {
            if (DistanceToPlayer(obj) < 1)
            {
                obj.GetComponent<IHoldable>().Pick(hand);
                leftHandFull = true;
                pickedObject = obj;
            }
            else
            {
                Debug.Log("objenin yakınına gitmelisiniz!");                
            }
        }
    }


    private GameObject NearestObject()
    {
        var nearestobj = HoldableObjects[0];
        foreach(GameObject obj in HoldableObjects) 
        {
            if ( DistanceToPlayer(obj) < DistanceToPlayer(nearestobj))
            {
                nearestobj = obj;
            }
        }

        return nearestobj;
    }

    private float DistanceToPlayer(GameObject obj)
    {
        return Mathf.Sqrt((float)(Math.Pow(obj.transform.position.x - transform.position.x, 2) + Math.Pow(obj.transform.position.z - transform.position.z, 2)));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (weaponİsComing)
        {
            if (collision.gameObject.CompareTag("weapon"))
            { 
                weaponİsComing = false;
                PickWeapon();
            }
        }       
    }

    private void KeepPlayerInBound()
    {
        if (transform.position.z < -43)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -43);
        }
        else if (transform.position.z > 43)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 43);
        }

        if (transform.position.x < -43)
        {
            transform.position = new Vector3(-43, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > 43)
        {
            transform.position = new Vector3(43, transform.position.y, transform.position.z);
        }
    }

    public void AddToHoldableObjects(GameObject gameObject)
    {
        HoldableObjects.Add(gameObject);
    }

    public Vector3 NextTarget()
    {
        return NearestObject().transform.position - transform.position;
    }

    public void PickedObjectNuller()
    {
        pickedObject = null;
        leftHandFull = false;
    }
}
