﻿using System.Collections.Generic;
using Random = System.Random;
using UnityEngine;

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

        public SamTags[] allTags;
        static Random random = new Random();
        void RandomTags(out List<SamTags> tags, int count)
        {
            tags = new List<SamTags>();
            for (int i = 0; i < count; ++i)
                tags.Add(allTags[random.Next(allTags.Length)]);
        }

        static T GetRandomEnum<T>()
        {
            System.Array A = System.Enum.GetValues(typeof(T));
            T V = (T)A.GetValue(UnityEngine.Random.Range(0, A.Length));
            return V;
        }

        void UpdateRandomRoomConfig()
        {
            RandomTags(out List<SamTags> tags, random.Next(5, 10));
            TriggerManager._instance.UpdateRoomConfig(tags, GetRandomEnum<Fear>(), (float)random.NextDouble());
        }

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
