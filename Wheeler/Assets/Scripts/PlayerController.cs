using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   public Camera Camera;
   public float Speed;
   public Vector3 CurrentTarget;

   SpriteRenderer spriteRenderer;

   private void Start()
   {
      spriteRenderer = GetComponent<SpriteRenderer>();
   }

   void Update()
   {
      
   }

   private void FixedUpdate()
   {
      Vector3 move = CurrentTarget - gameObject.transform.position;
      move.y = 0;
      move.z = 0;
      gameObject.transform.position += Vector3.ClampMagnitude(move, Time.deltaTime * Speed);
      if(Mathf.Abs(move.x) > Mathf.Epsilon)
         spriteRenderer.flipX = move.x < 0;
   }
}
