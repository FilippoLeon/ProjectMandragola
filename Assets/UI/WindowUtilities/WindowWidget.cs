using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowWidget : MonoBehaviour {

    Text title;

    private GameObject content_;
    public GameObject content
    {
        get
        {
            if(content_ == null) content_ = gameObject.transform.Find("Content").gameObject;
            return content_;
        }
        set
        {
            if (content_ != null) Destroy(content_);
            if (gameObject.transform.Find("Content").gameObject != null)
                Destroy( gameObject.transform.Find("Content").gameObject);
            content_ = value;
            content_.transform.SetParent(transform);
            content_.gameObject.SetActive(true);
            content_.name = "Content";
        }
    }


    // Use this for initialization
    void Start () {
        findTitle();
		//transform.get
	}

    public void moveToCanvas()
    {
        transform.SetParent(GameObject.Find("Canvas").transform);
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
