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
        EnableNodeBlinking(); // Enable blinking on all nodes
    }

    public void BuySniperTurret()
    {
        Debug.Log("BuySniperTurret");
        buildManager.SetTurretToBuild(buildManager.sniperTurretPrefab);
        EnableNodeBlinking(); // Enable blinking on all nodes
    }

    public void BuyBarricadeTower()
    {
        Debug.Log("BuyBarricadeTower");
        buildManager.SetTurretToBuild(buildManager.barricadeTowerPrefab);
        EnableNodeBlinking(); // Enable blinking on all nodes
    }

    private void EnableNodeBlinking()
    {
        GameObject nodesParent = GameObject.Find("Nodes"); // Find the "Nodes" parent object in the scene
        if (nodesParent == null)
        {
            Debug.LogError("No Nodes parent found in the scene!");
            return;
        }

        // Find all NodeControl components in the children of the "Nodes" parent object
        NodeControl[] nodes = nodesParent.GetComponentsInChildren<NodeControl>();

        // Stop any ongoing blinking for all nodes before re-triggering blinking
        foreach (NodeControl node in nodes)
        {
            node.StopBlinking(); // Stop previous blinking
            node.StartBlinking(); // Start blinking effect again
        }
    }

}