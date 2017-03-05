using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IPostInitialized
{
    void postInit();
}

static class Coord2DExtension
{
    public static Vector2 ToVector2(this World.Coord2D input)
    {
        return new Vector2(input.x, input.y);
    }
}

public class WorldController : MonoBehaviour {
    public GameObject regionControllerPrototype;
    public GameObject windowPrototype;
    public GameObject regionWidgetPrototype;

    public World world;
    public RegionController selected;

    public Material defaultMaterial;
    public Material outlineMaterial;

    static public List<IPostInitialized> postinitialized = new List<IPostInitialized>();

    public static void register(IPostInitialized item)
    {
        postinitialized.Add(item);
    }

    public Action<int> onSpeedChanged;
    private int speed_ = 1;
    public int speed
    {
        get { return speed_; }
        set {
            if (speed_ == value) return;
            if (value == 0)
            {
                speed_ = -speed_;
            }
            else if (value >= 3) speed_ = 3;
            else speed_ = value;
            if(onSpeedChanged != null) onSpeedChanged(speed_);
        }
    }
    public int[] speedArray = { 2, 10, 50 };
    public int time = 0, elapsed = 0, maxElapsed = 100;
    public Action onTic;

    static public WorldController Instance;

    // Use this for initialization
    void Start() {
        Debug.Assert(Instance == null);
        Instance = this;

        Region.onCreatedCallback += (Region region) => {
            if(regionControllerPrototype == null)
            {
                Debug.LogError("No region controller object prototype.");
                return;
            }
            GameObject regionControllerObject = Instantiate<GameObject>(regionControllerPrototype);
            regionControllerObject.transform.SetParent(transform);
            RegionController regionController = regionControllerObject.GetComponent<RegionController>();
            if(regionController == null)
            {
                Debug.LogError("No region controller component.");
                return;
            }
            //regionController.name = region.name;
            regionController.setRegion(region);
            regionControllerObject.SetActive(true);

        };

        regionControllerPrototype.SetActive(false);
        if(windowPrototype == null)
        {
            Debug.LogError("No window prototype.");
        }
        if(regionWidgetPrototype == null)
        {
            Debug.LogError("No region widget prototype.");
        } else
        {
            regionWidgetPrototype.SetActive(false);
        }
        windowPrototype.SetActive(false);

        world = new World(new World.Coord2D(100, 100));

        speed = 0;
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                if (objectHit.GetComponent<BarGraph>() == null) return;

                if (selected != null)
                {
                    BarGraph selectedBarGraph = selected.GetComponentInChildren<BarGraph>();
                    if (selectedBarGraph != null)
                    {
                        setSelected(selectedBarGraph, false);
                    }
                }

                selected = objectHit.GetComponentInParent<RegionController>();
                if (!WindowManager.isOpen("RegionWidget"))
                {
                    WindowManager.openWindowFromChildPrototype("RegionWidget");
                }
                WindowManager.activate("RegionWidget");
                WindowManager.get("RegionWidget").GetComponentInChildren<RegionWidget>().regionController = selected;

                BarGraph barGraph = objectHit.GetComponent<BarGraph>();
                if (barGraph != null)
                {
                    //Debug.Log(objectHit.gameObject.name);
                    setSelected(barGraph, true);
                }

                // Do something with the object that was hit by the raycast.
            }
        }

        // Handle spped
        if (Input.GetButtonDown("Pause")) speed = 0;
        else if (Input.GetButtonDown("Speed1")) speed = 1;
        else if (Input.GetButtonDown("Speed2")) speed = 2;
        else if (Input.GetButtonDown("Speed3")) speed = 3;
        else if (Input.GetButtonDown("Speed"))
        {
            float axisSpeed = Input.GetAxis("Speed");
            if (axisSpeed > 0) speed++;
            else if (axisSpeed < 0) speed--;
        }

        if (speed > 0) {
            elapsed += speedArray[speed - 1];
            if(elapsed > maxElapsed)
            {
                elapsed = 0;
                time += 1;
                world.tic();
                if (onTic != null) onTic();
                // MAIN LOOP
            }
        }
    }

    void setSelected(BarGraph bar, bool selected)
    {
        bar.gameObject.GetComponent<MeshRenderer>().sharedMaterial = selected ? outlineMaterial : defaultMaterial;
        bar.recolor();
    }

    public void setSpeed(int speed_)
    {
        speed = speed_;
    }
}
