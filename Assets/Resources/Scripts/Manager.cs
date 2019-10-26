using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using Sam;
/**
* Classe principale instanciée au lancement du programme.  
* Elle instancie les différents threads utilisés par l'Intelligence artificielle.
* Elle instancie également la partie Orchestration & Queue de communication.
*/
public class Manager : MonoBehaviour
{
    public static Manager _instance = null;
    public static Configuration configuration = Configuration.Load();
    public Client generation_client;

    /* Queue for watchers data : Main -> Thread */
    public MutexedQueue<EnvironmentMessage> queue_watchers_data;

    /* Queue for assets (decision by generation) : Thread -> Main */
    public MutexedQueue<GenerationMessage> queue_data;

    /* Connexion entre génération & environnement établie. */
    private bool is_connected = false;

    /* Première pièce crée pour commencer le jeu */
    private bool is_started = false;

    /* Variable de récupération des données de la queue */
    private GenerationMessage response;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else if (_instance != this)
        {
            Destroy(this);
        }
    }

    void Start()
    {
        /* Création des communications. */
        Console._instance.AddLog( "Starting environment...");
        queue_watchers_data = new MutexedQueue<EnvironmentMessage>();
        queue_data = new MutexedQueue<GenerationMessage>();
        generation_client = new Client();
        Console._instance.AddLog("Starting client connection...");
        generation_client.Start();
    }

    /* Commence une fois que la connection avec la partie génération est établie. */
    void StartEnvironnement()
    {
        /* Lancement de la partie l'orchestration & resources */
        Console._instance.AddLog("Creation of configuration files...");
        ResourcesManager._instance.CreateFiles();

        /* Prévenir la partie génération l'état des fichiers. */
        queue_watchers_data.Enqueue(new EnvironmentMessage {type = EnvironmentMessage.Type.Initialize});
    }

    private void Init()
    {
        if (queue_data.Count < 1)
            return;
        response = queue_data.Dequeue();
        if (response.type == GenerationMessage.Type.Initialize)
        {
            Console._instance.AddLog("Connected to generation.");
            is_connected = true;
            StartEnvironnement();
        }
        if (response.type == GenerationMessage.Type.Terminate)
        {
            Console._instance.AddLog("Exit on error.");
            Application.Quit();
        }
    }

    void Update()
    {
        generation_client.Update();
        if (!is_connected)
        {
            Init();
            return;
        }
        if (queue_data.Count > 0)
        {
            response = queue_data.Dequeue();
            if (response != null)
            {
                switch (response.type)
                {
                    case GenerationMessage.Type.RoomConfiguration:
                        if (is_started)
                            Orchestration._instance.GenerateNewMap(response);
                        break;
                    case GenerationMessage.Type.FearLevel:
                        TriggerManager._instance.UpdateFear(response.fearIntensity);
                        break;
                    case GenerationMessage.Type.Quit:
                        Console._instance.AddLog("Endding game.");
                        break;
                    case GenerationMessage.Type.Start:
                        Console._instance.AddLog("Starting game.");
                        is_started = true;
                        RoomManager._instance.RequestRoom();
                        break;
                    case GenerationMessage.Type.Terminate:
                        Console._instance.AddLog("Exit.");
                        Application.Quit();
                        break;
                    default:
                        print("Unknown data.");
                        break;
                }
            }
        }
    }
}
