
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

    public static System.Action<Vector2Int, Vector2Int> EntityMovedCall;

    private Dictionary<Vector2Int, TileType> tiles = new Dictionary<Vector2Int, TileType>();
    
    private void Start() {
        EntityMovedCall += EntityMoved;
    }

    private void Update() {
        
    }

    public TileType GetTileType(Vector2Int _pos) {
        if(!tiles.ContainsKey(_pos)) {
            tiles.Add(_pos, TileType.None);
            return TileType.None;
        }

        return tiles[_pos];
    }

    private void EntityMoved(Vector2Int _oldPos, Vector2Int _newPos) {
        if(tiles.ContainsKey(_oldPos)) {
            tiles.Remove(_oldPos);
        }
        if(tiles.ContainsKey(_newPos)) {
            tiles[_newPos] = TileType.Entity;
        }
        else {
            tiles.Add(_newPos, TileType.Entity);
        }
    }

}