using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
   public GameObject Instructions;

   void Start()
   {
      Instructions.SetActive(false);
   }

   public void ToggleInstructions()
   {
      Instructions.SetActive(!Instructions.activeInHierarchy);
   }

   public void StartGame()
   {
      SceneManager.LoadSceneAsync(1);
   }
}
