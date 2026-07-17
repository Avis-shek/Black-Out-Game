using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class Light : MonoBehaviour
{
    [Header("Light Flicker Settings")]
    [SerializeField] private Light2D lamp;
    [SerializeField] private float baseIntensity = 1.2f;
    [SerializeField] private float flickerAmount = 0.05f;
    [SerializeField] private float flickerSpeed = 2f;

    [Header("Static Light Settings")]
    [SerializeField] private float lightRadius = 5f;
    [SerializeField] private Color lightColor = new Color(1f, 0.6f, 0.2f);

    private float flickerTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (lamp == null)
        {
            lamp = GetComponent<Light2D>();
        }

        lamp.pointLightOuterRadius = lightRadius;
        lamp.color = lightColor;

    }

    // Update is called once per frame
    void Update()
    {
        if(lamp != null)
        {
            // give the light a natural flicker
            flickerTimer += Time.deltaTime * flickerSpeed;

            // the actual sine flicker for the lamp
            float sineFlicker = Mathf.Sin(flickerTimer) * flickerAmount;
            float subtleNoise = (UnityEngine.Random.value - 0.5f) * flickerAmount * 0.3f;
            // ** later on we'll increase the flickering when battery is about to die **

            // actually set the intensity now. This is how the flicker will be implemented
            lamp.intensity = baseIntensity + sineFlicker + subtleNoise;


        }
    }
}
