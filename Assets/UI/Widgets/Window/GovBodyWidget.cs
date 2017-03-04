﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GovBodyWidget : MonoBehaviour {

    public GameObject dropdownObject;
    private Dropdown dropdown;
    public GameObject worldControllerObject;
    private WorldController worldController;

    public Text govText;
    private Body selectedBody;

    public GameObject templateButton;
    public GameObject buttonPanel;

    private Government government_;
    public Government government
    {
        get {
            return government_;
        }
        set
        {
            if (government_ != null) {
                government_.onAddBody -= onAddBody;
                government_.onAddPower -= onAddPower;
            }
            government_ = value;
            if (government_ != null)
            {
                government_.onAddBody += onAddBody;
                government_.onAddPower += onAddPower;
                onChangeGovernment();
                onAddBody();
                onAddPower();
            }
        }
    }

    // Use this for initialization
    void Start () {
        dropdown = dropdownObject.GetComponent<Dropdown>();
        worldController = worldControllerObject.GetComponent<WorldController>();
	}
	
	// Update is called once per frame
	void Update () {
		if(worldController != null && government == null)
        {
            government = worldController.world.thisCountry.government;
        }
	}


    List<GameObject> buttons = new List<GameObject>();

    public void onSelectedBodyChanged(int idx)
    {
        if(government != null)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                Destroy(buttons[i]);
                buttons[i] = null;
            }
            buttons.Clear();

            selectedBody = government.bodies[idx];
            foreach(Power pow in government.powers)
            {
                foreach(Stage stage in pow.stages)
                {
                    if (stage.requiredBodies.Contains(selectedBody.id))
                    {
                        GameObject go = Instantiate(templateButton);
                        go.GetComponentInChildren<Text>().text = stage.id;
                        go.SetActive(true);
                        go.transform.SetParent( buttonPanel.transform );
                        buttons.Add(go);

                        go.GetComponent<Tooltip>().title = pow.name + "_" + stage.name;
                        string tooltip = stage.requirementType == Stage.RequirementType.All ? "Require all of:\n" : "Requires any of:\n";
                        foreach(string body in stage.requiredBodies)
                        {
                            tooltip += "\t" + body + "\n";
                        }
                        go.GetComponent<Tooltip>().text = tooltip;
                    }
                }
            }
        }
    }

    private void onChangeGovernment()
    {
        govText.text = government.name;
    }

    public void onAddBody()
    {
        dropdown.ClearOptions();

        if (government.bodies != null)
            Debug.Log(string.Join("," ,government.bodies.Select(x => x.name).ToArray()));
            dropdown.AddOptions(government.bodies.Select(x => x.name).ToList());
    }


    public void onAddPower()
    {

    }
}