﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public GameObject windowPrototype;
    public GameObject prefabWidgetsParent;
    static Dictionary<string, GameObject> windows = new Dictionary<string, GameObject>();
    static WindowManager Instance;
    
    // Use this for initialization
    void Start()
    {
        Debug.Assert(Instance == null);
        if (Instance != null) Debug.LogError("Multiple instances!");
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void dynamicOpenWindow(string name)
    {
        openWindowFromChildPrototype(name);
    }

    public static GameObject openEmptyWindow(string name)
    {
        if (isOpen(name))
        {
            windows[name].SetActive(true);
            //windows[name].GetComponentInChildren<DragMask>().gameObject.SetActive(true);
            Debug.Log("Window " + name + " already open.");
            return windows[name];
        }
        GameObject window = Instantiate(Instance.windowPrototype);
        window.transform.SetParent(GameObject.Find("Canvas").transform);
        window.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -40);
        window.SetActive(true);
        window.GetComponent<WindowWidget>().setTitle(name);
        // HACK to rescale window?
        window.transform.localScale *= window.GetComponentInParent<UnityEngine.UI.CanvasScaler>().scaleFactor;
        windows[name] = window;
        return window;
    }

    public static GameObject openWindowFromChildPrototype(string name)
    {
        if (isOpen(name))
        {
            windows[name].SetActive(true);
            //windows[name].GetComponentInChildren<DragMask>().gameObject.SetActive(true);
            Debug.Log("Window " + name + " already open.");
            return windows[name];
        }
        GameObject window = Instantiate(Instance.windowPrototype);
        window.transform.SetParent(GameObject.Find("Canvas").transform);
        window.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -40);
        window.GetComponent<WindowWidget>().content = 
            Instantiate(Instance.prefabWidgetsParent.transform.FindChild(name).gameObject);
        window.SetActive(true);
        // HACK to rescale window?
        window.transform.localScale *= window.GetComponentInParent<UnityEngine.UI.CanvasScaler>().scaleFactor;
        //content.SetActive(true);
        windows[name] = window;
        window.GetComponent<WindowWidget>().setTitle(name);
        return window;
    }

    public static bool isOpen(string name)
    {
        return windows.ContainsKey(name);
    }

    public static GameObject get(string name)
    {
        return windows[name];
    }

    public static void activate(string name)
    {
        windows[name].SetActive(true);
        //windows[name].GetComponentInChildren<DragMask>().gameObject.SetActive(true);
    }
}