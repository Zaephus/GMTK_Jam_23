
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridHelper {

    public static int ManhattanDistance(Vector2Int _a, Vector2Int _b) {
        return Mathf.Abs(_a.x - _b.x) + Mathf.Abs(_a.y - _b.y);
    }

    public static int ManhattanDistance(Vector3Int _a, Vector3Int _b) {
        return Mathf.Abs(_a.x - _b.x) + Mathf.Abs(_a.y - _b.y) + Mathf.Abs(_a.z - _b.z);
    }
    
}

public class GridNode {

    public Vector2Int position;
    public GridNode parent;

    public int finalScore {
        get { return traveledScore + heuristicScore; }
    }
    public int traveledScore;
    public int heuristicScore;

    public GridNode() {}
    public GridNode(Vector2Int _pos, GridNode _parent, int _tScore, int _hScore) {
        position = _pos;
        parent = _parent;
        traveledScore = _tScore;
        heuristicScore = _hScore;
    }

}