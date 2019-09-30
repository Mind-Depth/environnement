using UnityEngine;
using System.IO.Pipes;
using UnityEngine.Assertions;
using System;
using System.Linq;

public class PipeObject
{
    static System.Text.Encoding encoding = System.Text.Encoding.ASCII;

    NamedPipeClientStream pipe_in;
    NamedPipeClientStream pipe_out;

    public PipeObject(string name)
    {
        pipe_in = new NamedPipeClientStream(".", name + "_" + Manager.configuration.connection.server_to_client, PipeDirection.In);
        pipe_out = new NamedPipeClientStream(".", name + "_" + Manager.configuration.connection.client_to_server, PipeDirection.Out);
    }

    public void Connect()
    {
        pipe_in.Connect();
        Assert.IsTrue(pipe_in.CanRead);
        pipe_out.Connect();
        Assert.IsTrue(pipe_out.CanWrite);
    }

    public void Close()
    {
        pipe_in.Close();
        pipe_out.Close();
    }

    public bool Read(out string str)
    {
        byte[] bytes = new byte[Manager.configuration.connection.chunk_size];
        if (pipe_in.Read(bytes, 0, Manager.configuration.connection.chunk_size) < 1)
        {
            str = "";
            return false;
        }
        str = encoding.GetString(bytes);
        Debug.Log("Recv : " + str);
        return true;
    }

    public void Write(string str)
    {
        byte[] bytes = encoding.GetBytes(str);
        pipe_out.Write(bytes, 0, bytes.Length);
        Debug.Log("Send : " + str);
    }
}

public class Client
{
    bool read;
    AThreadJob reader;
    public PipeObject pipeClient;

    public void Start()
    {
        pipeClient = new PipeObject(Manager.configuration.connection.environment);
        Debug.Log("Attempting to connect to pipe...");
        pipeClient.Connect();
        Debug.Log("Pipe connected");
        reader = new Reader();
        reader.Start();
        read = true;
    }

    public void SendMessage(EnvironmentMessage msg) {
        pipeClient.Write(JsonUtility.ToJson(msg));
    }

    public void Update()
    {
        while (Manager._instance.queue_watchers_data.Count > 0)
            SendMessage(Manager._instance.queue_watchers_data.Dequeue());
        if (read)
            read = !reader.Update();
    }

    void OnApplicationQuit()
    {
        /* Tuer le thread avant de quitter */
        reader.Abort();
    }
}

public class Reader : AThreadJob {

    protected override void ThreadFunction()
    {
        ref PipeObject pipe = ref Manager._instance.generation_client.pipeClient;
  
        string str;
        while (pipe.Read(out str))
        {
            foreach (string asset_data in str.Split(new string[] { "\r\n" }, StringSplitOptions.None))
            {
                char c = asset_data[0];
                if (c != 0)
                {
                    Assert.AreEqual(c, '{');
                    GenerationMessage msg = JsonUtility.FromJson<GenerationMessage>(asset_data);
                    Manager._instance.queue_data.Enqueue(msg);
                }
            }
        }
    }

    protected override void OnFinished()
    {
        Manager._instance.generation_client.pipeClient.Close();
        Debug.Log("Pipe reader terminated");
    }
}