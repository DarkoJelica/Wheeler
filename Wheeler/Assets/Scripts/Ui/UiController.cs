using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiController : MonoBehaviour
{
   public GameObject SpriteHolder;
   public GameObject Claw;
   public ClickPanelController ClickPanel;
   public RectTransform ButtonsBar;
   public ButtonManager Hand1;
   public ButtonManager Hand2;
   public ButtonManager Combine;
   public Canvas Menu;

   PlayerController playerController;
   Camera mainCamera;
   Level level;
   Image spriteHolderImage;
   Image hand1Image;
   Image hand2Image;
   Animator clawAnimator;
   Item heldObject;
   Hands activeSlot;
   float bkgBarOffset;
   
   void Start()
   {
      InitReferences();
      spriteHolderImage = SpriteHolder.GetComponent<Image>();
      SpriteHolder.SetActive(false);
      clawAnimator = Claw.GetComponent<Animator>();
      Claw.SetActive(false);
      ClickPanel.ClickEvent += OnPanelClick;
      Hand1.DownEvent += OnButtonDown;
      Hand2.DownEvent += OnButtonDown;
      playerController.PickupEvent += OnPickup;
      playerController.DropEvent += OnDrop;
      bkgBarOffset = 2 * mainCamera.orthographicSize * mainCamera.aspect * ButtonsBar.rect.width / GetComponent<Canvas>().pixelRect.width;
      Menu.gameObject.SetActive(false);
   }

   void Update()
   {
      if(Menu.gameObject.activeInHierarchy)
         return;
      InitReferences();
      SetCameraPosition();
      ClickPanel.gameObject.SetActive(spriteHolderImage.sprite == null);
      Vector3 mousePos = Input.mousePosition;
      SpriteHolder.transform.position = new Vector3(mousePos.x, mousePos.y, SpriteHolder.transform.position.z);
      Vector3 clawPos = mainCamera.ScreenToWorldPoint(mousePos);
      clawPos.z = Claw.transform.position.z;
      Claw.transform.position = clawPos;
      if(Input.GetMouseButtonUp(0))
      {
         if(heldObject != null)
         {
            Vector3 dropPosition = mainCamera.ScreenToWorldPoint(mousePos);
            playerController.RemoveFromSlot(activeSlot, dropPosition);
            SetCursorSprite(null);
         }
         else if(Claw.activeInHierarchy && activeSlot != Hands.NONE)
         {
            RaycastHit2D raycast = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(mousePos), Vector2.zero, 0f);
            if(raycast.collider != null && raycast.collider.gameObject.CompareTag("Component"))
               playerController.AddToSlot(raycast.collider.gameObject.GetComponent<Item>(), activeSlot);
         }
         SetCursorSprite(null);
         Claw.SetActive(false);
         activeSlot = Hands.NONE;
      }
      if(Claw.activeInHierarchy)
      {
         RaycastHit2D raycast = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(mousePos), Vector2.zero, 0f);
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
      if(playerController.CombineItems())
      {
         Hand1.SetSprite(playerController.GetSlotSprite(Hands.LEFT));
         Hand2.SetSprite(playerController.GetSlotSprite(Hands.RIGHT));
         Combine.SetSprite(playerController.GetCombinationSprite());
      }
   }

   public void RestartLevel()
   {
      Menu.gameObject.SetActive(false);
      SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
   }

   public void Quit()
   {
      Application.Quit();
   }

   void OnGUI()
   {
      if(Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Menu || Event.current.keyCode == KeyCode.Escape))
         Menu.gameObject.SetActive(!Menu.gameObject.activeInHierarchy);
   }

   void SetCameraPosition()
   {
      Vector3 position = level.transform.position + (Vector3)level.Size / 2;
      if(level.Size.x > mainCamera.orthographicSize * mainCamera.aspect)
         position.x = Mathf.Clamp(playerController.transform.position.x, level.transform.position.x + mainCamera.orthographicSize * mainCamera.aspect - bkgBarOffset, level.transform.position.x + level.Size.x - mainCamera.orthographicSize * mainCamera.aspect);
      if(level.Size.y > mainCamera.orthographicSize)
         position.y = Mathf.Clamp(playerController.transform.position.y, level.transform.position.y + mainCamera.orthographicSize, level.transform.position.y + level.Size.y - mainCamera.orthographicSize);
      mainCamera.transform.position = position;
   }

   void SetCursorSprite(Sprite sprite)
   {
      spriteHolderImage.sprite = sprite;
      SpriteHolder.SetActive(sprite != null);
      Claw.SetActive(!SpriteHolder.activeInHierarchy);
   }

   void OnPanelClick()
   {
      if(Menu.gameObject.activeInHierarchy)
         return;
      playerController.CurrentTarget = mainCamera.ScreenToWorldPoint(Input.mousePosition);
   }

   void OnButtonDown(Hands slot)
   {
      if(slot == Hands.LEFT)
      {
         heldObject = playerController.Hand1;
         Hand1.SetSprite(playerController.GetSlotSprite(Hands.LEFT));
         SetCursorSprite(heldObject != null ? Hand1.GetSprite() : null);
         activeSlot = Hands.LEFT;
      }
      else if(slot == Hands.RIGHT)
      {
         heldObject = playerController.Hand2;
         Hand2.SetSprite(playerController.GetSlotSprite(Hands.RIGHT));
         SetCursorSprite(heldObject != null ? Hand2.GetSprite() : null);
         activeSlot = Hands.RIGHT;
      }
   }

   void OnPickup(Hands hand)
   {
      if(hand == Hands.LEFT)
         Hand1.SetSprite(playerController.GetSlotSprite(Hands.LEFT));
      else if(hand == Hands.RIGHT)
         Hand2.SetSprite(playerController.GetSlotSprite(Hands.RIGHT));
      if(playerController.Hand1 != null && playerController.Hand2 != null)
         Combine.SetSprite(playerController.GetCombinationSprite());
   }

   void OnDrop(Hands hand)
   {
      if(hand == Hands.LEFT)
         Hand1.SetSprite(null);
      else if(hand == Hands.RIGHT)
         Hand2.SetSprite(null);
   }

   void InitReferences()
   {
      if(playerController == null)
         playerController = GameObject.Find("Wheeler").GetComponent<PlayerController>();
      if(mainCamera == null)
         mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
      if(level == null)
         level = GameObject.Find("Level").GetComponent<Level>();
   }

}
