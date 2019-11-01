using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
   public Sprite Sprite;

   void OnCollisionEnter(Collision collision)
   {
      if(CompareTag("Component"))
         return;
      if(collision.collider.gameObject.CompareTag("Floor"))
         GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
   }
}
