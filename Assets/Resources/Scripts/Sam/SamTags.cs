using System.Collections.Generic;
using UnityEngine;

namespace Sam
{
    public class SamTags: MonoBehaviour
    {
        //TODO insert real data
        public string data;

        static public void FetchFromTransform(Transform room, List<SamTags> tags)
        {
            SamTags tag;
            foreach (Transform child in room)
            {
                FetchFromTransform(child, tags);
                if (tag = child.GetComponent<SamTags>())
                    tags.Add(tag);
            }
        }

        static public void FetchFromRoom(GameObject room, out List<SamTags> tags)
        {
            tags = new List<SamTags>();
            FetchFromTransform(room.transform, tags);
        }
    }
}
