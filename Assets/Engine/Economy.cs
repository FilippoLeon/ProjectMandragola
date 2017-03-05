using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

[MoonSharpUserData]
public class Economy {

    public int old_funds = 0;
    public int funds = 0;
    
    public void tic()
    {
        old_funds = funds;
    }
}
