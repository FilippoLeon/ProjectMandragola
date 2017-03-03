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
    //Text fundsTextDelta;

    // Use this for initialization
    void Start()
    {
        if (worldController == null) {
            worldController = worldControllerObject.GetComponentInChildren<WorldController>();
        }
        Text[] ret = GetComponentsInChildren<Text>();
        fundsText = ret[0];
        //fundsTextDelta = ret[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (economy == null)
        {
            economy = worldController.world.thisCountry.economy;
        }
        if (economy != null) {
            fundsText.text = StringUtilities.toCurrency(economy.funds) + " (" + StringUtilities.toCurrency(economy.funds - economy.old_funds) + ")";
        }
    }
}
