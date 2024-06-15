using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TextMeshProUGUI levelNumberText;
    [SerializeField] private Image endPanel;

    private void Awake()
    {
        int i = SceneManager.GetActiveScene().buildIndex;
        levelNumberText.text = i.ToString();
    }

    public void OnLevelEnd(bool failed)
    {
        endPanel.gameObject.SetActive(true);

        if (failed)
        {
            endPanel.DOFade(1, 1.5f).OnComplete(() =>
            {
                LevelManager.Instance.RestartLevel();
            });
        }
        else
        {
            endPanel.DOFade(1, 1.5f).OnComplete(() =>
            {
                LevelManager.Instance.NextLevel();
            });
        }
        
        
    }
    
    
    
}
