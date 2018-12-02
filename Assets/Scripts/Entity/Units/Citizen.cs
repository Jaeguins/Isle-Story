using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Citizen : MonoBehaviour {
    public Vector2Int coord;

    private void Awake() {
        coord = PathFinder.getCellCoordByOffset(transform.position.x, transform.position.z);
    }
    void LateUpdate () {
		
	}
}
