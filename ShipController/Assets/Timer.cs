using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public Text timerText;

    float startTime;
    public bool restart = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (restart)
            StartTimer();
        else
            UpdateTimer();
	}

    public void StartTimer()
    {
        startTime = Time.time;
        UpdateTimer();
        //restart = false;
    }

    private void UpdateTimer()
    {
        float t = Time.time - startTime;

        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f2");

        timerText.text = minutes + ":" + seconds;
    }
}
