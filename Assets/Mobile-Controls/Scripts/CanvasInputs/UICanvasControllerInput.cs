using UnityEngine;

public class UICanvasControllerInput : MonoBehaviour
{
    [Header("Output")]
    public PlayerController inputs;

    public void VirtualMoveInput(Vector2 virtualMoveDirection)
    {
        inputs.move = virtualMoveDirection; // Atualiza o movimento do jogador
    }

    public void VirtualLookInput(Vector2 virtualLookDirection)
    {
        // inputs.look = virtualLookDirection; // Atualiza a direção do olhar (se aplicável)
    }

    public void VirtualJumpInput(bool virtualJumpState)
    {
        inputs.jump = virtualJumpState; // Atualiza o estado do pulo
    }

    public void VirtualSprintInput(bool virtualSprintState)
    {
        // inputs.sprint = virtualSprintState; // Atualiza o estado de corrida
    }

    public void VirtualSwitchInput(bool virtualSwitchState)
    {
        inputs.switchMode = virtualSwitchState; // Atualiza o estado de troca de modo
    }
}
