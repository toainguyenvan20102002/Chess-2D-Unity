using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Type;

public class ControlUI : MonoBehaviour
{
    public static ControlUI instance;
    private void Start()
    {
        if(instance == null) instance = this;
    }

    [SerializeField] private GameObject endgamePanel;
    [SerializeField] private GameObject pausePanel;

    [SerializeField] private TMP_Text whitePlayer;
    [SerializeField] private TMP_Text blackPlayer;
    [SerializeField] private TMP_Text whoWin;
    [SerializeField] private Button restartGame1;
    [SerializeField] private Button restartGame2;



    public void ActivePausePanel()
    {
        pausePanel.SetActive(true);
    }

    public void Pack()
    {
        pausePanel.SetActive(false);
    }

    public void NotifyWinner(EColor colorWinner)
    {
        endgamePanel.gameObject.SetActive(true);
        whitePlayer.gameObject.SetActive(false);
        blackPlayer.gameObject.SetActive(false);
        restartGame2.gameObject.SetActive(false);

        if (colorWinner == EColor.WHITE)
        {
            whoWin.text = "White Player Win";
        }
        else whoWin.text = "Black Player Win";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
