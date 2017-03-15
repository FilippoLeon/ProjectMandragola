using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System;

/// <summary>
/// A power that is specialized in managing government bodies: appointment, etc..
/// 
/// Has a custom Xml Reader.
/// </summary>
public class BodyManagement : Power
{
    Body body;

    public BodyManagement(Body body)
    {
        this.body = body;
    }

    override public void ReadXml(XmlReader reader)
    {
        reader.MoveToContent();

        id = body.id + "_management";
        name = body.name + "_management";

        base.ReadXml(reader);

        Debug.Log("New body power with id = " + id + " and stages " + stages.Count + "  body = " + body.name);
    }

    override public object Clone()
    {
        BodyManagement bm = new BodyManagement(body);
        bm.Copy(this);
        return bm;
        //base.Clone();
        //return new object();
        //throw new NotImplementedException();
    }
}
