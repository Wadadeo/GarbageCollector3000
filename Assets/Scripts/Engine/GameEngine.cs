using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPlayer;
using NMap;
using NInterface;

namespace NEngine
{
    public class GameEngine : MonoBehaviour
    {
        public static GameEngine singleton;
        public delegate void scoreChangeDelegate(int score);
        public delegate void massChangedDelegate(int size);
        public delegate void hostilityChangedDelegate(float hostility);
        public delegate void healthChangedDelegate(byte health);
        public delegate void gamePausedDelegate();
        public delegate void gameResumedDelegate();
        public delegate void gameOverDelegate();

        public scoreChangeDelegate scoreChangedEvent;
        public massChangedDelegate massChangedEvent;
        public hostilityChangedDelegate hostilityChangedEvent;
        public healthChangedDelegate healthChangedEvent;
        public gamePausedDelegate gamePausedEvent;
        public gameResumedDelegate gameResumedEvent;
        public gameOverDelegate gameOverEvent;
        private int score;
        private int size;
        private float hostility = 0f;
        private int currentMass;
        private bool isPaused = false;

        [SerializeField]
        private PlayerBehaviour player;
        [SerializeField]
        private Generation generator;
        [SerializeField]
        private ATHBehaviour ath;
        [SerializeField]
        private HealthUIBehaviour health;

        #region Monobehaviour

        void Awake()
        {
            singleton = this;
        }

        void Start()
        {
            score = 0;
            size = 0;
            hostility = 0f;
            StartGame(); //
        }

        private void LateUpdate()
        {
            if (Input.GetKeyDown("escape") && !isPaused)
            {
                    PauseGame();
            }

        }

        private IEnumerator Cor_Score()
        {
            while (true)
            {
                AddScore(1 * currentMass);
                yield return new WaitForSeconds(1f);
            }
        }

        #endregion

        #region Private

        private void PauseTime()
        {
            Time.timeScale = 0f;
        }

        #endregion

        #region public

        public void StartGame()
        {
            ath.Init(this);
            health.Init(this);
            generator.init(this);
            player.Init();
            ResumeGame();
        }

        public void EndGame()
        {
            PauseGame(false, false);
            Invoke("pauseTime", 5f);
            HighScoreHandler.HandleNewScore(score);
            gameOverEvent();
        }

        public void PauseGame()
        {
            PauseGame(true, true);
        }

        public void PauseGame(bool _pauseTime = true, bool launchPauseEvent = true)
        {
            player.Stop();
            isPaused = true;
            generator.pause();
            if (_pauseTime)
                PauseTime();
            StopAllCoroutines();
            if (launchPauseEvent)
                gamePausedEvent();
            //      StartCoroutine(Cor_FadeTime());
        }

        public void ResumeGame()
        {
            player.Resume();
            isPaused = false;
            generator.resume();
            Time.timeScale = 1f;
            StartCoroutine(Cor_Score());
            //
            Debug.Log("Coroutine Score started");
            gameResumedEvent();
        }

        public void AddScore(int amount)
        {
            score += amount;
            scoreChangedEvent(score);
        }

        public void AddSize(int amount)
        {
            size += amount;
            massChangedEvent(size);
        }

        public void RefreshHealth(byte health)
        {
            healthChangedEvent(health);
        }

        public void AddHostility(float percent)
        {
            hostility += percent;
            hostility = Mathf.Clamp01(hostility);
            hostilityChangedEvent(hostility);
        }

        public void RefreshMass(int mass)
        {
            currentMass = mass;
            massChangedEvent(currentMass);
        }

        public int GetScore()
        {
            return score;
        }

        public int GetSize()
        {
            return size;
        }

        #endregion

        #region Effetcs

        private IEnumerator Cor_FadeTime()
        {
            int i = 0;
            int l = (int)(1f / Time.deltaTime);

            while (i < l)
            {
                Time.timeScale -= Time.deltaTime;
                ++i;
                yield return new WaitForEndOfFrame();
            }
            Time.timeScale = 0f;
        }

        #endregion
    }
}
