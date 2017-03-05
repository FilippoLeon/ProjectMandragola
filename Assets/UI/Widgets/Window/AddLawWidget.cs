using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddLawWidget : DynamicWidget {

	// Use this for initialization
	void Start () {

        //LayoutGroup l = addLayout<HorizontalLayoutGroup>();
        //GameObject  view = add(UIElement.ScrollView, l);

        LayoutGroup l2 = addScrollView();
        setMinSize(gameObject.GetComponentInChildren<ScrollRect>().gameObject, 80, 200);
        foreach(Law law in LawManager.lawPrototypes.Values)
        {
            LayoutGroup l3 = addLayout<HorizontalLayoutGroup>(l2);
            GameObject go = add(UIElement.Label, l3);
            go.GetComponent<Text>().text = law.name;
            GameObject go2 = add(UIElement.Label, l3);
            go2.GetComponent<Text>().text = law.description;
            GameObject go3 = add(UIElement.Button, l3);
            setMinSize(go3, 100, -1);
            go3.GetComponentInChildren<Text>().text = "Propose";
            go3.GetComponent<Button>().onClick.AddListener(
                () => {
                LawManager.Instance.propose(law.id);
                Destroy(l3.gameObject);
                }
            );
        }
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
