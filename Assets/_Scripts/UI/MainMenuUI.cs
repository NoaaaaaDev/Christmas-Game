using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {
    [SerializeField] private Button playBtn;
    [SerializeField] private Button quitBtn;

    private void Awake() {
        playBtn.onClick.AddListener(() => {
            Loader.Load(SceneName.SelectLevelScene);
        });
        quitBtn.onClick.AddListener(() => {
            Application.Quit();
        });
    }
}
