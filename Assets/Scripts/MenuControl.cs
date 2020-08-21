using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor) PlayerPrefs.DeleteAll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
