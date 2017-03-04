using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Linq;
using UnityEngine;

public class Stage : IXmlSerializable, ICloneable
{
    public enum RequirementType
    {
        Any = 0, All = 1
    }

    public string name, id;
    public RequirementType requirementType;
    public List<string> requiredBodies = new List<string>();
    public Power parent;

    public Stage() { }

    public Stage(Power parent_) { parent = parent_; }

    bool canTrigger()
    {
        return true;
    }

    void trigger()
    {

    }

    XmlSchema IXmlSerializable.GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        reader.MoveToContent();

        id = reader.GetAttribute("id");
        name = reader.GetAttribute("name");
        if (name == null) name = id;

        while (reader.Read())
        {
            XmlNodeType nodeType = reader.NodeType;
            switch (nodeType)
            {
                case XmlNodeType.Element:
                    if(reader.Name.Equals("Require"))
                    {
                        requirementType = (RequirementType)
                            Enum.Parse(typeof(RequirementType), reader.GetAttribute("type"), 
                            ignoreCase: true);
                        XmlReader subtree = reader.ReadSubtree();
                        subtree.MoveToContent();
                        while (subtree.Read())
                        {
                            if( subtree.NodeType == XmlNodeType.Element && 
                                subtree.Name.Equals("Body"))
                            {
                                requiredBodies.Add(subtree.GetAttribute("ref"));
                            }
                        }
                        subtree.Close();
                    }
                    break;
                case XmlNodeType.EndElement:
                default:
                    break;
            }
        }

        Debug.Log(string.Format("New stage {0} type {1} and {2}", 
            id, requirementType,
            string.Join(",", requiredBodies.ToArray())));
    }

    void IXmlSerializable.WriteXml(XmlWriter writer)
    {
        throw new NotImplementedException();
    }

    public object Clone()
    {
        Stage stage = new Stage();
        stage.parent = null;
        stage.id = id;
        stage.name = name;
        stage.requirementType = requirementType;
        stage.requiredBodies = new List<string>(requiredBodies.Select(x => x.Clone() as  string));

        return stage;
    }
}