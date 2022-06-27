using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController
{
    private readonly IPlayerInput playerInput;
    private readonly Transform transformToMove;
    private readonly PlayerSettings playerSettings;


    public EnemyController(IPlayerInput playerInput, Transform transformToMove, PlayerSettings playerSettings)
    {
        this.playerInput = playerInput;
        this.transformToMove = transformToMove;
        this.playerSettings = playerSettings;
    }
    
    public void MoveToDestination(Vector3 destination)
    {
        Vector3 lookdirection = destination - transformToMove.position;          
        Quaternion rotation = Quaternion.LookRotation(lookdirection, Vector3.forward );
        rotation.x = transformToMove.rotation.x;
        rotation.z = transformToMove.rotation.z;

        transformToMove.rotation = rotation;
        transformToMove.position += playerInput.Thrust * playerSettings.MoveSpeed * Time.deltaTime * transformToMove.forward;
    }
}