using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class DynamicXmlWindow : DynamicWindow
{
    public interface IParametrizable
    {
        Vector2 parameterRange(string param);
        bool parameterHasRange(string param);
        void setParameter<T>(string name, T value);
    }

    public IParametrizable target;
    
    void Start()
    {
        buildWidgets(Path.Combine(Application.streamingAssetsPath, Path.Combine("Data", "Laws.xml")));
    }
    
    public void buildWidgets(string path)
    {
        XmlReaderSettings settings = new XmlReaderSettings();
        XmlReader reader = XmlReader.Create(path, settings);

        reader.MoveToContent();
        while (reader.Read())
        {
            XmlNodeType nodeType = reader.NodeType;
            switch (nodeType)
            {
                case XmlNodeType.Element:
                    //Debug.Log(reader.Name);
                    if (reader.Name.Equals("Gui"))
                    {
                        XmlReader subtree = reader.ReadSubtree();
                        addRoot(null, subtree);
                    }
                    break;
                case XmlNodeType.EndElement:
                default:
                    //Debug.Log(reader.Name as string);
                    break;
            }
        }
    }

    public void addRoot(LayoutGroup layout, XmlReader reader)
    {
        reader.MoveToContent();
        while (reader.Read())
        {
            XmlNodeType nodeType = reader.NodeType;
            switch (nodeType)
            {
                case XmlNodeType.Element:
                    //Debug.Log(reader.Name);
                    if (reader.Name.Equals("Grid"))
                    {
                        GridLayoutGroup grid = addGrid(layout, reader);
                        XmlReader subtree = reader.ReadSubtree();
                        addRoot(grid, subtree);
                    }
                    else if (reader.Name.Equals("Label"))
                    {
                        addLabel(layout, reader);
                    }
                    else if (reader.Name.Equals("Field"))
                    {
                        addField(layout, reader);
                    }
                    else if (reader.Name.Equals("Slider"))
                    {
                        addSlider(layout, reader);
                    }
                    else if (reader.Name.Equals("Button"))
                    {
                        addButton(layout, reader);
                    }
                    else if (reader.Name.Equals("Dropdown"))
                    {
                        addDropdown(layout, reader);
                    }
                    break;
                case XmlNodeType.EndElement:
                default:
                    //Debug.Log(reader.Name as string);
                    break;
            }
        }
    }

    public void addLabel(LayoutGroup layout, XmlReader reader)
    {
        Debug.Assert(reader.Name.Equals("Label"));
        GameObject go = add(UIElement.Label, layout);
        Text txt = go.GetComponent<Text>();
        txt.text = reader.ReadElementContentAsString();
    }

    public void addField(LayoutGroup layout, XmlReader reader)
    {
        Debug.Assert(reader.Name.Equals("Field"));
        GameObject go = add(UIElement.Field, layout);
        //InputField txt = newElement(layout).AddComponent<InputField>();
        //txt.text = "Dummy";
    }

    public void addDropdown(LayoutGroup layout, XmlReader reader)
    {
        Debug.Assert(reader.Name.Equals("Dropdown"));
        add(UIElement.Dropdown, layout);
    }

    public void addSlider(LayoutGroup layout, XmlReader reader)
    {
        Debug.Assert(reader.Name.Equals("Slider"));

        string boundVariable = reader.ReadElementContentAsString();
        Vector2 range = new Vector2(0, 1);
        if (target != null && target.parameterHasRange(boundVariable))
        {
            range = target.parameterRange(boundVariable);
        }
        else
        {
            Debug.LogWarning("Parameter for slider should have a range!");
        }

        GameObject go = add(UIElement.Slider, layout);
        go.GetComponent<Slider>().onValueChanged.AddListener(
            (float value) => {
                if (target != null) target.setParameter(boundVariable, value);
                Debug.Log("Slide!");
            }
        );
        //Slider txt = newElement(layout).AddComponent<Slider>();
        //txt.value = 2;
    }


    public void addButton(LayoutGroup layout, XmlReader reader)
    {
        Debug.Assert(reader.Name.Equals("Button"));

        GameObject go = add(UIElement.Button, layout);
        go.GetComponent<Button>().onClick.AddListener(
            () => {
                Debug.Log("Click!");
            }
        );
    }

    public GridLayoutGroup addGrid(LayoutGroup layout, XmlReader reader)
    {
        Debug.Assert(reader.Name.Equals("Grid"));

        int sizeX = 20, sizeY = 100;
        if (reader.GetAttribute("size") != null)
        {
            string[] size = reader.GetAttribute("size").Split(',');
            sizeX = XmlConvert.ToInt32(size[0]);
            sizeY = XmlConvert.ToInt32(size[1]);
        }

        GridLayoutGroup glg;
        if (layout == null) glg = gameObject.AddComponent<GridLayoutGroup>();
        else
        {
            GameObject go = new GameObject();
            go.transform.SetParent(layout.transform);
            glg = go.AddComponent<GridLayoutGroup>();
        }

        glg.cellSize = new Vector2(sizeY, sizeX);
        glg.constraint = GridLayoutGroup.Constraint.Flexible;
        if (reader.GetAttribute("cols") != null)
        {
            glg.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            glg.constraintCount = XmlConvert.ToInt32(reader.GetAttribute("cols"));
        }
        else if (reader.GetAttribute("rows") != null)
        {
            glg.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            glg.constraintCount = XmlConvert.ToInt32(reader.GetAttribute("rows"));
        }

        return glg;
    }
}
