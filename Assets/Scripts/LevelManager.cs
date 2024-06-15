using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private Level[] levelPrefabs;
    
    private void Awake()
    {

        Level l = FindObjectOfType<Level>();
        Station.Instance.SetBusQueue(l.BusQueue());
        
    }

    public Level LoadLevel(int levelID)
    {
        Level l = Instantiate(levelPrefabs[levelID - 1]);

        for (int i = 0; i < l.BusQueue().Count; i++)
        {
            l.BusQueue()[i].transform.parent = null;
        }
        
        l.SetLevelToCenter();
        
        return l;
    }

    public void NextLevel()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextIndex == 6)
        {
            nextIndex = 1;
        }
        
        PlayerPrefs.SetInt("CurrentLevel",nextIndex);
        SceneManager.LoadSceneAsync(nextIndex);
        
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
