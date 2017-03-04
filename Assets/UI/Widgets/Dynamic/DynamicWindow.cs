using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class DynamicWindow : MonoBehaviour
{
    public GameObject labelPrototype;
    public GameObject fieldPrototype;
    public GameObject sliderPrototype;
    public GameObject dropdownPrototype;
    public GameObject buttonPrototype;

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
                return labelPrototype;
            case UIElement.Button:
                return buttonPrototype;
            case UIElement.Dropdown:
                return dropdownPrototype;
            case UIElement.Slider:
                return sliderPrototype;
            case UIElement.Field:
                return fieldPrototype;
            //case UIElement.Radio:
            //    return radioPrototype;
            //case UIElement.Checkbox:
            //    return checkboxPrototype;
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
