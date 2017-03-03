using System.Collections;
using System.Collections.Generic;

public class World {
    public class Coord2D
    {
        public static Coord2D Zero = new Coord2D(0, 0);
        public static Coord2D Up = new Coord2D(0, 1);
        public static Coord2D Down = new Coord2D(0, -1);
        public static Coord2D Left = new Coord2D(1, 0);
        public static Coord2D Right = new Coord2D(-1, 0);
        public static Coord2D UpRight = new Coord2D(1, 1);
        public static Coord2D DownRight = new Coord2D(1, -1);
        public static Coord2D DownLeft = new Coord2D(-1, -1);
        public static Coord2D UpLeft = new Coord2D(-1, 1);
        public int x, y;

        public Coord2D(int x_, int y_)
        {
            x = x_; y = y_;
        }

        public static bool operator <=(Coord2D A, Coord2D B)
        {
            return A.x <= B.x || A.y <= B.y;
        }
        public static bool operator >=(Coord2D A, Coord2D B)
        {
            return A.x >= B.x || A.y >= B.y;
        }

        public static bool operator <(Coord2D A, Coord2D B)
        {
            return A.x < B.x || A.y < B.y;
        }
        public static bool operator >(Coord2D A, Coord2D B)
        {
            return A.x > B.x || A.y > B.y;
        }

        public static Coord2D operator +(Coord2D A, Coord2D B)
        {
            return new Coord2D(A.x+B.x,A.y+B.y);
        }
    }

    public Coord2D size;

    public Country thisCountry;
    
    public List<Country> countries = new List<Country>();

    public System.Random rnd;

    private Region[,] regions;

    GovernmentManager governmentManager;

    public World(Coord2D size_)
    {
        rnd = new System.Random();
        size = size_;
        regions = new Region[size.x, size.y];
        for (int x = 0; x < size.x; ++x)
        {
            for (int y = 0; y < size.y; y++)
            {
                regions[x, y] = new Region(this, new Coord2D(x, y));
            }
        }

        governmentManager = new GovernmentManager();
        //

        /// GENERATE
        ///
        int numberOfCountries = 30;
        for (int c = 0; c < numberOfCountries;)
        {
            Region possibleRegion = getRegionAt(
                new Coord2D(
                    rnd.Next(size.x),
                    rnd.Next(size.y)
                    )
                );
            if (possibleRegion == null || possibleRegion.country != null) continue;
            Country country = new Country(rnd.Next(1000).ToString());
            if (thisCountry == null) thisCountry = country;
            addCountry(country);
            country.addRegion(possibleRegion);
            country.capital = possibleRegion;
            ++c;
        }
        for (int i = 0; i < 1000; i++)
        {
            foreach (Country country in countries)
            {
                int r = rnd.Next(country.regions.Count);
                Region[] nhbds = country.regions[r].getNeighbours();
                //foreach (Region nhbd in nhbds)
                //{
                //    if (nhbd.country == null) nhbd.country = country;
                //    break;
                //}
                Region nhbd = nhbds[rnd.Next(8)];
                if (nhbd != null && nhbd.country == null) nhbd.country = country;
            }
        }
        foreach (Country country in countries)
        {
            int size = country.regions.Count;
            int numCities = rnd.Next(size) / 10 + 3;
            for (int i = 0; i < numCities; i++)
            {
                int max = 100;
                if (i < 3) max = 200;
                int idx = rnd.Next(size);
                Region city = country.regions[idx];
                if (i == 0)
                {
                    country.capital = city;
                }
                country.regions[idx].population = rnd.Next(50, max);

                int metropolitanAreaSize = rnd.Next(1, 6);
                Region[] metropolitanArea = new Region[metropolitanAreaSize];
                metropolitanArea[0] = country.capital;
                int max_metro = 70;
                for (int j = 1; j < metropolitanAreaSize; j++)
                {
                    Region prev = metropolitanArea[rnd.Next(j)];
                    if (prev == null) continue;
                    Region tentative = prev.getNeighbours()[rnd.Next(8)];
                    if (tentative == null || tentative.country == null) continue;
                    metropolitanArea[j] = tentative;
                    metropolitanArea[j].population = rnd.Next(10, max_metro);
                }
            }
        }

        //// TEMPORARILY ASSIGN GOV. (randomly) to country
        int govsize = GovernmentManager.governmentPrototypes.Count;
        foreach (Country ctry in countries)
        {
            int idx = rnd.Next(govsize);
            ctry.government = GovernmentManager.governmentPrototypes[idx].Clone() as Government;
        }
    }

    public Region getRegionAt(Coord2D coord)
    {
        if (coord < Coord2D.Zero || coord >= size)
        {
            //Console.WriteLine("Region outside map!");
            return null;
        }
        else return regions[coord.x, coord.y];
    }

    public void addCountry(Country country)
    {
        countries.Add(country);
    }

    public void tic()
    {
        foreach (Country country in countries)
        {
            country.tic();
        }
        foreach (Region region in regions)
        {
            region.tic();
        }
    }
}
