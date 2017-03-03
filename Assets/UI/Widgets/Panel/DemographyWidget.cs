using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemographyWidget : MonoBehaviour {

    Demography demography;

    public GameObject worldControllerObject;
    public WorldController worldController;
    Text totalPopulationText;
    //Text totalPopulationDeltaText;

    // Use this for initialization
    void Start () {
        if (worldController == null)
        {
            worldController = worldControllerObject.GetComponentInChildren<WorldController>();
        }
        Text[] ret = GetComponentsInChildren<Text>();
        totalPopulationText = ret[0];
        //totalPopulationDeltaText = ret[1];
    }
	
	// Update is called once per frame
	void Update () {
        if(demography == null)
        {
            demography = worldController.world.thisCountry.demography;
        }
        if (demography != null)
        {
            totalPopulationText.text = StringUtilities.longToShortString(demography.population, 1)
                + " (" + StringUtilities.longToShortString(demography.population - demography.old_population, 1) + ")";
        }
	}
}
