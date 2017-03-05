using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

public class Law: IXmlSerializable, IParametrizable {
    public string id, name;
    public Dictionary<string, IGenericParameter> parameters = new Dictionary<string, IGenericParameter>();
    public Dictionary<string, IGenericAction> actions = new Dictionary<string, IGenericAction>();

    public interface IGenericParameter { }
    public interface IGenericAction { }

    //public XmlReader guiSubtree;

    public class GenericParameter : IGenericParameter
    {
        public GenericParameter(string type_, string defaultValue_)
        {
            type = type_;
            defaultValue = defaultValue_;
        }

        public string type;
        public string defaultValue;
        public string value;
        public string range;
    }
    public class GenericAction : IGenericAction
    {
        string type;
        string value;

        public GenericAction(string paramType, string actionName)
        {
            this.type = paramType;
            this.value = actionName;
        }
    }

    public Law()
    {

    }

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
                        string paramType = reader.GetAttribute("type");
                        string dv = reader.GetAttribute("default?value");
                        string range = reader.GetAttribute("range");
                        string name = reader.ReadElementContentAsString();
                        parameters[name] = new GenericParameter(paramType, dv);
                    }
                    else if (reader.Name.Equals("Action"))
                    {
                        string paramType = reader.GetAttribute("type");
                        string actionName = reader.ReadElementContentAsString();
                        actions[paramType] = new GenericAction(paramType, actionName);
                    } else if (reader.Name.Equals("Gui"))
                    {
                        LawManager.treeDispatcher("Gui", reader.ReadSubtree(), this);
                        //guiSubtree = reader.ReadSubtree();
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

    public Vector2 parameterRange(string param)
    {
        string[] rng = (parameters[param] as GenericParameter).range.Split(',');
        Vector2 vec = new Vector2(Convert.ToSingle(rng[0]), Convert.ToSingle(rng[1]));
        return vec;
    }

    public bool parameterHasRange(string param)
    {
        return (parameters[param] as GenericParameter).range != null;
    }

    public void setParameter<T>(string name, T value)
    {
        (parameters[name] as GenericParameter).value = value.ToString();
    }
}
