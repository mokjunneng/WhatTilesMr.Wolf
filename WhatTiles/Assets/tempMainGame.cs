using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tempMainGame : MonoBehaviour {

    public Texture2D[] buttonTextures;
    private float buttonWidth;
    private float buttonHeight;

    void OnGUI()
    {
        int i = 0;
        if (GUI.Button(new Rect((float)Screen.width,
                                      (float)Screen.height,
                                      buttonWidth,
                                      buttonHeight), buttonTextures[i]))
        {
            print("Load Game....");
            RetainedUserPicksScript.Instance.multiplayerGame = false;
            SceneManager.LoadScene("MainScene");
        }
    }

}
