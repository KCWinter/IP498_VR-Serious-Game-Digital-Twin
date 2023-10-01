using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void DES()
    {
        SceneManager.LoadScene("Discrete Event Simulation");
    }
    
    public void SBT()
    {
       SceneManager.LoadScene("Safety and Breakdown Training");
    }

    public void DT()
    {
        SceneManager.LoadScene("Digital Twin");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player has quit");
    }

 
}
