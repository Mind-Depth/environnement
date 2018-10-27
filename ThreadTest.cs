using System.Collections;
using System.Collections.Generic;
using UnityEngine;



 public class Job0 : AThreadJob
{
	public MutexedQueue<int> q;
 
     protected override void ThreadFunction()
     {
         for (int i = 0; i < 20; i++)
         {
			System.Threading.Thread.Sleep(10);
			q.Enqueue(i);
         }
     }

    protected override void OnFinished()
    {

    }
 }

 public class Job1 : AThreadJob
{
     public int[] chose = new int[10]; // arbitary job data
 
     protected override void ThreadFunction()
     {
         // Do your threaded task. DON'T use the Unity API here
         for (int i = 0; i < chose.Length * 1000000; i++)
         {
			 chose[i % chose.Length] += i;
         }
     }

     protected override void OnFinished()
     {
         for (int i = 0; i < chose.Length; i++)
         {
             Debug.Log("Results(" + i + "): " + chose[i]);
         }
     }
 }

 public class Job2 : AThreadJob
{
     protected override void ThreadFunction()
     {
		int i = 0;
		i /= (i - i);
     }

     protected override void OnFinished()
     {
		Debug.Log("After the error.");
     }
 }

