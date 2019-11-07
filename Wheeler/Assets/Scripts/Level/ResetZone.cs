using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetZone : MonoBehaviour
{
   void OnTriggerEnter2D(Collider2D collision)
   {
      SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
   }
}
