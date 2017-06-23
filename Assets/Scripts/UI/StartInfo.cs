using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NEngine;

public class StartInfo : MonoBehaviour
{
    [Header ("UI Elements")]
    [SerializeField]    private Text highscoreText;
    [SerializeField]    private Text versionText;
    [Header("Parameters")]
    [SerializeField]    private string highScorePrefix = "Highscore : ";
    [SerializeField] private string versionPrefix = "Version ";

    void Start ()
    {
        highscoreText.text = highScorePrefix + HighScoreHandler.GetCurrentHighScore().ToString();
        versionText.text = versionPrefix + Application.version;
	}
}
