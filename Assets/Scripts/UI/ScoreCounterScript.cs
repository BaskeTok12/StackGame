using System;
using Controllers;
using TMPro;
using UnityEngine;
public class ScoreCounterScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    public int Score { get; private set; } = 1;

    private void OnEnable()
    {
        MovingCubeController.OnStack += () => {text.text = Score++.ToString(); };
        GameManager.OnRestart += () => { Score = 0; };
    }

    private void OnDisable()
    {
        MovingCubeController.OnStack -= () => {text.text = Score++.ToString(); };
        GameManager.OnRestart -= () => { Score = 0; };
    }
}

