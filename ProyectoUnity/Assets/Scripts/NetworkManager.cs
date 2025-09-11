using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using UnityEngine;
using Newtonsoft.Json.Linq;  // Necesitas instalar JSON.NET para Unity (Newtonsoft.Json)

public class NetworkManager : MonoBehaviour
{
    public string host = "127.0.0.1";
    public int port = 65432;

    private TcpClient client;
    private StreamReader reader;
    private Thread listenerThread;

    private readonly Queue<string> messageQueue = new Queue<string>();
    private readonly object lockObject = new object();

    void Start()
    {
        try
        {
            client = new TcpClient(host, port);
            reader = new StreamReader(client.GetStream());

            listenerThread = new Thread(ListenForMessages);
            listenerThread.IsBackground = true;
            listenerThread.Start();

            Debug.Log("Conectado al servidor Python");
        }
        catch (Exception e)
        {
            Debug.LogError("Error al conectar: " + e.Message);
        }
    }

    void ListenForMessages()
    {
        try
        {
            while (true)
            {
                string line = reader.ReadLine(); // Python envía con "\n"
                if (line != null)
                {
                    lock (lockObject)
                    {
                        messageQueue.Enqueue(line);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error en lectura: " + e.Message);
        }
    }

    void Update()
    {
        // Procesar mensajes pendientes en el hilo principal
        lock (lockObject)
        {
            while (messageQueue.Count > 0)
            {
                string json = messageQueue.Dequeue();
                HandleMessage(json);
            }
        }
    }

    void HandleMessage(string json)
    {
        try
        {
            JObject data = JObject.Parse(json);

            // 🔹 Distribuir datos a cada sprite con ModelBehaviour
            foreach (var obj in FindObjectsOfType<ModelBehaviour>())
            {
                obj.UpdateData(data, obj.type);
            }

            Debug.Log("Datos recibidos paso " + data["step"]);
        }
        catch (Exception e)
        {
            Debug.LogError("Error al manejar JSON: " + e.Message);
        }
    }

    void OnApplicationQuit()
    {
        reader?.Close();
        client?.Close();
        listenerThread?.Abort();
    }
}
