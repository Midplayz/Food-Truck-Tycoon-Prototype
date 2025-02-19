using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [field: SerializeField] private Button playButton;
    [field: SerializeField] private Button quitButton;

    void Start()
    {
        playButton.onClick.AddListener(() => { SceneManager.LoadScene("MainGame"); });
        quitButton.onClick.AddListener(() => { Application.Quit(); });
    }
}
