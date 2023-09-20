using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBrightness : MonoBehaviour
{
    Light light;
    public Color color;
    private void Start()
    {
        light = GetComponentInChildren<Light>();
        //LightSet();
    }
    void LightSet()
    {
        light.range = 10*PlayerPrefs.GetInt("Light");
        light.color = color;
    }
    public void LedLight()
    {
        light.color = Color.red;
        light.range = 10;
    }
}
