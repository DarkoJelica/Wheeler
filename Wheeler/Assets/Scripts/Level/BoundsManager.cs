using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsManager : MonoBehaviour
{
   public GameObject Floor;
   public GameObject Wall;
   public Transform BoundsParent;
   public Vector2 Size;
   
   Vector2 floorSize;
   Vector2 wallSize;
   List<GameObject> bounds;

   void InitSize()
   {
      floorSize = Floor.GetComponent<BoxCollider2D>().size;
      wallSize = Wall.GetComponent<BoxCollider2D>().size;
   }

   public void GenerateBounds()
   {
      if(bounds == null)
      {
         bounds = new List<GameObject>();
         foreach(Transform child in BoundsParent)
            bounds.Add(child.gameObject);
      }
      for(int i = 0; i < bounds.Count; ++i)
      {
         if(Application.isEditor)
            DestroyImmediate(bounds[i]);
         else
            Destroy(bounds[i]);
      }
      bounds.Clear();
      if(floorSize.x < Mathf.Epsilon || wallSize.y < Mathf.Epsilon)
         InitSize();
      for(float x = floorSize.x / 2; x < Size.x; x += floorSize.x)
      {
         GameObject floorPiece = Instantiate(Floor);
         floorPiece.transform.parent = BoundsParent;
         floorPiece.transform.localPosition = new Vector3(x, 0, floorPiece.transform.position.z);
         bounds.Add(floorPiece);
         floorPiece = Instantiate(Floor);
         floorPiece.transform.parent = BoundsParent;
         floorPiece.transform.localPosition = new Vector3(x, Size.y, floorPiece.transform.position.z);
         bounds.Add(floorPiece);
      }
      for(float y = floorSize.y / 2; y < Size.y; y += wallSize.y)
      {
         GameObject wallPiece = Instantiate(Wall);
         wallPiece.transform.parent = BoundsParent;
         wallPiece.transform.localPosition = new Vector3(0, y, wallPiece.transform.position.z);
         bounds.Add(wallPiece);
         wallPiece = Instantiate(Wall);
         wallPiece.transform.parent = BoundsParent;
         wallPiece.transform.localPosition = new Vector3(Size.x, y, wallPiece.transform.position.z);
         bounds.Add(wallPiece);
      }
   }

}
