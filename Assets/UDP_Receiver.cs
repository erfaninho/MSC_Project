using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class UDP_Receiver : MonoBehaviour
{
    private Thread receiveThread;
    private UdpClient udpClient;
    private const int listenPort = 8899;
    private string latestMessage = "";
    private bool running = false;


    float originalHeight = 0f; 
    void Start()
    {
        StartReceiver();

        originalHeight = transform.position.y; 

    }

    void Update()
    {
        if (!string.IsNullOrEmpty(latestMessage))
        {
            string[] parts = latestMessage.Split(',');
            if (parts.Length >= 4)
            {
                float vel = float.Parse(parts[0])/100;
                float xPos = float.Parse(parts[1])/100;
                float yPos = float.Parse(parts[2])/100;
                float force = float.Parse(parts[3]) / 1000;

                Debug.Log($"Received message: velocity: {vel}, X position: {xPos}, Y position: {yPos}, Force: {force}");
                transform.position = new Vector3(transform.position.x, originalHeight - force, transform.position.z); //new Vector3(xPos, 0.0f, yPos);
            }
            else
            {
                Debug.Log("Invalid message format.");
            }

            latestMessage = ""; // Clear to avoid re-printing
        }
    }

    void StartReceiver()
    {
        udpClient = new UdpClient(listenPort);
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        running = true;
        receiveThread.Start();
    }

    private void ReceiveData()
    {
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, listenPort);
        try
        {
            while (running)
            {
                byte[] data = udpClient.Receive(ref remoteEndPoint);
                string message = Encoding.UTF8.GetString(data);
                latestMessage = message; // Thread-safe since string is immutable
            }
        }
        catch (SocketException ex)
        {
            Debug.Log("Socket closed or error: " + ex.Message);
        }
    }

    void OnApplicationQuit()
    {
        running = false;
        if (udpClient != null)
        {
            udpClient.Close();
        }
        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Join();
        }
    }
}
