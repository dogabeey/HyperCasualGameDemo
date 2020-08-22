using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelScreen : MonoBehaviour
{
    public List<string> levels = new List<string>();
    public GameObject LevelButtonPrefab; 
    public Image noStar,oneStar,twoStars,threeStars;
    LevelManager manager;
    Timer timer;
    // Start is called before the first frame update
    void Start()
    {
        int counter = 0;
        manager = FindObjectOfType<LevelManager>();
        timer = FindObjectOfType<Timer>();
        foreach (string level in levels)
        {
            counter++;
            GameObject l = Instantiate(LevelButtonPrefab, GameObject.FindGameObjectWithTag("panel").transform);
            l.GetComponent<Button>().onClick.AddListener(delegate { SceneManager.LoadScene(level); });
            l.GetComponentInChildren<Text>().text = counter.ToString();

            if (PlayerPrefs.GetFloat(level + "_FinishTime") > 0) {
                l.GetComponentsInChildren<Image>()[1].color = Color.yellow;
                l.GetComponentsInChildren<Text>()[1].text = PlayerPrefs.GetFloat(level + "_FinishTime").ToString("00.##").Replace(',', ':');
            }
            if (PlayerPrefs.GetFloat(level + "_FinishTime") < PlayerPrefs.GetFloat(level + "_twoStarsThresholdTime")) { l.GetComponentsInChildren<Image>()[2].color = Color.yellow;  }
            if (PlayerPrefs.GetFloat(level + "_FinishTime") < PlayerPrefs.GetFloat(level + "_threeStarsThresholdTime")) { l.GetComponentsInChildren<Image>()[3].color = Color.yellow; }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
