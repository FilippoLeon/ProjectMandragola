using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionWidget : MonoBehaviour {

    public RegionController regionController = null;

    public Text populationText;
    public Text taxText;
    public Text countryText;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		if(regionController != null)
        {
            gameObject.GetComponentInParent<WindowWidget>().setTitle(regionController.region.name);
            populationText.text = StringUtilities.longToShortString(regionController.region.population);
            taxText.text = StringUtilities.floatToPercentage(regionController.region.rate);
            Country ctry = regionController.region.country;
            if (ctry != null)
                countryText.text = "<color=#" + StringUtilities.colorToHex(ctry.color) + ">"
                    + regionController.region.country.name
                    + "</color>";
        }
	}

    public void regionRaiseTax()
    {
        regionController.region.rate += 0.01f;
    }


    public void regionLowerTax()
    {
        regionController.region.rate -= 0.01f;
    }
}
