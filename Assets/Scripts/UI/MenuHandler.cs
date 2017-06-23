using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NInterface
{
    public class MenuHandler : MonoBehaviour
    {
        public MixLevels audioHandler;

        private void Start()
        {
            audioHandler.InitVolumes();
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void LoadScene(int buildIndex)
        {
            SceneManager.LoadScene(buildIndex);
        }

        public void LoadNextScene()
        {
            LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void LoadPreviousScene()
        {
            LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}