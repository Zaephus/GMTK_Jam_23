
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

    private void Start() {
        MoveTo(new Vector2Int(3, 6));
    }

    private void Update() {

    }

    protected void MoveTo(Vector2Int _target) {

        List<Vector2Int> path = CalculatePath(_target);
        for(int i = 1; i < path.Count; i++) {
            Debug.DrawLine(new Vector3(path[i-1].x, path[i-1].y, 0), new Vector3(path[i].x, path[i].y, 0), Color.yellow, 1000000000f);
        }
        
        isMoving = true;
        StartCoroutine(Move(_target, path));

    }

    private IEnumerator Move(Vector2Int _target, List<Vector2Int> _path) {

        CalculateCurrentPos();

        int pathNodeNum = 0;

        while(isMoving && Vector2Int.Distance(_target, currentPos) > Mathf.Epsilon) {

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
        List<Vector2Int> path = new List<Vector2Int>();

        Vector2Int tempPos = currentPos;
        Vector2Int diffVector = _target - tempPos;

        int i = 0;
        int maxIterations = (diffVector.x + diffVector.y) * 10;

        while(i < maxIterations && diffVector != Vector2Int.zero) {

            if(diffVector.x != 0 && diffVector.x >= diffVector.y) {
                if(diffVector.x > 0) {
                    tempPos += new Vector2Int(1, 0);
                }
                else if(diffVector.x < 0) {
                    tempPos += new Vector2Int(-1, 0);
                }
            }
            else if(diffVector.y != 0 && diffVector.y > diffVector.x) {
                if(diffVector.y > 0) {
                    tempPos += new Vector2Int(0, 1);
                }
                else if(diffVector.y < 0) {
                    tempPos += new Vector2Int(0, -1);
                }
            }

            path.Add(tempPos);

            diffVector = _target - tempPos;
            i++;

        }

        return path;

    }

    private void CalculateCurrentPos() {
        currentPos = new Vector2Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y)
        );
    }
    
}