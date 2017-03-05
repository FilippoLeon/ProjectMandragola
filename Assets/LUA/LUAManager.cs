using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using System.IO;
using System;

public class LUAManager : MonoBehaviour {

    protected Script script;
    private string scriptName;

    private string directory = "Scripts";
    private string filename = "laws.lua";

    public static LUAManager Instance;

    void Start() {
        Instance = this;

        UserData.RegisterAssembly();

        script = new Script();

        script.Options.DebugPrint += (string str) =>
        {
            Debug.Log("LUA: " + str);
        };

        FileInfo info = new FileInfo(
            Path.Combine(Application.streamingAssetsPath, 
                Path.Combine(directory, filename)
            )
        );
        Load(info);

        Debug.Log("Test call to LUA!");
        DynValue ret = Call("generic", "test");
        Debug.Assert(ret.CastToBool() == true);
        
    }

    // Update is called once per frame
    void Update() {

    }

    public void Load(FileInfo file) {
        StreamReader reader = new StreamReader(file.OpenRead());
        string textScript = reader.ReadToEnd();

        try
        {
            script.DoString(textScript);
        }
        catch (SyntaxErrorException e)
        {
            Debug.LogError("LUA error: " + e.DecoratedMessage);
        }
    }

    public DynValue Call(string category, string function, params object[] args)
    {
        object func = script.Globals[function];
        try
        {
            return script.Call(func, args);
        }
        catch (ScriptRuntimeException e)
        {
            Debug.LogError("Script exception: " + e.DecoratedMessage);
            return null;
        }
        catch (ArgumentException e)
        {
            Debug.LogError("Script exception while running '" + function + "': " + e.Message);
            return null;
        }
    }
}
