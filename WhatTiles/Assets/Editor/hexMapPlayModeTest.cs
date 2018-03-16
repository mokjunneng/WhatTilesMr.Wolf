using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class hexMapPlayModeTest {

    [Test]
    public void hexMapTestingGameObject()
    {
        // Use the Assert class to test conditions.

        var go = GameObject.FindGameObjectWithTag("TileMap");
        Assert.AreEqual("HexMap", go.GetComponent<HexMap>().name);
    }

    //check for correct total number of tiles (correct initialisation of map) => 0 since game havent run
    [Test]
    public void hexMapTestingTotalTiles()
    {
        var go = GameObject.FindGameObjectWithTag("TileMap");
        Assert.AreEqual(0, go.GetComponent<HexMap>().totalCount);
    }

    //check for correct total number of tiles (correct initialisation of map) => 0 since game havent run 
    [Test]
    public void hexMapTestingPlayerTiles()
    {
        var go = GameObject.FindGameObjectWithTag("TileMap");
        var player = GameObject.FindGameObjectWithTag("Player");
        Assert.AreEqual(0, go.GetComponent<HexMap>().playerCount);
    }

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
	public IEnumerator hexMapPlayModeTestWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}
}
