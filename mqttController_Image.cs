using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mqttController_Image : MonoBehaviour
{
    public string nameController = "Controller 1";

    public mqttReceiver _eventSender;

    [System.Serializable]
    public class MessageData
    {
        public string ts;
        public string data; 
    }

    void Start()
    {
        _eventSender.OnMessageArrived += OnMessageArrivedHandler;
    }

    private void OnMessageArrivedHandler(string newMsg)
    {
        MessageData messageData = JsonUtility.FromJson<MessageData>(newMsg);

            // Check if the message contains image data
            if (!string.IsNullOrEmpty(messageData.data))
            {
                DisplayImage(messageData.data);
            }
            else
            {
                Debug.LogWarning("Invalid JSON format received: " + newMsg);
            }
    }

    private void DisplayImage(string base64Image)
    {
        Texture2D texture = LoadTextureFromBase64(base64Image);
        // Assign the texture to your image display component
    }

    private Texture2D LoadTextureFromBase64(string base64)
    {
        byte[] imageData = System.Convert.FromBase64String(base64);
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(imageData);
        return texture;
    }
}

