using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
   public GameObject SpriteHolder;
   public GameObject Claw;
   public ClickPanelController ClickPanel;
   public PlayerController PlayerController;
   public Camera Camera;
   public ButtonManager Hand1;
   public ButtonManager Hand2;
   
   Image spriteHolderImage;
   Image hand1Image;
   Image hand2Image;
   Animator clawAnimator;
   GameObject heldObject;
   Slots activeSlot;
   
   void Start()
   {
      spriteHolderImage = SpriteHolder.GetComponent<Image>();
      SpriteHolder.SetActive(false);
      clawAnimator = Claw.GetComponent<Animator>();
      Claw.SetActive(false);
      ClickPanel.ClickEvent += OnPanelClick;
      Hand1.DownEvent += OnButtonDown;
      Hand2.DownEvent += OnButtonDown;
   }

   void Update()
   {
      ClickPanel.gameObject.SetActive(spriteHolderImage.sprite == null);
      Vector3 mousePos = Input.mousePosition;
      SpriteHolder.transform.position = new Vector3(mousePos.x, mousePos.y, SpriteHolder.transform.position.z);
      Vector3 clawPos = Camera.ScreenToWorldPoint(mousePos);
      clawPos.z = Claw.transform.position.z;
      Claw.transform.position = clawPos;
      if(Input.GetMouseButtonUp(0))
      {
         if(heldObject != null)
         {
            GameObject item = PlayerController.RemoveFromSlot(activeSlot);
            if(item != null)
            {
               Vector3 dropPosition = Camera.ScreenToWorldPoint(mousePos);
               dropPosition.z = item.transform.position.z;
               item.transform.position = dropPosition;
            }
            SetCursorSprite(null);
            if(activeSlot == Slots.HAND1)
               Hand1.SetSprite(null);
            if(activeSlot == Slots.HAND2)
               Hand2.SetSprite(null);
         }
         else if(Claw.activeInHierarchy && activeSlot != Slots.NONE)
         {
            RaycastHit2D raycast = Physics2D.Raycast(Camera.ScreenToWorldPoint(mousePos), Vector2.zero, 0f);
            if(raycast.collider != null)
            {
               PlayerController.AddToSlot(raycast.collider.gameObject, activeSlot);
               if(activeSlot == Slots.HAND1)
                  Hand1.SetSprite(PlayerController.Hand1.GetComponent<SpriteRenderer>().sprite);
               else
                  Hand2.SetSprite(PlayerController.Hand2.GetComponent<SpriteRenderer>().sprite);
            }
         }
         SetCursorSprite(null);
         Claw.SetActive(false);
         activeSlot = Slots.NONE;
      }
      if(Claw.activeInHierarchy)
      {
         RaycastHit2D raycast = Physics2D.Raycast(Camera.ScreenToWorldPoint(mousePos), Vector2.zero, 0f);
         if(raycast.collider != null)
         {
            if(!clawAnimator.GetBool("Open"))
               clawAnimator.SetTrigger("Open");
         }
         else if(!clawAnimator.GetBool("Close"))
            clawAnimator.SetTrigger("Close");
      }
   }

   public void SetCursorSprite(Sprite sprite)
   {
      spriteHolderImage.sprite = sprite;
      SpriteHolder.SetActive(sprite != null);
      Claw.SetActive(!SpriteHolder.activeInHierarchy);
   }

   void OnPanelClick()
   {
      PlayerController.CurrentTarget = Camera.ScreenToWorldPoint(Input.mousePosition);
   }

   void OnButtonDown(Slots slot)
   {
      if(slot == Slots.HAND1)
      {
         heldObject = PlayerController.Hand1;
         Hand1.SetSprite(heldObject != null ? heldObject.GetComponent<SpriteRenderer>().sprite : null);
         SetCursorSprite(heldObject != null ? Hand1.GetSprite() : null);
         activeSlot = Slots.HAND1;
      }
      else
      {
         heldObject = PlayerController.Hand2;
         Hand2.SetSprite(heldObject != null ? heldObject.GetComponent<SpriteRenderer>().sprite : null);
         SetCursorSprite(heldObject != null ? Hand2.GetSprite() : null);
         activeSlot = Slots.HAND2;
      }
   }

}
