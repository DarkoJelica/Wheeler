using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelLineTypes { WALL, FLOOR }

public class LevelLine
{
   public Vector2 Anchor
   {
      get
      {
         return lineAnchor;
      }
      set
      {
         if(segmentSize.SqrMagnitude() < Mathf.Epsilon)
            SetSegmentSize();
         lineAnchor = value - (Type == LevelLineTypes.FLOOR ? new Vector2(0, segmentSize.y / 2) : new Vector2(segmentSize.x / 2, 0));
         MoveSegments();
      }
   }
   public float Size
   {
      get
      {
         return lineSize;
      }
      set
      {
         lineSize = value;
         Resize();
      }
   }
   public LevelLineTypes Type { get; private set; }

   GameObject parentObject;
   GameObject segmentObject;
   Vector2 segmentSize;
   float lineSize;
   Vector2 lineAnchor;
   List<GameObject> segments;

   public LevelLine(GameObject parent, GameObject segment, LevelLineTypes type, Vector2 anchor, float size = 0)
   {
      parentObject = parent;
      segmentObject = segment;
      segments = new List<GameObject>();
      Type = type;
      Anchor = anchor;
      SetSegmentSize();
      Size = size;
   }

   public void Clear()
   {
      for(int i = 0; i < segments.Count; ++i)
         Object.DestroyImmediate(segments[i]);
   }

   void Resize()
   {
      if(segmentSize.SqrMagnitude() < Mathf.Epsilon)
         SetSegmentSize();
      float segmentLength = Type == LevelLineTypes.FLOOR ? segmentSize.x : segmentSize.y;
      int segmentCount = lineSize > Mathf.Epsilon ? Mathf.CeilToInt(lineSize / segmentLength) : 0;
      while(segments.Count < segmentCount)
      {
         GameObject newSegment = Object.Instantiate(segmentObject);
         newSegment.transform.parent = parentObject.transform;
         segments.Add(newSegment);
      }
      while(segments.Count > segmentCount)
      {
         Object.DestroyImmediate(segments[segments.Count - 1]);
         segments.RemoveAt(segments.Count - 1);
      }
      MoveSegments();
   }

   void MoveSegments()
   {
      if(segments.Count == 0)
         return;
      if(segmentSize.SqrMagnitude() < Mathf.Epsilon)
         SetSegmentSize();
      Vector2 offset = Type == LevelLineTypes.FLOOR ? Vector2.right * segments.Count * segmentSize.x : Vector2.up * segments.Count * segmentSize.y;
      for(int i = 0; i < segments.Count - 1; ++i)
      {
         if(segments[i] == null)
            continue;
         segments[i].transform.position = Anchor + segmentSize / 2 + offset * i / segments.Count;
         segments[i].transform.position = new Vector3(segments[i].transform.position.x, segments[i].transform.position.y, segmentObject.transform.position.z);
      }
      Vector2 lastOffset = Type == LevelLineTypes.FLOOR ? Vector2.right * (lineSize - segmentSize.x) : Vector2.up * (lineSize - segmentSize.y);
      Vector2 lastPos = Anchor + segmentSize / 2 + lastOffset;
      segments[segments.Count - 1].transform.position = new Vector3(lastPos.x, lastPos.y, segmentObject.transform.position.z);
   }

   void SetSegmentSize()
   {
      if(segmentObject == null)
         segmentSize = Vector2.zero;
      else
      {
         segmentSize = segmentObject.GetComponent<BoxCollider2D>().size;
         segmentSize = segmentObject.transform.rotation * segmentSize;
      }
   }

}
