using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionWidget : MonoBehaviour {

    public RegionController regionController = null;

    public Text populationText;
    public Text taxText;

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
