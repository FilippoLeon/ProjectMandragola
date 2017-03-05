using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionController : MonoBehaviour {

    public Region region;

    public BarGraph barGraph;

	// Use this for initialization
	void Start () {
        barGraph = GetComponentInChildren<BarGraph>();
        if(barGraph == null)
        {
            Debug.LogWarning("No BarGraph in child.");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setRegion(Region region_)
    {
        if(region != null)
        {
            Debug.LogWarning("Overrding region!");
        }
        region = region_;
        gameObject.name = region.name;

        region.onRegionCangedCallback += () =>
        {
            updateRegion();
        };

        updatePosition();
    }

    public void setDisplayMode()
    {

    }

    void updatePosition()
    {
        transform.position = region.coord.ToVector2();
    }

    void updateRegion()
    {
        barGraph.setHeight(region.population);
        barGraph.setColor(region.country == null ? (Color32) new Color(1,1,1) : region.country.color);
    }

}
