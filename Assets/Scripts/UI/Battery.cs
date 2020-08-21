using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour
{
    DustCleaner cleaner;
    Image batteryIndicator;
    float maxCharge;
    Text batteryText;
    // Start is called before the first frame update
    void Start()
    {
        cleaner = FindObjectOfType<DustCleaner>();
        maxCharge = cleaner.battery;
        batteryIndicator = GetComponentsInChildren<Image>()[2];
    }

    // Update is called once per frame
    void Update()
    {
        batteryIndicator.fillAmount = cleaner.battery / maxCharge;
        batteryIndicator.color = new Color(1- batteryIndicator.fillAmount, batteryIndicator.fillAmount, batteryIndicator.color.b);
    }
}
