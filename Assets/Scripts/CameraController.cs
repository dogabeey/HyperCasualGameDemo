using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Joystick joystick;
    DustCleaner cleaner;

    public float scrollSensitivity;

    // Start is called before the first frame update
    void Start()
    {
        joystick = FindObjectOfType<FixedJoystick>();
        cleaner = FindObjectOfType<DustCleaner>();
    }

    // Update is called once per frame
    void Update()
    {
        Transform t = cleaner.dustyObject.transform;
        t.Rotate(new Vector3(joystick.Vertical, -joystick.Horizontal) * scrollSensitivity,Space.World);
    }
}
