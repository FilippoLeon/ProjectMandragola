using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class CloneUtilities {
    static public Dictionary<string, T> CloneDictionary<T>(Dictionary<string, T> dict) where T : ICloneable
    {
        Dictionary<string, T> clone = dict.ToDictionary(
            x =>  x.Key, x => (T) x.Value.Clone()
        );

        return clone;
    }
}
