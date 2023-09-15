using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOn : MonoBehaviour
{
    Light light;

    private void Start()
    {
        light = GetComponentInChildren<Light>();
    }

    public void LightSet()
    {
        light.range = 10;
    }
}
