using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsManager : MonoBehaviour
{
   public GameObject Floor;
   public GameObject Wall;
   public Vector2 Size;
   
   Vector2 floorSize;
   Vector2 wallSize;

   void Start()
   {
      floorSize = Floor.GetComponent<BoxCollider2D>().size;
      wallSize = Wall.GetComponent<BoxCollider2D>().size;
      GenerateBounds();
   }

   public void GenerateBounds()
   {
      foreach(Transform child in transform)
         Destroy(child.gameObject);
      for(float x = floorSize.x / 2; x < Size.x; x += floorSize.x)
      {
         GameObject floorPiece = Instantiate(Floor);
         floorPiece.transform.parent = transform;
         floorPiece.transform.localPosition = new Vector3(x, 0, floorPiece.transform.position.z);
         floorPiece = Instantiate(Floor);
         floorPiece.transform.parent = transform;
         floorPiece.transform.localPosition = new Vector3(x, Size.y, floorPiece.transform.position.z);
      }
      for(float y = floorSize.y / 2; y < Size.y; y += wallSize.y)
      {
         GameObject wallPiece = Instantiate(Wall);
         wallPiece.transform.parent = transform;
         wallPiece.transform.localPosition = new Vector3(0, y, wallPiece.transform.position.z);
         wallPiece = Instantiate(Wall);
         wallPiece.transform.parent = transform;
         wallPiece.transform.localPosition = new Vector3(Size.x, y, wallPiece.transform.position.z);
      }
   }

}
