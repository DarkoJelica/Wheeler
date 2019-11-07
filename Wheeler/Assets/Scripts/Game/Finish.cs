using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
   void OnTriggerEnter2D(Collider2D collider)
   {
      if(collider.CompareTag("Player"))
         SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
   }

}
