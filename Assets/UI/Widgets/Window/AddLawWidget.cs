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
        foreach(Law law in WorldController.Instance.world.thisCountry.laws.availableLaws.Values)
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
                    WorldController.Instance.world.thisCountry.laws.propose(law.id);
                    WorldController.Instance.world.thisCountry.laws.enact(law.id);
                    // REMOVE ME: Enable for every available country (to test performance)
                    foreach(Country ctry in WorldController.Instance.world.countries)
                    {
                        if (WorldController.Instance.world.thisCountry == ctry) continue;

                        ctry.laws.propose(law.id);
                        ctry.laws.enact(law.id);
                    }
                    Destroy(l3.gameObject);
                }
            );
        }
        //add(UIElement.Label, l2);
        //add(UIElement.Label, l2);
        //add(UIElement.Label, l2);
        //add(UIElement.Label, l2);
        //add(UIElement.Label, l2);
        //add(UIElement.Label, l2);
        //add(UIElement.Label, l2);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
