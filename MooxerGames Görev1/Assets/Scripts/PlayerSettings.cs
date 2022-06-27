using UnityEngine;

[CreateAssetMenu(menuName = "Player/Settings", fileName = "PlayerData")]
public class PlayerSettings : ScriptableObject
{
    [SerializeField] private float turnSpeed = 100f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private bool useAi = false;

    public float TurnSpeed { get { return turnSpeed; } }
    public float MoveSpeed { get { return moveSpeed; } }
    public bool UseAi { get { return useAi; } }
}
