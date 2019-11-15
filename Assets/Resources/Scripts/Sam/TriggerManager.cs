using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

namespace Sam
{
    public class TriggerManager : MonoBehaviour
    {
        public static TriggerManager _instance = null;

        private List<string> mood = new List<string>();
        private List<string> rooms = new List<string>();
        private List<string> currentFear = new List<string>();
        private List<History> histories = new List<History>();

        private Line samLinePrez;

        private string[]        samLines;
        private List<Line>      samLinesObject;
        public string           lang;

        private SamLinesManager samLineManager;
        private MoodManager     moodManager;
        private HistoryManager  historyManager;
        private AudioSource     audioSource;
        private string          currenMood;

        public float startTime = 0.0f;
        public float targetTime = 5.0f;

        public float startAmbianceLine = 10.0f;
        public float startPrez = 1.0f;

        public int intervalTurn = 1;
        private float nextTurn = 0.0f;
        public bool introduction = true;
        private int fearLevel = 0;

        void Awake()
        {
            // Singleton
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
            else if (_instance != this)
            {
                Destroy(this);
            }

            audioSource = GetComponent<AudioSource>();
            currentFear.Add("spider");
            currentFear.Add("nothing");

            rooms.Add("SpiderRoom");
            rooms.Add("VertigoRoom");
            rooms.Add("ClaustroRoom");

            mood.Add("happy");
            mood.Add("angry");
            mood.Add("septic");

            samLineManager = new SamLinesManager(audioSource, lang);
            samLines = samLineManager.GetSamLinesName();
            samLinesObject = samLineManager.GetSamLinesObject();
            samLinePrez = samLineManager.GetSamPrez();

            samLineManager.SetLinesByMood(samLinesObject);

            historyManager = new HistoryManager();

            moodManager = new MoodManager(50.0f);
        }

        // Return a number between 0 and the variable max
        public int RandomNumber(int max)
        { return UnityEngine.Random.Range(0, max); }

        void TimerEnded()
        {
            historyManager.AddHistoryPoint(rooms[RandomNumber(rooms.Count)], currentFear, startTime, fearLevel);
            histories = historyManager.GetHistories();

            //moodManager.ComputeMood(histories);

            this.currenMood = moodManager.GetMoodName();
            List<Line> lines = samLineManager.GetLinesByMood(this.currenMood);
            if (this.startTime > this.startAmbianceLine)
            {
                //Debug.Log("current Mood (" + moodManager.GetMoodValue() + ") = " + this.currenMood);
                int lineToPlay = RandomNumber(lines.Count - 1);
               // samLineManager.Play(lines[lineToPlay].name);

            }

            if (this.startTime > this.startPrez && this.startTime < this.startAmbianceLine)
            {
                Debug.Log("Intro started");
                // samLineManager.Play(this.samLinePrez.name);
            }

            targetTime = 7.0f;
        }


       private void Update()
        {
            if (Time.time >= nextTurn)
            {
                if (!introduction)
                {
                    moodManager.ComputeMood(fearLevel);
                }
                nextTurn += intervalTurn;
            }
        }

        public void UpdateFear(float newFearLevel)
        {
            fearLevel = (int)newFearLevel;
            Debug.Log("fearLevel :: " + fearLevel);
            Debug.Log("SAM mood :: " + moodManager.GetMoodValue());
            //Debug.Log("DebugLog/ [SAM] received a fear level of " + fearLevel.ToString());
            //Console._instance.AddLog("ConsoleInstance/ [SAM] received a fear level of " + fearLevel.ToString());
        }

        public void UpdateRoom()
        {
            string msg = "[SAM] received a fear level of " + fearLevel.ToString();
            Debug.Log("DebugLog -- " + msg);
            Console._instance.AddLog("ConsoleInstance -- " + msg);
        }

        public void UpdateRoomConfig(List<SamTags> tags, Fear fearType, float fearIntensity)
        {
            string msg = "[SAM] received a room config of type " + fearType.ToString() + " and intensity " + fearIntensity.ToString();
            Debug.Log("DebugLog -- " + msg);
            Console._instance.AddLog("ConsoleInstance -- " + msg);
        }

        public void UpdateTriggerEvents(SamObject eventTriggered)
        {
            string msg = "[SAM] received a event is trigger " + eventTriggered.name;
            Debug.Log("DebugLog -- " + msg);
            Console._instance.AddLog("ConsoleInstance --" + msg);
        }
    }
}
