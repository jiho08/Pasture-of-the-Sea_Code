using Code.UI.Setting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.UI.Title
{
    public class TitleUI : MonoBehaviour
    {
        [SerializeField] private string gameSceneName;

        [Header("<color=yellow>[ GameOver UI ]</color>")]
        [SerializeField] private Button startButton;
        [SerializeField] private Button settingButton;
        [SerializeField] private Button quitButton;

        private void Awake()
        {
            startButton.onClick.AddListener(HandleStartButton);
            settingButton.onClick.AddListener(HandleSettingButton);
            quitButton.onClick.AddListener(HandleQuitButton);
            
            FixedScreen.FixedScreenSet();
        }

        private void OnDestroy()
        {
            startButton.onClick.RemoveListener(HandleStartButton);
            settingButton.onClick.RemoveListener(HandleSettingButton);
            quitButton.onClick.RemoveListener(HandleQuitButton);
        }

        #region Event Method

        private void HandleStartButton()
        {
            SceneManager.LoadScene(gameSceneName);
            SettingViewModel.Instance.Close();
        }

        private void HandleSettingButton()
        {
            SettingViewModel.Instance.Open();
        }
        
        private void HandleQuitButton() => Application.Quit();

        #endregion
    }
}