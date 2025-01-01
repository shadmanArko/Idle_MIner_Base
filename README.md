# Kolibri Task - for Arko

## Overview
The main objective was to implement a mine-switching feature in a simplified version of *Idle Miner Tycoon*.

## Features Implemented
- **UI Panel and Animations**:
  - A panel for selecting mines, designed according to the provided Figma mockup.
  - Buttons with press-and-hold animations for better interactivity.
  - Smooth transitions for the panel’s appearance and disappearance.

- **State Persistence**:
  - Money, unlocked mine shafts, upgrade levels, and stash amounts are saved and restored for each mine.
    
- **Mine Switching**:
  - Players can start a second mine.
  - Players can switch back and forth between mines.
  - The current state of each mine is preserved when switching.




## Thought Process
### Requirement Analysis
- Carefully analyzed the task requirements and mockup to understand the core features.
- Focused on ensuring data persistence and smooth user experience.

### Architecture
- Aligned my code extensions with the existing **Model-View-Controller (MVC)** architecture and codebase conventions.
- Architected a data persistence system using **Repository** with **Unit of Work** patterns with **JSON** serialization.

### Scalability
- Designed the system to easily support more mines in the future.
- Used modular code to make extending the functionality straightforward.
- Engineered a scalable data context system supporting diverse storage requirements and future expansions.

## Implementation Details
### Data Models
The game state is managed using the following models:
- **`SaveData`**: Represents the overall game state.
- **`MineData`**: Represents the state of a single mine.
- **`MineshaftData`**: Represents the state of a single shaft in a mine.

### Save and Load System
- Used **JSON serialization** to save and load data.
- Data is saved when switching mines and loaded when the game starts.
- **`Repository`**: Handles specific CRUD operations (Create, Read, Update, Delete) for a single entity type, abstracting the data access details from the business layer.
- **`UnitOfWork`**: Coordinates and manages all CRUD operations across multiple repositories in a single transaction, ensuring data consistency and atomic commits.
- **`DataContext`**: Provides the underlying mechanism for executing CRUD operations by managing connections, tracking entity states, and persisting changes to the data store.

### UI and Animations
- Implemented the mine selection panel based on the Figma mockup.
- I used DOTween for button, panel, scene transition, and screen tint animations to enhance flexibility, scalability, and ease of modification.

### Key Classes and Scripts
1. **`SaveManager`**: Manages mine data saving.
2. **`Initializer`**: Handles data loading from save file and initilize the datas in core game.
3. **`PlayerPrefsManager`**: Tried to maintain a simple system for using Unity's PlayerPrefs

### Tools and Assets
- **Unity Version**: Unity 2021.3.13f1.
- **Assets**: Provided assets from `AssetPack.zip`.

## Challenges and Solutions
- **Challenge**: Ensuring data persistence when switching mines.
  - **Solution**: Implemented a robust save/load system using JSON serialization.

- **Challenge**: Smooth animations for the UI panel.
  - **Solution**: Used DOTween and tweaked transition settings for a polished feel.

- **Challenge**: Managing state efficiently.
  - **Solution**: Adopted an MVC pattern to ensure clean separation of logic and UI.

## How to Run
1. Open the project in Unity 2021.3.13f1.
2. Open Assets -> _Scenes -> Main
3. Play the game in the editor.
4. Use the mine selection panel to switch between mines and observe the state persistence.

## Possible Improvements
- Add more mines dynamically without hardcoding.
- Implement a proper backend for cloud saving.
- Optimize performance for larger datasets.

## Submission Notes
- **Duration**: Task completed within one week.
- **Contact**: For any questions or clarifications, feel free to reach out via email.
