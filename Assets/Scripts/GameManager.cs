using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] private Button startButton;
    public static Action LevelStarted;
    public static Action LevelSucceed;
    public static Action LevelFailed;
    public bool levelStarted;
    public bool levelSucceed;

    private void OnEnable()
    {
        Timer.TimeIsOut += OnLevelFailed;
    }

    private void OnDisable()
    {
        Timer.TimeIsOut -= OnLevelFailed;
    }

    public void OnLevelStarted()
    {
        startButton.gameObject.SetActive(false);
        LevelStarted.Invoke();
        levelStarted = true;
        PlayerPrefs.SetInt("CurrentLevel",SceneManager.GetActiveScene().buildIndex);
    }

    public void OnLevelSucceed()
    {
        print("Level Succeed!");
        int id = PlayerPrefs.GetInt("LevelID");
        id++;
        PlayerPrefs.SetInt("LevelID",id);
        
        VFXManager.Instance.PlayConfettiBlast();
        
        //LevelManager.Instance.NextLevel();
        //LevelManager.Instance.LoadLevel(id);

    }

    public void OnLevelFailed()
    {
        print("Level Failed!");
        UIManager.Instance.OnLevelEnd(true);
    }
    
    
}
