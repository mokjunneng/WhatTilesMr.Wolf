using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class testCases {
	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode

    // Test cases for respective game object generation 
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

        Debug.Log("Test 1 completed. Please see results..");
    }

    // Test cases for random tile generation for player
    [UnityTest]
    public IEnumerator tileRandomGeneration()
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
}
