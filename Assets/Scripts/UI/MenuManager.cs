using Controllers;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button restartButton;
    private void OnEnable()
    {
        GameManager.OnStart += () => SetActiveButton(startButton, true);
        GameManager.OnRestart += () => SetActiveButton(restartButton, true);
    }
    private void OnDisable()
    {
        GameManager.OnStart -= () => SetActiveButton(startButton, true);
        GameManager.OnRestart -= () => SetActiveButton(restartButton, true);
    }

    private void SetActiveButton(Button button, bool state)
    {
        button.gameObject.SetActive(state);
    }
}
