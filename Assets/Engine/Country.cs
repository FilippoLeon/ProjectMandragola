using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Country {

    public List<State> states;

    public Demography demography = new Demography();
    public Economy economy = new Economy();

    public Color32 color;

    string name;

    private Region capital_;
    public Region capital
    {
        get { return capital_; }
        set {
            capital_ = value;
            if(capital_.country != this)
            {
                capital_.country = this;
            }
        }
    }

    public List<Region> regions = new List<Region>();

    public Country(string name_)
    {
        name = name_;
        color = Random.ColorHSV();
    }

    public void addRegion(Region region)
    {
        region.country = this;
    }
    
}
