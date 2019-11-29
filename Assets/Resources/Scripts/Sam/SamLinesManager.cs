using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Collections;

namespace Sam
{
    public class SamLinesManager
    {
        private string[]        samLines2;
        private string          language;
        

        private SamLinesJson    samLinesJson;
        //private Lines           lines;
        private SamLines        samLines;
        private List<Line>      samLinesObject;
        private List<Line>      samAmbiancesObject;
        private List<Line>      samIntroductionObject;

        private float           currentSongDuration = 0;
        private AudioSource     audioSource;
        private SoundManager    soundManager;
        private List<Line>      pipe;

        // TODO: Set and use current Sam's state moods.
        // TODO: Set and use current call to action.
        // TODO: Set and use current fear.
        //
        // TODO: Set and use Game State.
        // TODO: Set and use Mind States.

        // Mood Sam Line
        private List<Line> happyLines = new List<Line>();
        private List<Line> exitementLines = new List<Line>();
        private List<Line> cynicalLines = new List<Line>();
        private List<Line> frustratedLines = new List<Line>();
        private List<Line> angerLines = new List<Line>();
        private List<Line> lineCurrentRoom = new List<Line>();
        
        public SamLinesManager(AudioSource audioSource, string language)
        {
            this.audioSource = audioSource;
            this.language = language;

            samLinesJson = new SamLinesJson(this.language);
            samLines = samLinesJson.LoadJSONLines();
            // TODO: Each paramaters here will be changed by class global variables.
            // TODO: if (intro) 3 states availables (normal | stressed | anger); get them in introduction list. Select line for each roooms (firstroom | second room).
            // TODO: if (play mode) 5 states availables (HELPER(normal | stressed | anger) | PLOT_TWIST | PSYCHOPATHE); get them in lines list. Select line for each roooms (Arachnophobia | Vertigo | Nyctophobia | Claustrophobia) for each CTA (levier | armoire) for each mood (happyness | exitement | cynical | frustrated | anger) but (normal | stressed | anger) for helper.

            /*samLinesObject = samLinesJson.GetLines();
            samAmbiancesObject = samLinesJson.GetAmbiances();
            samIntroductionObject = samLinesJson.GetIntroduction();
            */
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

        public Line GetCurrentSound()
        {
            if (this.pipe.Count > 0)
            {
                return this.pipe[0];
            }
            return null;
        }

        // Return a number between 0 and the variable max
        public int RandomNumber(int max)
        { return UnityEngine.Random.Range(0, max); }

        public Line FindIntroductionByName(string name)
        { return samLinesJson.FindIntroductionByName(name); }

        public Line FindHelperByName(string name)
        { return samLinesJson.FindHelperByName(name); }

        public Line FindPsychopatheByName(string name)
        { return samLinesJson.FindPsychopatheByName(name); }

        public Line FindIntroductionByCTA(string cta)
        { return samLinesJson.FindIntroductionByCTA(cta); }

        public List<Line> FindHelper(string cta, string fear, string mood, string step)
        { return samLinesJson.FindHelper(cta, fear, mood, step); }

        public Line SelectPsychopathe(string mood, string fear)
        {
            List<Line> line = samLinesJson.FindPsychopatheByMoodAndFearAndAll(mood, fear);
            return line[RandomNumber(line.Count)];
        }

        public void AddListToPipe(List<Line> list)
        {
            foreach (Line line in list)
            {
                AddToPipe(line);
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

        public List<Line> GetPipe()
        {
            return this.pipe;
        }

        public void CleanPipe()
        {
            pipe.Clear();
        }

        public void PausePipe(float time)
        {
            this.pipe.Add(new Line { name = "blank", duration = time, mood = "" });
        }

        /*public Lines GetSamLines()
        {
            return this.lines;
        }*/

        public bool SongIsRunning()
        {
            if (Time.time > currentSongDuration)
                return false;
            return true;
        }

        public void SetLinesByMood(List<Line> allSamLines)
        {
            for (int i = 0; i < allSamLines.Count; i++)
            {
                if (allSamLines[i].mood == "happy")
                {
                    this.happyLines.Add(allSamLines[i]);
                }
                else if (allSamLines[i].mood == "exitement")
                {
                    this.exitementLines.Add(allSamLines[i]);
                }
                else if (allSamLines[i].mood == "cynical")
                {
                    this.cynicalLines.Add(allSamLines[i]);
                }
                else if (allSamLines[i].mood == "frustrated")
                {
                    this.frustratedLines.Add(allSamLines[i]);
                }
                else if (allSamLines[i].mood == "anger")
                {
                    this.angerLines.Add(allSamLines[i]);
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
