using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one build manager in scene.");
            return;
        }
        instance = this;
    }
    
    public GameObject rifleTurretPrefab;
    public GameObject sniperTurretPrefab;
    public GameObject barricadeTowerPrefab;
    
    private GameObject turretToBuild;

    public GameObject GetTurretToBuild()
    {
        return turretToBuild;
    }

    public void SetTurretToBuild(GameObject turret)
    {
        turretToBuild = turret;
    }
}
