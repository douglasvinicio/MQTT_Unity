using UnityEngine;
using System.Collections;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using UnityEngine.UI;

using System;

public class mqttTest : MonoBehaviour {

	private MqttClient client;

	public Text mqttID; //MQTT ID Text
	public Text mqttMessage; //MQTT Message sent

	public string ipAdress; //Broker adress
	public int brokerPort; //Port of the broker
	public string topic; //Topic to be subscribed
	public string messageMQTT; //String to hold and convert message received by the broker ( will be used with mqttMessage Text ).


	// Use this for initialization


	void Start () {

		ipAdress = "localhost"; //Insert here the ipAdress of the broker
		brokerPort = 1883; //Insert here the port (1883 is the most used.)
		topic = "Machine/"; //Insert here the topic to subscribe. 

		//Create client instance - Args/Params are (idAdress, BrokerPort, # , #)
		client = new MqttClient(IPAddress.Parse(ipAdress), brokerPort , false , null );

		/* // Create client instance 
		client = new MqttClient(IPAddress.Parse("143.185.118.233"),8080 , false , null );
		*/
		
		//Register to message received 
		client.MqttMsgPublishReceived += client_MqttMsgPublishReceived; 
		
		string clientId = Guid.NewGuid().ToString(); 
		client.Connect(clientId);
		mqttID.text = "Client ID: " + clientId; //clientID being stored as text to be shown inside Unity

		
		//Subscribe to the topic "/home/temperature" with QoS 2 
		client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

		

	}
	void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) 
	{ 
		Debug.Log("Received: " + System.Text.Encoding.UTF8.GetString(e.Message));
		messageMQTT = System.Text.Encoding.UTF8.GetString(e.Message);

	} 

	// Update is called once per frame
	void Update () {

		//Displayng message received inside Unity application / environment
		mqttMessage.text = "Received from " + topic + " " + messageMQTT;

	}
}
