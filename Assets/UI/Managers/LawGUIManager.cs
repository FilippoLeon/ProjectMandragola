using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class LawGUIManager : MonoBehaviour {

    public GameObject dynamicWidgetPrototype;
    static Dictionary<string, GameObject> lawGUIs = new Dictionary<string, GameObject>();

    static LawGUIManager Instance;
    
	void Start () {
        if(Instance != null)
        {
            Debug.LogWarning("LawGUIManager is supposed to have only one instance");
        }
        Instance = this;
        LawManager.registeredDispatchers["Gui"] = prepareGui;
	}
	
    void prepareGui(XmlReader reader, Law law)
    {
        GameObject win = WindowManager.openEmptyWindow(law.id + "_window");
        GameObject go = Instantiate(dynamicWidgetPrototype);
        win.GetComponent<WindowWidget>().content = go;
        if(go.GetComponent<DynamicXmlWindow>() == null)
        {
            Debug.LogError("Missing DynamicXMlWindow component.");
            return;
        }
        DynamicXmlWindow proto = go.GetComponent<DynamicXmlWindow>();

        //proto.moveToCanvas();
        go.name = law.id + "_widget";
        proto.target = law;
        proto.buildGui(reader);
        lawGUIs[law.id] = go;
    }
}
