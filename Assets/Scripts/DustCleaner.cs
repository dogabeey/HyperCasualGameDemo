using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class DustCleaner : MonoBehaviour
{
    public GameObject dustyObject;
    [Range(1,50)]public int brushSize = 40;
    [Range(1,10)]public int smoothness = 1;
    public string textureName;
    public float zoomSensitivity = 30;
    public GameObject dust;
    public float battery = 100;
    public float costPerVacuum = 0.1f;
    Texture2D tex;

    Material material;
    Vector2 prevTouchDiff = Vector2.zero;
    public Texture2D originalTex;
    float originalTexPixel;

    int totalChangedPixels;

    // Start is called before the first frame update
    void Start()
    {
        material = dustyObject.GetComponent<Renderer>().material;
        originalTex = (Texture2D) material.GetTexture(textureName);
        originalTexPixel = originalTex.GetPixels().Length;
        tex = new Texture2D(originalTex.width, originalTex.height, originalTex.format, false);
        tex.SetPixels(originalTex.GetPixels());

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector2 delta0, delta1;
        Touch touchZero;
        Touch touchOne;

        if (Input.touches.Length == 1)
        {
            touchZero = Input.GetTouch(0);
            ChangeBattery(-costPerVacuum);
            Ray ray = Camera.main.ScreenPointToRay(touchZero.position);

            if (Physics.Raycast(ray, out hit, 1000.0f, 1024))
            {
                CleanDust(hit);
            }
        }
        if (Input.touches.Length == 2)
        {
            touchZero = Input.GetTouch(0);
            touchOne = Input.GetTouch(1);
            Vector2 avgInput = ((delta0 = touchZero.deltaPosition) + (delta1 = touchOne.deltaPosition)) / 2;
            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            if (deltaMagnitudeDiff > 2 || deltaMagnitudeDiff < -2) Camera.main.fieldOfView += (deltaMagnitudeDiff / zoomSensitivity);
            else
            {
                avgInput = new Vector2(avgInput.y, -avgInput.x);
                dustyObject.transform.Rotate(avgInput, Space.World);
            }
        }
    }

    private void CleanDust(RaycastHit hit)
    {
        Vector3 colPoint = hit.point;
        Vector2 pixels = hit.textureCoord;

        pixels.x *= tex.width;
        pixels.y *= tex.height;
        int cleanedColorCount = AddPixelGroup(tex, (int)pixels.x, (int)pixels.y, new Color(-1.0f, -1.0f, -1.0f, 1.0f), brushSize);
        CreateDust(colPoint, cleanedColorCount);

        tex.Apply();
        material.SetTexture(textureName, tex);
        originalTex = tex;
    }

    private void CreateDust(Vector3 colPoint, int cleanedColorCount)
    {
        Instantiate(dust, colPoint, Quaternion.identity);
        ParticleSystem.MainModule p = dust.GetComponent<ParticleSystem>().main;
        p.maxParticles = cleanedColorCount / 2;
    }

    private void ChangeBattery(float amount)
    {
        battery = battery + amount;
    }

    int AddPixelGroup(Texture2D texture, int x, int y, Color color, int brushSize)
    {
        int changedPixels = 0;
        for(int i = x - brushSize; i < x + brushSize;i++)
        {
            for (int j = y - brushSize; j < y + brushSize; j++)
            {
                Color c;
                
                c = texture.GetPixel(i, j);
                if (c.a != 0.0f)
                {
                    changedPixels++;
                    totalChangedPixels++;
                }
                if (Mathf.Pow(x-i,2)+ Mathf.Pow(y - j, 2) < Mathf.Pow(brushSize, 2))
                texture.SetPixel(i, j, c - color);
            }
        }
        return changedPixels;
    }

    public float GetCompletionRate()
    {
        return totalChangedPixels / originalTexPixel;
    }
    public float GetTextureSize()
    {
        return originalTexPixel;
    }
    public float GetCleanedPixels()
    {
        return totalChangedPixels;
    }
}
