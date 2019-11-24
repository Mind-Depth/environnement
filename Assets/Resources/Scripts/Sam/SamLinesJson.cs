using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Collections;

// https://www.youtube.com/watch?v=fesZnOaSXT4

namespace Sam
{
    public class SamLinesJson
    {
        private string  language;
        private string  linesPath;
        private Lines   linesFromJSON;
        private string  resourcePath = Application.dataPath + "/Resources/";

        public SamLinesJson(string language)
        {
            this.language = language;
            this.linesPath = GenerateLinesPath();
        }

        public Lines LoadJSONLines()
        {
            using (StreamReader stream = new StreamReader(linesPath))
            {
                string json = stream.ReadToEnd();
                linesFromJSON = JsonUtility.FromJson<Lines>(json);
            }
            return linesFromJSON;
        }
        
        private string GenerateLinesPath()
        {
            if (this.language == "fr")
            { return this.resourcePath + "Sam/FrenchLine.json"; }
            else if (this.language == "en")
            { return this.resourcePath + "Sam/EnglishLine.json"; }

            return this.resourcePath + "Saml/EnglishLine.json";
        }

        public string GetLanguage()
        { return this.language; }

        public void SetLanguage(string language)
        {
            this.language = language;
            this.linesPath = GenerateLinesPath();
        }

        public string[] LinesToStringArray(Lines l)
        {
            List<string> samLines = new List<string>();

            for (int i = 0; i < l.lines.Length; i++)
            { samLines.Add(l.lines[i].name); }

            return samLines.ToArray();
        }

        public string[] AmbiancesToStringArray(Lines l)
        {
            List<string> samAmbiances = new List<string>();

            for (int i = 0; i < l.lines.Length; i++)
            { samAmbiances.Add(l.ambiances[i].name); }

            return samAmbiances.ToArray();
        }

        public string[] IntroductionToStringArray(Lines l)
        {
            List<string> samIntroduction = new List<string>();

            for (int i = 0; i < l.lines.Length; i++)
            { samIntroduction.Add(l.introduction[i].name); }

            return samIntroduction.ToArray();
        }

        public List<Line>   GetLines()
        {
            List<Line> samLines = new List<Line>();

            for (int i = 0; i < linesFromJSON.lines.Length; i++)
            { samLines.Add(linesFromJSON.lines[i]); }

            return samLines;
        }

        public List<Line> GetAmbiances()
        {
            List<Line> samAmbiances = new List<Line>();

            for (int i = 0; i < linesFromJSON.ambiances.Length; i++)
            { samAmbiances.Add(linesFromJSON.ambiances[i]); }

            return samAmbiances;
        }

        public List<Line> GetIntroduction()
        {
            List<Line> samIntroduction = new List<Line>();

            for (int i = 0; i < linesFromJSON.introduction.Length; i++)
            { samIntroduction.Add(linesFromJSON.introduction[i]); }

            return samIntroduction;
        }
    }
}
