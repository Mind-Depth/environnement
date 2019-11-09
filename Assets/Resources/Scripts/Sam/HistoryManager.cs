using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

namespace Sam
{
    public class HistoryManager : MonoBehaviour
    {
        private List<History> histories = new List<History>();

        // Return a number between 0 and the variable max
        public int RandomNumber(int max)
        { return UnityEngine.Random.Range(0, max); }

        public void AddHistoryPoint(string currentRoom, List<string> currentFears, float startTime, int fearLevel)
        {
            History history = new History();

            history.fearLevelDelta = fearLevel;
            history.currentRoom = currentRoom;
            history.currentFears = currentFears;
            history.timeSpend = Time.time;

            histories.Add(history);
        }

        public List<History> GetHistories()
        {
            return this.histories;
        }

        public void PrintHistory()
        {
            Debug.Log("---- Print History ----");
            for (int i = 0; i < histories.Count; ++i)
            {
                Debug.Log("fearLevelDelta : " + histories[i].fearLevelDelta + " | currentRoom : " + histories[i].currentRoom + " | timeSpend : " + histories[i].timeSpend);
                for (int a = 0; a < histories[i].currentFears.Count; ++a)
                {
                    Debug.Log("Current Fears == " + histories[i].currentFears[a]);
                }
            }
            Debug.Log("---- {{ END }} ----");
        }
    }
}
