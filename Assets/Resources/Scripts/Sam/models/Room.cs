using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sam
{
    public class Room
    {
        private float           timeSpent = 0;
        private String          roomName;
        private float           intensity = 0;
        private Fear            fearType;
        private List<SamTags>   samTags;

        public Room(String roomName, float intensity, Fear fearType, List<SamTags> samTags, float timeSpent)
        {
            this.roomName = roomName;
            this.intensity = intensity;
            this.fearType = fearType;
            this.samTags = samTags;
            this.timeSpent = timeSpent;
        }

        public Room() {}

        // Setters

        public void SetTimeSpent(float time)
        { this.timeSpent = time; }

        public void UpdateTimeSpent(float newTime)
        { this.timeSpent = newTime; }

        public void SetRoomName(String name)
        { this.roomName = name; }

        public void SetIntensity(float value)
        { this.intensity = value; }

        public void SetFearType(Fear fearType)
        { this.fearType = fearType; }

        public void SetSamTags(List<SamTags> samTags)
        { this.samTags = samTags; }

        // Getters

        public float GetTimeSpent()
        { return this.timeSpent; }

        public String GetRoomName()
        { return this.roomName; }

        public float GetIntensity()
        { return this.intensity; }

        public Fear GetFearType()
        { return this.fearType; }

        public List<SamTags> GetSamTags()
        { return this.samTags; }

        // Find

        public SamTags FindSamTagsByName(string name)
        { return GetSamTags.Find((tag) => tag.name == name); }

    }
}