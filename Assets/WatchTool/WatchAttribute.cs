using System;

namespace WatchTool{
	[AttributeUsage(AttributeTargets.Field)]
	public class WatchAttribute : Attribute{
		
		public string name;
		
		public WatchAttribute(string n){
			name = n;
		}
		
		public WatchAttribute(){
			name = null;
		}
	}
}
