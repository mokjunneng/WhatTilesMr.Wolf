using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class testCases {
	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode

//	// Test cases for respective game object generation 
	[UnityTest]
	public IEnumerator gameObjectGeneration()
	{
		// Use the Assert class to test conditions.
		SceneManager.LoadScene("MainScene");
		// yield to skip a frame
		yield return null;

		NetworkManager networkManager = NetworkManager.singleton;
		if (networkManager.matchMaker == null)
		{
			networkManager.StartMatchMaker();
		}
		networkManager.matchMaker.CreateMatch("default", 2, true, "", "", "", 0, 0, networkManager.OnMatchCreate);

		Debug.Log("Match created, Loading Map.. Please wait...");

		yield return new WaitForSeconds(1);

		// Test for wolf gameobject generated
		GameObject wolfCheck = GameObject.FindGameObjectWithTag("WolfAI");
		Assert.IsNotNull(wolfCheck);

		// Test for hexMap gameobject generated 
		GameObject tileMap = GameObject.FindGameObjectWithTag("TileMap");
		Assert.IsNotNull(tileMap);

		// Test for blueTile gamaobject generated
		GameObject[] blueTiles = GameObject.FindGameObjectsWithTag("BlueTile");
		Assert.IsNotNull(blueTiles);

		// Test for redTile gamaobject generated
		GameObject[] redTiles = GameObject.FindGameObjectsWithTag("RedTile");
		Assert.IsNotNull(redTiles);

        // Test for player gameObejct generated 
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Assert.IsNotNull(player);

        Debug.Log("Test completed. Please see results..");
	}

	[UnityTest]
	public IEnumerator gameWolfTimerGeneration()
	{
		Debug.Log("Testing Wolf Variables");

		// Test for wolf gameobject generated
		GameObject wolfCheck = GameObject.FindGameObjectWithTag("WolfAI");
		Assert.IsNotNull(wolfCheck);

		var wolf = wolfCheck.GetComponent<WolfEye> ();

		// Test for wolf timer
		float wolfTimer = wolf.getCountTimer();
		Assert.GreaterOrEqual (wolfTimer, 3);
		Assert.LessOrEqual (wolfTimer, 6);
		Assert.IsFalse (wolf.facingPlayers);

//		wolf.startRotation ();
		yield return new WaitForSeconds(1f);
//		Assert.IsTrue (wolf.facingPlayers);

		// Test for wolf rotation amount
		Assert.AreEqual (wolf.rotationAmount, 180);

		// Test for wolf shake intensity
		Assert.AreEqual (wolf.shakeIntensity, 0.3f);

		Debug.Log("Wolf Test completed");
	}

	[UnityTest]
	public IEnumerator playerMovementGeneration()
	{
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		Assert.IsNotNull (player);

		var playerComponent = player.GetComponent<playerMovement> ();

		// Test for stationary player on initialise
		Assert.IsFalse (playerComponent.isMoving);

		// Variables for moving player
		Vector3 initialPosition = player.transform.position - new Vector3 (0, 2f, 1);
		Vector3 endPosition = player.transform.position + new Vector3 (3, 0, 0);

		// Test for player's movespeed under normal
		player.transform.position = initialPosition;
		player.transform.position = Vector3.MoveTowards (initialPosition, endPosition, 10f);
		yield return new WaitForSeconds (0.5f);
		Assert.AreEqual (playerComponent.returnSpeed (), 3);

		yield return new WaitForSeconds (1f);

		// Test for player's movespeed under powerup
		player.transform.position = initialPosition;
		playerComponent.setHighSpeed ();
		player.transform.position = Vector3.MoveTowards (initialPosition, endPosition, 10f);
		yield return new WaitForSeconds (0.5f);
		Assert.AreEqual (playerComponent.returnSpeed (), 4.5);

		Debug.Log ("Player movement test completed");

	}

	[UnityTest]
	public IEnumerator tileRandomGeneration()
	{
//        // Use the Assert class to test conditions.
//		SceneManager.LoadScene("MainScene");
//        // yield to skip a frame
//		yield return new WaitForFixedUpdate();
//
//
//		NetworkManager networkManager = NetworkManager.singleton;
//		if (networkManager.matchMaker == null)
//		{
//			networkManager.StartMatchMaker();
//		}
//		networkManager.matchMaker.CreateMatch("default", 2, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
//
//		Debug.Log("Match created, Loading Map.. Please wait...");
//
//		yield return new WaitForSeconds(1);
		yield return null;

		GameObject[] blueTiles = GameObject.FindGameObjectsWithTag("BlueTile");
		Assert.IsNotNull(blueTiles);

		int noOfTimes = 0;
		for(int i = 0; i<blueTiles.Length/2;i+=2){
			Vector3 firstPosition = blueTiles[i].transform.position;
			Vector3 secondPosition = blueTiles[i + 1].transform.position;
			if(Vector3.Distance(firstPosition, secondPosition) > 2.0f)
			{
				noOfTimes++;
			}
		}
		Assert.GreaterOrEqual(noOfTimes,3);

		Debug.Log("Test 2 completed. Please see results..");
	}

 // test case for count increment/decrement of tiles for each player
	[UnityTest]
	public IEnumerator tileCount()
	{
//        // Use the Assert class to test conditions.
//		SceneManager.LoadScene("MainScene");
//        // yield to skip a frame
//		yield return null;
//
//		NetworkManager networkManager = NetworkManager.singleton;
//		if (networkManager.matchMaker == null)
//		{
//			networkManager.StartMatchMaker();
//		}
//		networkManager.matchMaker.CreateMatch("default", 2, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
//
//		Debug.Log("Match created, Loading Map.. Please wait...");
//
//		yield return new WaitForSeconds(1);

		yield return null;
		GameObject[] blueTiles = GameObject.FindGameObjectsWithTag("BlueTile");
		Assert.IsNotNull(blueTiles);

		GameObject[] redTiles = GameObject.FindGameObjectsWithTag("RedTile");
		Assert.IsNotNull(redTiles);

		GameObject player = GameObject.FindGameObjectWithTag("Player");
		Assert.IsNotNull(player);

		Vector3 initialPosition = player.transform.position - new Vector3(0,0.7f,0);
		player.transform.position = initialPosition;
		Vector3 endPosition = player.transform.position + new Vector3(3, 0, 0);

		
		float i = 0f;
		float rate = 1.0f/0.4f;

		Vector3 newPosition = initialPosition;
		while (i <1.0) {
			if (!GameObject.FindGameObjectWithTag("WolfAI").GetComponent<WolfEye>().facingPlayers)
			{
				player = GameObject.FindGameObjectWithTag("Player");
				i += Time.deltaTime * rate;
				newPosition = Vector3.LerpUnclamped(player.transform.position, endPosition, i);
				Debug.Log("Debug " + newPosition + "END" + endPosition);
				player.transform.position = newPosition;
			}
			
			yield return new WaitForSeconds(0.5f);
		}
		Assert.LessOrEqual(Vector3.Distance(endPosition, player.transform.position), 2.0f);
		Debug.Log("Test 3 completed. Please see results..");
	}

}