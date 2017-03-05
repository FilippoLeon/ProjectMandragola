
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;
using MoonSharp.Interpreter;

[MoonSharpUserData]
public class Law: IXmlSerializable, IParametrizable, ICloneable {
    public string id, name;
    public string description;
    public Dictionary<string, IGenericParameter> parameters = new Dictionary<string, IGenericParameter>();
    public Dictionary<string, IGenericAction> actions = new Dictionary<string, IGenericAction>();

    public interface IGenericParameter : ICloneable { }
    public interface IGenericAction : ICloneable { }

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

        public object Clone()
        {
            return this.MemberwiseClone();
            //throw new NotImplementedException();
        }
    }
    public class GenericAction : IGenericAction
    {
        public string eventName;
        public string value;
        public string eventType;

        public GenericAction(string eventName, string actionName, string eventType = "lua")
        {
            this.eventName = eventName;
            this.value = actionName;
            this.eventType = eventType;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
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
                        string dv = reader.GetAttribute("default");
                        string range = reader.GetAttribute("range");
                        string name = reader.ReadElementContentAsString();
                        parameters[name] = new GenericParameter(paramType, dv, range);
                    }
                    else if (reader.Name.Equals("Action"))
                    {
                        string eventType = reader.GetAttribute("type");
                        if (eventType == null) eventType = "lua";
                        string eventName = reader.GetAttribute("event");
                        string actionName = reader.ReadElementContentAsString();
                        actions[eventName] = new GenericAction(eventName, actionName, eventType);
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

    public void setParameter<T>(string paramname, T value)
    {
        Debug.Log(string.Format("Setting parameter {0} of {1} to {2}.", paramname, this.name, value));
        (parameters[paramname] as GenericParameter).value = value.ToString();
    }

    public int getParameterAsInt(string paramname) 
    {
        //(parameters[paramname] as GenericParameter).value = value.ToString();
        if(!parameters.ContainsKey(paramname))
        {
            Debug.LogError("No parameter with this name!");
        }
        return Convert.ToInt32((parameters[paramname] as GenericParameter).value);
    }

    public void OnEvent(string eventname, object[] args)
    {
        if(actions.ContainsKey(eventname) && (actions[eventname] as GenericAction) != null)
        {
            GenericAction act = (actions[eventname] as GenericAction);
            if (act.eventType.Equals("lua"))
            {
                // Dispatch Call with saeme args (law is usually included in args[0]
                if (args == null) args = new object[] { };
                LUAManager.Instance.Call("law", act.value, args);
            }
        } else
        {
            //Debug.Log("Law does not contain action " + eventname);
        }
    }

    public object Clone()
    {
        Law clone = new Law();
        clone.id = id.Clone() as string;
        clone.name = name.Clone() as string;
        clone.description = description.Clone() as string;
        clone.parameters = CloneUtilities.CloneDictionary(parameters);
        clone.actions = CloneUtilities.CloneDictionary(actions);
        return clone;
    }
}
