using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Home : MonoBehaviour
{
    [SerializeField] private Image transitionPanel;

    private void Awake()
    {

        var currentLevel = 1;
        if (PlayerPrefs.HasKey("CurrentLevel"))
        {
            currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        }
        
        transitionPanel.DOFade(1, 1).OnComplete(() =>
        {
            SceneManager.LoadSceneAsync(currentLevel);
        });
    }
}
