using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class LawGUIManager : MonoBehaviour {

    public GameObject dynamicWidgetPrototype;
    static Dictionary<string, GameObject> lawGUIs = new Dictionary<string, GameObject>();

    public static LawGUIManager Instance;
    
	void Start () {
        if(Instance != null)
        {
            Debug.LogWarning("LawGUIManager is supposed to have only one instance");
        }
        Instance = this;
        LawManager.registeredDispatchers["Gui"] = prepareGui;
    }
	
    /// <summary>
    /// Reads an XmlSubtree an a law and prepares a GUI (with windows) to 
    /// represent the Law.
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="law"></param>
    public void prepareGui(XmlReader reader, Law law)
    {
        // TODO: Only one window and many widget?

        GameObject gowin = WindowManager.openEmptyWindow(law.id + "_window");
        GameObject go = Instantiate(dynamicWidgetPrototype);
        WindowWidget window = gowin.GetComponent<WindowWidget>();
        window.content = go;
        if(go.GetComponent<DynamicXmlWidget>() == null)
        {
            Debug.LogError("Missing DynamicXMlWindow component.");
            return;
        }
        DynamicXmlWidget law_widget = go.GetComponent<DynamicXmlWidget>();

        //proto.moveToCanvas();
        go.name = law.id + "_widget";
        law_widget.target = law;
        law_widget.buildGui(reader);

        window.minimumSize = law_widget.minimumSize + WindowWidget.margin;

        lawGUIs[law.id] = go;
    }
}
