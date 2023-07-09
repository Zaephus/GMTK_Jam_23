
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private GameState state;
    public GameState gameState {
        get {
            return state;
        }
        set {
            state = value;
            HandleStateChanges();
        }
    }

    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject level;
    [SerializeField]
    private PlacementManager placementManager;
    
    private void Start() {
        gameState = GameState.MainMenu;
    }

    private void Update() {
        
    }

    public void StartGame() {
        gameState = GameState.Placing;
    }

    public void StartSimulating() {
        gameState = GameState.Simulating;
    }

    private void HandleStateChanges() {

        switch(gameState) {
            
            case GameState.MainMenu:
                mainMenu.SetActive(true);
                level.SetActive(false);
                break;

            case GameState.Placing:
                mainMenu.SetActive(false);
                level.SetActive(true);
                placementManager.gameObject.SetActive(true);
                break;

            case GameState.Simulating:
                mainMenu.SetActive(false);
                level.SetActive(true);
                placementManager.gameObject.SetActive(false);
                break;

        }
        
    }
    
}