using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

public class Body : IXmlSerializable, ICloneable
{
    // Manages duration/conditions for term duration
    public class Term
    {
        public string type; // LUA; int?
        public string value;
    }
    public bool termConditionAll = false;
    public Dictionary<string, Term> terms;
    private Government government;

    // Manages a "Decision" type, i.e. the mean by which the body makes a decision
    public class Decision
    {
        public string id;
        public string type;
        public string value;
    }
    public Dictionary<string, Decision> decisions;

    public enum Type { User, Chamber, God, People };

    public Government.Branch branch = Government.Branch.None;
    public Type bodyType;
    public string id, name, type;


    public Body(Government government)
    {
        this.government = government;
    }

    XmlSchema IXmlSerializable.GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        reader.MoveToContent();

        id = reader.GetAttribute("id");
        Debug.Assert(id != null, string.Format("Invalid Body id, name = {0}", name));
        name = reader.GetAttribute("name");
        if (name == null) name = id;
        type = reader.GetAttribute("type");

        while (reader.Read())
        {
            XmlNodeType nodeType = reader.NodeType;
            switch (nodeType)
            {
                case XmlNodeType.Element:
                    if (reader.Name.Equals("Management"))
                    {
                        BodyManagement bm = new BodyManagement(this);
                        XmlReader subtree = reader.ReadSubtree();
                        bm.ReadXml(subtree);
                        government.addPower(bm);
                        subtree.Close();
                    }
                    break;
                case XmlNodeType.EndElement:
                default:
                    break;
            }
        }
        //Debug.Log("New body with id = " + id);
    }

    public void WriteXml(XmlWriter writer)
    {
        throw new NotImplementedException();
    }
    public object Clone()
    {
        Body body = new Body(government);
        body.Copy(this);
        return body;
    }

    public void Copy(Body body)
    {
        government = body.government;
        branch = body.branch;
        id = body.id;
        name = body.name;
        bodyType = body.bodyType;
    }
}
