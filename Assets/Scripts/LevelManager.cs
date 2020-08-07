using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public DustCleaner dustCleaner;
    public int winThreshold = 100000;
    Texture2D dustTexture;

    
    // Start is called before the first frame update
    void Start()
    {
        dustTexture = (Texture2D) dustCleaner.dustyObject.GetComponent<Renderer>().material.GetTexture(dustCleaner.textureName);
    }

    // Update is called once per frame
    void Update()
    {
        if (dustCleaner.GetCleanedPixels() > winThreshold ) WonLevel();
        if (dustCleaner.battery < 0.01f) LostLevel();
    }

    void WonLevel()
    {
        Debug.Log("YOU WIN");
    }
    void LostLevel()
    {
        Debug.Log("YOU LOST");
    }
}
