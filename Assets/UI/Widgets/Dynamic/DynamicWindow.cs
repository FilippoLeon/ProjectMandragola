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
        Label, Button, Dropdown, Slider, Field, Radio, Checkbox, ScrollView
    }
    //public enum UILayout
    //{
    //    Grid, Horizontal, Vertical
    //}

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
            case UIElement.Radio:
                return DynamicWidgetManager.Instance.radioPrototype;
            case UIElement.Checkbox:
                return DynamicWidgetManager.Instance.checkboxPrototype;
            case UIElement.ScrollView:
                return DynamicWidgetManager.Instance.scrollViewPrototype;
            default:
                throw new System.NotImplementedException();
        }
    }

    public T addLayout<T>(LayoutGroup layout = null) where T : LayoutGroup {
        T glg;
        if (layout == null) glg = gameObject.AddComponent<T>();
        else
        {
            GameObject go = new GameObject();
            go.transform.SetParent(layout.transform);
            glg = go.AddComponent<T>();
        }
        return glg;
    }

    public VerticalLayoutGroup addScrollView(LayoutGroup layout = null)
    {
        LayoutGroup glg = layout;
        if (glg == null)
        {
            if (gameObject.GetComponent<LayoutGroup>() != null) glg = gameObject.GetComponent<LayoutGroup>();
            else glg = gameObject.AddComponent<VerticalLayoutGroup>();

        }
        GameObject view = add(UIElement.ScrollView, glg);
        return view.transform.Find("Viewport").Find("Content").gameObject.AddComponent<VerticalLayoutGroup>();
    }

    public GameObject add(UIElement element, LayoutGroup layout)
    {
        GameObject go = Instantiate( getPrototype(element) );
        go.transform.SetParent(layout.transform);
        go.SetActive(true);
        return go;
    }

}
