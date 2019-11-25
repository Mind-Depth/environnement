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

        private string[]        samLines;
        private List<Line>      samLinesObject;
        public string           lang;

        private SamLinesManager samLineManager;
        private MoodManager     moodIntroduction;
        private HistoryManager  historyManager;
        private AudioSource     audioSource;
        private string          currenMood;
        private List<Line>      samIntroductionLines;

        public float startTime = 0.0f;
        public float targetTime = 5.0f;

        public float startAmbianceLine = 10.0f;
        public float startPrez = 1.0f;

        public int intervalTurn = 1;
        private float nextTurn = 0.0f;
        private int fearLevel = 0;
        public MindStates mindStates = MindStates.HELPER;
        public OrderStates orderState = OrderStates.NO_ORDER;
        public SamLyricStates samLyricStates = SamLyricStates.NO_TALK;
        public GameStates gameStates = GameStates.INTRODUCTION;
        private States states;
        private Room currentRoom;
        private Room oldRoom;

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
            /*currentFear.Add("spider");
            currentFear.Add("nothing");

            rooms.Add("SpiderRoom");
            rooms.Add("VertigoRoom");
            rooms.Add("ClaustroRoom");

            mood.Add("happy");
            mood.Add("angry");
            mood.Add("septic");*/

            samLineManager = new SamLinesManager(audioSource, lang);
            //samLines = samLineManager.GetSamLinesName();
            //samLinesObject = samLineManager.GetSamLinesObject();
            //samIntroductionLines = samLineManager.GetSamIntroductionObject();

            //samLineManager.SetLinesByMood(samLinesObject);

            /*historyManager = new HistoryManager();*/

            moodIntroduction = new MoodManager(50.0f);
            states = new States();
            currentRoom = new Room();
            currentRoom.SetRoomName("none");
            oldRoom = new Room();
            oldRoom.SetRoomName("none");
        }

        // Return a number between 0 and the variable max
        public int RandomNumber(int max)
        { return UnityEngine.Random.Range(0, max); }

        /*void TimerEnded()
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
        }*/

        private void ConfigureSoundFirstRoom()
        {
            /*
            samLineManager.AddToPipe(samIntroductionLines[0]);
            Debug.Log("petit pause.");
            samLineManager.PausePipe(5.0f);
            samLineManager.AddToPipe(samLineManager.GetSamLineObjectByName("tu_te_sent_bien_la_ahahah"));
            samLineManager.AddToPipe(samLineManager.GetSamLineObjectByName("attention_il_y_en_a_une_sur_toi_ahahah"));
            samLineManager.AddToPipe(samLineManager.GetSamLineObjectByName("c_est_sur_que_si_tu_restes_dans_ton_coin_aussi"));
            */
        }

        private void ConfigureSoundSecondRoom()
        {
            /*
            samLineManager.AddToPipe(samLineManager.GetSamIntroductionObjectByName("presentation_sam"));
            samLineManager.AddToPipe(samLineManager.GetSamLineObjectByName("tu_fais_le_malin_mais_change_de_salle_pour_voir"));
            samLineManager.AddToPipe(samLineManager.GetSamLineObjectByName("attention_il_y_en_a_une_sur_toi_ahahah"));
            samLineManager.AddToPipe(samLineManager.GetSamLineObjectByName("tu_fais_le_malin_mais_change_de_salle_pour_voir"));
            */    
        }

        private void Update()
        {
            // TODO: Correctly call all SamLinesManager functions.
            // TODO: Modify mood with mood Manager and pass new moods in SamLinesManager class. 
            if (states.GetGameState() == GameStates.INTRODUCTION)
            {
                Debug.Log("INTRODUCTION IS PLAYING.");
            } else if (states.GetGameState() == GameStates.PLAY_MODE)
            {
                Debug.Log("Let's the party begin !");
            }
            /*if (!samLineManager.SongIsRunning())
            {
                samLineManager.PlayPipe();
            }*/
        }

        public void UpdateFear(float newFearLevel)
        {
            fearLevel = (int)newFearLevel;
            Debug.Log("fearLevel :: " + fearLevel);
            Debug.Log("SAM mood :: " + moodIntroduction.GetMoodValue());
            moodIntroduction.IncrementUserFeelingChangement();
            //Debug.Log("DebugLog/ [SAM] received a fear level of " + fearLevel.ToString());
            //Console._instance.AddLog("ConsoleInstance/ [SAM] received a fear level of " + fearLevel.ToString());
        }

        public void UpdateRoom(String roomName)
        {
            string msg = "[SAM] received a fear level of " + fearLevel.ToString();
            Debug.Log("DebugLog -- " + msg);
            currentRoom = new Room();
            currentRoom.SetRoomName(roomName);
            currentRoom.SetTimeSpent(Time.time);
            //samLineManager.CleanPipe();
            states.SetGameState(GameStates.INTRODUCTION);

            if (currentRoom.GetRoomName() == "first_room")
            {
                ConfigureSoundFirstRoom();
            }
            else if (currentRoom.GetRoomName() == "second_room")
            {
                ConfigureSoundSecondRoom();
            }
            //Console._instance.AddLog("ConsoleInstance -- " + msg);
        }

        public void UpdateRoomConfig(List<SamTags> tags, Fear fearType, float fearIntensity)
        {
            string msg = "[SAM] received a room config of type " + fearType.ToString() + " and intensity " + fearIntensity.ToString();
            Debug.Log("DebugLog -- " + msg);
            states.SetGameState(GameStates.PLAY_MODE);
            //samLineManager.CleanPipe();
            //Console._instance.AddLog("ConsoleInstance -- " + msg);
        }

        public void UpdateTriggerEvents(SamObject eventTriggered)
        {
            string msg = "[SAM] received a event is trigger " + eventTriggered.name;
            Debug.Log("DebugLog -- " + msg);
            //Console._instance.AddLog("ConsoleInstance --" + msg);
        }
    }
}
