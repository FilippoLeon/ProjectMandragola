﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeControlWidget : MonoBehaviour {

    public GameObject worldController;
    GameObject[] speeds;
    // Use this for initialization
    void Start () {
        speeds = new GameObject[]{
            transform.Find("Pause").gameObject,
            transform.Find("Speed1").gameObject,
            transform.Find("Speed2").gameObject,
            transform.Find("Speed3").gameObject,
            transform.Find("Text").gameObject
        };
        for (int i = 0; i < 4; i++)
        {
            GameObject go = speeds[i];
            ColorBlock blk = go.GetComponent<Button>().colors;
            blk.highlightedColor = new Color(0.5f, 0, 0);
            go.GetComponent<Button>().colors = blk;
        }
            worldController.GetComponent<WorldController>().onSpeedChanged += (int speed) =>
        {
            if (speed <= 0) speed = 0;
            for (int i = 0; i < 4; i++)
            {
                GameObject go = speeds[i];
                if (i == speed)
                {
                    go.GetComponent<Image>().color = new Color(1f, 0, 0);
                    go.GetComponent<Button>().enabled = false;

                } else
                {
                    go.GetComponent<Image>().color = new Color(1f, 1f, 1f);
                    go.GetComponent<Button>().enabled = true;
                }
            }
        };
        worldController.GetComponent<WorldController>().onTic += () => {
            speeds[4].GetComponent<Text>().text = "Day " + worldController.GetComponent<WorldController>().time;
        };
    }
	
	// Update is called once per frame
	void Update () {
	}
}