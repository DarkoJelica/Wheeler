using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Hands { LEFT, RIGHT, NONE }

[System.Serializable]
public struct ItemCombination
{
   [SerializeField]
   public Item InputA;
   [SerializeField]
   public Item InputB;
   [SerializeField]
   public Item Output;
}

public class PlayerController : MonoBehaviour
{
   public float Speed;
   public float Range;
   public Vector3 CurrentTarget;
   public Item Hand1 { get; private set; }
   public Item Hand2 { get; private set; }
   public ItemCombination[] ItemCombinations;

   public delegate void OnPickup(Hands hand);
   public event OnPickup PickupEvent;
   public delegate void OnDrop(Hands hand);
   public event OnDrop DropEvent;

   SpriteRenderer spriteRenderer;
   Item itemToPickup;
   Hands pickupSlot = Hands.NONE;
   Hands itemToDrop = Hands.NONE;
   Vector3 dropPosition;
   int activeCombination = -1;

   public Sprite GetSlotSprite(Hands slot)
   {
      Item item = null;
      switch(slot)
      {
         case Hands.LEFT:
            item = Hand1;
            break;
         case Hands.RIGHT:
            item = Hand2;
            break;
      }
      if(item == null)
         return null;
      return item.Sprite;
   }

   public void AddToSlot(Item item, Hands slot)
   {
      if(item == null)
         return;
      itemToPickup = item;
      pickupSlot = slot;
   }

   public void RemoveFromSlot(Hands slot, Vector2 position)
   {
      Item item = null;
      if(slot == Hands.LEFT)
         item = Hand1;
      else if(slot == Hands.RIGHT)
         item = Hand2;
      if(item == null)
         return;
      itemToDrop = slot;
      dropPosition = new Vector3(position.x, position.y, item.transform.position.z);
   }

   public Sprite GetCombinationSprite()
   {
      if(activeCombination < 0)
         return null;
      return ItemCombinations[activeCombination].Output.Sprite;
   }

   public bool CombineItems()
   {
      if(activeCombination < 0)
         return false;
      if(Hand1 == null || Hand2 == null)
         return false;
      Destroy(Hand1);
      Hand1 = null;
      Destroy(Hand2);
      Hand2 = null;
      Item output = Instantiate(ItemCombinations[activeCombination].Output);
      output.name = output.name.Substring(0, output.name.Length - 7);
      output.transform.position = transform.position;
      PickupItem(output, Hands.LEFT);
      activeCombination = -1;
      return true;
   }

   bool PickupItem(Item item, Hands hand)
   {
      if(item == null)
         return false;
      if((item.transform.position - transform.position).magnitude > Range)
         return false;
      switch(hand)
      {
         case Hands.LEFT:
            if(Hand1 != null)
               return false;
            Hand1 = item;
            break;
         case Hands.RIGHT:
            if(Hand2 != null)
               return false;
            Hand2 = item;
            break;
      }
      item.gameObject.SetActive(false);
      item.transform.parent = transform;
      item.transform.localPosition = new Vector3(0, 0, item.transform.localPosition.z);
      if(Hand1 != null && Hand2 != null)
      {
         for(int i = 0; i < ItemCombinations.Length; ++i)
         {
            ItemCombination combination = ItemCombinations[i];
            if(combination.InputA == null || combination.InputB == null || combination.Output == null)
               continue;
            if((combination.InputA.name == Hand1.name && combination.InputB.name == Hand2.name) || (combination.InputA.name == Hand2.name && combination.InputB.name == Hand1.name))
            {
               activeCombination = i;
               break;
            }
         }
      }
      PickupEvent.Invoke(hand);
      return true;
   }

   bool DropItem(Hands hand)
   {
      Item item = null;
      if(hand == Hands.LEFT)
         item = Hand1;
      else if (hand == Hands.RIGHT)
         item = Hand2;
      if(item == null)
         return false;
      if((dropPosition - transform.position).magnitude > Range)
         return false;
      item.transform.parent = null;
      item.gameObject.SetActive(true);
      item.transform.position = dropPosition;
      if(dropPosition.x < transform.position.x)
         item.transform.rotation = Quaternion.Euler(0, 180, 0);
      if(hand == Hands.LEFT)
         Hand1 = null;
      else if(hand == Hands.RIGHT)
         Hand2 = null;
      DropEvent.Invoke(hand);
      activeCombination = -1;
      return true;
   }

   void Start()
   {
      spriteRenderer = GetComponent<SpriteRenderer>();
      CurrentTarget = transform.position;
   }

   void Update()
   {
      if(itemToPickup != null)
      {
         CurrentTarget = itemToPickup.transform.position;
         if(PickupItem(itemToPickup, pickupSlot))
         {
            itemToPickup = null;
            pickupSlot = Hands.NONE;
            CurrentTarget = transform.position;
         }
      }
      else if(itemToDrop != Hands.NONE)
      {
         CurrentTarget = dropPosition;
         if(DropItem(itemToDrop))
         {
            itemToDrop = Hands.NONE;
            dropPosition = Vector2.zero;
            CurrentTarget = transform.position;
         }
      }
   }

   void FixedUpdate()
   {
      Rigidbody2D body = GetComponent<Rigidbody2D>();
      if(Mathf.Abs(body.velocity.y) > Mathf.Epsilon)
         return;
      Vector2 move = Vector2.ClampMagnitude(CurrentTarget - gameObject.transform.position, Speed);
      move.y = 0;
      //move.z = 0;
      gameObject.transform.position += Vector3.ClampMagnitude(move, Time.deltaTime * Speed);
      if(Mathf.Abs(move.x) > Mathf.Epsilon)
         spriteRenderer.flipX = move.x < 0;
   }

}
