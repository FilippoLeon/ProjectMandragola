using System.Collections;
using System.Collections.Generic;
using System;

public class Demography {

    //public Action onPopulationChanged;
    private int population_ = 0;
    public int population
    {
        get { return population_;  }
        set
        {
            population_ = value;
            //if(onPopulationChanged)
        }
    }

}
