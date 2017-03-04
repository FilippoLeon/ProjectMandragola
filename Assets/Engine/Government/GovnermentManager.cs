using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

// Read and contains all prototypes and all bodies
public class GovernmentManager
{
    static public List<Body> bodyPrototypes;
    static public List<Power> powerPrototypes;
    static public List<Government> governmentPrototypes;

    static public Dictionary<string, Law> lawPrototypes;

    static string directory = "Data";
    static string governmentPrototypesFile = "Government.xml";
    static string governmentLawFile = "Laws.xml";

    //public Government currentGovernment;

    public GovernmentManager() {
        
        if (governmentPrototypes == null)
        {
            loadGovernmentPrototypes(Path.Combine(Application.streamingAssetsPath, Path.Combine(directory, governmentPrototypesFile)));
        }
        if(lawPrototypes == null)
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
                        law.ReadXml(reader.ReadSubtree());
                        lawPrototypes[law.id] = law;
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

    void loadGovernmentPrototypes(string path)
    {
            governmentPrototypes = new List<Government>();
            bodyPrototypes = new List<Body>();
            powerPrototypes = new List<Power>();

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
                        if (reader.Name.Equals("Government"))
                        {
                            //Debug.Log(reader.Name);
                            Government gov = new Government();
                            gov.ReadXml(reader.ReadSubtree());
                            governmentPrototypes.Add(gov);
                            //currentGovernment = gov;
                        }
                        else if (reader.Name.Equals("Body"))
                        {
                            Body body = new Body();
                            body.ReadXml(reader.ReadSubtree());
                            bodyPrototypes.Add(body);
                        }
                        else if (reader.Name.Equals("Power"))
                        {
                            Power power = new Power();
                            power.ReadXml(reader.ReadSubtree());
                            powerPrototypes.Add(power);
                        }
                        break;
                    case XmlNodeType.EndElement:
                    default:
                        //Debug.Log(reader.Name as string);
                        break;
                }
            }

        }
    
}
