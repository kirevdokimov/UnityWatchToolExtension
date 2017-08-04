using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
// ReSharper disable DelegateSubtraction

namespace WatchTool.Editor{
	public class WatchToolWindow : EditorWindow{

		private Vector2 scrollpos;
		private readonly List<WatchToolWindow.WatchField> watchFields = new List<WatchToolWindow.WatchField>();

		[MenuItem("Window/Watch Tool")]
		static void OpenWindow(){
			EditorWindow.GetWindow<WatchToolWindow>(false, "Watch");
		}

		private void OnEnable(){

			EditorApplication.playmodeStateChanged -= OnPlaymodeStateChanged;
			EditorApplication.playmodeStateChanged += OnPlaymodeStateChanged;
			Do();
		}

		void OnPlaymodeStateChanged(){
			// Enter to playmode
			if(EditorApplication.isPlayingOrWillChangePlaymode){
				Do();
			}
//			// Exit playmode
//			if(EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode){
//			}
		}

		private void OnDisable(){
			EditorApplication.playmodeStateChanged -= OnPlaymodeStateChanged;
		}

		private bool wasCompiling = false;

		private void OnGUI(){
			
			if(EditorApplication.isCompiling && !wasCompiling){
				wasCompiling = true;
				
			}else if(!EditorApplication.isCompiling && wasCompiling){
				wasCompiling = false;
				//Debug.Log("End");
				Do();
			}

			if(!position.size.Equals(TypeDrawer.windowSize)){
				TypeDrawer.windowSize = position.size;
			}
			
			EditorGUILayout.BeginVertical();
				scrollpos = EditorGUILayout.BeginScrollView(scrollpos);
				watchFields.ForEach((value) => {
					TypeDrawer.Draw(value.GetName(),value.GetValue());
				});
				EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();

			if(GUILayout.Button("Force Update")){
				Do();
			}

			this.Repaint();
		}


		private void Do(){
			watchFields.Clear();
			foreach(var mono in GameObject.FindObjectsOfType<MonoBehaviour>()){
				//Debug.Log(string.Format("Scan {0} : {1}",mono.gameObject.name,mono.GetType().Name));
				
				IEnumerable<FieldInfo> fieldInfos = mono.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).
					Where(item => item.IsDefined(typeof(WatchAttribute), true));
				
				foreach(var field in fieldInfos){
					WatchAttribute attribute = (WatchAttribute)Attribute.GetCustomAttribute(field,typeof(WatchAttribute),true);

					string label = attribute.name == null
						? string.Format("{0} : {1}", mono.name, field.Name)
						: attribute.name[0] == '!'
							? attribute.name.Substring(1)
							: string.Format("{0} : {1}", mono.name, attribute.name);
					
					/*if(attribute.name == null){
						label = string.Format("{0} : {1}", mono.name, field.Name);
					} else{
						label = attribute.name[0] == '!' ? attribute.name.Substring(1) : string.Format("{0} : {1}", mono.name, attribute.name);
					}*/
					
					watchFields.Add(new WatchField(label,mono,field));
				}
			}
		}

		public struct WatchField{
			private readonly string label;
			private readonly object context;
			private readonly FieldInfo field;

			public WatchField(string lb, object con, FieldInfo fi){
				label = lb;
				context = con;
				field = fi;
			}

			public object GetValue(){
				return field.GetValue(context);
			}

			public string GetName(){
				return label;
			}
		}

	}
}
