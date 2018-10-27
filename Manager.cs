using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Classe principale instanciée au lancement du programme.  
 * Elle instancie les différents threads utilisés par l'Intelligence artificielle.
 * Elle instancie également la partie Orchestration & Queue de communication.
 */
public class Manager : MonoBehaviour {
    List<AThreadJob> jobs;
    Orchestration orchestration;
    public MutexedQueue<QueueData> items_queue;
    public static Manager _instance = null;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        } else if (_instance != this)
        {
            Destroy(this);
        }
    }

    void Start()
    {
        items_queue = new MutexedQueue<QueueData>();
        jobs = new List<AThreadJob> {
            new Profiling()
        };

        /* Lancement de toutes les tâches utilisées par l'IA. */
        foreach (var job in jobs)
        {
            job.Start();
        }

        /* Lancement de la partie l'orchestration */
        orchestration = new Orchestration();
    }

    void Update()
    {
        /* Vérification de la fin des tâches afin d'appeler leur méthode "OnFinished" */
        foreach (var job in jobs)
            job.Update();
        orchestration.Update();       
    }
}
