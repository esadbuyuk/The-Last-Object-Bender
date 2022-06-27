using UnityEngine;

public class AiInput : IPlayerInput
{
    public void ReadInput()
    {        
        Thrust = 1f;
    }

    public float Rotation { get; private set; }
    public float Thrust { get; private set; }

    private Cpu cpu;
}
