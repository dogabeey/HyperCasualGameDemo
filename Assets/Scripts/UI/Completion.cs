using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Completion : MonoBehaviour
{
    DustCleaner cleaner;
    public LevelManager manager;
    // Start is called before the first frame update
    void Start()
    {
        cleaner = FindObjectOfType<DustCleaner>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = ((cleaner.GetCleanedPixels() / manager.winThreshold) * 100).ToString(".##") + "%";
    }
}
