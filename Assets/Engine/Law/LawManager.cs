using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System;

// Read and contains all prototypes and all bodies
public class LawManager : IPostInitialized, ITickable
{
    static public Dictionary<string, Law> lawPrototypes;
    static public Dictionary<string, Law> proposedLaws = new Dictionary<string, Law>();
    static public Dictionary<string, Law> activeLaws = new Dictionary<string, Law>();

    static string directory = "Data";
    static string governmentLawFile = "Laws.xml";

    public static Dictionary<string, Action<XmlReader, Law>> registeredDispatchers = 
        new Dictionary<string, Action<XmlReader, Law>>();

    static public LawManager Instance;
    //public Government currentGovernment;

    public LawManager()
    {
        if (Instance != null) Debug.LogError("Multiple instances of LawManager.");
        Instance = this;
        WorldController.register(this);
        //LawManager.registeredDispatchers["Gui"] += LawGUIManager.Instance.prepareGui;
    }

    public void tic()
    {
        foreach(Law activeLaw in activeLaws.Values)
        {
            activeLaw.OnEvent("OnUpdate");
        }
    }

    /// <summary>
    /// Called after all world objects have been started
    /// </summary>
    public void postInit()
    {
        if (lawPrototypes == null)
        {
            loadLawPrototypes(Path.Combine(Application.streamingAssetsPath, Path.Combine(directory, governmentLawFile)));
        }
    }

    void loadLawPrototypes(string path)
    {
        lawPrototypes = new Dictionary<string, Law>();

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
                    if (reader.Name.Equals("Law"))
                    {
                        //Debug.Log(reader.Name);
                        Law law = new Law();
                        XmlReader subtree = reader.ReadSubtree();
                        law.ReadXml(subtree);
                        lawPrototypes[law.id] = law;
                        subtree.Close();
                        //currentGovernment = gov;
                    }
                    break;
                case XmlNodeType.EndElement:
                default:
                    //Debug.Log(reader.Name as string);
                    break;
            }
        }
    }

    static public void treeDispatcher(string name, XmlReader subtree, Law law)
    {
        if (!registeredDispatchers.ContainsKey(name))
        {
            Debug.LogWarning("Key " + name + " not registered in dispatcher.");
        } else if (LawManager.registeredDispatchers[name] != null) {
            LawManager.registeredDispatchers[name](subtree, law);
        } else
        {
            Debug.LogWarning("Dispatcher already registered.");
        }
        //subtree.Close();
        return;
    }

    public void propose(string id)
    {
        Debug.Log("Law " + id + " is being proposedd.");
        if (!lawPrototypes.ContainsKey(id))
        {
            Debug.LogError("Cannot find law " + id + " in prototypes.");
        } else
        {
            Law law = lawPrototypes[id];
            proposedLaws[id] = law;
            law.OnEvent("OnPropose");
            lawPrototypes.Remove(id);
        }
    }
    public void veto(string id)
    {
        Debug.Log("Law " + id + " is being vetoed.");
        if (!proposedLaws.ContainsKey(id))
        {
            Debug.LogError("Cannot find law " + id + " in proposed laws.");
        }
        else
        {
            Law law = proposedLaws[id];
            lawPrototypes[id] = law;
            law.OnEvent("OnVeto");
            proposedLaws.Remove(id);
        }
    }

    public void enact(string id)
    {
        if (!proposedLaws.ContainsKey(id))
        {
            Debug.LogError("Cannot find law " + id + " in proposed laws.");
        }
        else
        {
            Law law = proposedLaws[id];
            activeLaws[id] = law;
            law.OnEvent("OnEnact");
            proposedLaws.Remove(id);
        }
    }

    public void remove(string id)
    {
        if (!activeLaws.ContainsKey(id))
        {
            Debug.LogError("Cannot find law " + id + " in active laws.");
        }
        else
        {
            Law law = activeLaws[id];
            lawPrototypes[id] = law;
            law.OnEvent("OnRemove");
            activeLaws.Remove(id);
        }
    }
    //public void modify(string name)
    //{

    //}
}
