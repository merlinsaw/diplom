using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CellFactory {
    public static CellState Create(Vector3 position, bool isAlive) {
        var instance = GameObject.Instantiate(CellPrefab, position, Quaternion.identity) as GameObject;
		instance.GetComponent<CellInitializer>().Initialize(isAlive);
		instance.transform.SetParent(Parent.transform);
        return instance.GetComponent<CellState>();
    }

    private static GameObject CellPrefab {
        get { return PrefabRepository.Instance.CellPrefab; }
    }
	private static GameObject Parent {
		get { return PrefabRepository.Instance.GridLayer; }
	}
}
