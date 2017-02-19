using System;
using KI.Gfx.KIAsset;
namespace RenderApp.Render_System.Post_Effect
{
	public partial class Selection : RenderTechnique
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
	public partial class Bloom : RenderTechnique
	{
		private int _uScale;
		public int uScale
		{
			get
			{
				return _uScale;
			}
			set
			{
				SetValue<int>(ref _uScale,value); 
			}
		}

		private int _uWeight;
		public int uWeight
		{
			get
			{
				return _uWeight;
			}
			set
			{
				SetValue<int>(ref _uWeight,value); 
			}
		}

		private Texture _uTarget;
		public Texture uTarget
		{
			get
			{
				return _uTarget;
			}
			set
			{
				SetValue<Texture>(ref _uTarget,value); 
			}
		}


	}
	public partial class Sobel : RenderTechnique
	{
		private Texture _uTarget;
		public Texture uTarget
		{
			get
			{
				return _uTarget;
			}
			set
			{
				SetValue<Texture>(ref _uTarget,value); 
			}
		}

		private int _uWidth;
		public int uWidth
		{
			get
			{
				return _uWidth;
			}
			set
			{
				SetValue<int>(ref _uWidth,value); 
			}
		}

		private int _uHeight;
		public int uHeight
		{
			get
			{
				return _uHeight;
			}
			set
			{
				SetValue<int>(ref _uHeight,value); 
			}
		}

		private float _uThreshold;
		public float uThreshold
		{
			get
			{
				return _uThreshold;
			}
			set
			{
				SetValue<float>(ref _uThreshold,value); 
			}
		}


	}
}