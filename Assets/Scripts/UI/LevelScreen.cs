using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelScreen : MonoBehaviour
{
    public List<string> levels = new List<string>();
    public GameObject LevelButtonPrefab; 
    public Sprite noStar,oneStar,twoStars,threeStars;
    LevelManager manager;
    Timer timer;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<LevelManager>();
        timer = FindObjectOfType<Timer>();
        foreach (string level in levels)
        {
            GameObject l = Instantiate(LevelButtonPrefab, FindObjectOfType<Canvas>().transform);
            l.GetComponent<Button>().onClick.AddListener(delegate { SceneManager.LoadScene(level); });
            
            if (PlayerPrefs.GetFloat(level + "_FinishTime") > 0) l.GetComponent<Image>().sprite = oneStar;
            if (PlayerPrefs.GetFloat(level + "_FinishTime") < PlayerPrefs.GetFloat(level + "_twoStarsThresholdTime")) l.GetComponent<Image>().sprite = twoStars;
            if (PlayerPrefs.GetFloat(level + "_FinishTime") < PlayerPrefs.GetFloat(level + "_threeStarsThresholdTime")) l.GetComponent<Image>().sprite = threeStars;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
