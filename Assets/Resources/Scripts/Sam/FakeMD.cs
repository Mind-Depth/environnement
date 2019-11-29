using System.Collections.Generic;
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
        public enum GAME_STATES : int { FIRST_ROOM, SECOND_ROOM, NORMAL_GAME, NO_ROOM };
        public GAME_STATES currentRoom = GAME_STATES.NO_ROOM;
        private GAME_STATES oldRoom = GAME_STATES.NO_ROOM;
        public enum TRIGGERED_EVENT : int { NO_EVENT, LEVER };
        public TRIGGERED_EVENT triggeredEvent = TRIGGERED_EVENT.NO_EVENT;
        private TRIGGERED_EVENT oldTriggeredEvent = TRIGGERED_EVENT.NO_EVENT;



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
                if (oldRoom != currentRoom)
                {
                    if (currentRoom == GAME_STATES.FIRST_ROOM)
                    {
                        oldRoom = currentRoom;
                    }
                    else if (currentRoom == GAME_STATES.SECOND_ROOM)
                    {
                        oldRoom = currentRoom;
                    }
                    else if (currentRoom == GAME_STATES.NORMAL_GAME)
                    {
                        //UpdateRandomRoomConfig();
                        RandomTags(out List<SamTags> tags, random.Next(5, 10));
                        TriggerManager._instance.UpdateRoomConfig(tags, Fear.Nyctophobia, (float)random.NextDouble());
                        oldRoom = currentRoom;
                    }
                }
                if (oldTriggeredEvent != triggeredEvent)
                {
                    TriggerManager._instance.UpdateTriggerEvents();
                    oldTriggeredEvent = triggeredEvent;
                }
                player.ComputeFearLevel();

                int fearLevel = player.GetCurrentFearLevel();
                TriggerManager._instance.UpdateFear((float)fearLevel);
                nextTime += interval;
            }
        }
    }
}
