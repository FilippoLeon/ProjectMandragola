using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddLawWidget : DynamicWindow {

	// Use this for initialization
	void Start () {

        //LayoutGroup l = addLayout<HorizontalLayoutGroup>();
        //GameObject  view = add(UIElement.ScrollView, l);

        LayoutGroup l2 = addScrollView();
        add(UIElement.Label, l2);
        add(UIElement.Label, l2);
        add(UIElement.Label, l2);
        add(UIElement.Label, l2);
        add(UIElement.Label, l2);
        add(UIElement.Label, l2);
        add(UIElement.Label, l2);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
