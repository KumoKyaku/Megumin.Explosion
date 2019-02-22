using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.IO;
using System.Collections;
using System.Collections.Generic;

[Category("Megumin.Explosion")]
class MeguminExplosion4UnityTest
{
    [OneTimeSetUp]
    public void Setup()
    {
           
    }

    [Test]
    public void TestDebug()
    {
        var res = Megumin.MeguminDebug.HookUnity();
        Assert.AreEqual(true, res);
        Megumin.MeguminDebug.UnHookUnity();
    }
}