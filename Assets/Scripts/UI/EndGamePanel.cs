using System;
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
    
    Timer timer;
    Color starColor = Color.yellow;

    // Start is called before the first frame update
    void Start()
    {
        timer = FindObjectOfType<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WinMessage(int twoST, int threeST)
    {
        string levelName = SceneManager.GetActiveScene().name;
        string[] levelNames = levelName.Split('-');
        int currentLevel = Convert.ToInt32(levelNames[1]);
        currentLevel++;
        cont.onClick.AddListener(delegate { SceneManager.LoadScene(levelNames[0] + "-" + currentLevel.ToString()); });
        Debug.Log(levelNames[0] + currentLevel.ToString());

        endMessage.text = "Nice Work!";
        star1.color = starColor;
        if (PlayerPrefs.GetFloat(levelName + "_FinishTime") < twoST) star2.color = starColor;
        if (PlayerPrefs.GetFloat(levelName + "_FinishTime") < threeST) star3.color = starColor;

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
