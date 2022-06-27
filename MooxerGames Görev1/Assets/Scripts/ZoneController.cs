using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneController : MonoBehaviour
{
    [SerializeField]
    private string color;
    [SerializeField]
    private GameObject GameManagerObj;    
    private GameObject player1;
    private Player player;

    private GameManager GameManager;


    private void Awake()
    {
        GameManager = GameManagerObj.GetComponent<GameManager>();

        player1 = GameObject.Find("Player");        
        player = player1.GetComponent<Player>();        
    }    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(color))
        {            
            collision.gameObject.SetActive(false);

            if (collision.gameObject == player.pickedObject)
            {
                GameManager.UpdateScore(1);
            }
            else
            {
                GameManager.UpdateEnemyScore(1);
            }
        }
    }
}
