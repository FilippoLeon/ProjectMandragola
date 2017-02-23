using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowWidget : MonoBehaviour {

    Text title;

    // Use this for initialization
    void Start () {
        title = transform.Find("Title").GetComponent<Text>();
		//transform.get
	}
	

    public void setTitle(string name)
    {
        title.text = name;
    }
}
