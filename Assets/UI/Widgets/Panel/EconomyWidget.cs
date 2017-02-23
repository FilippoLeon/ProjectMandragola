using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EconomyWidget : MonoBehaviour
{
    Economy economy;

    public GameObject worldControllerObject;
    public WorldController worldController;
    Text fundsText;

    // Use this for initialization
    void Start()
    {
        if (worldController == null) {
            worldController = worldControllerObject.GetComponentInChildren<WorldController>();
        }
        fundsText = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (economy == null)
        {
            economy = worldController.world.thisCountry.economy;
        }
        if (economy != null) {
            fundsText.text = StringUtilities.longToShortString(economy.funds, 1);
        }
    }
}
