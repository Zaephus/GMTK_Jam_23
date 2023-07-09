
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour {

    public Dictionary<Vector2Int, TileType> tiles = new Dictionary<Vector2Int, TileType>();
    public Dictionary<Vector2Int, Entity> entities = new Dictionary<Vector2Int, Entity>();

    [SerializeField]
    private Tilemap topMap;
    
    private void Awake() {

        for(int x = 0; x < topMap.localBounds.extents.x * 2; x++) {
            for(int y = 0; y < topMap.localBounds.extents.y * 2; y++) {
                if(topMap.HasTile(new Vector3Int(topMap.origin.x, topMap.origin.y) + new Vector3Int(x, y))) {
                    tiles.Add((new Vector2Int(topMap.origin.x, topMap.origin.y) + new Vector2Int(x, y)), TileType.Wall);
                }
                else {
                    tiles.Add((new Vector2Int(topMap.origin.x, topMap.origin.y) + new Vector2Int(x, y)), TileType.None);
                }
            }
        }

    }

    private void Update() {}

    public TileType GetTileType(Vector2Int _pos) {
        if(!tiles.ContainsKey(_pos)) {
            tiles.Add(_pos, TileType.None);
            return TileType.None;
        }

        return tiles[_pos];
    }

    public void AddEntity(Vector2Int _pos, Entity _entity) {
        if(tiles.ContainsKey(_pos)) {
            tiles[_pos] = TileType.Entity;
        }
        else {
            tiles.Add(_pos, TileType.Entity);
        }

        if(entities.ContainsKey(_pos)) {
            entities[_pos] = _entity;
        }
        else {
            entities.Add(_pos, _entity);
        }
    }

    public void RemoveEntity(Vector2Int _pos) {
        if(tiles.ContainsKey(_pos)) {
            tiles[_pos] = TileType.None;
        }
        if(entities.ContainsKey(_pos)) {
            entities.Remove(_pos);
        }
    }

}