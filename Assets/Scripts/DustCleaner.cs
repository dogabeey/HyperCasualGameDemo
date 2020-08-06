using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustCleaner : MonoBehaviour
{
    public GameObject dustyObject;
    [Range(1,50)]public int brushSize = 40;
    public string textureName;
    public float zoomSensitivity = 30;

    Material material;
    Texture3D dustTexture;
    Vector2 prevTouchDiff = Vector2.zero;
    Vector2 currentTouchDiff;

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
                tex = (Texture2D) material.GetTexture(textureName);

                Vector2 pixels = hit.textureCoord;
                pixels.x *= tex.width;
                pixels.y *= tex.height;
                SetPixelGroup(tex, (int)pixels.x, (int)pixels.y, new Color(-1.0f, -1.0f, -1.0f, 1.0f), brushSize);
                tex.Apply();
                material.SetTexture(textureName, tex);
                
            }
        }
        if (Input.touches.Length == 2)
        {
            Vector2 delta0, delta1;
            Vector2 avgInput = ((delta0 = Input.GetTouch(0).deltaPosition) + ( delta1 = Input.GetTouch(1).deltaPosition)) / 2;
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            Debug.Log(deltaMagnitudeDiff);

            if (deltaMagnitudeDiff > 2 || deltaMagnitudeDiff < -2) Camera.main.fieldOfView += (deltaMagnitudeDiff / zoomSensitivity);
            else
            {
                avgInput = new Vector2(avgInput.y, -avgInput.x);
                dustyObject.transform.Rotate(avgInput, Space.World);
            }
        }
    }

    void SetPixelGroup(Texture2D texture, int x, int y, Color color, int brushSize)
    {
        for(int i = x - brushSize; i < x + brushSize;i++)
        {
            for (int j = y - brushSize; j < y + brushSize; j++)
            {
                texture.SetPixel(i, j, texture.GetPixel(i,j) - color);
            }
        }
    }
}
