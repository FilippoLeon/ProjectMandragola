﻿using System.Collections;
using System.Collections.Generic;
using System;
using MoonSharp.Interpreter;

[MoonSharpUserData]
public class Region {
    public static Action<Region> onCreatedCallback;
    public Action onRegionCangedCallback;

    public World world;
    public World.Coord2D coord;

    private Country country_;
    public Country country
    {
        get { return country_; }
        set
        {
            if (country_ == value) return;
            country_ = value;
            country_.regions.Add(this);
            onRegionCangedCallback();
        }
    }

    public State state;
    private int population_;
    public int population
    {
        get { return population_; }
        set
        {
            int diff = value - population_;
            if(country != null) country.demography.population += diff;
            population_ = value;
            if (onRegionCangedCallback != null) onRegionCangedCallback();
        }
    }

    public float[] demographic = new float[10];
    public String name;
    public int climate;
    public float rate = 0.1f;

    public Region(World world_, World.Coord2D coord_)
    {
        world = world_;
        coord = coord_;
        name = coord_.x.ToString() + "-" + coord_.y.ToString();
        population = world.rnd.Next(1,5);
        if (onCreatedCallback != null) onCreatedCallback(this);
    }

    public Region[] getNeighbours(bool diagonal = true) {
        Region[] ret = new Region[diagonal ? 8 : 4];

        ret[0] = world.getRegionAt(coord + World.Coord2D.Up);
        ret[1] = world.getRegionAt(coord + World.Coord2D.Right);
        ret[2] = world.getRegionAt(coord + World.Coord2D.Down);
        ret[3] = world.getRegionAt(coord + World.Coord2D.Left);
        if (diagonal)
        {
            ret[4] = world.getRegionAt(coord + World.Coord2D.UpRight);
            ret[5] = world.getRegionAt(coord + World.Coord2D.DownRight);
            ret[6] = world.getRegionAt(coord + World.Coord2D.DownLeft);
            ret[7] = world.getRegionAt(coord + World.Coord2D.UpLeft); ;
        }

        return ret;
    }

    public void increaseRate()
    {
        rate += 0.01f;
    }
    public void decreaseRate()
    {
        rate -= 0.01f;
    }

    public void tic()
    {
        if (country == null) return;
        int taxes = (int) (population * rate);
        //country.economy.funds += taxes;
        foreach (Law activeLaw in country.laws.activeLaws.Values)
        {
            activeLaw.OnEvent("OnRegionTic", new Object[] { activeLaw, this });
        }
    }
}
