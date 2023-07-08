
public enum TileType {
    None = 0,
    Wall = 1,
    Entity = 2
}

public enum EntityType {
    Hero = 0,
    Slime = 1,
    TestEnemy = 2
}

public enum GameState {
    Waiting = 0,
    MainMenu = 1,
    Placing = 100,
    Simulating = 101
}