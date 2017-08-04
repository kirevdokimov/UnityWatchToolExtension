using System;
using UnityEditor;
using UnityEngine;

namespace WatchTool.Editor{
	public static class TypeDrawer{
		
		public static Vector2 windowSize = Vector2.zero;
		
		private static string v2s(Vector3 v3){
			return string.Format("({0:F2}, {1:F2}, {2:F2})", v3.x, v3.y, v3.z);
		}

		public static void Draw(string name,object value){
			Type type;
			if(value == null){
				type = typeof(string);
				value = "null";
			}else
				type = value.GetType();
			
			EditorGUILayout.BeginHorizontal();
				GUILayout.Label(name,GUILayout.Width(windowSize.x/2));
				
				if(type == typeof(Vector3)){
					GUILayout.Label(v2s((Vector3)value));
				} else {
					GUILayout.Label(value.ToString());
				}
			EditorGUILayout.EndHorizontal();
		}
	}
}