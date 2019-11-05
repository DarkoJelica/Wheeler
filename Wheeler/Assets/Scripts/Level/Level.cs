using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
   public BoundsManager Bounds
   {
      get
      {
         if(bounds == null)
            bounds = GetComponent<BoundsManager>();
         return bounds;
      }
   }
   public Vector2 Size
   {
      get
      {
         return Bounds.Size;
      }
      set
      {
         Bounds.Size = value;
         Bounds.GenerateBounds();
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
   BoundsManager bounds;
   List<LevelLine> floors;
   List<LevelLine> walls;

   public void AddLine(Vector2 anchor, LevelLineTypes type)
   {
      if(type == LevelLineTypes.FLOOR)
         Floors.Add(new LevelLine(gameObject, Bounds.Floor, LevelLineTypes.FLOOR, transform.position));
      else
         Walls.Add(new LevelLine(gameObject, Bounds.Wall, LevelLineTypes.WALL, transform.position));
   }

   public void RemoveLine(LevelLineTypes type, int index)
   {
      if(index < 0)
         return;
      if (type == LevelLineTypes.FLOOR && index < Floors.Count)
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

}
