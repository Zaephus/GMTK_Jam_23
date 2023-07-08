
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour {

    [SerializeField]
    private GridManager grid;

    private Vector2Int currentPos;

    [SerializeField]
    private float moveSpeed;

    [SerializeField, Tooltip("Wait time after entity has moved a single tile.")]
    private float afterMovingWaitTime;

    private bool isMoving;

    private Dictionary<Vector2Int, GridNode> gridNodes;
    private List<GridNode> openNodes;
    private List<GridNode> closedNodes;

    public List<Vector2Int> path;

    private void Start() {
        MoveTo(new Vector2Int(10, 1));
    }

    private void Update() {}

    protected void MoveTo(Vector2Int _target) {

        List<Vector2Int> path = CalculatePath(_target);
        Debug.DrawLine(transform.position, new Vector3(path[0].x, path[0].y, 0), Color.yellow, 1000000000f);
        for(int i = 1; i < path.Count; i++) {
            Debug.DrawLine(new Vector3(path[i-1].x, path[i-1].y, 0), new Vector3(path[i].x, path[i].y, 0), Color.yellow, 1000000000f);
        }
        
        // isMoving = true;
        // StartCoroutine(MoveOverPath(path));

    }

    private IEnumerator MoveOverPath(List<Vector2Int> _path) {

        CalculateCurrentPos();

        int pathNodeNum = 0;

        while(isMoving && Vector2Int.Distance(_path[_path.Count-1], currentPos) > Mathf.Epsilon) {

            float completion = 0;

            while(isMoving && Vector2.Distance(_path[pathNodeNum], new Vector2(transform.position.x, transform.position.y)) > 0.001f) {
                transform.position = Vector3.Lerp(new Vector3(currentPos.x, currentPos.y), new Vector3(_path[pathNodeNum].x, _path[pathNodeNum].y), completion);

                completion += moveSpeed * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            pathNodeNum++;
            
            CalculateCurrentPos();

            yield return new WaitForSeconds(afterMovingWaitTime);

        }

    }

    private List<Vector2Int> CalculatePath(Vector2Int _target) {

        CalculateCurrentPos();

        gridNodes = new Dictionary<Vector2Int, GridNode>();
        
        openNodes = new List<GridNode>();
        closedNodes = new List<GridNode>();

        openNodes.Add(new GridNode(currentPos, null, 0, GridHelper.ManhattanDistance(currentPos, _target)));
        gridNodes.Add(openNodes[0].position, openNodes[0]);

        GridNode currentNode = new GridNode();

        bool targetReached = false;

        int i = 0;

        while(i < 100000 && openNodes.Count > 0) {

            currentNode = GetNodeWithLowestScore(openNodes);
            Debug.DrawLine(transform.position, new Vector3(currentNode.position.x, currentNode.position.y), Color.blue, 10000000f);
            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);

            if(currentNode.position == _target) {
                Debug.Log("Found Target");
                targetReached = true;
                break;
            }

            foreach(GridNode neighbour in GetNeighbouringNodes(currentNode)) {

                if(closedNodes.Contains(neighbour)) {
                    continue;
                }

                if(neighbour.finalScore <= currentNode.finalScore || !openNodes.Contains(neighbour)) {
                    neighbour.traveledScore = currentNode.traveledScore + GridHelper.ManhattanDistance(neighbour.position, currentNode.position);
                    neighbour.heuristicScore = GridHelper.ManhattanDistance(neighbour.position, _target);
                    neighbour.parent = currentNode;
                    if(!openNodes.Contains(neighbour)) {
                        openNodes.Add(neighbour);
                    }
                }

            }
            
            i++;

        }

        path = new List<Vector2Int>();

        while(currentNode.parent != null) {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        if(!targetReached) {
            
            int closest = path.Count - 1;
            for(int k = closest-1; k > 0; k--) {
                if(GridHelper.ManhattanDistance(path[k], _target) <= GridHelper.ManhattanDistance(path[k+1], _target)) {
                    closest = k;
                }
            }

            int startAmount = path.Count - closest;
            for(int k = path.Count-1; k > startAmount+1; k--) {
                path.RemoveAt(k);
            }

        }

        return path;

    }

    private GridNode GetNodeWithLowestScore(List<GridNode> _nodes) {
        GridNode node = _nodes[0];
        for(int i = 0; i < _nodes.Count; i++) {
            if(_nodes[i].finalScore <= node.finalScore) {
                node = _nodes[i];
            }
        }
        return node;
    }

    private List<GridNode> GetNeighbouringNodes(GridNode _node) {
        List<GridNode> neighbours = new List<GridNode>();

        Vector2Int neighbourPos;

        neighbourPos = _node.position + new Vector2Int(-1, 0);
        if(grid.tiles.ContainsKey(neighbourPos) && grid.tiles[neighbourPos] == TileType.None) {
            if(gridNodes.ContainsKey(neighbourPos)) {
                neighbours.Add(gridNodes[neighbourPos]);
            }
            else {
                GridNode newNeighbour = new GridNode(neighbourPos, null, 0, 0);
                neighbours.Add(newNeighbour);
                gridNodes.Add(neighbourPos, newNeighbour);
            }
        }

        neighbourPos = _node.position + new Vector2Int(1, 0);
        if(grid.tiles.ContainsKey(neighbourPos) && grid.tiles[neighbourPos] == TileType.None) {
            if(gridNodes.ContainsKey(neighbourPos)) {
                neighbours.Add(gridNodes[neighbourPos]);
            }
            else {
                GridNode newNeighbour = new GridNode(neighbourPos, null, 0, 0);
                neighbours.Add(newNeighbour);
                gridNodes.Add(neighbourPos, newNeighbour);
            }
        }

        neighbourPos = _node.position + new Vector2Int(0, -1);
        if(grid.tiles.ContainsKey(neighbourPos) && grid.tiles[neighbourPos] == TileType.None) {
            if(gridNodes.ContainsKey(neighbourPos)) {
                neighbours.Add(gridNodes[neighbourPos]);
            }
            else {
                GridNode newNeighbour = new GridNode(neighbourPos, null, 0, 0);
                neighbours.Add(newNeighbour);
                gridNodes.Add(neighbourPos, newNeighbour);
            }
        }

        neighbourPos = _node.position + new Vector2Int(0, 1);
        if(grid.tiles.ContainsKey(neighbourPos) && grid.tiles[neighbourPos] == TileType.None) {
            if(gridNodes.ContainsKey(neighbourPos)) {
                neighbours.Add(gridNodes[neighbourPos]);
            }
            else {
                GridNode newNeighbour = new GridNode(neighbourPos, null, 0, 0);
                neighbours.Add(newNeighbour);
                gridNodes.Add(neighbourPos, newNeighbour);
            }
        }

        return neighbours;
    }

    private void CalculateCurrentPos() {
        currentPos = new Vector2Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y)
        );
    }
    
}