using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemographyWidget : MonoBehaviour {

    Demography demography;

    public GameObject worldControllerObject;
    public WorldController worldController;
    Text totalPopulationText;

    // Use this for initialization
    void Start () {
        totalPopulationText = GetComponentInChildren<Text>();
        if (worldController == null)
        {
            worldController = worldControllerObject.GetComponentInChildren<WorldController>();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(demography == null)
        {
            demography = worldController.world.thisCountry.demography;
        }
        if (demography != null)
        {
            totalPopulationText.text = StringUtilities.longToShortString(demography.population, 1);
        }
	}
}
