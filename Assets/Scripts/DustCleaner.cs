using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
[RequireComponent(typeof(ParticleSystem))]
public class DustCleaner : MonoBehaviour
{
    public GameObject dustyObject;
    [Range(1, 20)] public int brushSize = 5;
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
    Vector3 mouseDelta;

    ParticleSystem particle;
    public List<ParticleCollisionEvent> collisionEvents;

    int totalChangedPixels;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        material = dustyObject.GetComponent<Renderer>().material;
        originalTex = (Texture2D)material.GetTexture(textureName);
        originalTexPixel = originalTex.GetPixels().Length;
        tex = new Texture2D(originalTex.width, originalTex.height, originalTex.format, false);
        tex.SetPixels(originalTex.GetPixels());
        mouseDelta = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Touch touch;

        if (Input.touches.Length > 0)
        {
            touch = Input.GetTouch(0);
            mouseDelta = Input.mousePosition - mouseDelta;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000, 768))
            {
                transform.LookAt(hit.point);
                if (!particle.isPlaying) particle.Play();
            }
            else
            {
                if (particle.isPlaying) particle.Stop();
            }
        }
        else
        {
            if (particle.isPlaying) particle.Stop();
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

    }

    private void ChangeBattery(float amount)
    {
        battery = battery + amount;
    }

    int AddPixelGroup(Texture2D texture, int x, int y, Color color, int brushSize)
    {
        int changedPixels = 0;
        for (int i = x - brushSize; i < x + brushSize; i++)
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
                if (Mathf.Pow(x - i, 2) + Mathf.Pow(y - j, 2) < Mathf.Pow(brushSize, 2))
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

    private void OnParticleCollision(GameObject other)
    {
        ParticleSystem.MainModule main = particle.main;
        int numCollisionEvents = particle.GetCollisionEvents(other, collisionEvents);
        int i = 0;

        while (i < numCollisionEvents)
        {
            Vector3 pos = collisionEvents[i].intersection;
            Ray ray = new Ray(transform.position, pos - transform.position);
            Debug.DrawRay(transform.position, pos - transform.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000.0f, 256) && other == dustyObject)
            {
                CleanDust(hit);
                ChangeBattery(-costPerVacuum);
            }
            i++;
        }
    }
}
