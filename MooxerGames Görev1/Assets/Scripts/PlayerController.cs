using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController
{
    private readonly IPlayerInput playerInput;
    private readonly Transform transformToMove;
    private readonly PlayerSettings playerSettings;


    public PlayerController(IPlayerInput playerInput, Transform transformToMove, PlayerSettings playerSettings)
    {
        this.playerInput = playerInput;
        this.transformToMove = transformToMove;
        this.playerSettings = playerSettings;
    }

    public void Move()
    {
        transformToMove.Rotate(playerInput.Rotation * playerSettings.TurnSpeed * Time.deltaTime * Vector3.up);
        transformToMove.position += playerInput.Thrust * playerSettings.MoveSpeed * Time.deltaTime * transformToMove.forward;
    }
}
