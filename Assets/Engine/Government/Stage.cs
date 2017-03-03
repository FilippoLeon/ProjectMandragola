using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

public class Stage : IXmlSerializable
{
    string stageType;
    enum StageType
    {
        Propose, Enact, Enforce, Veto, Repeal, Modify
    }
    enum RequirementType
    {
        Any, All
    }

    StageType type;
    RequirementType requirementType;
    List<Body> requiredBodies;
    Power parent;

    Stage() { }

    Stage(Power parent_) { parent = parent_; }

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

    void IXmlSerializable.ReadXml(XmlReader reader)
    {
        reader.MoveToContent();

        stageType = reader.GetAttribute("type");

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
    }

    void IXmlSerializable.WriteXml(XmlWriter writer)
    {
        throw new NotImplementedException();
    }
}