using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowWidget : MonoBehaviour {

    public GameObject resizeBox;
    public Text title;

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
    public Size2 minimumSize {
        get
        {
            return minimumSize_;
        }
        set
        {
            minimumSize_ = value;
            adaptSizeToContent();
        }
    }

    private ResizePanel resizePanel_;
    public ResizePanel resizePanel
    {
        get
        {
            if (resizePanel_ == null)
            {
                resizePanel_ = resizeBox.GetComponent<ResizePanel>();
                if (resizePanel_ == null) {
                    Debug.LogError("ResizeBox has no ResizePanel!");
                }
            }
            return resizePanel_;
        }

        set
        {
            resizePanel_ = value;
        }
    }

    /// <summary>
    /// Set minimum size for resize to the minimum size of the window, set content size to that of the window.
    /// </summary>
    void adaptSizeToContent()
    {
        if (resizePanel != null)
            resizePanel.minSize = minimumSize_.ToVector2() + margin.ToVector2();
        (transform as RectTransform).sizeDelta = minimumSize_.ToVector2() + margin.ToVector2();
        if (content != null && content.GetComponent<DynamicXmlWidget>() != null)
            (transform as RectTransform).sizeDelta = content.GetComponent<DynamicXmlWidget>().minimumSize.ToVector2() + margin.ToVector2();
        else if (content != null && content.GetComponent<LayoutElement>() != null)
        {
            if (content.GetComponent<LayoutElement>().minWidth < 0 && content.GetComponent<LayoutElement>().minHeight < 0) return;
            Vector2 size = margin.ToVector2();
            if (content.GetComponent<LayoutElement>().minWidth >= 0) size.x += content.GetComponent<LayoutElement>().minWidth;
            if (content.GetComponent<LayoutElement>().minHeight >= 0) size.y += content.GetComponent<LayoutElement>().minHeight;
            (transform as RectTransform).sizeDelta = size;
            resizePanel.minSize = size;
        }
    }

    void Awake()
    {
        resizeBox = transform.Find("ResizeBox").gameObject;
        if (resizeBox == null)
        {
            Debug.LogError("Cannot find ResizeBox child!");
        }
    }
    
    // Use this for initialization
    void Start () {
        findTitle();
        
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
