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
   public BoundsManager Bounds;
   public RectTransform ButtonsBar;
   public ButtonManager Hand1;
   public ButtonManager Hand2;
   public ButtonManager Combine;
   
   Image spriteHolderImage;
   Image hand1Image;
   Image hand2Image;
   Animator clawAnimator;
   Item heldObject;
   Hands activeSlot;
   float bkgBarOffset;
   
   void Start()
   {
      spriteHolderImage = SpriteHolder.GetComponent<Image>();
      SpriteHolder.SetActive(false);
      clawAnimator = Claw.GetComponent<Animator>();
      Claw.SetActive(false);
      ClickPanel.ClickEvent += OnPanelClick;
      Hand1.DownEvent += OnButtonDown;
      Hand2.DownEvent += OnButtonDown;
      PlayerController.PickupEvent += OnPickup;
      PlayerController.DropEvent += OnDrop;
      bkgBarOffset = 2 * Camera.orthographicSize * Camera.aspect * ButtonsBar.rect.width / GetComponent<Canvas>().pixelRect.width;
   }

   void Update()
   {
      SetCameraPosition();
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
            Vector3 dropPosition = Camera.ScreenToWorldPoint(mousePos);
            PlayerController.RemoveFromSlot(activeSlot, dropPosition);
            SetCursorSprite(null);
         }
         else if(Claw.activeInHierarchy && activeSlot != Hands.NONE)
         {
            RaycastHit2D raycast = Physics2D.Raycast(Camera.ScreenToWorldPoint(mousePos), Vector2.zero, 0f);
            if(raycast.collider != null && raycast.collider.gameObject.CompareTag("Component"))
               PlayerController.AddToSlot(raycast.collider.gameObject.GetComponent<Item>(), activeSlot);
         }
         SetCursorSprite(null);
         Claw.SetActive(false);
         activeSlot = Hands.NONE;
      }
      if(Claw.activeInHierarchy)
      {
         RaycastHit2D raycast = Physics2D.Raycast(Camera.ScreenToWorldPoint(mousePos), Vector2.zero, 0f);
         if(raycast.collider != null && raycast.collider.gameObject.CompareTag("Component"))
         {
            if(!clawAnimator.GetBool("Open"))
               clawAnimator.SetTrigger("Open");
         }
         else if(!clawAnimator.GetBool("Close"))
            clawAnimator.SetTrigger("Close");
      }
   }

   public void OnCombine()
   {
      if(PlayerController.CombineItems())
      {
         Hand1.SetSprite(PlayerController.GetSlotSprite(Hands.LEFT));
         Hand2.SetSprite(PlayerController.GetSlotSprite(Hands.RIGHT));
         Combine.SetSprite(PlayerController.GetCombinationSprite());
      }
   }

   void SetCameraPosition()
   {
      Vector3 position = new Vector3(PlayerController.transform.position.x, PlayerController.transform.position.y, Camera.transform.position.z);
      position.x = Mathf.Clamp(position.x, Bounds.transform.position.x + Camera.orthographicSize * Camera.aspect - bkgBarOffset, Bounds.transform.position.x + Bounds.Size.x - Camera.orthographicSize * Camera.aspect);
      position.y = Mathf.Clamp(position.y, Bounds.transform.position.y + Camera.orthographicSize, Bounds.transform.position.y + Bounds.Size.y - Camera.orthographicSize);
      Camera.transform.position = position;
   }

   void SetCursorSprite(Sprite sprite)
   {
      spriteHolderImage.sprite = sprite;
      SpriteHolder.SetActive(sprite != null);
      Claw.SetActive(!SpriteHolder.activeInHierarchy);
   }

   void OnPanelClick()
   {
      PlayerController.CurrentTarget = Camera.ScreenToWorldPoint(Input.mousePosition);
   }

   void OnButtonDown(Hands slot)
   {
      if(slot == Hands.LEFT)
      {
         heldObject = PlayerController.Hand1;
         Hand1.SetSprite(PlayerController.GetSlotSprite(Hands.LEFT));
         SetCursorSprite(heldObject != null ? Hand1.GetSprite() : null);
         activeSlot = Hands.LEFT;
      }
      else if(slot == Hands.RIGHT)
      {
         heldObject = PlayerController.Hand2;
         Hand2.SetSprite(PlayerController.GetSlotSprite(Hands.RIGHT));
         SetCursorSprite(heldObject != null ? Hand2.GetSprite() : null);
         activeSlot = Hands.RIGHT;
      }
   }

   void OnPickup(Hands hand)
   {
      if(hand == Hands.LEFT)
         Hand1.SetSprite(PlayerController.GetSlotSprite(Hands.LEFT));
      else if(hand == Hands.RIGHT)
         Hand2.SetSprite(PlayerController.GetSlotSprite(Hands.RIGHT));
      if(PlayerController.Hand1 != null && PlayerController.Hand2 != null)
         Combine.SetSprite(PlayerController.GetCombinationSprite());
   }

   void OnDrop(Hands hand)
   {
      if(hand == Hands.LEFT)
         Hand1.SetSprite(null);
      else if(hand == Hands.RIGHT)
         Hand2.SetSprite(null);
   }

}
