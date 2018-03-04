using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    public RectTransform RemainingShips;
    public Text ScoreText;

    public RectTransform Overlay;
    public Text WaveText;
    public RectTransform GameOverScreen;
    public RectTransform SubmitScoreScreen;
    public RectTransform TopScoresScreen;
    public Text FinalScoreText;
    public InputField NameInputField;
    public RectTransform TopScores;

    private Image[] m_shipImages;

	void Awake()
    {
        GameOverScreen.gameObject.SetActive(false);
        TopScoresScreen.gameObject.SetActive(false);

        m_shipImages = RemainingShips.GetComponentsInChildren<Image>();
	}

	void Update()
    {
        // Update the amount of backup ships remaining
        for (int i = 0; i < m_shipImages.Length; ++i)
        {
            if (i < GameManager.Instance.RemainingBackupShips) m_shipImages[i].enabled = true;
            else m_shipImages[i].enabled = false;
        }

        // Update texts
        ScoreText.text = GameManager.Instance.Score.ToString();
        WaveText.text = "Wave " + (GameManager.Instance.WaveNumber + 1).ToString();
        FinalScoreText.text = "Final Score: " + ScoreText.text;
	}

    public void ButtonClick_Start()
    {
        GameManager.Instance.StartWave(GameManager.Instance.WaveEnemyAmount);
        Overlay.gameObject.SetActive(false);
    }

    public void ButtonClick_SubmitScore()
    {
        string _allScores = PlayerPrefs.GetString("Scores");
        string _stringToSave = NameInputField.text + ":" + GameManager.Instance.Score + "|";
        _allScores += _stringToSave;
        PlayerPrefs.SetString("Scores", _allScores);

        SetTopScoreTexts();

        SubmitScoreScreen.gameObject.SetActive(false);
        TopScoresScreen.gameObject.SetActive(true);
    }

    public void ButtonClick_Restart()
    {
        GameManager.Instance.RestartGame();
    }

    public void ButtonDown_Right() { PlayerInput.RightButtonDown = true; }
    public void ButtonUp_Right() { PlayerInput.RightButtonDown = false; }

    public void ButtonDown_Left() { PlayerInput.LeftButtonDown = true; }
    public void ButtonUp_Left() { PlayerInput.LeftButtonDown = false; }

    public void ButtonDown_Up() { PlayerInput.UpButtonDown = true; }
    public void ButtonUp_Up() { PlayerInput.UpButtonDown = false; }

    public void ButtonDown_Down() { PlayerInput.DownButtonDown = true; }
    public void ButtonUp_Down() { PlayerInput.DownButtonDown = false; }

    public void ButtonDown_Shoot() { PlayerInput.ShootInput = true; }

    public void SetTopScoreTexts()
    {
        // Load all scores to a string, example of what it could look like:
        // Name1:200|Name2:100|Name3:1000|
        string _allScores = PlayerPrefs.GetString("Scores");

        string _nameAndScore = "";
        string _name = "";
        int _score = 0;

        Dictionary<string, int> _namesAndScores = new Dictionary<string, int>();

        for (int i = 0; i < _allScores.Length; ++i)
        {
            if (_allScores[i] != '|')
            {
                _nameAndScore += _allScores[i];
            }
            else
            {
                string[] _splitStrings = _nameAndScore.Split(':');
                _nameAndScore = "";

                _name = _splitStrings[0];
                _score = Convert.ToInt32(_splitStrings[1]);

                _namesAndScores.Add(_name, _score);
            }
        }

        _namesAndScores = _namesAndScores.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        for (int i = 1; i < TopScores.transform.childCount; ++i)
        {
            if (i <= _namesAndScores.Count)
            {
                _name = _namesAndScores.Keys.ElementAt(i - 1);
                _score = _namesAndScores.Values.ElementAt(i - 1);
                TopScores.transform.GetChild(i).GetComponent<Text>().text = ProjectConstants.AddOrdinal(i) + ": " + _name + " | " + _score;
            }
            else TopScores.transform.GetChild(i).GetComponent<Text>().text = ProjectConstants.AddOrdinal(i) + ": ";
        }
    }
}
