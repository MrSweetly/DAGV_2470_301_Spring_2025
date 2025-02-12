using UnityEngine;

public class Shop : MonoBehaviour
{
    BuildManager buildManager;

    void Start()
    {
        if (BuildManager.instance == null)
        {
            Debug.LogError("BuildManager instance is missing!");
        }
        buildManager = BuildManager.instance;
    }
    
    public void BuyRifleTurret()
    {
        Debug.Log("BuyRifleTurret");
        buildManager.SetTurretToBuild(buildManager.rifleTurretPrefab);
    }

    public void BuySniperTurret()
    {
        Debug.Log("BuySniperTurret");
        buildManager.SetTurretToBuild(buildManager.sniperTurretPrefab);
    }

    public void BuyBarricadeTower()
    {
        Debug.Log("BuyBarricadeTower");
        buildManager.SetTurretToBuild(buildManager.barricadeTowerPrefab);
    }
}
