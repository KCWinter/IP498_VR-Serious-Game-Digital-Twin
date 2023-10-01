using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class mqttController_Environment : MonoBehaviour
{
    public string nameController = "Controller 1";

    public mqttReceiver _eventSender;

    [System.Serializable]
    public class MessageData
    {
        public string ts;
        public string br;
        public int ldr;
    }

    void Start()
    {
        _eventSender.OnMessageArrived += OnMessageArrivedHandler;
    }

    private void OnMessageArrivedHandler(string newMsg)
    {
        MessageData messageData = JsonUtility.FromJson<MessageData>(newMsg);

        if (messageData != null)
        {
            string extractedbr = messageData.br;
            this.GetComponent<TextMeshPro>().text = extractedbr;
            Debug.Log("Event Fired. The message, from Object " + nameController + " is = " + newMsg);
        }
        else
        {
            Debug.LogWarning("Invalid JSON format received: " + newMsg);
        }
    }
}
