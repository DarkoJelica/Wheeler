using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
   Level level;
   int selectedLine = 0;
   LevelLineTypes lineType = LevelLineTypes.FLOOR;

   void OnEnable()
   {
      level = target as Level;
   }

   public override void OnInspectorGUI()
   {
      Vector2 size = level.Size;
      EditorGUI.BeginChangeCheck();
      size = EditorGUILayout.Vector2Field("Level Size", size);
      if(EditorGUI.EndChangeCheck())
         level.Size = size;
      lineType = (LevelLineTypes)EditorGUILayout.EnumPopup(lineType);
      if((lineType == LevelLineTypes.FLOOR && level.Floors.Count > 0) || (lineType == LevelLineTypes.WALL && level.Walls.Count > 0))
         selectedLine = EditorGUILayout.IntSlider(selectedLine, 0, lineType == LevelLineTypes.FLOOR ? level.Floors.Count - 1 : level.Walls.Count - 1);
      else
         selectedLine = -1;
      if(GUILayout.Button("Add Line"))
         level.AddLine(Vector2.zero, lineType);
      if(GUILayout.Button("Remove Line"))
         level.RemoveLine(lineType, selectedLine);
      LevelLine line = GetActiveLine();
      if(line == null)
         return;
      Vector2 anchor = line.Anchor;
      EditorGUI.BeginChangeCheck();
      anchor = EditorGUILayout.Vector2Field("Line Anchor", anchor);
      if(EditorGUI.EndChangeCheck())
         line.Anchor = anchor;
      float lineSize = line.Size;
      EditorGUI.BeginChangeCheck();
      lineSize = EditorGUILayout.FloatField("Line Size", lineSize);
      if(EditorGUI.EndChangeCheck())
         line.Size = lineSize;
   }

   void OnSceneGUI()
   {
      LevelLine line = GetActiveLine();
      if(line == null)
         return;
      Vector3 anchorPos = Handles.PositionHandle(line.Anchor, Quaternion.identity);
      if((line.Anchor - new Vector2(anchorPos.x, anchorPos.y)).SqrMagnitude() > Mathf.Epsilon)
         line.Anchor = anchorPos;
      Vector3 sizeOffset = lineType == LevelLineTypes.FLOOR ? Vector3.right * line.Size : Vector3.up * line.Size;
      Vector3 sizePos = Handles.PositionHandle(anchorPos + sizeOffset, Quaternion.identity);
      float lineSize = lineType == LevelLineTypes.FLOOR ? sizePos.x - anchorPos.x : sizePos.y - anchorPos.y;
      if(Mathf.Abs(lineSize - line.Size) > Mathf.Epsilon)
         line.Size = lineSize;
   }

   LevelLine GetActiveLine()
   {
      LevelLine line = null;
      if(lineType == LevelLineTypes.FLOOR && selectedLine >= 0 && selectedLine < level.Floors.Count)
         line = level.Floors[selectedLine];
      if(lineType == LevelLineTypes.WALL && selectedLine >= 0 && selectedLine < level.Walls.Count)
         line = level.Walls[selectedLine];
      return line;
   }

}