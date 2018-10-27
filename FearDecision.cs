using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 * Prend la décision du type de peur à afficher en se basant sur le profil du joueur.
 */
public class FearDecision {

	public static FearType RandomFear () {
		var v = Enum.GetValues (typeof (FearType));
	    return (FearType) v.GetValue (new System.Random ().Next(v.Length));
	}

	public static List<FearType> Compute(Profile profile) {
		List<FearType> fears = new List<FearType>();
		foreach(KeyValuePair<FearType, FearLevel> kv in profile.fears) {
			if (kv.Value.tested < 1 || kv.Value.level > 60) {
				fears.Add(kv.Key);
			}
		}
		for (int i = fears.Count; i < 3; ++i) {
			fears.Add(RandomFear());
		}
		return fears;
	}
 }