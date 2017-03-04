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
        openWindow(name);
    }

    public static void openWindow(string name)
    {
        if (isOpen(name))
        {
            windows[name].SetActive(true);
            //windows[name].GetComponentInChildren<DragMask>().gameObject.SetActive(true);
            Debug.Log("Window " + name + " already open.");
            return;
        }
        GameObject window = Instantiate(Instance.windowPrototype);
        window.transform.SetParent(GameObject.Find("Canvas").transform);
        window.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -40);
        GameObject content = Instantiate(Instance.transform.FindChild(name).gameObject);
        content.transform.SetParent(window.transform);
        window.SetActive(true);
        content.SetActive(true);
        windows[name] = window;
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