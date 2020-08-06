using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    Camera cam;
    public GameObject dustyObject;
    void Start()
    {
        cam = GetComponent<Camera>();    
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TurnX(float amount)
    {
        dustyObject.transform.Rotate(new Vector3(amount, 0, 0), Space.World);
    }

    public void TurnY(float amount)
    {
        dustyObject.transform.Rotate(new Vector3(0, amount, 0), Space.World);
    }

    public void TurnZ(float amount)
    {
        dustyObject.transform.Rotate(new Vector3(0, 0, amount), Space.World);
    }
}
