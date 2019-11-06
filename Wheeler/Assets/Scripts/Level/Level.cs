using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
   public GameObject Floor;
   public GameObject Wall;
   public Vector2 Size
   {
      get
      {
         return size;
      }
      set
      {
         size = value;
         SetEdges();
      }
   }

   public List<LevelLine> Floors
   {
      get
      {
         if(floors == null)
            floors = new List<LevelLine>();
         return floors;
      }
   }
   public List<LevelLine> Walls
   {
      get
      {
         if(walls == null)
            walls = new List<LevelLine>();
         return walls;
      }
   }
   List<LevelLine> floors;
   List<LevelLine> walls;
   LevelLine bottomEdge;
   LevelLine topEdge;
   LevelLine leftEdge;
   LevelLine rightEdge;
   [SerializeField]
   Vector2 size;
   GameObject linesParent;

   public void AddLine(Vector2 anchor, LevelLineTypes type)
   {
      if(linesParent == null)
         linesParent = transform.Find("Lines").gameObject;
      if(type == LevelLineTypes.FLOOR)
         Floors.Add(new LevelLine(linesParent, Floor, LevelLineTypes.FLOOR, transform.position));
      else
         Walls.Add(new LevelLine(linesParent, Wall, LevelLineTypes.WALL, transform.position));
   }

   public void RemoveLine(LevelLineTypes type, int index)
   {
      if(index < 0)
         return;
      if(type == LevelLineTypes.FLOOR && index < Floors.Count)
      {
         Floors[index].Clear();
         Floors.RemoveAt(index);
      }
      else if(index < Walls.Count)
      {
         Walls[index].Clear();
         Walls.RemoveAt(index);
      }
   }

   public void ResizeLine(LevelLineTypes type, int index, float newSize)
   {
      if(index < 0)
         return;
      if((type == LevelLineTypes.FLOOR && index >= Floors.Count) || (type == LevelLineTypes.WALL && index >= Walls.Count))
         return;
      if(type == LevelLineTypes.FLOOR)
         Floors[index].Size = newSize;
      else
         Walls[index].Size = newSize;
   }

   public void MoveLine(LevelLineTypes type, int index, Vector2 newAnchor)
   {
      if(index < 0)
         return;
      if((type == LevelLineTypes.FLOOR && index >= Floors.Count) || (type == LevelLineTypes.WALL && index >= Walls.Count))
         return;
      if(type == LevelLineTypes.FLOOR)
         Floors[index].Anchor = newAnchor;
      else
         Walls[index].Anchor = newAnchor;
   }

   public void Clear()
   {
      if(Floors != null)
      {
         for(int i = 0; i < Floors.Count; ++i)
            Floors[i].Clear();
         Floors.Clear();
      }
      if(Walls != null)
      {
         for(int i = 0; i < Walls.Count; ++i)
            Walls[i].Clear();
         Walls.Clear();
      }
      if(bottomEdge != null)
      {
         bottomEdge.Clear();
         bottomEdge = null;
      }
      if(topEdge != null)
      {
         topEdge.Clear();
         topEdge = null;
      }
      if(leftEdge != null)
      {
         leftEdge.Clear();
         leftEdge = null;
      }
      if(rightEdge != null)
      {
         rightEdge.Clear();
         rightEdge = null;
      }
      if(linesParent == null)
         linesParent = transform.Find("Lines").gameObject;
      for(int i = linesParent.transform.childCount - 1; i >= 0; --i)
         DestroyImmediate(linesParent.transform.GetChild(i).gameObject);
   }

   void SetEdges()
   {
      if(linesParent == null)
         linesParent = transform.Find("Lines").gameObject;
      if(bottomEdge == null)
         bottomEdge = new LevelLine(linesParent, Floor, LevelLineTypes.FLOOR, Vector2.zero);
      if(topEdge == null)
         topEdge = new LevelLine(linesParent, Floor, LevelLineTypes.FLOOR, Vector2.zero);
      if(leftEdge == null)
         leftEdge = new LevelLine(linesParent, Wall, LevelLineTypes.WALL, Vector2.zero);
      if(rightEdge == null)
         rightEdge = new LevelLine(linesParent, Wall, LevelLineTypes.WALL, Vector2.zero);
      bottomEdge.Anchor = transform.position;
      bottomEdge.Size = Size.x;
      topEdge.Anchor = (Vector2)transform.position + Vector2.up * Size.y;
      topEdge.Size = Size.x;
      leftEdge.Anchor = transform.position;
      leftEdge.Size = Size.y;
      rightEdge.Anchor = (Vector2)transform.position + Vector2.right * Size.x;
      rightEdge.Size = Size.y;
   }

}
