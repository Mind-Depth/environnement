using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameEvent {
	public enum Type {
		LoadProfiles,
		QueueEvent,
		UpdateTmpProfile,
		UpdateCurrentProfile,
		UpdateTestedProfile,
		TestFears
	}
	public float time;
	public string type;
	public SerializableProfile data_profile;
	public List<SerializableProfile> data_profiles;
	public GameEvent(Type t, object obj) {
		time = Time.realtimeSinceStartup;
		type = Enum.GetName(typeof(Type), t);
		if (obj.GetType() == typeof(SerializableProfile)) {
			data_profile = (SerializableProfile)obj;
		} else if (obj.GetType() == typeof(List<SerializableProfile>)) {
			data_profiles = (List<SerializableProfile>)obj;
		}
	}
	protected GameEvent(Type t, string suffix, object obj) : this(t, obj) {
		type += " (" + suffix + ")";
	}
}

[Serializable]
public abstract class QueueData : GameEvent {
	public enum DataType {
		NewRoom,
		SeeEntity
	}
	public DataType datatype;
	public object data;
	public QueueData(DataType t, object obj) : base(Type.QueueEvent, Enum.GetName(typeof(DataType), t), obj) {
		datatype = t;
		data = obj;
	}
}

[Serializable]
public class GameLog {
	public static GameLog _instance = new GameLog();
	public List<GameEvent> events = new List<GameEvent>();
	public void AddEvent(GameEvent.Type type, object obj) {
		GameEvent e = new GameEvent(type, obj);
		events.Add(e);
	}
}