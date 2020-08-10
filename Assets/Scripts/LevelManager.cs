using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class LevelManager : MonoBehaviour
{
    public static List<LevelManager> levels = new List<LevelManager>();
    public EndGamePanel endGamePanel;

    DustCleaner dustCleaner;
    public int winThreshold = 100000;
    public int twoStarsThresholdTime = 200;
    public int threeStarsThresholdTime = 50;

    Texture2D dustTexture;
    Timer timer;
    PlayerStats stats;

    public float FinishTime { get; set; }
    public float CurrentFinishTime { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        stats = FindObjectOfType<PlayerStats>();
        AnalyticsEvent.Custom("Level Stats", new Dictionary<string, object>
        {
            { SceneManager.GetActiveScene().name + "_wins", stats.wins },
            { SceneManager.GetActiveScene().name + "_losses", stats.loses },
            { SceneManager.GetActiveScene().name + "_resets", stats.tries }
        });
        Debug.Log(stats.tries);
        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "_twoStarsThresholdTime", twoStarsThresholdTime);
        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "_threeStarsThresholdTime", threeStarsThresholdTime);
        FinishTime = float.PositiveInfinity;
        dustCleaner = FindObjectOfType<DustCleaner>();
        dustTexture = (Texture2D) dustCleaner.dustyObject.GetComponent<Renderer>().material.GetTexture(dustCleaner.textureName);
        timer = FindObjectOfType<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dustCleaner.GetCleanedPixels() > winThreshold ) WonLevel();
        if (dustCleaner.battery < 0.01f) LostLevel();
    }

    void WonLevel()
    {
        stats.wins++;

        endGamePanel.gameObject.SetActive(true);
        CurrentFinishTime = timer.time;
        if (CurrentFinishTime < FinishTime) FinishTime = CurrentFinishTime;
        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "_FinishTime", FinishTime);

        endGamePanel.WinMessage();

    }

    void LostLevel()
    {
        stats.loses++;

        endGamePanel.gameObject.SetActive(true);
        endGamePanel.LoseMessage();
    }

    private void Reset()
    {
        stats.tries++;
    }
}
