using UnityEngine;

public class Shop : MonoBehaviour
{
    private BuildManager buildManager;
    private NodeControl[] nodes; // Cached list of nodes to avoid redundant GameObject.Find calls

    void Start()
    {
        buildManager = BuildManager.instance;
        if (buildManager == null)
        {
            Debug.LogError("BuildManager instance is missing!");
            return;
        }

        // Cache all nodes at start instead of calling GameObject.Find each time
        GameObject nodesParent = GameObject.Find("Nodes");
        if (nodesParent != null)
        {
            nodes = nodesParent.GetComponentsInChildren<NodeControl>();
        }
        else
        {
            Debug.LogError("No Nodes parent found in the scene!");
        }
    }

    public void BuyTurret(GameObject turretPrefab)
    {
        if (turretPrefab == null)
        {
            Debug.LogError("Turret prefab is null!");
            return;
        }

        Debug.Log($"Buying {turretPrefab.name}");
        buildManager.SetTurretToBuild(turretPrefab);
        EnableNodeBlinking();
    }

    private void EnableNodeBlinking()
    {
        if (nodes == null || nodes.Length == 0) return; // No nodes found

        foreach (NodeControl node in nodes)
        {
            node.StopBlinking(); // Stop previous blinking
            node.StartBlinking(); // Start blinking effect again
        }
    }
}