using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class TogglePanelButton : MonoBehaviour, IPointerDownHandler
{

    public void OnPointerDown(PointerEventData data)
    {
        transform.parent.gameObject.SetActive(!transform.parent.gameObject.activeSelf);
    }
}