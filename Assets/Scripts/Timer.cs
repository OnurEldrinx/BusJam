using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private int totalTime;
    [SerializeField] private bool countdownStarted;
    private float _counter;

    public static Action TimeIsOut;

    private void OnEnable()
    {
        GameManager.LevelStarted += () =>
        {
            countdownStarted = true;
        };
    }

    private void Awake()
    {
        timerText.text = totalTime.ToString();
        _counter = totalTime;
    }

    private void Update()
    {
        if (!countdownStarted) return;
        
        if (_counter > 0)
        {
            _counter -= Time.deltaTime;
            if (_counter < 0)
            {
                _counter = 0;
            }
            timerText.text = Mathf.RoundToInt(_counter).ToString();
        }
        else
        {
            print("Time is out!");
            countdownStarted = false;
            TimeIsOut.Invoke();
        }
    }
}
