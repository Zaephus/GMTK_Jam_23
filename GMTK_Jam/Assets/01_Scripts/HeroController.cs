
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour {

    [SerializeField]
    private Grid grid;

    private void Start() {
        Debug.Log(grid.LocalToCell(new Vector3(0.0f, -1.0f, 0.0f)));
    }

}