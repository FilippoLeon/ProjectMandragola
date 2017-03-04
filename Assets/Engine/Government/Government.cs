using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Linq;
using UnityEngine;

public class Government : IXmlSerializable, ICloneable
{
    public enum Branch { None, Legislative, Executive, Judiciary };
    public List<Body> bodies = new List<Body>();
    public List<Power> powers = new List<Power>();
    public Body user;

    public Action onAddBody;
    public Action onAddPower;

    public string id, type, name;
    //private Government government;

    XmlSchema IXmlSerializable.GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        Branch branch = Branch.None;

        reader.MoveToContent();

        id = reader.GetAttribute("id");
        name = reader.GetAttribute("name");
        type = reader.GetAttribute("type");

        Debug.Log(string.Format("New goverment {0} with id {1} and name {2}", type, id, name));

        while (reader.Read())
        {
            XmlNodeType nodeType = reader.NodeType;
            switch (nodeType)
            {
                case XmlNodeType.Element:
                    if (reader.Name.Equals("Body"))
                    {
                        //Debug.Log(reader.Name);
                        //Debug.Log(reader.Name);
                        Body body = new Body();
                        body.branch = branch;
                        body.ReadXml(reader.ReadSubtree());
                        bodies.Add(body);
                        if (body.id != null && body.id.Equals("user")) user = body;
                    } else if (reader.Name.Equals("Power"))
                    {
                        //Debug.Log(reader.Name);
                        Power power = new Power();
                        power.branch = branch;
                        power.ReadXml(reader.ReadSubtree());
                        powers.Add(power);
                    } else if (reader.Name.Equals("Legislative"))
                    {
                        branch = Branch.Legislative;
                    } else if (reader.Name.Equals("Executive"))
                    {
                        branch = Branch.Executive;
                    } else if (reader.Name.Equals("Judiciary"))
                    {
                        branch = Branch.Judiciary;
                    }
                    break;
                case XmlNodeType.EndElement:
                    if (reader.Name.Equals("Legislative")
                        ||  reader.Name.Equals("Executive")
                        || reader.Name.Equals("Judiciary"))
                    {
                        branch = Branch.None;
                    }
                    break;
                default:
                    break;
            }
        }
        if (onAddBody != null) onAddBody();
        if (onAddPower != null) onAddPower();
    }

    public void WriteXml(XmlWriter writer)
    {
        throw new NotImplementedException();
    }



    public object Clone()
    {
        Government gov = new Government();

        gov.bodies = new List<Body>(bodies.Select(x => x.Clone() as Body));
        gov.powers = new List<Power>(powers.Select(x => x.Clone() as Power));
        gov.user = user;

        gov.id = id;
        gov.type = type;
        gov.name = name;

        return gov;
}
}
