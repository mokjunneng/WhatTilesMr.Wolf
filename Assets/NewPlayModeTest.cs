//using UnityEngine;
//using UnityEngine.TestTools;
//using NUnit.Framework;
//using System.Collections;
//using UnityEditor.SceneManagement;
//using UnityEngine.Networking;

//public class NewPlayModeTest {

//	[Test]
//	public void NewPlayModeTestSimplePasses() {
//        // Use the Assert class to test conditions.
//        EditorSceneManager.OpenScene("Assets/Scenes/MainScene.unity");
//        GameObject obj = GameObject.Find("NetworkManager");
//        NetworkManager manager = obj.GetComponent<NetworkManager>();

//        manager.StartMatchMaker();
//    }

//	// A UnityTest behaves like a coroutine in PlayMode
//	// and allows you to yield null to skip a frame in EditMode
//	[UnityTest]
//	public IEnumerator NewPlayModeTestWithEnumeratorPasses() {
//		// Use the Assert class to test conditions.
//		// yield to skip a frame
//		yield return null;
//	}
//}
