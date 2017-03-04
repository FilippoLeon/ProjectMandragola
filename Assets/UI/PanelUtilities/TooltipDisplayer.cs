using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipDisplayer : MonoBehaviour
{
    // The acual Tooltip displayer and subobjects
    public GameObject tooltipDisplayerObject;
    public Text title, text;

    // A remote instance
    public static TooltipDisplayer instance = null;

    // Tooltip component with info
    Tooltip tooltipComponent; 

	// Use this for initialization
	void Start () {
        // Wimprove
        instance = this;
	}

    // Update is called once per frame
    void Update()
    {
        if (tooltipComponent != null)
        {
            tooltipDisplayerObject.transform.position = Input.mousePosition + new Vector3(5f, -5f, 0f);

            title.text = tooltipComponent.title;
            text.text = tooltipComponent.text;
        }
        //RaycastHit hit;
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Physics.Raycast(Camera.main.ScreenToViewportPoint(Input.mousePosition) - Vector3.forward * 10f, Vector3.forward, out hit, 100.0F);

        ////Physics.Raycast(ray, out hit)
        //if (hit.transform != null && hit.transform.GetComponent<Tooltip>() != null)
        //{
        //    //Transform objectHit = hit.transform;
        //}
        //else
        //{
        //    deactivate();
        //}
        //Debug.Log(hit.transform.name);
    }

    public void OnPointerEnter(BaseEventData ev) {
        //if((ev as PointerEventData) != null && (ev as PointerEventData).selectedObject.GetComponent<Tooltip>() != null)
            //activate(ev.selectedObject.GetComponent<Tooltip>());
    }

    public void OnPointerExit(BaseEventData ev)
    {
        //deactivate();
    }

    public void activate(Tooltip ttp)
    {
        tooltipComponent = ttp;
        tooltipDisplayerObject.gameObject.SetActive(true);
    }

    public void deactivate()
    {
        tooltipComponent = null;
        tooltipDisplayerObject.gameObject.SetActive(false);
    }
}
