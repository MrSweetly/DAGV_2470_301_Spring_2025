using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    public GameObject rifleTurretPrefab;
    public GameObject sniperTurretPrefab;
    public GameObject barricadeTowerPrefab;

    public NodeUI nodeUI;
    private GameObject turretToBuild;
    private NodeControl selectedNode;

    public GameObject cancelButton;

    public GameObject rifleButton;
    public GameObject sniperButton;
    public GameObject barricadeButton;

    private RectTransform selectedButtonRectTransform;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one build manager in scene.");
            return;
        }
        instance = this;
    }

    public GameObject GetTurretToBuild() => turretToBuild;

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

        // Trigger blinking effect on all nodes
        foreach (var n in FindObjectsOfType<NodeControl>())
        {
            n.StartBlinking();
        }
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
            SetCancelButtonPosition(turret);
        }
    }

    public void CancelTurretSelection()
    {
        turretToBuild = null;

        if (cancelButton != null)
        {
            cancelButton.SetActive(false);
        }

        // Disable blinking effect on all nodes
        foreach (var node in FindObjectsOfType<NodeControl>())
        {
            node.StopBlinking();
        }

        Debug.Log("Turret selection canceled.");
    }

    private void SetCancelButtonPosition(GameObject turret)
    {
        if (turret == null || cancelButton == null) return;

        // Use the selected turret button for positioning
        GameObject turretButton = null;
        if (turret == rifleTurretPrefab) turretButton = rifleButton;
        else if (turret == sniperTurretPrefab) turretButton = sniperButton;
        else if (turret == barricadeTowerPrefab) turretButton = barricadeButton;

        if (turretButton != null)
        {
            selectedButtonRectTransform = turretButton.GetComponent<RectTransform>();
            cancelButton.GetComponent<RectTransform>().position = selectedButtonRectTransform.position;
        }
    }
}
