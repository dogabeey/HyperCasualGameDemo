using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGamePanel : MonoBehaviour
{
    public Text endMessage;
    public Button menu, reset, cont;
    public Image star1, star2, star3;

    LevelManager manager;
    Timer timer;
    Color starColor = Color.yellow;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<LevelManager>();
        timer = FindObjectOfType<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WinMessage()
    {
        endMessage.text = "Nice Work!";
        star1.color = starColor;
        if (timer.time < manager.twoStarsThresholdTime) star2.color = starColor;
        if (timer.time < manager.threeStarsThresholdTime) star3.color = starColor;
    }

    public void LoseMessage()
    {
        endMessage.text = "Better luck next time...";
        star1.gameObject.SetActive(false);
        star2.gameObject.SetActive(false);
        star3.gameObject.SetActive(false);
        cont.interactable = false;
    }
}
