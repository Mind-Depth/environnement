using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;

public class Gen {

    [Test]
    public void Profile_diff() {
		Profile spider = new Profile();
		spider.fears[FearType.Arachno].level = 100f;
		spider.fears[FearType.Arachno].tested = 1;
		spider.fears[FearType.Vertigo].level = 0f;
		spider.fears[FearType.Vertigo].tested = 1;

		Profile vertigo = new Profile();
		vertigo.fears[FearType.Arachno].level = 0f;
		vertigo.fears[FearType.Arachno].tested = 1;
		vertigo.fears[FearType.Vertigo].level = 100f;
		vertigo.fears[FearType.Vertigo].tested = 1;

		Profile both = new Profile();
		both.fears[FearType.Arachno].level = 100f;
		both.fears[FearType.Arachno].tested = 1;
		both.fears[FearType.Vertigo].level = 100f;
		both.fears[FearType.Vertigo].tested = 1;

		Assert.IsTrue(0f == spider.difference(spider));
		Assert.IsTrue(0f < spider.difference(vertigo));
		Assert.IsTrue(0f < spider.difference(both));
		Assert.IsTrue(spider.difference(both) < spider.difference(vertigo));
    }

    [Test]
    public void Profile_level() {
		Profile spider = new Profile();
		spider.add_fear_value(FearType.Arachno, 100);
		spider.add_fear_value(FearType.Arachno, 0);
		spider.add_fear_value(FearType.Arachno, 75);
		spider.add_fear_value(FearType.Arachno, 100);
		spider.add_fear_value(FearType.Arachno, 100);
		Assert.IsTrue(spider.fears[FearType.Arachno].level == 75f);
		spider.reset();
		Assert.IsTrue(spider.fears[FearType.Arachno].level == 0f);
    }

	[Test]
	public void Profile_serial() {
		System.Collections.Generic.List<Profile> p1 = new System.Collections.Generic.List<Profile>();
		p1.Add(new Profile());
		p1.Add(new Profile());
		p1[0].fears[FearType.Arachno].level = 100f;
		p1[0].fears[FearType.Arachno].tested = 1;
		p1[1].fears[FearType.Vertigo].level = 100f;
		p1[1].fears[FearType.Vertigo].tested = 1;
		System.Collections.Generic.List<Profile> p2 = SerializableProfile.AsList.FromJson(SerializableProfile.AsList.ToJson(p1));
		Assert.IsTrue(p1.Count == p2.Count);
		for (int i = 0; i < p1.Count; ++i) {
			Assert.IsTrue(p1[i].difference(p2[i]) == 0f);
		}
	}

	[Test]
	public void Profiling_explore() {
		Profiling pf = new Profiling();
		pf.LoadCommonProfiles("Assets\\UT\\Profiles\\SimpleSpiderVertigo.json");
		Profile profile = pf.UpdateProfile(new Profile());
		Assert.IsTrue(profile.fears[FearType.Arachno].tested < 0 || profile.fears[FearType.Vertigo].tested < 0);
	}

	[Test]
	public void Profiling_confirm() {
		Profiling pf = new Profiling();
		pf.LoadCommonProfiles("Assets\\UT\\Profiles\\SimpleSpiderVertigo.json");

		Profile spider = new Profile();
		foreach (FearType fear in Enum.GetValues(typeof(FearType))) {
			spider.fears[fear].tested = 1;
			spider.fears[fear].level = 0f;
		}
		spider.fears[FearType.Arachno].level = 100f;

		Profile tmp = null;
		for (int i = 0; i < 100; ++i) {
			 tmp = pf.UpdateProfile(spider);
		}
		Assert.IsTrue(tmp.difference(spider) == 0f);
	}

	[Test]
	public void FearDecision_new() {
		Profile spider = new Profile();
		foreach (FearType fear in Enum.GetValues(typeof(FearType))) {
			spider.fears[fear].tested = 100;
			spider.fears[fear].level = 0f;
		}
		spider.fears[FearType.Arachno].tested = -1;
		Assert.IsTrue(FearDecision.Compute(spider).Contains(FearType.Arachno));
	}

	[Test]
	public void FearDecision_maximum() {
		Profile spider = new Profile();
		foreach (FearType fear in Enum.GetValues(typeof(FearType))) {
			spider.fears[fear].tested = 100;
			spider.fears[fear].level = 0f;
		}
		spider.fears[FearType.Arachno].level = 100f;
		Assert.IsTrue(FearDecision.Compute(spider).Contains(FearType.Arachno));
	}

	[Test]
	public void AssertDecision_simple() {
		GameObject spider = new GameObject();
		(spider.AddComponent(typeof(EntityTags)) as EntityTags).fears.Add(FearType.Arachno);
		GameObject vertigo = new GameObject();
		(vertigo.AddComponent(typeof(EntityTags)) as EntityTags).fears.Add(FearType.Vertigo);

		ResourcesManager._instance = (new GameObject()).AddComponent(typeof(ResourcesManager)) as ResourcesManager;
		ResourcesManager._instance.assets = new GameObject[]{spider, vertigo};

		Assert.IsTrue(AssetsDecision.Compute(new System.Collections.Generic.List<FearType>{FearType.Arachno}).Contains(spider));
	}

	[Test]
	public void GameLog_integrity() {
		GameLog log = new GameLog();
		Profile profile1 = new Profile();
		profile1.add_fear_value(FearType.Arachno, 10f);
		log.AddEvent(GameEvent.Type.UpdateCurrentProfile, SerializableProfile.ToObject(profile1));
		Profile profile2 = new Profile();
		profile2.add_fear_value(FearType.Arachno, 10f);
		log.AddEvent(GameEvent.Type.UpdateCurrentProfile, SerializableProfile.ToObject(profile2));
		log = JsonUtility.FromJson<GameLog>(JsonUtility.ToJson(log));
		Assert.IsTrue(profile1.difference(SerializableProfile.FromObject(log.events[0].data_profile)) == 0f);
		Assert.IsTrue(profile2.difference(SerializableProfile.FromObject(log.events[1].data_profile)) == 0f);
	}
}