using UnityEngine;
using System.Collections;

public class FramesPerSecond : MonoBehaviour
{
	public bool ShowFPS = true;
	Rect fpsRect;
	GUIStyle style;
	float fps;
	// Use this for initialization
	void Start ()
	{
		fpsRect = new Rect(0,0,400,100);
		style = new GUIStyle();
		style.normal.textColor = Color.red; 
		style.fontSize = 20;

		StartCoroutine(RecalculateFPS());

	}

	private IEnumerator RecalculateFPS()
	{
		while (ShowFPS)
		{
			fps=1/Time.deltaTime;
			yield return new WaitForSeconds(1);
		}
	}

	void OnGUI()
	{
		if (ShowFPS)
		{
		GUI.Label(fpsRect, "FPS: " + string.Format ("{0:0.0}" ,fps),style);
		}
	}
}

