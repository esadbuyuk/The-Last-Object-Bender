using UnityEngine;

public class ControllerInput : IPlayerInput
{   
    public void ReadInput()
    {
        Rotation = Input.GetAxis("Horizontal");
        Thrust = Input.GetAxis("Vertical");
    }

    public float Rotation { get; private set; }
    public float Thrust { get; private set; }
}
