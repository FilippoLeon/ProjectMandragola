using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowWidget : MonoBehaviour {

    GameObject resizeBox;
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
            adaptSizeToContent();
        }
    }

    static public Size2 margin = new Size2(8, 8);

    private Size2 minimumSize_;
    public Size2 minimumSize { get
        {
            return minimumSize_;
        }
        set
        {
            minimumSize_ = value;
            adaptSizeToContent();
        }
    }

    void adaptSizeToContent()
    {
        if (resizeBox != null)
            resizeBox.GetComponent<ResizePanel>().minSize = minimumSize_.ToVector2();
        if (content_ != null && content_.GetComponent<DynamicXmlWidget>() != null)
            (transform as RectTransform).sizeDelta = content_.GetComponent<DynamicXmlWidget>().minimumSize.ToVector2();
    }

    // Use this for initialization
    void Start () {
        findTitle();
        resizeBox = transform.Find("ResizeBox").gameObject;
        adaptSizeToContent();
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
