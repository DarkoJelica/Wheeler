using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
   public GameObject SpriteHolder;
   public ClickPanelController ClickPanel;
   public PlayerController PlayerController;
   public Camera Camera;
   
   Image spriteHolderImage;
   
   void Start()
   {
      spriteHolderImage = SpriteHolder.GetComponent<Image>();
      SpriteHolder.SetActive(false);
      ClickPanel.ClickEvent += OnPanelClick;
   }
   
   void Update()
   {
      ClickPanel.gameObject.SetActive(spriteHolderImage.sprite == null);
      Vector3 mousePos = Input.mousePosition;
      SpriteHolder.transform.position = new Vector3(mousePos.x, mousePos.y, SpriteHolder.transform.position.z);
      if(Input.GetMouseButtonUp(0))
         SetCursorSprite(null);
   }

   public void SetCursorSprite(Sprite sprite)
   {
      spriteHolderImage.sprite = sprite;
      SpriteHolder.SetActive(sprite != null);
   }

   void OnPanelClick()
   {
      PlayerController.CurrentTarget = Camera.ScreenToWorldPoint(Input.mousePosition);
   }

}
