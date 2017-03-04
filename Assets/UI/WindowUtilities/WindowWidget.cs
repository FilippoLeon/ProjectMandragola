using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowWidget : MonoBehaviour {

    Text title;

    public GameObject content
    {
        get
        {
            return gameObject.transform.Find("Content").gameObject;
        }
    }


    // Use this for initialization
    void Start () {
        findTitle();
		//transform.get
	}

    private void findTitle()
    {
        title = transform.Find("Title").GetComponent<Text>();
    }

    public void setTitle(string name)
    {
        if (title == null) findTitle();
        title.text = name;
        transform.name = "window_" + name; 
    }
}
