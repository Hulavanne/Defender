using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    public RectTransform RemainingShips;
    public Text ScoreText;

    public RectTransform Overlay;
    public Text WaveText;
    public RectTransform GameOverScreen;
    public Text FinalScoreText;

    private Image[] m_shipImages;

	void Awake()
    {
        GameOverScreen.gameObject.SetActive(false);
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
}
