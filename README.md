# Line Shooter

One of the test task projects.

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
