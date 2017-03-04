using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicWidgetManager : MonoBehaviour
{
    public GameObject labelPrototype;
    public GameObject fieldPrototype;
    public GameObject sliderPrototype;
    public GameObject dropdownPrototype;
    public GameObject buttonPrototype;
    public GameObject checkboxPrototype;
    public GameObject radioPrototype;

    static public DynamicWidgetManager Instance;

    void Start () {
        Instance = this;
	}
	
}
