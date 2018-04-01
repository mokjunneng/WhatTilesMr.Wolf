using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    private Text timeDisplay;
    private Text timesUp;
    private float countDown = 30f;

    private Slider slider;

    int minutes;
    int seconds;
    // Use this for initialization
    void Start()
    {
        slider = GameObject.FindGameObjectWithTag("Slider").GetComponent<Slider>();
        timeDisplay = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();
        timesUp = GameObject.FindGameObjectWithTag("Times Up").GetComponent<Text>();
        timesUp.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        minutes = Mathf.FloorToInt(countDown / 60F);
        seconds = Mathf.FloorToInt(countDown - minutes * 60);

        countDown -= Time.deltaTime;
        timeDisplay.text = string.Format("{0:0}:{1:00}", minutes, seconds);

        if (countDown <= 0)
        {
            countDown = 0;
            timesUp.text = "Game Ends";
            print("times up && end game");

            if (slider.value > 5)
            {
                timesUp.text = "Game Ends\n Player Red wins !!!";
            }
        }
    }
}

