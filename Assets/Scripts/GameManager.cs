using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int currentLevel = 1;
    int MaxLevelNumber = 15;
    public bool isGameStarted;

    #region UI Elements
    public GameObject WinPanel, LosePanel, InGamePanel;
    public Button VibrationButton, TapToStartButton;
    public Sprite on, off;
    public Text LevelText;
    #endregion

    private void Awake()
    {
        //if (!PlayerPrefs.HasKey("VIBRATION"))
        //{
        //    PlayerPrefs.SetInt("VIBRATION", 1);
        //    VibrationButton.GetComponent<Image>().sprite = on;
        //}
        //else
        //{
        //    if (PlayerPrefs.GetInt("VIBRATION") == 1)
        //    {
        //        VibrationButton.GetComponent<Image>().sprite = on;
        //    }
        //    else
        //    {
        //        VibrationButton.GetComponent<Image>().sprite = off;
        //    }
        //}
        currentLevel = PlayerPrefs.GetInt("LevelId");

        LevelText.text = "Level " + currentLevel;
    }

    public IEnumerator WaitAndGameWin()
    {
        //SoundManager.Instance.StopAllSounds();
        //SoundManager.Instance.playSound(SoundManager.GameSounds.Win);

        yield return new WaitForSeconds(1f);

        //if (PlayerPrefs.GetInt("VIBRATION") == 1)
        //    TapticManager.Impact(ImpactFeedback.Light);

        currentLevel++;
        PlayerPrefs.SetInt("LevelId", currentLevel);
        WinPanel.SetActive(true);
        InGamePanel.SetActive(false);
    }

    public IEnumerator WaitAndGameLose()
    {
        //SoundManager.Instance.playSound(SoundManager.GameSounds.Lose);

        yield return new WaitForSeconds(.5f);

        //if (PlayerPrefs.GetInt("VIBRATION") == 1)
        //    TapticManager.Impact(ImpactFeedback.Light);

        LosePanel.SetActive(true);
        InGamePanel.SetActive(false);
    }

    public void TapToNextButtonClick()
    {
        if (currentLevel > MaxLevelNumber)
        {
            int rand = Random.Range(1, MaxLevelNumber);
            if (rand == PlayerPrefs.GetInt("LastRandomLevel"))
            {
                rand = Random.Range(1, MaxLevelNumber);
            }
            else
            {
                PlayerPrefs.SetInt("LastRandomLevel", rand);
            }
            SceneManager.LoadScene("Level" + rand);
        }
        else
        {
            SceneManager.LoadScene("Level" + currentLevel);
        }
    }

    public void TapToTryAgainButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VibrateButtonClick()
    {
        if (PlayerPrefs.GetInt("VIBRATION").Equals(1))
        {//Vibration is on
            PlayerPrefs.SetInt("VIBRATION", 0);
            VibrationButton.GetComponent<Image>().sprite = off;
        }
        else
        {//Vibration is off
            PlayerPrefs.SetInt("VIBRATION", 1);
            VibrationButton.GetComponent<Image>().sprite = on;
        }

        //if (PlayerPrefs.GetInt("VIBRATION") == 1)
        //    TapticManager.Impact(ImpactFeedback.Light);
    }

    public void TapToStartButtonClick()
    {
        isGameStarted = true;
    }
}
