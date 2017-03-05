using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public GameObject windowPrototype;
    static Dictionary<string, GameObject> windows = new Dictionary<string, GameObject>();
    static WindowManager Instance;
    
    // Use this for initialization
    void Start()
    {
        Debug.Assert(Instance == null);
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
        window.GetComponent<WindowWidget>().content = Instantiate(Instance.transform.FindChild(name).gameObject);
        window.SetActive(true);
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