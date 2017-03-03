﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

// Read and contains all prototypes and all bodies
public class GovernmentManager
{
    static public List<Body> bodyPrototypes;
    static public List<Power> powerPrototypes;
    static public List<Government> governmentPrototypes;

    public Government currentGovernment;

    public GovernmentManager() {

        if (governmentPrototypes == null)
        {
            governmentPrototypes = new List<Government>();
            bodyPrototypes = new List<Body>();
            powerPrototypes = new List<Power>();

            string path = Application.streamingAssetsPath + "/Government.xml";

            XmlReaderSettings settings = new XmlReaderSettings();
            XmlReader reader = XmlReader.Create(path, settings);
            reader.MoveToContent();
            while (reader.Read())
            {
                XmlNodeType nodeType = reader.NodeType;
                switch (nodeType) {
                    case XmlNodeType.Element:
                        //Debug.Log(reader.Name);
                        if (reader.Name.Equals("Government")) {
                            //Debug.Log(reader.Name);
                            Government gov = new Government();
                            gov.ReadXml(reader.ReadSubtree());
                            governmentPrototypes.Add(gov);
                            currentGovernment = gov;
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
    
}
