using System;
namespace RenderApp.Render_System.Post_Effect
{
	public partial class Selection : PostEffect
	{
		private int _uID;
		public int uID
		{
			get
			{
				return _uID;
			}
			set
			{
				SetValue<int>(ref _uID,value); 
			}
		}


	}
}