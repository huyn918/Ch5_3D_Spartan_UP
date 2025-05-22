using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyRotator : MonoBehaviour
{
    public float rotationSpeed = 1.0f;

    public void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed);
    }
}
