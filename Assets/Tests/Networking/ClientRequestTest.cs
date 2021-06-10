using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Networking;

/// <summary>
/// Test if all Packets are constructed correctly
/// </summary>
public class ClientRequestTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void NetworkTestSimplePasses()
    {
        // Use the Assert class to test conditions
        Assert.IsTrue(true);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.

    [UnityTest]
    public IEnumerator NetworkTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
