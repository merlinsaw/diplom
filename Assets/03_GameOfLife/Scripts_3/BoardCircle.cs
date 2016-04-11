using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardCircle {
    public const int SIZE = 12;
	public const int SIZE_X = 12;
	public const int SIZE_Y = 12;
	public const float SCALE = 2f; // height distance from each other
    public CellState[,] Matrix;

	public Vector3 centerPos = new Vector3(5,-1.0f,5);    //center of circle/elipsoid
	public float radiusX = 7f,radiusY = 4f;                    //radii for each x,y axes, respectively
	public float radiusOffset = 7; 

	public bool isCircular = false;                  //is the drawn shape a complete circle?
	public bool vertical = false;                     //is the drawb shape on the xy-plane?

	Vector3 pointPos;                                //position to place each prefab along the given circle/eliptoid

    public BoardCircle() {
		Matrix = new CellState[SIZE_X, SIZE_Y];

		for (int x = 0; x < SIZE_X; x++) {
			for (int y = 0; y < SIZE_Y; y++) {
				//multiply 'i' by '1.0f' to ensure the result is a fraction
				float pointNum = (x*1.0f)/SIZE_X;
				//angle along the unit circle for placing points
				float angle = pointNum*Mathf.PI*2;

				 float X = Mathf.Sin (angle)*radiusX; // for radial placement and simple height
				 float Y = Mathf.Cos (angle)*radiusY; // for radial placement and simple height
				//float X = Mathf.Sin (angle)*(radiusX + radiusOffset* y); // for placement in a ring like fasion on the ground
				//float Y = Mathf.Cos (angle)*(radiusY + radiusOffset* y); // for placement in a ring like fasion on the ground

				//position for the point prefab
				if(vertical)
					pointPos = new Vector3(X, Y)+centerPos;
				else if (!vertical){
					pointPos = new Vector3(X, y*SCALE- (SIZE_Y / 2)*SCALE, Y)+centerPos; // for centered pivot of the entire group
					pointPos = new Vector3(X, y*SCALE, Y)+centerPos; // for placement from bottom to top
					//pointPos = new Vector3(X, centerPos.y, Y)+centerPos; // for placement in a ring like fasion on the ground
				}
                //var position = new Vector3(x - (SIZE_X / 2), y - (SIZE_Y / 2), 0);
				//var position = new Vector3(x*SCALE- (SIZE_X / 2)*SCALE,y*SCALE- (SIZE_Y / 2)*SCALE,0);
				var position = pointPos;
                Matrix[x, y] = CellFactory.Create(position, Random.Range(1, 3) == 1);
            }
        }
    }

    public CellState Cell(int x, int y) {
        return Matrix[x, y];
    }

    public bool IsLiveCell(int x, int y) {
        return Matrix[x, y].IsAlive;
    }

    public int LiveNeighborCount(int x, int y) {
        var count = 0;
        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                if (!(i == 0 && j == 0)) {
                    var newX = x + i;
                    var newY = y + j;
                    if (IsOnBoard(newX, newY) && IsLiveCell(newX, newY)) {
                        count++;
                    }
                }
            }
        }

        return count;
    }

    private bool IsOnBoard(int x, int y) {
        return x >= 0 && y >= 0 && x < SIZE_X && y < SIZE_Y;
    }
}
