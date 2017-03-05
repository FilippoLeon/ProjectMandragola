using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Laws
{
    public Dictionary<string, Law> availableLaws;
    public Dictionary<string, Law> proposedLaws = new Dictionary<string, Law>();
    public Dictionary<string, Law> activeLaws = new Dictionary<string, Law>();
    
    public Laws () {
        availableLaws = CloneUtilities.CloneDictionary(LawManager.lawPrototypes);
    }

    public void tic()
    {
        foreach (Law activeLaw in activeLaws.Values)
        {
            activeLaw.OnEvent("OnTic", null);
        }
    }

    public void propose(string id)
    {
        Debug.Log("Law " + id + " is being proposedd.");
        if (!availableLaws.ContainsKey(id))
        {
            Debug.LogError("Cannot find law " + id + " in prototypes.");
        }
        else
        {
            Law law = availableLaws[id];
            proposedLaws[id] = law;
            law.OnEvent("OnPropose", null);
            availableLaws.Remove(id);
        }
    }
    public void veto(string id)
    {
        Debug.Log("Law " + id + " is being vetoed.");
        if (!proposedLaws.ContainsKey(id))
        {
            Debug.LogError("Cannot find law " + id + " in proposed laws.");
        }
        else
        {
            Law law = proposedLaws[id];
            availableLaws[id] = law;
            law.OnEvent("OnVeto", null);
            proposedLaws.Remove(id);
        }
    }

    public void enact(string id)
    {
        if (!proposedLaws.ContainsKey(id))
        {
            Debug.LogError("Cannot find law " + id + " in proposed laws.");
        }
        else
        {
            Law law = proposedLaws[id];
            activeLaws[id] = law;
            law.OnEvent("OnEnact", null);
            proposedLaws.Remove(id);
        }
    }

    public void remove(string id)
    {
        if (!activeLaws.ContainsKey(id))
        {
            Debug.LogError("Cannot find law " + id + " in active laws.");
        }
        else
        {
            Law law = activeLaws[id];
            availableLaws[id] = law;
            law.OnEvent("OnRemove", null);
            activeLaws.Remove(id);
        }
    }
}
