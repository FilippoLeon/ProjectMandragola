using UnityEngine;
using System.Collections;

public class ChamberBuilderScript : MonoBehaviour
{
    public GameObject obj;

    public void BuildObject()
    {
        Instantiate(obj);
    }
}