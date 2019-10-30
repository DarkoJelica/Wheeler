using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Slots { HAND1, HAND2, NONE }

public class PlayerController : MonoBehaviour
{
   public float Speed;
   public Vector3 CurrentTarget;
   public GameObject HandPosition;
   public GameObject Hand1 { get; private set; }
   public GameObject Hand2 { get; private set; }

   SpriteRenderer spriteRenderer;

   public Sprite GetSlotSprite(Slots slot)
   {
      GameObject item = null;
      switch(slot)
      {
         case Slots.HAND1:
            item = Hand1;
            break;
         case Slots.HAND2:
            item = Hand2;
            break;
      }
      if(item == null)
         return null;
      return item.GetComponent<Sprite>();
   }

   public bool AddToSlot(GameObject item, Slots slot)
   {
      if(item == null)
         return false;
      switch(slot)
      {
         case Slots.HAND1:
            if(Hand1 != null)
               return false;
            Hand1 = item;
            break;
         case Slots.HAND2:
            if(Hand2 != null)
               return false;
            Hand2 = item;
            break;
      }
      item.SetActive(false);
      item.transform.parent = transform;
      item.transform.localPosition = Vector3.zero;
      return true;
   }

   public GameObject RemoveFromSlot(Slots slot)
   {
      GameObject item = null;
      switch(slot)
      {
         case Slots.HAND1:
            item = Hand1;
            Hand1 = null;
            break;
         case Slots.HAND2:
            item = Hand2;
            Hand2 = null;
            break;
      }
      if(item != null)
      {
         item.transform.parent = null;
         item.SetActive(true);
      }
      return item;
   }

   public void SwapSlots(Slots a, Slots b)
   {
      GameObject itemA = RemoveFromSlot(a);
      GameObject itemB = RemoveFromSlot(b);
      AddToSlot(itemA, b);
      AddToSlot(itemB, a);
   }

   void Start()
   {
      spriteRenderer = GetComponent<SpriteRenderer>();
   }

   void Update()
   {
      
   }

   void FixedUpdate()
   {
      Vector3 move = CurrentTarget - gameObject.transform.position;
      move.y = 0;
      move.z = 0;
      gameObject.transform.position += Vector3.ClampMagnitude(move, Time.deltaTime * Speed);
      if(Mathf.Abs(move.x) > Mathf.Epsilon)
         spriteRenderer.flipX = move.x < 0;
   }

}
