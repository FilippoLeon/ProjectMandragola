using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

public class Body : IXmlSerializable
{
    public enum Type { User, Chamber, God, People };
    public Government.Branch branch = Government.Branch.None;
    public Type bodyType;
    public string id, name, type;

    XmlSchema IXmlSerializable.GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        reader.MoveToContent();

        id = reader.GetAttribute("id");
        name = reader.GetAttribute("name");
        type = reader.GetAttribute("type");

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
        Debug.Log("New body with id = " + id);
    }

    public void WriteXml(XmlWriter writer)
    {
        throw new NotImplementedException();
    }
}
