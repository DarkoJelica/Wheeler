using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour, IPointerDownHandler
{
   public Sprite DefaultSprite;
   public Slots Slot;

   public delegate void OnDown(Slots slot);
   public event OnDown DownEvent;

   Image buttonImage;

   private void OnEnable()
   {
      buttonImage = GetComponent<Image>();
      buttonImage.sprite = DefaultSprite;
   }

   public Sprite GetSprite()
   {
      return buttonImage.sprite;
   }

   public void SetSprite(Sprite sprite)
   {
      if(sprite == null)
         buttonImage.sprite = DefaultSprite;
      else
         buttonImage.sprite = sprite;
   }

   public void OnPointerDown(PointerEventData eventData)
   {
      DownEvent.Invoke(Slot);
   }

}
