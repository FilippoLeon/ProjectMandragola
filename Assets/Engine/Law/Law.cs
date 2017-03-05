using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

public class Law: IXmlSerializable, IParametrizable {
    public string id, name;
    public string description;
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
            value = defaultValue_;
        }
        public GenericParameter(string type_, string defaultValue_, string range_)
        {
            type = type_;
            defaultValue = defaultValue_;
            range = range_;
            value = defaultValue_;
        }

        public string type;
        public string defaultValue;
        public string value;
        public string range;
    }
    public class GenericAction : IGenericAction
    {
        public string type;
        public string value;

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
        //Debug.Log("New law " + id + " " + name);

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
                        parameters[name] = new GenericParameter(paramType, dv, range);
                    }
                    else if (reader.Name.Equals("Action"))
                    {
                        string actionType = reader.GetAttribute("type");
                        if (actionType == null) actionType = "lua";
                        string actionName = reader.ReadElementContentAsString();
                        actions[actionType] = new GenericAction(actionType, actionName);
                    } else if (reader.Name.Equals("Gui"))
                    {
                        XmlReader reader2 = reader.ReadSubtree();
                        LawManager.treeDispatcher("Gui", reader2, this);
                        reader2.Close();
                        //guiSubtree = reader.ReadSubtree();
                    }
                    else if (reader.Name.Equals("Description"))
                    {
                        description = reader.ReadElementContentAsString();
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

    public bool hasParameter(string param)
    {
        return parameters.ContainsKey(name);
    }

    public bool parameterHasRange(string param)
    {
        return (parameters[param] as GenericParameter).range != null;
    }

    public void setParameter<T>(string name, T value)
    {
        Debug.Log(string.Format("Setting parameter {0} of {1} to {2}.", name, this.name, value));
        (parameters[name] as GenericParameter).value = value.ToString();
    }

    public void OnEvent(string eventname)
    {
        if(actions.ContainsKey(eventname) && (actions[name] as GenericAction) != null)
        {
            GenericAction act = (actions[name] as GenericAction);
            if (act.type.Equals("lua"))
            {
                //LUAManager.run("law", act.value);
            }
        }
    }
}
