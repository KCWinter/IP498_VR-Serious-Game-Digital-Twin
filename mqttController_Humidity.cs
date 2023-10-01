using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class mqttController_Humidity : MonoBehaviour
{
    public string nameController = "Controller 1";

    public mqttReceiver _eventSender;

    [System.Serializable]
    public class MessageData
    {
        public string ts;
        public string t;
        public string rt;
        public string h;
        public string rh;
        public string p;
        public string iaq;
        public string aq;
        public string gr;
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
            string extractedrh = messageData.rh;
            this.GetComponent<TextMeshPro>().text = extractedrh;
            Debug.Log("Event Fired. The message, from Object " + nameController + " is = " + newMsg);
        }
        else
        {
            Debug.LogWarning("Invalid JSON format received: " + newMsg);
        }
    }
}
