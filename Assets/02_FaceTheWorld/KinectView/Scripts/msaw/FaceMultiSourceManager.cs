using UnityEngine;
using System.Collections;
using Windows.Kinect;
using UnityEngine.Networking;

public class FaceMultiSourceManager : NetworkBehaviour {
	public int ColorWidth { get; private set; }
	public int ColorHeight { get; private set; }

	private KinectSensor _Sensor;
	private MultiSourceFrameReader _Reader;
	private byte[] _ColorData;
	private Texture2D _ColorTexture;
	private Texture2D _FaceColorTexture;

	private ushort[] _DepthData;

	public Rect FaceSourceRect;

	private Body[] _BodyData = null;

	public void RestartSensor(){
		_Sensor.Close();
		Start ();
	}


	public Body[] GetBodyData()
	{
		return _BodyData;
	}

	public ushort[] GetDepthData()
	{
		return _DepthData;
	}
	
//	public Texture2D GetFaceColorTexture (float _x, float _y, float _witdh, float _height)
//	{
//		FaceSourceRect = new Rect(_x, _y, _witdh, _height);
//		return _FaceColorTexture;
//	}

	public Texture2D GetColorTexture ()
	{
		return _ColorTexture;
	}

	void Start () 
	{

		_Sensor = KinectSensor.GetDefault();
		
		if (_Sensor != null) 
		{
			_Reader = _Sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth |FrameSourceTypes.Body);

			// Color
			var colorFrameDesc = _Sensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Rgba);
			ColorWidth = colorFrameDesc.Width;
			ColorHeight = colorFrameDesc.Height;
			
			_ColorTexture = new Texture2D(colorFrameDesc.Width, colorFrameDesc.Height, TextureFormat.RGBA32, false);
			_ColorData = new byte[colorFrameDesc.BytesPerPixel * colorFrameDesc.LengthInPixels];

			// Depth
			var depthFrameDesc = _Sensor.DepthFrameSource.FrameDescription;
			_DepthData = new ushort[depthFrameDesc.LengthInPixels];


			//body?
			
			if (!_Sensor.IsOpen)
			{
				_Sensor.Open();
			}
		}
	}
	
	void Update () 
	{
		if (_Reader != null) 
		{
			// ich bin mir nicht sicher ob es notwendig 
			// oder überhaupt sinvoll ist die einzelnen If's zu verschachteln
			// dies ist entnommen aus der demo die depth und color in einem objekt angewendet hat
			var frame = _Reader.AcquireLatestFrame();
			if (frame != null)
			{
				// begin color
				var colorFrame = frame.ColorFrameReference.AcquireFrame();
				if (colorFrame != null)
				{
					// begin body
					var bodyFrame = frame.BodyFrameReference.AcquireFrame();
					if (bodyFrame != null)
					{
						if (_BodyData == null)
						{
							_BodyData = new Body[_Sensor.BodyFrameSource.BodyCount];
						}
						
						bodyFrame.GetAndRefreshBodyData(_BodyData);
						// pause body
						// begin depth
						var depthFrame = frame.DepthFrameReference.AcquireFrame();
						if (depthFrame != null)
						{
							depthFrame.CopyFrameDataToArray(_DepthData);
							
							depthFrame.Dispose();
							depthFrame = null;
						}
						// end depth
						// contnue body
						bodyFrame.Dispose();
						bodyFrame = null;
						// end body
						// continue color
						colorFrame.CopyConvertedFrameDataToArray(_ColorData, ColorImageFormat.Rgba);
						
						_ColorTexture.LoadRawTextureData(_ColorData);
						//_FaceColorTexture = CropedTexture(_ColorTexture); 
						//_FaceColorTexture.Apply();
						_ColorTexture.Apply();
					}

					colorFrame.Dispose();
					colorFrame = null;
					// end color
				}
				
				frame = null;
			}
		}
	}

	Texture2D CropedTexture(Texture2D _sourceColorTexture){
		int x = Mathf.FloorToInt(FaceSourceRect.x);
		int y = Mathf.FloorToInt(FaceSourceRect.y);
		int width = Mathf.FloorToInt(FaceSourceRect.width);
		int height = Mathf.FloorToInt(FaceSourceRect.height);
		Color[] pix = _sourceColorTexture.GetPixels(x, y, width, height);
		Texture2D destTex = new Texture2D(width, height);
		// got an error here sometimes 
		/*
		 * Texture '' is degenerate (dimensions 0x0)
UnityEngine.Texture2D:SetPixels(Color[])
FaceMultiSourceManager:CropedTexture(Texture2D) (at Assets/KinectView/Scripts/msaw/FaceMultiSourceManager.cs:137)
FaceMultiSourceManager:Update() (at Assets/KinectView/Scripts/msaw/FaceMultiSourceManager.cs:116)
		 */
		destTex.SetPixels(pix);
		//GetComponent<Renderer>().material.mainTexture = destTex;
		return destTex;
	}

	void OnApplicationQuit()
	{
		if (_Reader != null)
		{
			_Reader.Dispose();
			_Reader = null;
		}
		
		if (_Sensor != null)
		{
			if (_Sensor.IsOpen)
			{
				_Sensor.Close();
			}
			
			_Sensor = null;
		}
	}
}
