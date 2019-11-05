using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickPanelController : MonoBehaviour, IPointerClickHandler
{

   public delegate void OnClick();
   public event OnClick ClickEvent;

   public void OnPointerClick(PointerEventData eventData)
   {
      if(ClickEvent != null)
         ClickEvent();
   }

}
