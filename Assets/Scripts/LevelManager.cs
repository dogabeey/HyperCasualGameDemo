using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public float FinishTime { get; set; }
    public float CurrentFinishTime { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {

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
        endGamePanel.gameObject.SetActive(true);
        CurrentFinishTime = timer.time;
        if (CurrentFinishTime < FinishTime ) FinishTime = CurrentFinishTime;
        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "_FinishTime", FinishTime);

        endGamePanel.WinMessage();
        
    }
    void LostLevel()
    {
        endGamePanel.gameObject.SetActive(true);
        endGamePanel.LoseMessage();
    }
}
