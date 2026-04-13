# Traffic Racer

A Unity-based endless racing game where you control a car, avoid traffic, and score points based on distance and speed. The project is built with C# scripts targeting .NET Framework 4.7.1.

## Features

- Realistic car physics and controls
- Endless city environment
- Dynamic traffic vehicles
- Score, distance, and speed tracking UI
- Game over and retry system
- Audio feedback for engine, braking, and collisions

## Project Structure

Key scripts in the project:

- **CarController.cs**: Handles car movement, physics, input, and collision logic.
- **UIManager.cs**: Manages UI updates for speed, distance, score, and game over state.
- **CarSound.cs**: Controls car-related audio (engine, brake, collision).
- **TrafficManager.cs**: Spawns and manages traffic vehicles.
- **Vehicle.cs**: Base class for traffic vehicles.
- **LaneMovement.cs**: Handles lane-based movement for vehicles.
- **EndlessCity.cs**: Manages endless city environment spawning.
- **CameraController.cs / CamMovement.cs**: Controls camera following and effects.
- **DestroyOnContact.cs**: Handles object destruction on collision.
- **AudioManager.cs**: (If present) Manages background music and sound effects.
- **CarSpawn.cs**: (If present) Handles spawning of player or AI cars.

> There are approximately 17 scripts in total, including audio, spawning, and utility scripts.

## Getting Started

### Prerequisites

- Unity 2021.3 LTS or newer (recommended)
- TextMeshPro package (for UI)
- .NET Framework 4.7.1 compatibility

### Setup

1. Clone this repository:
2. Open the project in Unity.
3. Ensure all dependencies (like TextMeshPro) are installed via the Unity Package Manager.
4. Open the main scene and press Play.

## Controls

- **Arrow Keys / WASD**: Steer and accelerate/brake the car
- **Spacebar**: Apply brakes

## Contributing

Pull requests are welcome! For major changes, please open an issue first to discuss what you would like to change.



---

*This project is for educational and entertainment purposes.*
