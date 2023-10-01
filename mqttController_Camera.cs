using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mqttController_Camera : MonoBehaviour
{
    public string nameController = "Controller 1";
    public string tagOfTheMQTTReceiver="";
    public mqttReceiver _eventSender;

    public Animator mqttAnimator;  // Reference to the Animator component

    void Start()
    {
        _eventSender = GameObject.FindGameObjectsWithTag(tagOfTheMQTTReceiver)[0].gameObject.GetComponent<mqttReceiver>();
        _eventSender.OnMessageArrived += OnMessageArrivedHandler;
    }

    private void OnMessageArrivedHandler(string newMsg)
    {
        MessageData messageData = JsonUtility.FromJson<MessageData>(newMsg);

        if (messageData != null)
        {
            Debug.Log("Event Fired. The message, from Object " + nameController + " is = " + messageData.cmd);

            if (messageData.cmd == "relmove_right")
            {
                Debug.Log("Received relmove_right and extracted it correctly");
                // Trigger the animation if the command is "relmove_right"
                mqttAnimator.SetTrigger("MoveRight");
            }
            if (messageData.cmd == "relmove_left")
            {
                Debug.Log("Received relmove_left and extracted it correctly");
                // Trigger the animation if the command is "relmove_left"
                mqttAnimator.SetTrigger("MoveLeft");
            }
        }
        else
        {
            Debug.LogWarning("Invalid JSON format received: " + newMsg);
        }
    }

    [System.Serializable]
    public class MessageData
    {
        public string ts;
        public string cmd;
        public int degrees;
    }
}





