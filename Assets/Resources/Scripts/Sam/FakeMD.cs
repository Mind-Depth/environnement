using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sam
{
    public class Player
    {
        
        private int currentFearLevel = 0;
        private int currentMentalState = 0;

        int RandomNumber(int max)
        { return UnityEngine.Random.Range(0, max); }

        public void ComputeFearLevel()
        { 
            GenerateCurrentMentalState();
            if (GetCurrentMentalState() == 0) {
                currentFearLevel = RandomNumber(100) % 2;
            }
        }

        void GenerateCurrentMentalState()
        {
           currentMentalState = RandomNumber(100) % 2;
        }

        public int GetCurrentFearLevel()
        {
            return currentFearLevel;
        }

        int GetCurrentMentalState()
        {
            return currentMentalState;
        }
    }

    public class FakeMD : MonoBehaviour
    {
        public float interval = 1;
        private float nextTime = 0;
        private Player player = new Player();

        int RandomNumber(int max)
        { return UnityEngine.Random.Range(0, max); }

        void Update()
        {
            if (Time.time >= nextTime)
            {
                interval = 10;
                player.ComputeFearLevel();
                int fearLevel = player.GetCurrentFearLevel();
                TriggerManager._instance.UpdateFear((float)fearLevel);
                nextTime += interval;
            }
        }
    }
}
