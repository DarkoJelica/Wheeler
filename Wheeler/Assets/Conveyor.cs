using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{

   public Vector2 Velocity;

   void Start()
   {
      for(int i = 0; i < transform.childCount; ++i)
      {
         Animator animator = transform.GetChild(i).GetComponent<Animator>();
         AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
         animator.Play(currentState.fullPathHash, 0, (float)i / transform.childCount);
      }
   }

   void OnCollisionEnter2D(Collision2D collision)
   {
      if(collision.rigidbody.bodyType == RigidbodyType2D.Dynamic)
         collision.rigidbody.velocity = Velocity;
   }

}
