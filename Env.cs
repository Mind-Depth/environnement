using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;

public class Env : MonoBehaviour {

    [Test]
    public void Movement_Object()
    {
        Movements obj1 = new Movements(Movements.State.CEILING);
        Movements obj2 = new Movements(Movements.State.GROUND);
        Movements obj3 = new Movements(Movements.State.WALLX_INV);
        Movements obj4 = new Movements(Movements.State.WALLZ);
        Movements obj5 = new Movements(Movements.State.WALLZ_INV);

        Assert.IsTrue(0 == obj1.GetNewTarget(new Vector3(0, 0, 0)).y);
        Assert.IsTrue(6 == obj2.GetNewTarget(new Vector3(2, 6, 1)).y);
        Assert.IsTrue(-12 == obj3.GetNewTarget(new Vector3(2, 14, -12)).z);
        Assert.IsTrue(2 == obj2.GetNewTarget(new Vector3(2, 0, 4)).x);
        Assert.IsTrue(0 == obj2.GetNewTarget(new Vector3(0, 1, 8)).x);
    }
}
