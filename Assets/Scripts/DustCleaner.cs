using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustCleaner : MonoBehaviour
{
    public GameObject dustyObject;
    [Range(1,50)]public int brushSize = 40;
    Material material;
    Texture3D dustTexture;

    // Start is called before the first frame update
    void Start()
    {
        material = dustyObject.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        Texture2D tex;
        RaycastHit hit;
        if (Input.touches.Length == 1)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            if (Physics.Raycast(ray, out hit, 1000.0f, 1024))
            {
                tex = (Texture2D) material.GetTexture("_MainTex");
                Color[] colors = new Color[brushSize* brushSize];
                for (int i = 0; i < colors.Length; i++)
                {
                    colors[0] = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                }

                Vector2 pixels = hit.textureCoord;
                pixels.x *= tex.width;
                pixels.y *= tex.height;
                Debug.Log(pixels);
                tex.SetPixels((int)pixels.x, (int)pixels.y, brushSize, brushSize, colors);
                tex.Apply();
                material.SetTexture("_MainTex", tex);
                
            }
        }
        if (Input.touches.Length == 2)
        {
            Vector2 avgInput = (Input.GetTouch(0).deltaPosition + Input.GetTouch(1).deltaPosition) / 2;
            avgInput = new Vector2(avgInput.y, -avgInput.x);
            dustyObject.transform.Rotate(avgInput, Space.World);
        }
    }
}
