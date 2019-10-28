using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour, IPointerDownHandler
{
   public Sprite defaultSprite;
   UiController cursorController;

   private void OnEnable()
   {
      cursorController = GetComponentInParent<UiController>();
   }

   public void OnPointerDown(PointerEventData eventData)
   {
      if(cursorController != null)
         cursorController.SetCursorSprite(defaultSprite);
   }

}
