using System;
using KI.Gfx.KITexture;
namespace KI.Renderer
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
				SetValue<int>(ref _uID, value); 
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
				SetValue<int>(ref _uScale, value); 
			}
		}
		private float[] _uWeight;
		public float[] uWeight
		{
			get
			{
				return _uWeight;
			}

			set
			{
				SetValue<float[]>(ref _uWeight, value); 
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
				SetValue<Texture>(ref _uTarget, value); 
			}
		}
		private bool _uHorizon;
		public bool uHorizon
		{
			get
			{
				return _uHorizon;
			}

			set
			{
				SetValue<bool>(ref _uHorizon, value); 
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
				SetValue<Texture>(ref _uTarget, value); 
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
				SetValue<int>(ref _uWidth, value); 
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
				SetValue<int>(ref _uHeight, value); 
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
				SetValue<float>(ref _uThreshold, value); 
			}
		}
	}
	public partial class SSAO : RenderTechnique
	{
		private Texture _uPosition;
		public Texture uPosition
		{
			get
			{
				return _uPosition;
			}

			set
			{
				SetValue<Texture>(ref _uPosition, value); 
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
				SetValue<Texture>(ref _uTarget, value); 
			}
		}
		private float[] _uSample;
		public float[] uSample
		{
			get
			{
				return _uSample;
			}

			set
			{
				SetValue<float[]>(ref _uSample, value); 
			}
		}
	}
	public partial class SSLIC : RenderTechnique
	{
		private Texture _uPosit;
		public Texture uPosit
		{
			get
			{
				return _uPosit;
			}

			set
			{
				SetValue<Texture>(ref _uPosit, value); 
			}
		}
		private Texture _uNormal;
		public Texture uNormal
		{
			get
			{
				return _uNormal;
			}

			set
			{
				SetValue<Texture>(ref _uNormal, value); 
			}
		}
		private Texture _uVector;
		public Texture uVector
		{
			get
			{
				return _uVector;
			}

			set
			{
				SetValue<Texture>(ref _uVector, value); 
			}
		}
		private Texture _uNoize;
		public Texture uNoize
		{
			get
			{
				return _uNoize;
			}

			set
			{
				SetValue<Texture>(ref _uNoize, value); 
			}
		}
		private Texture _uTexCoord;
		public Texture uTexCoord
		{
			get
			{
				return _uTexCoord;
			}

			set
			{
				SetValue<Texture>(ref _uTexCoord, value); 
			}
		}
	}
	public partial class OutputBuffer : RenderTechnique
	{
		private Texture _uSelectMap;
		public Texture uSelectMap
		{
			get
			{
				return _uSelectMap;
			}

			set
			{
				SetValue<Texture>(ref _uSelectMap, value); 
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
				SetValue<Texture>(ref _uTarget, value); 
			}
		}
	}
}