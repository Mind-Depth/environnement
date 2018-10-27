using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ISerial<S, T> where S : ISerial<S, T>, new() where T : new() {
	protected abstract void Import(T p);
	protected abstract void Export(T p);
	public static S ToObject(T param) {
		S obj = new S();
		obj.Import(param);
		return obj;
	}
	public static T FromObject(S obj) {
		T ret = new T();
		obj.Export(ret);
		return ret;
	}
	public static string ToJson(T param) {
		return JsonUtility.ToJson(ToObject(param));
	}
	public static T FromJson(string json) {
		return FromObject(JsonUtility.FromJson<S>(json));
	}
	[Serializable]
	public class AsList {
		public AsList(List<T> param) { Import(param); }
		public List<S> list = new List<S>();
		protected void Import(List<T> param) {
			foreach (T p in param) {
				list.Add(ToObject(p));
			}
		}
		protected void Export(List<T> param) {
			foreach (S obj in list) {
				param.Add(FromObject(obj));
			}
		}
		public static string ToJson(List<T> param) {
			return JsonUtility.ToJson(new AsList(param));
		}
		public static List<T> FromJson(string json) {
			List<T> ret = new List<T>();
			JsonUtility.FromJson<AsList>(json).Export(ret);
			return ret;
		}
	}
}

[Serializable]
public class SerializableProfile : ISerial<SerializableProfile, Profile> {
	[Serializable]
	public class Node {
		public string type;
		public FearLevel values;
	}
	public List<Node> fears = new List<Node>();
	public SerializableProfile(){}
	public SerializableProfile(Profile p) { Import(p); }
	protected override void Import(Profile p) {
		foreach (FearType fear in Enum.GetValues(typeof(FearType))) {
			Node node = new Node();
			node.type = Enum.GetName(typeof(FearType), fear);
			node.values = p.fears[fear];
			fears.Add(node);
		}
	}
	protected override void Export(Profile p) {
		foreach (Node node in fears) {
			p.fears[(FearType)Enum.Parse(typeof(FearType), node.type)] = node.values;
		}
	}
}