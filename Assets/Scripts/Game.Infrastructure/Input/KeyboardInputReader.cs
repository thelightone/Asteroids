using Game.Core;
using UnityEngine;

namespace Game.Infrastructure
{
public class KeyboardInputReader : IInputReader
{
    public PlayerInputData GetInput()
    {
        PlayerInputData input = new PlayerInputData();

        if (Input.GetKey(KeyCode.A))
        {
            input.TurnDirection = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            input.TurnDirection = 1f;
        }
        else
        {
            input.TurnDirection = 0f;
        }

        input.Thrust = Input.GetKey(KeyCode.W) ? 1f : 0f;
        input.FireBullet = Input.GetKeyDown(KeyCode.Space);
        input.FireLaser = Input.GetKeyDown(KeyCode.LeftShift);

        return input;
    }
}
}
