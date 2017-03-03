using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

public class Power : IXmlSerializable
{
    public Government.Branch branch = Government.Branch.None;
    public string id;
    public string name;
    public List<Stage> stage = new List<Stage>();

    XmlSchema IXmlSerializable.GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        reader.MoveToContent();

        id = reader.GetAttribute("id");
        name = reader.GetAttribute("name");

        while (reader.Read())
        {
            XmlNodeType nodeType = reader.NodeType;
            switch (nodeType)
            {
                case XmlNodeType.Element:
                    break;
                case XmlNodeType.EndElement:
                default:
                    break;
            }
        }
        Debug.Log("New power with id = " + id + " and stages " + stage.Count);
    }

    public void WriteXml(XmlWriter writer)
    {
        throw new NotImplementedException();
    }
}
