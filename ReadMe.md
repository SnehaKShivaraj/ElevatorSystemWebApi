Great! Here’s your **final polished README**, concise, backend-focused, and including the architecture + API example. Ready to submit:

---

# Elevator System

## Overview

Backend system simulating **elevator operations** in a multi-floor building. Handles elevator requests, tracks positions, and moves elevators asynchronously. A **minimal Angular UI** demonstrates functionality.

---

## Features

* Multiple elevators with request handling
* Elevator movement simulation
* Real-time position tracking via API
* Unit tests for core logic

---

## Tech Stack

* **Backend:** .NET 8 Web API, C#
* **Frontend:** Angular (minimal UI)
* **Testing:** xUnit

---

## Setup

### Backend

```bash
git clone https://github.com/SnehaKShivaraj/ElevatorSystemWebApi.git
cd ElevatorSystemWebApi
dotnet restore
dotnet run --project ElevatorControlSystem.Api
```

API runs at `https://localhost:5001`

### Frontend (Optional)

```bash
git clone https://github.com/SnehaKShivaraj/ElevatorSystemUI.git
cd ElevatorSystemUI
npm install
ng serve
```

UI runs at `http://localhost:4200`

---

## Usage

* Send requests to the API to call elevators
* UI shows elevator positions and status
* Unit tests verify elevator assignment, movement, and request handling

---

## Architecture Overview

```
+-------------------+       +-------------------+
|   ElevatorSystem  |       |    Minimal UI     |
|       API         | <---> |  (Angular)       |
+-------------------+       +-------------------+
        |
        v
+-------------------+
|   Core Logic      |
| - Elevator Queue  |
| - Movement Engine |
| - Request Handler |
+-------------------+
        |
        v
+-------------------+
| Background Task   |
| Simulates elevator|
| movement asynchronously |
+-------------------+
```

* **API Layer:** Handles HTTP requests and responses.
* **Core Logic:** Assigns elevators, calculates movements, and tracks status.
* **Background Task:** Moves elevators asynchronously over time.
* **UI:** Minimal interface to visualize elevator positions and send requests.

---

## API Example

**Call an Elevator to a Floor:**

```
POST /api/elevators/call
Body:
{
  "floor": 5
}
```

**Get Elevator Status:**

```
GET /api/elevators/status
Response:
[
  { "id": 1, "currentFloor": 3, "status": "MovingUp" },
  { "id": 2, "currentFloor": 7, "status": "Idle" }
]
```

---

## Notes

* UI is minimal; focus is on **backend logic**
* Includes basic error handling and logging
* Background processing simulates asynchronous elevator movement

---

## Author

**Sneha Shivaraj**
[GitHub](https://github.com/SnehaKShivaraj)

