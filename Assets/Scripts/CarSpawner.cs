using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    private const string CarIndexKey = "CarIndexValue";
    private const string CarNameKey = "SelectedCarName";
    private const string CarIdKey = "SelectedCarId";
    [SerializeField] GameObject[] carsPrefabs;
    [SerializeField] string[] carIds;
    [SerializeField] Transform explicitSpawnPoint;
    [SerializeField] AudioClip defaultEngineSound;
    [SerializeField] AudioClip defaultBrakeSound;
    [SerializeField] AudioClip defaultHitSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnCar();
    }

    // Update is called once per frame
    void SpawnCar()
    {
        if (carsPrefabs == null || carsPrefabs.Length == 0)
        {
            Debug.LogError("CarSpawner has no prefab list. Assign player cars from Assets/Prefabs/PlayerCar.");
            return;
        }

        string selectedCarId = CarSelectionState.HasSelection
            ? CarSelectionState.SelectedId
            : PlayerPrefs.GetString(CarIdKey, string.Empty);
        string selectedCarName = CarSelectionState.HasSelection
            ? CarSelectionState.SelectedName
            : PlayerPrefs.GetString(CarNameKey, string.Empty);
        int savedIndex = CarSelectionState.HasSelection
            ? CarSelectionState.SelectedIndex
            : PlayerPrefs.GetInt(CarIndexKey, 0);

        int resolvedIndex = FindCarIndexById(selectedCarId);

        if (resolvedIndex < 0)
        {
            resolvedIndex = FindCarIndexByName(selectedCarName);
        }

        if (resolvedIndex < 0)
        {
            resolvedIndex = Mathf.Clamp(savedIndex, 0, carsPrefabs.Length - 1);
        }

        int selectedIndex = resolvedIndex >= 0 ? resolvedIndex : 0;

        GameObject selectedPrefab = carsPrefabs[selectedIndex];
        if (selectedPrefab == null)
        {
            Debug.LogError($"CarSpawner selected prefab is null at index {selectedIndex}.");
            return;
        }

        
        string prefabName = string.IsNullOrEmpty(selectedPrefab.name) ? $"Unnamed_Index{selectedIndex}" : selectedPrefab.name;
        if (string.IsNullOrEmpty(selectedPrefab.name))
        {
            Debug.LogWarning($"CarSpawner: Prefab at index {selectedIndex} has empty name. Using fallback: {prefabName}");
        }

        CarController[] existingPlayers = FindObjectsByType<CarController>(FindObjectsSortMode.None);
        Vector3 spawnPosition = GetSpawnPosition(existingPlayers);
        Quaternion spawnRotation = GetSpawnRotation(existingPlayers);

        GameObject spawned = InstantiateCarObject(selectedPrefab, spawnPosition, spawnRotation);
        if (spawned == null)
        {
            Debug.LogError($"CarSpawner failed to instantiate selected object '{prefabName}' at index {selectedIndex}. Check if the prefab is valid.");
            return;
        }
        
        // Rename spawned car if it has empty name
        if (string.IsNullOrEmpty(spawned.name))
        {
            spawned.name = $"PlayerCar_{(selectedIndex == 0 ? "Maruti" : selectedIndex == 1 ? "Volkswagen" : "PickupTruck")}";
            Debug.Log($"CarSpawner: Spawned car had empty name, renamed to '{spawned.name}'");
        }

        spawned.SetActive(true);
        spawned.tag = "Player";

        CarController spawnedPlayer = ResolveOrBuildPlayerController(spawned);

        if (spawnedPlayer == null)
        {
            Debug.LogError("CarSpawner could not find CarController on spawned player.");
            return;
        }

        RemoveExistingPlayers(existingPlayers, spawnedPlayer);
        EnsureCarSound(spawnedPlayer);
        RebindSystems(spawnedPlayer);

        Debug.Log($"CarSpawner spawned '{spawned.name}' using id='{selectedCarId}', name='{selectedCarName}', index={selectedIndex} at {spawnPosition}. runtimeSelection={CarSelectionState.HasSelection}");
    }

    int FindCarIndexByName(string selectedCarName)
    {
        if (string.IsNullOrWhiteSpace(selectedCarName))
        {
            return -1;
        }

        string selectedNormalized = NormalizeName(selectedCarName);
        if (selectedNormalized == "volkswagen")
        {
            selectedNormalized = "maruti8001";
        }

        for (int i = 0; i < carsPrefabs.Length; i++)
        {
            if (carsPrefabs[i] == null)
            {
                continue;
            }

            string prefabNormalized = NormalizeName(carsPrefabs[i].name);
            if (prefabNormalized == selectedNormalized)
            {
                return i;
            }
        }

        return -1;
    }

    int FindCarIndexById(string selectedCarId)
    {
        if (string.IsNullOrWhiteSpace(selectedCarId) || carIds == null || carIds.Length == 0)
        {
            return -1;
        }

        string selectedIdNormalized = NormalizeId(selectedCarId);
        int max = Mathf.Min(carIds.Length, carsPrefabs.Length);
        for (int i = 0; i < max; i++)
        {
            if (NormalizeId(carIds[i]) == selectedIdNormalized)
            {
                return i;
            }
        }

        return -1;
    }

    string NormalizeName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        System.Text.StringBuilder builder = new System.Text.StringBuilder(value.Length);
        foreach (char c in value)
        {
            if (char.IsLetterOrDigit(c))
            {
                builder.Append(char.ToLowerInvariant(c));
            }
        }

        return builder.ToString();
    }

    string NormalizeId(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        return value.Trim().ToLowerInvariant().Replace("_", "-").Replace(" ", "-");
    }

    Vector3 GetSpawnPosition(CarController[] existingPlayers)
    {
        if (explicitSpawnPoint != null)
        {
            return explicitSpawnPoint.position;
        }

        if (existingPlayers != null)
        {
            for (int i = 0; i < existingPlayers.Length; i++)
            {
                if (existingPlayers[i] != null)
                {
                    return existingPlayers[i].transform.position;
                }
            }
        }

        return transform.position;
    }

    Quaternion GetSpawnRotation(CarController[] existingPlayers)
    {
        if (explicitSpawnPoint != null)
        {
            return explicitSpawnPoint.rotation;
        }

        if (existingPlayers != null)
        {
            for (int i = 0; i < existingPlayers.Length; i++)
            {
                if (existingPlayers[i] != null)
                {
                    return existingPlayers[i].transform.rotation;
                }
            }
        }

        return transform.rotation;
    }

    void RemoveExistingPlayers(CarController[] existingPlayers, CarController keepPlayer)
    {
        if (existingPlayers == null || existingPlayers.Length == 0)
        {
            return;
        }

        for (int i = 0; i < existingPlayers.Length; i++)
        {
            CarController player = existingPlayers[i];
            if (player == null || player == keepPlayer)
            {
                continue;
            }

            player.gameObject.SetActive(false);
            Destroy(player.gameObject);
        }
    }

    void RebindSystems(CarController player)
    {
        UIManager uiManager = FindFirstObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.ConfigurePlayer(player, player.transform);
        }

        TrafficManager trafficManager = FindFirstObjectByType<TrafficManager>();
        if (trafficManager != null)
        {
            trafficManager.ConfigurePlayer(player);
        }

        CamMovement camMovement = FindFirstObjectByType<CamMovement>();
        if (camMovement != null)
        {
            camMovement.SetTarget(player.transform);
        }

        CameraController cameraController = FindFirstObjectByType<CameraController>();
        if (cameraController != null)
        {
            cameraController.SetTarget(player.transform);
        }

        LaneMovement[] laneMovements = FindObjectsByType<LaneMovement>(FindObjectsSortMode.None);
        for (int i = 0; i < laneMovements.Length; i++)
        {
            if (laneMovements[i] != null)
            {
                laneMovements[i].SetTarget(player.transform);
            }
        }

        EndlessCity[] endlessCities = FindObjectsByType<EndlessCity>(FindObjectsSortMode.None);
        for (int i = 0; i < endlessCities.Length; i++)
        {
            if (endlessCities[i] != null)
            {
                endlessCities[i].SetTarget(player.transform);
            }
        }
    }

    void EnsureCarSound(CarController player)
    {
        if (player == null)
        {
            Debug.LogError("[CarSpawner] EnsureCarSound called with null CarController");
            return;
        }

        CarSound carSound = player.GetComponent<CarSound>();
        if (carSound == null)
        {
            carSound = player.GetComponentInParent<CarSound>();
        }
        if (carSound == null)
        {
            carSound = player.GetComponentInChildren<CarSound>();
        }
        if (carSound == null)
        {
            carSound = player.gameObject.AddComponent<CarSound>();
            Debug.Log($"[CarSpawner] Added CarSound component to {player.gameObject.name}");
        }

        carSound.enabled = true;
        
        if (defaultEngineSound != null && defaultBrakeSound != null && defaultHitSound != null)
        {
            carSound.ConfigureClips(defaultEngineSound, defaultBrakeSound, defaultHitSound);
            Debug.Log($"[CarSpawner] Configured clips for {player.gameObject.name}");
        }
        else
        {
            Debug.LogWarning($"[CarSpawner] Missing audio clips - engine:{(defaultEngineSound != null)}, brake:{(defaultBrakeSound != null)}, hit:{(defaultHitSound != null)}");
        }
    }

    GameObject InstantiateCarObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null)
        {
            Debug.LogError("[CarSpawner] InstantiateCarObject called with null prefab.");
            return null;
        }

        try
        {
            // Try standard instantiation
            Object spawnedObject = Instantiate((Object)prefab, position, rotation);
            
            if (spawnedObject == null)
            {
                Debug.LogError($"[CarSpawner] Instantiate returned null for prefab '{prefab.name}'. This may indicate a corrupted prefab.");
                return null;
            }

            GameObject spawnedGameObject = spawnedObject as GameObject;
            if (spawnedGameObject != null)
            {
                Debug.Log($"[CarSpawner] Successfully instantiated GameObject: {spawnedGameObject.name}");
                return spawnedGameObject;
            }

            Component spawnedComponent = spawnedObject as Component;
            if (spawnedComponent != null)
            {
                Debug.Log($"[CarSpawner] Instantiated Component, returning its GameObject.");
                return spawnedComponent.gameObject;
            }

            Debug.LogError($"[CarSpawner] Instantiate returned object of unexpected type: {spawnedObject.GetType().Name}");
            return null;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[CarSpawner] Exception during Instantiate for '{prefab.name}': {ex.Message}\n{ex.StackTrace}");
            return null;
        }
    }

    CarController ResolveOrBuildPlayerController(GameObject spawned)
    {
        CarController controller = spawned.GetComponent<CarController>();
        if (controller == null)
        {
            controller = spawned.GetComponentInChildren<CarController>();
        }

        if (controller != null)
        {
            return controller;
        }

        return BuildVolkswagenPlayerController(spawned);
    }

    CarController BuildVolkswagenPlayerController(GameObject spawned)
    {
        Transform root = spawned.transform;

        // Remove traffic-driving logic when Volkswagen traffic prefab is used as a player source.
        Vehicle vehicle = spawned.GetComponent<Vehicle>();
        if (vehicle != null)
        {
            Destroy(vehicle);
        }

        Rigidbody rb = spawned.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = spawned.AddComponent<Rigidbody>();
        }
        rb.mass = 700f;
        rb.linearDamping = 0f;
        rb.angularDamping = 0.05f;
        rb.useGravity = true;

        Transform frontRightWheelTransform = FindChildRecursive(root, "Frontright Wheel");
        Transform frontLeftWheelTransform = FindChildRecursive(root, "Frontleft Wheel");
        Transform backRightWheelTransform = FindChildRecursive(root, "Backright Wheel");
        Transform backLeftWheelTransform = FindChildRecursive(root, "Backleft Wheel");

        if (frontRightWheelTransform == null || frontLeftWheelTransform == null ||
            backRightWheelTransform == null || backLeftWheelTransform == null)
        {
            Debug.LogError("CarSpawner could not find Volkswagen wheel transforms.");
            return null;
        }

        WheelCollider frontRightWheelCollider = CreateOrGetWheelCollider(root, "FrontRightWheelCollider", frontRightWheelTransform.localPosition);
        WheelCollider frontLeftWheelCollider = CreateOrGetWheelCollider(root, "FrontLeftWheelCollider", frontLeftWheelTransform.localPosition);
        WheelCollider backRightWheelCollider = CreateOrGetWheelCollider(root, "BackRightWheelCollider", backRightWheelTransform.localPosition);
        WheelCollider backLeftWheelCollider = CreateOrGetWheelCollider(root, "BackLeftWheelCollider", backLeftWheelTransform.localPosition);

        Transform centerOfMass = FindChildRecursive(root, "CenterOfMass");
        if (centerOfMass == null)
        {
            GameObject com = new GameObject("CenterOfMass");
            centerOfMass = com.transform;
            centerOfMass.SetParent(root, false);
            centerOfMass.localPosition = Vector3.zero;
        }

        Transform cameraPoint = FindChildRecursive(root, "CameraPoint");
        if (cameraPoint == null)
        {
            GameObject cp = new GameObject("CameraPoint");
            cameraPoint = cp.transform;
            cameraPoint.SetParent(root, false);
            cameraPoint.localPosition = new Vector3(0f, 1.4f, -3.5f);
        }

        CarController controller = spawned.AddComponent<CarController>();
        controller.ConfigureRuntimeSetup(
            frontRightWheelCollider,
            frontLeftWheelCollider,
            backRightWheelCollider,
            backLeftWheelCollider,
            frontRightWheelTransform,
            frontLeftWheelTransform,
            backRightWheelTransform,
            backLeftWheelTransform,
            centerOfMass);

        return controller;
    }

    WheelCollider CreateOrGetWheelCollider(Transform root, string colliderName, Vector3 localPosition)
    {
        Transform existing = FindChildRecursive(root, colliderName);
        if (existing == null)
        {
            GameObject wheelColliderObject = new GameObject(colliderName);
            existing = wheelColliderObject.transform;
            existing.SetParent(root, false);
            existing.localPosition = localPosition;
        }

        WheelCollider wheelCollider = existing.GetComponent<WheelCollider>();
        if (wheelCollider == null)
        {
            wheelCollider = existing.gameObject.AddComponent<WheelCollider>();
        }

        wheelCollider.radius = 0.19f;
        wheelCollider.suspensionDistance = 0.3f;
        wheelCollider.mass = 20f;

        return wheelCollider;
    }

    Transform FindChildRecursive(Transform parent, string childName)
    {
        if (parent == null)
        {
            return null;
        }

        if (parent.name == childName)
        {
            return parent;
        }

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform found = FindChildRecursive(parent.GetChild(i), childName);
            if (found != null)
            {
                return found;
            }
        }

        return null;
    }
}
