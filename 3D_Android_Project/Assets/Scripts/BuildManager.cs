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
    
    public NodeUI nodeUI;

    private GameObject turretToBuild;
    private NodeControl selectedNode;

    // Reference to the Cancel button UI
    public GameObject cancelButton;
    
    public GameObject GetTurretToBuild()
    {
        return turretToBuild;
    }

    public void SelectNode(NodeControl node)
    {
        if (selectedNode == node)
        {
            DeselectNode();
            return;
        }
        
        selectedNode = node;
        turretToBuild = null;

        nodeUI.SetTarget(node);
    }

    public void DeselectNode()
    {
        selectedNode = null;
        nodeUI.Hide();
    }

    public void SetTurretToBuild(GameObject turret)
    {
        turretToBuild = turret;
        DeselectNode();

        // Show Cancel button only if a turret is selected
        if (cancelButton != null)
        {
            cancelButton.SetActive(turret != null);
        }
    }

    public void CancelTurretSelection()
    {
        turretToBuild = null;

        // Hide Cancel button when canceling
        if (cancelButton != null)
        {
            cancelButton.SetActive(false);
        }

        // Disable blinking effect on all nodes
        GameObject nodesParent = GameObject.Find("Nodes");
        if (nodesParent != null)
        {
            NodeControl[] nodes = nodesParent.GetComponentsInChildren<NodeControl>();
            foreach (NodeControl node in nodes)
            {
                node.StopBlinking();
            }
        }

        Debug.Log("Turret selection canceled.");
    }
    
}