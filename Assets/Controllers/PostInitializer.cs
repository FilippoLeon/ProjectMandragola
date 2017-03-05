using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostInitializer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        foreach(IPostInitialized pos in WorldController.postinitialized)
            pos.postInit();
	}
	
}
