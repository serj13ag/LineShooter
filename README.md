# Line Shooter

One of the test task projects.

# Task

Summary:
Enemies are moving from top to bottom of the screen in portrait orientation along 3 lines. The player moves along the bottom of the playing field and shoots enemies
to prevent them from reaching the finish line.

Player logic:
- Player located at the bottom of the screen, user controls player movement with WASD keys
- Player movement is limited to a rectangular area. On the left, on the right, at the bottom - the borders of the screen, on top - the finish line
- Player automatically shoots at the nearest enemy inside the radius of his attack

Enemy logic:
- Enemies are spawned randomly in one of 3 spawn points with a certain cooldown
- Enemy moving straight to the bottom of the screen with a certain speed
- When enemy crosses finish line it deals 1 damage to the player and dissapears
- When player hits the enemy the health of the enemy decreases by player damage value
- When enemy health decreased by 0 he dies;

Win and defeat:
- Player wins when all enemies died
- Player loses when his health reaches 0

UI:
- At the top of the screen you can see player healthbar
- When you lose, a window opens with the title "Defeat" and a button “Restart”, by clicking on that button the game restarts
- When you win, a window opens with the title "Victory" and the “Restart” button

Settings:
- Number of enemies that player needs to destroy in order to win [range int]. At the start of the game this number is randomized between min and max values
- Cooldown for a enemy to spawn [range float]. Next enemy spawns after randomized value between min and max
- Enemy moving speed [range float]. Speed of each enemy randomized between min and max
- Enemy health [int]
- Player health [int]
- Player attack range [float]
- Player attack speed [float]
- Player shoot damage [int]
- Player projectile speed [float]

Tech requirements:
- Project must be 2D
- Implement enemy spawn logic using "Factory" pattern
- Scene orientation is vertical

## Key Features

### Application Entry Point
All application initialization starts from a single place. That approach helps you to control services initialization and to avoid problems related to Unity Script Execution Order. <br/>
[Bootstrapper.cs](../master/Assets/Scripts/Infrastructure/Bootstrapper.cs)

### Game State Machine
State machine is used to define core application states (LoadSaveData, MainMenu, GameplayLoop). It allows you to switch states from any place when needed. <br/>
States have Enter() and Exit() methods where you can control transitions between states and define core logic. <br/>
[GameStateMachine.cs](../master/Assets/Scripts/Infrastructure/StateMachine/GameStateMachine.cs)

### Composition Root
Defined places where all services initialization is happening. That approach is useful for dependencies control and clear overview of functionality. <br/>
One place is Bootstrapper for global services that active during application lifetime. <br/>
Second place is GameplayLevelState where all game loop related services are initializing and disposing. <br/>
[Bootstrapper.cs](../master/Assets/Scripts/Infrastructure/Bootstrapper.cs)
[GameplayLevelState.cs](../master/Assets/Scripts/Infrastructure/StateMachine/GameplayLevelState.cs)

### Service Locator
One place where all services being registered, disposed and accessed from. <br/>
Service locator have static Instance property to easy access services from MonoBehaviour objects <br/>
[ServiceLocator.cs](../master/Assets/Scripts/Infrastructure/ServiceLocator.cs)

### Dependency Injection
All services and entities gets their dependencies in constructor or Awake() method (for MonoBehaviour objects). <br/>
Services are passed through interfaces, that allows to change concrete implementations, apply tests and use DI container in the future without lots of refactoring. <br/>
