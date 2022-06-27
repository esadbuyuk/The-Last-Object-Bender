using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cpu : MonoBehaviour
{
    private GameObject player1;
    private Player player;

    public GameObject hand;

    private Vector3 forwardVector;
    
    [SerializeField] 
    private PlayerSettings playerSettings;
    
    private EnemyController enemyController;

    private IPlayerInput playerInput;
    private IWeapon weapon;
    private IHoldable holdableObject;

    public List<GameObject> HoldableObjects { get; private set; } = new List<GameObject>(); // put all objects which has IHoldable interface!
    public List<GameObject> greenZones;
    public List<GameObject> pinkZones;

    private bool leftHandFull = false;
    
    private GameObject pickedObject;

    private Animator cpuAnim;

    private GameObject nearestobj;
    private GameObject nearestzone;

    
    void Start()
    {
        player1 = GameObject.Find("Player");
        player = player1.GetComponent<Player>();

        playerInput = playerSettings.UseAi ? new AiInput() as IPlayerInput : new ControllerInput();
        enemyController = new EnemyController(playerInput, transform, playerSettings);

        weapon = GetComponent<IWeapon>();

        cpuAnim = GetComponent<Animator>();        
    }

    
    void Update()
    {
        forwardVector = Vector3.forward.normalized;
        Quaternion forwardRot = Quaternion.Euler(forwardVector.x, 0, forwardVector.z);

        playerInput.ReadInput();
        enemyController.MoveToDestination(NextTarget());

        KeepCpuInBound();

        if (playerInput.Thrust == 0)
        {
            cpuAnim.SetBool("isMooving", false);
        }
        else
        {
            cpuAnim.SetBool("isMooving", true);
        }
                
        if (leftHandFull)
        {
            if (DistanceToPlayer(nearestzone) < 8)
            {
                Debug.Log("dropped");
                DropObject();                
            }
        }
        else
        {
            if (DistanceToPlayer(nearestobj) < 2)
            {
                PickNearestObject(nearestobj);
                Debug.Log("picked");
            }
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
   

    public void PickNearestObject(GameObject obj)
    {        
        if (!leftHandFull)
        {
            if (DistanceToPlayer(obj) < 2)
            {
                obj.GetComponent<IHoldable>().Pick(hand);
                leftHandFull = true;
                pickedObject = obj;

                if (pickedObject == player.pickedObject)
                {
                    player.PickedObjectNuller();
                    player.DropObject();
                }

                HoldableObjects.Remove(pickedObject);
            }
            else
            {
                Debug.Log("objenin yakınına gitmelisiniz!");                
            }
        }
    }


    private GameObject NearestObject()
    {
        nearestobj = HoldableObjects[0];

        foreach (GameObject obj in HoldableObjects)
        {
            if (DistanceToPlayer(obj) < DistanceToPlayer(nearestobj))
            {
                nearestobj = obj;
            }
        }

        return nearestobj;
    }

    private GameObject NearestZone(List<GameObject> zones)
    {
        nearestzone = zones[0];

        foreach (GameObject zone in zones)
        {
            if (DistanceToPlayer(zone) < DistanceToPlayer(nearestzone))
            {
                nearestzone = zone;
            }
        }

        return nearestzone;
    }

    private float DistanceToPlayer(GameObject obj)
    {        
        return Vector3.Distance(obj.transform.position, transform.position);
    }

     private void KeepCpuInBound()
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
        if (pickedObject == null)
        {
            return NearestObject().transform.position;
        }
        else
        {
            if (pickedObject.CompareTag("pink"))
            {
                NearestZone(pinkZones);
            }
            else if (pickedObject.CompareTag("green"))
            {
                NearestZone(greenZones);
            }
        }       
        
        return leftHandFull ? nearestzone.transform.position : NearestObject().transform.position;        
    }    
}
