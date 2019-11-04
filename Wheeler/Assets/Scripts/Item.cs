using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
   public Sprite Sprite;

   void OnCollisionEnter2D(Collision2D collision)
   {
      if(CompareTag("Component"))
         return;
      if(collision.collider.gameObject.CompareTag("Floor"))
      {
         Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
         rigidbody.bodyType = RigidbodyType2D.Kinematic;
         rigidbody.velocity = Vector2.zero;
      }
   }
}
