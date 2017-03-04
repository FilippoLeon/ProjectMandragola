using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public string title;
    public string text;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData != null)
            TooltipDisplayer.instance.activate(this.GetComponent<Tooltip>());
        //if((ev as PointerEventData) != null && (ev as PointerEventData).selectedObject.GetComponent<Tooltip>() != null)
        //activate(ev.selectedObject.GetComponent<Tooltip>());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData != null)
            TooltipDisplayer.instance.deactivate();
        //deactivate();
    }

    public void OnMouseOver()
    {
        Debug.Log("over!!!");
    }
    
}
