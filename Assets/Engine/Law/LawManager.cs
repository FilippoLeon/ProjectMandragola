﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System;

// Read and contains all prototypes and all bodies
public class LawManager : IPostInitialized
{
    static string directory = "Data";
    static string governmentLawFile = "Laws.xml";

    static public Dictionary<string, Law> lawPrototypes;
    
    public static Dictionary<string, Action<XmlReader, Law>> registeredDispatchers = 
        new Dictionary<string, Action<XmlReader, Law>>();
    
    //public Government currentGovernment;

    public LawManager()
    {
        WorldController.register(this);
        //LawManager.registeredDispatchers["Gui"] += LawGUIManager.Instance.prepareGui;
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
        foreach(Country ctry in WorldController.Instance.world.countries)
        {
            ctry.laws = new Laws();
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
    
    //public void modify(string name)
    //{

    //}
}
