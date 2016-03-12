using UnityEngine;
using System.Collections;

/// <summary>
/// The M1 array contains GameObjects as cells.
/// M1[0, 0, 0].GetComponent<MeshRenderer>().enabled = true; // = living cell.
/// M1[0, 0, 0].GetComponent<MeshRenderer>().enabled = false; // = dead cell.
/// </summary>
public class GOL : MonoBehaviour {
	public bool activeGUI = true;
	public GameObject obj1, obj2;
	private GameObject obj;//,  cam, center;
	private GameObject[, ,] M1;
	private bool[, ,] M2;
	private bool running = false, autoApply = true, cube = false;
	public float
		lifeP = 12f, // % random life
		objS = 9; // object size
	private int
		gridS = 16, gS = 16,  // grid size
		popMin = 1, popMax = 8, // min, max live neighbors
		repMin = 3, repMax = 4, // min, max live neighbors for reproduction
		gen = 0, // generation count
		cells = 0; // live cell count
	
	/// <summary>
	/// Constructor
	/// </summary>
	void Start() {
		//cam = GameObject.Find("Camera");
		//center = GameObject.Find("Center");
		obj = obj1;
		obj2.GetComponent<MeshRenderer>().enabled = false;
		InitGrid();
	}
	
	/// <summary>
	/// 
	/// </summary>
	void InitGrid() {
		gen = cells = 0;
		obj.transform.localScale = new Vector3(objS * .1f, objS * .1f, objS * .1f);
		M1 = new GameObject[gridS, gridS, gridS];
		M2 = new bool[gridS, gridS, gridS];
		for (int z = 0; z < gridS; z++)
			for (int y = 0; y < gridS; y++)
			for (int x = 0; x < gridS; x++) {
				M1[x, y, z] = Instantiate(obj, new Vector3((float)x, (float)y * -1, (float)z), transform.rotation) as GameObject;
				cells++;
			}
		obj.GetComponent<MeshRenderer>().enabled = false;
		//center.transform.position = M1[(int)gridS / 2, (int)gridS / 2, (int)gridS / 2].transform.position;
		//cam.transform.position = center.transform.position * 2.5f;
		//cam.transform.LookAt(center.transform.position);
		//cam.GetComponent<ShakeCamera>().Shake();
	}
	
	/// <summary>
	/// Game of Life
	/// </summary>
	/// <returns></returns>
	IEnumerator ApplyRules() {
		while (running || autoApply) {
			for (int z = 0; z < gridS; z++) 
				for (int y = 0; y < gridS; y++)
				for (int x = 0; x < gridS; x++) {
					int n = 0; // neighbor count
					for (int l = -1; l < 2; l++)
						for (int k = -1; k < 2; k++)
							for (int j = -1; j < 2; j++)
							if (x == 0 && j == -1 || y == 0 && k == -1 || z == 0 && l == -1) {
							} else if (x == gridS - 1 && j == 1 || y == gridS - 1 && k == 1 || z == gridS - 1 && l == 1) {
					} else if (M1[x + j, y + k, z + l].GetComponent<MeshRenderer>().enabled) n++;
					if (M1[x, y, z].GetComponent<MeshRenderer>().enabled) {
						if (n < popMin || n > popMax) { M2[x, y, z] = false; cells--; } else { M2[x, y, z] = true; }
					} else {
						if (n > repMin && n < repMax) { M2[x, y, z] = true; cells++; } else { M2[x, y, z] = false; }
					}
				}
			for (int z = 0; z < gridS; z++) {
				yield return new WaitForSeconds(.0001f);
				for (int y = 0; y < gridS; y++) for (int x = 0; x < gridS; x++) M1[x, z, y].GetComponent<MeshRenderer>().enabled = M2[x, y, z];
			} 
			gen++;
			running = false;
			//yield return new WaitForSeconds(1 - (.0001f * gridS));
			yield return new WaitForSeconds(0.0001f);
		}
	}
	
	/// <summary>
	/// GUI Handler
	/// </summary>
	void OnGUI() {
		if (activeGUI){
		GUI.Box(new Rect(5, 10, 130, 440), "ESC to Quit");
		//
		GUI.Box(new Rect(10, 35, 120, 55), "Life: " + lifeP + "%");
		lifeP = GUI.HorizontalSlider(new Rect(10, 55, 120, 10), lifeP, 1f, 100f);
		if (GUI.Button(new Rect(10, 70, 120, 20), "Randomize")) {
			cells = 0;
			for (int z = 0; z < gridS; z++) for (int y = 0; y < gridS; y++) for (int x = 0; x < gridS; x++) if (Random.Range(0F, 100.0F) > lifeP) M1[x, y, z].GetComponent<MeshRenderer>().enabled = false; else { M1[x, y, z].GetComponent<MeshRenderer>().enabled = true; cells++; }
			//cam.GetComponent<ShakeCamera>().Shake();
			gen = 1;
		}
		//
		GUI.Box(new Rect(10, 95, 120, 60), "Population\nMin: " + popMin + " Max: " + popMax);
		popMin = (int)GUI.HorizontalSlider(new Rect(10, 130, 120, 10), popMin, 1f, 26f);
		popMax = (int)GUI.HorizontalSlider(new Rect(10, 145, 120, 10), popMax, 1f, 26f);
		GUI.Box(new Rect(10, 160, 120, 60), "Reproduction\nMin: " + repMin + " Max: " + repMax);
		repMin = (int)GUI.HorizontalSlider(new Rect(10, 195, 120, 10), repMin, 1f, 26f);
		repMax = (int)GUI.HorizontalSlider(new Rect(10, 210, 120, 10), repMax, 1f, 26f);
		GUI.Box(new Rect(10, 225, 120, 80), "Generation: " + gen + "\nLiving: " + cells);
		autoApply = GUI.Toggle(new Rect(15, 260, 120, 20), autoApply, " Auto Generate");
		if (GUI.Button(new Rect(10, 285, 120, 20), "Generate")) { running = true; StartCoroutine("ApplyRules"); }
		//
		GUI.Box(new Rect(10, 310, 120, 30), "Grid Size: " + gS);
		gS = (int)GUI.HorizontalSlider(new Rect(10, 330, 120, 20), gS, 2, 24);
		GUI.Box(new Rect(10, 345, 120, 80), "Cell Size: " + objS * 10 + "%");
		objS = (int)GUI.HorizontalSlider(new Rect(10, 365, 120, 20), objS, 2.0f, 20.0f);
		if (GUI.Toggle(new Rect(15, 380, 120, 20), cube, " Cube")) { obj = obj2; cube = true; } else { obj = obj1; cube = false; }
		if (GUI.Button(new Rect(10, 405, 120, 20), "New")) {
			StopCoroutine("ApplyRules");
			for (int z = 0; z < gridS; z++) for (int y = 0; y < gridS; y++) for (int x = 0; x < gridS; x++) Destroy(M1[x, y, z]);
			gridS = gS;
			obj.GetComponent<MeshRenderer>().enabled = true;
			InitGrid();
		}
		//
		GameObject.Find("Lens").GetComponent<MeshRenderer>().enabled = GUI.Toggle(new Rect(15, 430, 120, 20), GameObject.Find("Lens").GetComponent<MeshRenderer>().enabled, " Filter");
	}
	}
}