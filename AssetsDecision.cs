using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Prend la décision des assets présents sur l'envionnement dans lequel va évoluer le joueur.
 * Il prend en compte le type de peur déterminé par FearDecision.
 */
public class AssetsDecision {

	public static List<GameObject> Compute(List<FearType> fears) {
		List<GameObject> assets = new List<GameObject>();
		foreach (FearType fear in fears) {
		    foreach (GameObject asset in ResourcesManager._instance.assets) {
				bool valid = false;
				foreach (FearType tag in asset.GetComponent<EntityTags>().fears) {
					if (tag == fear) {
						valid = true;
						break;
					}
				}
				if (valid) {
					assets.Add(asset);
					break;
				}
			}
		}
		return assets;
	}
}
