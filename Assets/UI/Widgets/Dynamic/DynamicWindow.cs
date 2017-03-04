using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class DynamicWindow : MonoBehaviour
{
    public enum UIElement
    {
        Label, Button, Dropdown, Slider, Field, Radio, Checkbox
    }

    public GameObject newEmptyElement(LayoutGroup layout)
    {
        GameObject go = new GameObject();
        go.AddComponent<LayoutElement>();
        go.transform.SetParent(layout.transform);
        return go;
    }

    public GameObject getPrototype(UIElement element)
    {

        switch(element)
        {
            case UIElement.Label:
                return DynamicWidgetManager.Instance.labelPrototype;
            case UIElement.Button:
                return DynamicWidgetManager.Instance.buttonPrototype;
            case UIElement.Dropdown:
                return DynamicWidgetManager.Instance.dropdownPrototype;
            case UIElement.Slider:
                return DynamicWidgetManager.Instance.sliderPrototype;
            case UIElement.Field:
                return DynamicWidgetManager.Instance.fieldPrototype;
            //case UIElement.Radio:
            //    return DynamicWidgetManager.Instance.radioPrototype;
            //case UIElement.Checkbox:
            //    return DynamicWidgetManager.Instance.checkboxPrototype;
            default:
                throw new System.NotImplementedException();
        }
    }

    public GameObject add(UIElement element, LayoutGroup layout)
    {
        GameObject go = Instantiate( getPrototype(element) );
        go.transform.SetParent(layout.transform);
        go.SetActive(true);
        return go;
    }

}
