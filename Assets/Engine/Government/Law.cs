using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

public class Law: IXmlSerializable {
    public string id, name;
    public Dictionary<string, IGenericParameter> parameters;
    public Dictionary<string, IGenericAction> actions;

    public interface IGenericParameter { }
    public interface IGenericAction { }

    public class GenericParameter : IGenericParameter
    {
        public GenericParameter(System.Type type_, string defaultValue_)
        {
            type = type_;
            defaultValue = defaultValue_;
        }

        System.Type type;
        string defaultValue;
        string value;
    }
    public class GenericAction : IGenericAction { }

    public XmlSchema GetSchema()
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
                    if (reader.Name.Equals("Parameter"))
                    {
                        System.Type paramType = Type.GetType(reader.GetAttribute("type"));
                        string dv = reader.GetAttribute("default?value");
                        string name = reader.GetAttribute("name");
                        parameters[name] = new GenericParameter(paramType, dv);
                    }
                    break;
                case XmlNodeType.EndElement:
                default:
                    break;
            }
        }
    }

    public void WriteXml(XmlWriter writer)
    {
        throw new NotImplementedException();
    }
}
