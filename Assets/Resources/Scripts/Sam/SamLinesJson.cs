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
        private SamLines   linesFromJSON;
        private string  resourcePath = Application.dataPath + "/Resources/";

        public SamLinesJson(string language)
        {
            this.language = language;
            this.linesPath = GenerateLinesPath();
        }

        public SamLines LoadJSONLines()
        {
            using (StreamReader stream = new StreamReader(linesPath))
            {
                string json = stream.ReadToEnd();
                linesFromJSON = JsonUtility.FromJson<SamLines>(json);
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

        public MindStateLines GetMindStateLines()
        { return linesFromJSON.mindStateLines; }

        public List<Line> GetAmbiances()
        { return linesFromJSON.ambiances;}

        public List<Line> GetIntroduction()
        { return linesFromJSON.introduction; }

        public List<Line> GetHelper()
        { return GetMindStateLines().helper; }

        public List<Line> GetPlotTwist()
        { return GetMindStateLines().plot_twist; }

        public List<Line> GetPsychopathe()
        { return GetMindStateLines().psychopathe; }

        // Mood

        public List<Line> FindAmbiancesByMood(string mood)
        { return GetAmbiances().FindAll((ambiance) => ambiance.mood.Contains(mood)); }

        public List<Line> FindHelperByMood(string mood)
        { return GetHelper().FindAll((helperLine) => helperLine.mood.Contains(mood)); }

        public List<Line> FindPlotTwistByMood(string mood)
        { return GetPlotTwist().FindAll((plotTwist) => plotTwist.mood.Contains(mood)); }

        public List<Line> FindPsychopatheByMood(string mood)
        { return GetPsychopathe().FindAll((psychopathe) => psychopathe.mood.Contains(mood)); }

        // Fear

        public List<Line> FindHelperByFear(string fear)
        { return GetHelper().FindAll((helperLine) => helperLine.fear.Contains(fear)); }

        public List<Line> FindPlotTwistByFear(string fear)
        { return GetPlotTwist().FindAll((plotTwist) => plotTwist.fear.Contains(fear)); }

        public List<Line> FindPsychopatheByFear(string fear)
        { return GetPsychopathe().FindAll((psychopathe) => psychopathe.fear.Contains(fear)); }

        // call to action

        public List<Line> FindHelperByCTA(string cta)
        { return GetHelper().FindAll((helperLine) => helperLine.callToAction.Contains(cta)); }

        public List<Line> FindPlotTwistByCTA(string cta)
        { return GetPlotTwist().FindAll((plotTwist) => plotTwist.callToAction.Contains(cta)); }

        public List<Line> FindPsychopatheByCTA(string cta)
        { return GetPsychopathe().FindAll((psychopathe) => psychopathe.callToAction.Contains(cta)); }

        // call to action && Fear && Mood && step

        public List<Line> FindHelper(string cta, string fear, string mood, string step)
        { return GetHelper().FindAll((helperLine) => helperLine.callToAction == cta && helperLine.fear == fear && helperLine.mood == mood && helperLine.step == step); }

        public List<Line> FindPlotTwist(string cta, string fear, string mood)
        { return GetPlotTwist().FindAll((plotTwist) => plotTwist.callToAction == cta && plotTwist.fear == fear && plotTwist.mood == mood); }

        public List<Line> FindPsychopathe(string cta, string fear, string mood)
        { return GetPsychopathe().FindAll((psychopathe) => psychopathe.callToAction == cta && psychopathe.fear == fear && psychopathe.mood == mood); }

        // name

        public Line FindIntroductionByName(string name)
        { return GetIntroduction().Find((intro) => intro.name == name); }

        public Line FindHelperByName(string name)
        { return GetHelper().Find((helper) => helper.name == name); }

        public Line FindPsychopatheByName(string name)
        { return GetPsychopathe().Find((psychoathe) => psychoathe.name == name); }

        // intro cta

        public Line FindIntroductionByCTA(string cta)
        { return GetIntroduction().Find((intro) => intro.callToAction.Contains(cta)); }


        /*public string[] LinesToStringArray(Lines l)
        {
            List<string> samLines = new List<string>();

            for (int i = 0; i < l.lines.Count; i++)
            { samLines.Add(l.lines[i].name); }

            return samLines.ToArray();
        }

        public string[] AmbiancesToStringArray(Lines l)
        {
            List<string> samAmbiances = new List<string>();

            for (int i = 0; i < l.lines.Count; i++)
            { samAmbiances.Add(l.ambiances[i].name); }

            return samAmbiances.ToArray();
        }

        public string[] IntroductionToStringArray(Lines l)
        {
            List<string> samIntroduction = new List<string>();

            for (int i = 0; i < l.lines.Count; i++)
            { samIntroduction.Add(l.introduction[i].name); }

            return samIntroduction.ToArray();
        }

        public List<Line>   GetLines()
        {
            return linesFromJSON.lines;
        }

        public List<Line> GetAmbiances()
        {
            return linesFromJSON.ambiances;
        }

        public List<Line> GetIntroduction()
        {
            return linesFromJSON.introduction;
        }*/
    }
}
