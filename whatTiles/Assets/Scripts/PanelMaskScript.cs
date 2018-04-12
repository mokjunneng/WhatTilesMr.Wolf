using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelMaskScript : MonoBehaviour, IPointerDownHandler
{
    public GameObject hostPopUp;
    public GameObject roomPopUp;
    public void OnPointerDown(PointerEventData eventData)
    {
        hostPopUp.SetActive(false);
        roomPopUp.SetActive(false);
    }

}
