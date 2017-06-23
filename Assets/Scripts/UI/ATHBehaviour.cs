using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NEngine;
using UnityEngine.UI;

namespace NInterface
{
    public class ATHBehaviour : MonoBehaviour
    {
        [Header("Elements")]
        [SerializeField]
        private Text scoreText = null;
        [SerializeField]
        private Text massText = null;
        [SerializeField]
        private Image hostilityMeter = null;
        [SerializeField]
        private GameObject menuCanvas = null;
        [SerializeField]
        private GameObject gameOverCanvas = null;
        [SerializeField]
        private Text endScoreText = null;
        [SerializeField]
        private Text highscoreText = null;

        private GameEngine gameEngine;
        private bool isGameOver;

        public void Init(GameEngine engine)
        {
            isGameOver = false;
            gameEngine = engine;
            engine.scoreChangedEvent += SetScore;
            engine.hostilityChangedEvent += SetHostility;
            engine.massChangedEvent += SetMass;
            engine.gamePausedEvent += OnGamePaused;
            engine.gameResumedEvent += OnGameResumed;
            engine.gameOverEvent += OnGameOver;
            Debug.Log("Score event setted");
        }

        public void SetScore(int score)
        {
            scoreText.text = score.ToString();
        }

        public void SetHostility(float hostility)
        {
            hostilityMeter.transform.localScale = new Vector3(1f, hostility, 1f);
        }

        public void SetMass(int mass)
        {
            massText.text = mass.ToString();
        }

        private void OnGamePaused()
        {
            if (!isGameOver)
                menuCanvas.SetActive(true);
        }

        private void OnGameResumed()
        {
            isGameOver = false;
            menuCanvas.SetActive(false);
        }

        private void OnGameOver()
        {
            isGameOver = true;
            endScoreText.text = gameEngine.GetScore().ToString();
            if (HighScoreHandler.GetCurrentHighScore() == gameEngine.GetScore())
            {
                highscoreText.text = "New High Score !";
                highscoreText.color = Color.green;
            }
            else
            {
                highscoreText.text += HighScoreHandler.GetCurrentHighScore();
            }
            gameOverCanvas.SetActive(true);
        }
    }
}
