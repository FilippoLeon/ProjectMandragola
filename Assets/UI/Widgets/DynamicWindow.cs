using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class DynamicWindow : MonoBehaviour {
    public GameObject labelPrototype;
    public GameObject fieldPrototype;
    public GameObject sliderPrototype;
    public GameObject dropdownPrototype;
    public GameObject buttonPrototype;

    // Use this for initialization
    void Start () {
        buildWidgets(Path.Combine(Application.streamingAssetsPath, Path.Combine("Data", "Laws.xml")));
	}
	
	// Update is called once per frame
	void Update () {
		
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
                        GridLayoutGroup grid = addGrid(layout, 2, 2);
                        XmlReader subtree = reader.ReadSubtree();
                        addRoot(grid, subtree);
                    } else if (reader.Name.Equals("Label"))
                    {
                        addLabel(layout);
                    }
                    else if (reader.Name.Equals("Field"))
                    {
                        addField(layout);
                    }
                    else if (reader.Name.Equals("Slider"))
                    {
                        addSlider(layout);
                    }
                    break;
                case XmlNodeType.EndElement:
                default:
                    //Debug.Log(reader.Name as string);
                    break;
            }
        }
    }

    public GameObject newElement(LayoutGroup layout)
    {
        GameObject go = new GameObject();
        go.AddComponent<LayoutElement>();
        go.transform.SetParent(layout.transform);
        return go;
    }

    public void addLabel(LayoutGroup layout)
    {
        GameObject go = Instantiate(labelPrototype);
        go.transform.SetParent(layout.transform);
        go.SetActive(true);
        //Text txt = newElement(layout).AddComponent<Text>();
        //txt.text = "Dummy";
    }

    public void addField(LayoutGroup layout)
    {
        GameObject go = Instantiate(fieldPrototype);
        go.transform.SetParent(layout.transform);
        go.SetActive(true);
        //InputField txt = newElement(layout).AddComponent<InputField>();
        //txt.text = "Dummy";
    }

    public void addSlider(LayoutGroup layout)
    {
        GameObject go = Instantiate(sliderPrototype);
        go.transform.SetParent(layout.transform);
        go.SetActive(true);
        //Slider txt = newElement(layout).AddComponent<Slider>();
        //txt.value = 2;
    }
    
    public GridLayoutGroup addGrid(LayoutGroup layout, int x, int y)
    {
        if (layout == null) return gameObject.AddComponent<GridLayoutGroup>();
        GameObject go = new GameObject();
        GridLayoutGroup glg = go.AddComponent<GridLayoutGroup>();
        go.transform.SetParent(layout.transform);
        return glg;
    }
}
