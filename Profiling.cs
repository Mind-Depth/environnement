using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using System.IO;

public enum FearType {
	Arachno,
	Vertigo
}

[Serializable]
public class FearLevel {
	public float level = 0f;
	public int tested = 0;
	public void reset() {
		level = 0f;
		tested = 0;
	}
}

/*
* Profile of a player. 
*/
public class Profile {
	public Dictionary<FearType, FearLevel> fears;
	public void reset() {
		foreach (FearType fear in Enum.GetValues(typeof(FearType))) {
			fears[fear].reset();
		}
	}
	public Profile() {
		fears = new Dictionary<FearType, FearLevel>();
		foreach (FearType fear in Enum.GetValues(typeof(FearType))) {
			fears[fear] = new FearLevel();
		}
		reset();
	}
	public void add_fear_value(FearType fear, float value) {
		fears[fear].level = (fears[fear].level * fears[fear].tested + value) / (fears[fear].tested + 1);
		fears[fear].tested += 1;
	}
	public float difference(Profile other) {
		float diff = 0;
		foreach (FearType fear in Enum.GetValues(typeof(FearType))) {
			if (fears[fear].tested > 0 && other.fears[fear].tested > 0) {
				diff += Mathf.Abs(fears[fear].level - other.fears[fear].level);
			}
		}
		return diff;
	}
}

/*
 * Classe qui génère un profil en fonction des actions et réactions du joueur.
 * Utilise l'ensemble des données envoyées par les "Watchers" via la MutexedQueue.
 */
public class Profiling : AThreadJob {
	Profile current_profile = new Profile();
	Profile temporary_profile = new Profile();
	List<Profile> common_profiles = new List<Profile>();

	public Profile UpdateProfile(Profile result) {
		foreach (FearType fear in Enum.GetValues(typeof(FearType))) {
			if (result.fears[fear].tested > 0) {
				current_profile.add_fear_value(fear, result.fears[fear].level);
			}
		}
		Profile candidate = new Profile();
		List<Profile> sorted = common_profiles.OrderBy(p=>current_profile.difference(p)).ToList();
		foreach (FearType fear in Enum.GetValues(typeof(FearType))) {
			if (current_profile.fears[fear].tested > 0) {
				candidate.fears[fear] = current_profile.fears[fear];
			} else if (sorted[0].fears[fear].tested > 0) {
				candidate.fears[fear] = sorted[0].fears[fear];
				candidate.fears[fear].tested *= -1;
			}
		}
		return candidate;
	}

	public void Compute(QueueData item) {
		if (item.datatype == QueueData.DataType.SeeEntity) {
			GameObject entity = (GameObject)item.data;
			foreach (FearType fear in entity.GetComponent<EntityTags>().fears) {
				temporary_profile.add_fear_value(fear, 10f);
			}
			GameLog._instance.AddEvent(GameEvent.Type.UpdateTmpProfile, SerializableProfile.ToObject(temporary_profile));
		} else if (item.datatype == QueueData.DataType.NewRoom) {
			Profile profile = UpdateProfile(temporary_profile);
			GameLog._instance.AddEvent(GameEvent.Type.UpdateCurrentProfile, SerializableProfile.ToObject(current_profile));
			GameLog._instance.AddEvent(GameEvent.Type.UpdateTestedProfile, SerializableProfile.ToObject(profile));
			temporary_profile.reset();
			List<FearType> fears = FearDecision.Compute(profile);
			GameLog._instance.AddEvent(GameEvent.Type.TestFears, fears);
			Map map = new Map();
			map.assets = AssetsDecision.Compute(fears);
			AssetsList._instance.AddElement(map);
		}
	}

	public void LoadCommonProfiles(string file) {
		common_profiles = SerializableProfile.AsList.FromJson(File.ReadAllText(file));
	}

    protected override void ThreadFunction() {
		LoadCommonProfiles("Assets\\UT\\Profiles\\SimpleSpiderVertigo.json");
		GameLog._instance.AddEvent(GameEvent.Type.LoadProfiles, new SerializableProfile.AsList(common_profiles));
		while (true) {
	        for (int count = Manager._instance.items_queue.Count; count > 0; count--) {
				Compute(Manager._instance.items_queue.Dequeue());
			}
			if (IsDone) break;
			System.Threading.Thread.Sleep(1000);
		}
    }

    protected override void OnFinished() {
		// end
    }
}


