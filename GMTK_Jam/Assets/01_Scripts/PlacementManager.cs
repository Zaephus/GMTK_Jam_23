
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementManager : MonoBehaviour {

    private enum PlacementMode {
        None,
        Entities,
        Bulldozing
    }
    private PlacementMode mode;

    [SerializeField]
    private GridManager grid;

    [SerializeField]
    private GameObject[] placeableEntities;

    [Header("Entity Cards")]
    [SerializeField]
    private GameObject entityCardPrefab;
    [SerializeField]
    private Transform entityCardTransform;

    [SerializeField]
    private float entityCardPadding;

    private List<EntityCard> entityCards = new List<EntityCard>();

    [Header("SelectIndicator")]

    [SerializeField]
    private SpriteRenderer selectIndicator;
    [SerializeField]
    private Sprite cannotSelectSprite;
    [SerializeField]
    private Color canSelectColor, bulldozeSelectColor, cannotSelectColor;

    private bool canPlace = false;
    private int currentEntityIndex = -1;
    private Vector2Int currentGridPos;

    private void Awake() {
        EntityCard.CardClicked += SelectEntity;
    }
    
    private void Start() {
        
        for(int i = entityCards.Count-1; i >= 0; i--) {
            Destroy(entityCards[i].gameObject);
            entityCards.RemoveAt(i);
        }

        float cardWidth = entityCardPrefab.GetComponent<RectTransform>().rect.width;

        for(int i = 0; i < placeableEntities.Length; i++) {
            EntityCard card = Instantiate(entityCardPrefab, entityCardTransform).GetComponent<EntityCard>();
            card.SetThumbnail(placeableEntities[i].GetComponent<Entity>().thumbnail);
            card.transform.position = entityCardTransform.transform.position + new Vector3(i * (cardWidth + entityCardPadding), 0.0f, 0.0f);
            card.index = i;
            entityCards.Add(card);
        }
        
    }

    private void Update() {
        if(mode != PlacementMode.None) {
            CheckPlacement();
        }

        if(Input.GetMouseButtonDown(0)) {
            if(canPlace && !EventSystem.current.IsPointerOverGameObject()) {
                if(mode == PlacementMode.Entities) {
                    PlaceEntity();
                }
                else if(mode == PlacementMode.Bulldozing) {
                    BulldozeEntity();
                }
            }
        }

        if(Input.GetMouseButtonDown(1)) {
            SelectEntity(-1);
        }
    }

    public void ToggleBulldozing() {
        if(mode == PlacementMode.Bulldozing) {
            selectIndicator.gameObject.SetActive(false);
            mode = PlacementMode.None;
        }
        else {
            mode = PlacementMode.Bulldozing;
            selectIndicator.gameObject.SetActive(true);
            currentEntityIndex = -1;
        }
    }

    private void PlaceEntity() {
        if(grid.entities.ContainsKey(currentGridPos)) {
            Destroy(grid.entities[currentGridPos].gameObject);
        }
        Entity e = Instantiate(placeableEntities[currentEntityIndex], new Vector3(currentGridPos.x, currentGridPos.y), Quaternion.identity).GetComponent<Entity>();
        grid.AddEntity(currentGridPos, e);
    }

    private void BulldozeEntity() {
        Destroy(grid.entities[currentGridPos].gameObject);
        grid.RemoveEntity(currentGridPos);
    }

    private void CheckPlacement() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));

        if(mode == PlacementMode.Entities) {
            if(grid.GetTileType(gridPos) == TileType.None) {
                canPlace = true;
                selectIndicator.color = canSelectColor;
                selectIndicator.sprite = placeableEntities[currentEntityIndex].GetComponent<Entity>().thumbnail;
            }
            else if(grid.GetTileType(gridPos) == TileType.Entity && grid.entities[gridPos].type != EntityType.Hero && grid.entities[gridPos].type != placeableEntities[currentEntityIndex].GetComponent<Entity>().type) {
                canPlace = true;
                selectIndicator.color = canSelectColor;
                selectIndicator.sprite = placeableEntities[currentEntityIndex].GetComponent<Entity>().thumbnail;
            }
            else {
                canPlace = false;
                selectIndicator.color = cannotSelectColor;
                selectIndicator.sprite = cannotSelectSprite;
            }
        }
        else if(mode == PlacementMode.Bulldozing) {
            if(grid.GetTileType(gridPos) == TileType.Entity) {
                canPlace = true;
                selectIndicator.color = bulldozeSelectColor;
                selectIndicator.sprite = cannotSelectSprite;
            }
            else {
                canPlace = false;
                selectIndicator.color = cannotSelectColor;
                selectIndicator.sprite = cannotSelectSprite;
            }
        }

        currentGridPos = gridPos;
        selectIndicator.transform.position = new Vector3(gridPos.x, gridPos.y);
    }

    private void SelectEntity(int _index) {
        if(currentEntityIndex == _index || _index == -1) {
            currentEntityIndex = -1;
            selectIndicator.gameObject.SetActive(false);
            mode = PlacementMode.None;
        }
        else {
            currentEntityIndex = _index;
            selectIndicator.gameObject.SetActive(true);
            mode = PlacementMode.Entities;
        }
    }
    
}