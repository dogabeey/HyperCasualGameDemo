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
    public float waitBeforeEndScreen = 3;
    public ParticleSystem confetti;

    Texture2D dustTexture;
    Timer timer;
    PlayerStats stats;
    bool isEnded = false;

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
        if (dustCleaner.GetCleanedPixels() > winThreshold && !isEnded ) StartCoroutine(WonLevel());
        if (dustCleaner.battery < 0.01f && !isEnded) StartCoroutine(LostLevel());
    }

    IEnumerator WonLevel()
    {
        Debug.Log("Level is won");
        isEnded = true;
        stats.wins++;
        dustCleaner.dustyObject.GetComponent<Renderer>().material.SetTexture(dustCleaner.textureName, null);
        dustCleaner.gameObject.SetActive(false);

        StartCoroutine(PopConfetti());
        yield return new WaitForSeconds(waitBeforeEndScreen);

        endGamePanel.gameObject.SetActive(true);
        CurrentFinishTime = timer.time;
        if (CurrentFinishTime < FinishTime) FinishTime = CurrentFinishTime;
        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "_FinishTime", FinishTime);

        endGamePanel.WinMessage(twoStarsThresholdTime,threeStarsThresholdTime);

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("in-game"))
        {
            obj.SetActive(false);
        }

    }

    IEnumerator PopConfetti()
    {
        ParticleSystem c = Instantiate(confetti);
        ParticleSystem.MainModule main = c.main;
        main.duration = waitBeforeEndScreen;
        c.Play();
        yield return new WaitForSeconds(0);
    }

    IEnumerator LostLevel()
    {
        Debug.Log("Level is lost");
        isEnded = true;
        stats.loses++;
        dustCleaner.gameObject.SetActive(false);
        yield return new WaitForSeconds(2.5f);

        endGamePanel.gameObject.SetActive(true);
        endGamePanel.LoseMessage();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("in-game"))
        {
            obj.SetActive(false);
        }
    }

    private void Reset()
    {
        stats.tries++;
    }
}
