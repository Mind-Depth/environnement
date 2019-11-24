﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Collections;

namespace Sam
{
    public class SamLinesManager
    {
        private string[]        samLines;
        private string          language;
        

        private SamLinesJson    samLinesJson;
        private Lines           lines;
        private List<Line>      samLinesObject;
        private List<Line>      samAmbiancesObject;
        private List<Line>      samIntroductionObject;

        private float           currentSongDuration = 0;
        private AudioSource     audioSource;
        private SoundManager    soundManager;
        private List<Line>      pipe;


        // Mood Sam Line
        private List<Line> happyLines = new List<Line>();
        private List<Line> exitementLines = new List<Line>();
        private List<Line> cynicalLines = new List<Line>();
        private List<Line> frustratedLines = new List<Line>();
        private List<Line> angerLines = new List<Line>();

        public SamLinesManager(AudioSource audioSource, string language)
        {
            this.audioSource = audioSource;
            this.language = language;

            samLinesJson = new SamLinesJson(this.language);
            lines = samLinesJson.LoadJSONLines();
            Debug.Log(lines.introduction);
            samLinesObject = samLinesJson.GetLines();
            samAmbiancesObject = samLinesJson.GetAmbiances();
            samIntroductionObject = samLinesJson.GetIntroduction();

            soundManager = new SoundManager(audioSource, "Sam/SamLines/" + this.language + "/");
            pipe = new List<Line>();
        }

        public void AddToPipe(Line line)
        {
            if (line != null)
            {
                this.pipe.Add(line);
            }
        }

        public void PlayPipe()
        {
            if (pipe.Count != 0)
            {
                Play(pipe[0]);
                pipe.RemoveAt(0);
            }
        }

        public void CleanPipe()
        {
            pipe.Clear();
        }

        public void PausePipe(float time)
        {
            this.pipe.Add(new Line { name = "blank", duration = time, mood = "" });
        }

        public Lines GetSamLines()
        {
            return this.lines;
        }

        public bool SongIsRunning()
        {
            if (Time.time > currentSongDuration)
                return false;
            return true;
        }

        public void SetLinesByMood(List<Line> samLines)
        {
            Debug.Log("SetLinesByMood > " + samLines[0].name);

            for (int i = 0; i < samLines.Count; i++)
            {
                if (samLines[i].mood == "happy")
                {
                    this.happyLines.Add(samLines[i]);
                }
                else if (samLines[i].mood == "exitement")
                {
                    this.exitementLines.Add(samLines[i]);
                }
                else if (samLines[i].mood == "cynical")
                {
                    this.cynicalLines.Add(samLines[i]);
                }
                else if (samLines[i].mood == "frustrated")
                {
                    this.frustratedLines.Add(samLines[i]);
                }
                else if (samLines[i].mood == "anger")
                {
                    this.angerLines.Add(samLines[i]);
                }
            }
        }

        public string GetLanguage()
        { return this.language; }

        public void SetLanguage(string language)
        {
            this.language = language;
            soundManager.SetPathFolder("SamLines/" + this.language + "/");
        }

        public AudioSource GetAudioSource()
        { return audioSource; }

        public void SetAudioSource(AudioSource audioSource)
        {
            this.audioSource = audioSource;
            soundManager.SetAudioSource(this.audioSource);
        }

        public string[] GetSamLinesName()
        {
            /*Debug.Log("samelinesJSON Object = " + samLinesJson);*/
            samLines = samLinesJson.LinesToStringArray(lines);
            
            return samLines;
        }

        public List<Line> GetLinesByMood(string mood)
        {
            if (mood == "happy")
            {
                return this.happyLines;
            } else if (mood == "exitement")
            {
                return this.exitementLines;
            } else if (mood == "cynical")
            {
                return this.cynicalLines;
            } else if (mood == "frustrated")
            {
                return this.frustratedLines;
            } else if (mood == "anger")
            {
                return this.angerLines;
            }
            return this.cynicalLines;
        }

        public List<Line> GetSamLinesObject()
        {
            return this.samLinesObject;
        }

        public List<Line> GetSamIntroductionObject()
        {
            return this.samIntroductionObject;
        }

        public List<Line> GetSamAmbiancesObject()
        {
            return this.samAmbiancesObject;
        }

        public Line GetSamLineObjectByName(string name)
        {
            foreach (Line line in this.samLinesObject)
            {
                if (line.name == name)
                {
                    return line;
                }
            }
            return null;
        }

        public Line GetSamIntroductionObjectByName(string name)
        {
            foreach (Line line in this.samIntroductionObject)
            {
                if (line.name == name)
                {
                    return line;
                }
            }
            return null;
        }

        /*public Line FindSamLineObjectByName(string name)
        {
            Line samLineObject ;

            for (int i = 0; i < lines.lines.lenght; i++)
            {
                Debug.Log("--- " + lines + " ---");
            }
            return samLineObject;
        }
        */
        // Use the sound manager for load and play the sound asked in parameter.
        public void Play(Line songToPlay)
        {
            currentSongDuration = Time.time + songToPlay.duration;
            if (songToPlay.name != "blank")
            {
                soundManager.LoadSound(songToPlay.name);
                soundManager.PlaySound();
            }
        }
    }

}
