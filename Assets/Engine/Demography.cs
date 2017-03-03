using System.Collections;
using System.Collections.Generic;
using System;

public class Demography {

    //public Action onPopulationChanged;
    public int old_population = 0;

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

    public void tic()
    {
        old_population = population;
    }

}
