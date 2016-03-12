using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameInitializer : MonoBehaviour {
    public GenerationTimer GenerationTimer;
    public BoardTicker BoardTicker;

    void Start () {
        var board = new BoardCircle();
        BoardTicker.Initialize(board);
        GenerationTimer.Initialize(BoardTicker);
    }
}
