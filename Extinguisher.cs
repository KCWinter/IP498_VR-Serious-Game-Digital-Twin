using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extinguisher : MonoBehaviour
{
    [SerializeField] private float amountExtinguishedPerSecond = 1.0f;

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("Collision Particle: " + other.gameObject.name);
        
        Fire fire = other.GetComponent<Fire>();
        if (fire != null)
        {
            fire.TryExtinguish(amountExtinguishedPerSecond * Time.deltaTime);
        }
    }
}