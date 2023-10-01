using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

public class mqttSender : MonoBehaviour
{
    private MqttClient client;
    private string brokerAddress = "test.mosquitto.org"; // Replace with your MQTT broker address
    private string topic = "T_i/ptu/pos"; // Replace with your desired MQTT topic


    private void Start()
    {
        // Create an MQTT client instance
        client = new MqttClient(brokerAddress);

        // Register to the published message event
        client.MqttMsgPublished += Client_MqttMsgPublished;

        // Connect to the MQTT broker
        string clientId = "UnitySender_" + Random.Range(1, 1000); // Generate a random client ID
        client.Connect(clientId);

        // Publish a message (replace with your message)
        //string message = "Hello, MQTT!";
        //client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
    }

    private void OnDestroy()
    {
        // Disconnect from the MQTT broker when the script is destroyed
        if (client.IsConnected)
        {
            client.Disconnect();
        }
    }

    private void Client_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
    {
        // Handle published message confirmation here
        Debug.Log("Message Published");
    }

    public void SendMessageOnButtonClick()
    {
        // Get the message from the InputField
        string message = "{\"pan\": 1.0, \"tilt\": -0.31764706969261169, \"ts\": \"2023-07-31T09:18:23.766Z\"}";

        // Publish the message to the MQTT topic
        client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
    }
}


