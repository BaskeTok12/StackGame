using UnityEngine;
public class InputManager : MonoBehaviour
{
    private InputController _inputController;
    public InputController.PlayerInteractionsActions PlayerInteractions { get; private set; }
    public InputController.UIActions  UIActions { get; private set; }
    
    private void Awake()
    {
        _inputController = new InputController();
        
        PlayerInteractions = _inputController.PlayerInteractions;
        UIActions = _inputController.UI;
    }
    private void OnEnable()
    {
        _inputController.Enable();
        PlayerInteractions.Enable();
        UIActions.Enable();
    }
    private void OnDisable()
    {
        _inputController.Disable();
        PlayerInteractions.Disable();
        UIActions.Disable();
    }
}
