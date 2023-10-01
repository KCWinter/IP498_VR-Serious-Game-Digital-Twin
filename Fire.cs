using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField, Range(0f,1f)]  private float currentIntensity = 1.0f;
    private float startIntensity = 5.0f;

    [SerializeField] private ParticleSystem firePS = null;

    private void Start()
    {
        startIntensity = firePS.emission.rateOverTime.constant;
    }

    private void Update()
    {
        //ChangeIntensity();
    }

    public bool TryExtinguish(float amount)
    {
        currentIntensity -= amount;

        ChangeIntensity();

        return currentIntensity <= 0; //fire is out
    }

    private void ChangeIntensity()
    {
        var emission = firePS.emission;
        emission.rateOverTime = currentIntensity * startIntensity;
    }

}

