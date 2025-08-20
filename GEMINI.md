# GEMINI.md

## Project Overview

This is a Unity project for a game called "FlappyBoiz". It appears to be a "Flappy Bird" style game. The project is built with Unity `6000.0.55f1` and utilizes a modern, decoupled architecture with various assets from the Unity Asset store and other third-party libraries.

### Key Technologies & Libraries

*   **Unity:** Version `6000.0.55f1`
*   **IDE:** JetBrains Rider (suggested by the presence of `.sln.DotSettings` file)
*   **Dependency Injection:** `VContainer` is used for managing dependencies, promoting a clean and decoupled architecture.
*   **Asynchronous Operations:** `UniTask` is used for managing asynchronous operations, which is a modern and efficient way to handle async code in Unity.
*   **Messaging:** `MessagePipe` is used for event handling and communication between different parts of the game.
*   **Animation:** `LitMotion` is used for tweening and animations.
*   **Input:** The new Unity Input System (`com.unity.inputsystem`) is used for handling user input.
*   **Unity Gaming Services:** The project is integrated with Unity Gaming Services for features like:
    *   Authentication
    *   Cloud Save
    *   Leaderboards

### Scenes

The project is organized into the following scenes:

*   `Splash`: The initial splash screen.
*   `Menu`: The main menu of the game.
*   `Lobby`: A lobby scene, possibly for multiplayer or pre-game setup.
*   `Gameplay`: The core gameplay scene where the player controls the "Flappy Boi".
*   `Bootstrap`: The initial scene that loads and initializes the game's systems and services.

## Building and Running

To build and run the project, you will need to have Unity Hub and the correct version of the Unity Editor installed.

1.  **Open the project in Unity Hub:**
    *   Open Unity Hub.
    *   Click on "Projects" on the left-hand side.
    *   Click the "Open" button.
    *   Navigate to the root directory of this project (`C:\Dev\Projects\FlappyBoiz`) and click "Open".

2.  **Open the project in the Unity Editor:**
    *   Once the project is added to Unity Hub, click on it to open it in the Unity Editor. Unity will automatically detect the correct editor version (`6000.0.55f1`) and install it if it's not already installed.

3.  **Run the game:**
    *   The `Bootstrap` scene (`Assets/FlappyBoiz/Scenes/Bootstrap.unity`) is the entry point of the game.
    *   Open the `Bootstrap` scene in the editor.
    *   Press the "Play" button at the top of the editor to run the game.

## Development Conventions

Based on the project structure and dependencies, the following development conventions are likely in place:

*   **Dependency Injection:** Use `VContainer` to inject dependencies into your classes. Avoid using singletons or static classes for dependencies.
*   **Asynchronous Code:** Use `UniTask` for all asynchronous operations instead of Coroutines or `async/await` with `Task`.
*   **Messaging:** Use `MessagePipe` to communicate between different systems. This helps to keep the codebase decoupled and easier to maintain.
*   **Input Handling:** Use the Unity Input System for all player input. The input actions are defined in `Assets/FlappyBoiz/InputSystemActions.inputactions`.
*   **Scene Management:** The game uses a multi-scene setup. The `Bootstrap` scene is the entry point, and other scenes are loaded and unloaded as needed.
*   **UI:** The UI is built with Unity's UGUI system.
