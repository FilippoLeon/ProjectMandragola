using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Linq;
using UnityEngine;

public class Power : IXmlSerializable, ICloneable
{
    public Government.Branch branch = Government.Branch.None;
    public string id;
    public string name;
    // Type of power, delegate to subclass? TODO
    public string type; 
    public List<Stage> stages = new List<Stage>();

    XmlSchema IXmlSerializable.GetSchema()
    {
        return null;
    }

    virtual public void ReadXml(XmlReader reader)
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
                    if (reader.Name.Equals("Stage")) {
                        Stage stage = new Stage(this);
                        stage.ReadXml(reader.ReadSubtree());
                        stages.Add(stage);
                    }
                    break;
                case XmlNodeType.EndElement:
                default:
                    break;
            }
        }
        //Debug.Log("New power with id = " + id + " and stages " + stages.Count);
    }

    public void WriteXml(XmlWriter writer)
    {
        throw new NotImplementedException();
    }

    virtual public object Clone()
    {
        Power pow = new Power();
        pow.branch = branch;
        pow.id = id;
        pow.name = name;
        pow.stages = new List<Stage>(
            stages.Select((Stage x) => {
                Stage y = x.Clone() as Stage;
                y.parent = this;
                return y;
            }
            )
        );

        return pow;
    }

    public void Copy(Power other)
    {
        branch = other.branch;
        id = other.id;
        name = other.name;
        stages = new List<Stage>(
            other.stages.Select((Stage x) => {
                Stage y = x.Clone() as Stage;
                y.parent = this;
                return y;
            }
            )
        );
    }
}