using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDeviceAssigner : MonoBehaviour
{
    public PlayerInput[] players; // Assign in Inspector

    void Start()
    {
        var gamepads = Gamepad.all;
        Debug.Log("total con ===== " + gamepads.Count);
        for (int i = 0; i < players.Length; i++)
        {
            if (i < gamepads.Count)
            {
                // Assign gamepad i to player i
                players[i].SwitchCurrentControlScheme("Gamepad", gamepads[i]);
                Debug.Log($"Assigned {gamepads[i].displayName} to Player {i + 1}");
            }
            else
            {
                Debug.LogWarning("Not enough gamepads for all players!");
            }
        }
    }
}
