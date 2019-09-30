using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Queue principale.
 * Utilisée pour la communication entre le manager et les différentes tâches.
 * Utilise une mutex pour sécuriser l'utilsation de la queue par plusieurs threads.
 */ 
public class MutexedQueue<T>
{
    Queue<T> q = new Queue<T>();
    public int Count = 0;
    private readonly object obj = new object();
    public T Dequeue()
    {
        T t;
        lock (obj)
        {
            t = q.Dequeue();
            Count -= 1;
        }
        return t;
    }
    public void Enqueue(T t)
    {
        lock (obj)
        {
            q.Enqueue(t);
            Count += 1;
        }
    }
}
