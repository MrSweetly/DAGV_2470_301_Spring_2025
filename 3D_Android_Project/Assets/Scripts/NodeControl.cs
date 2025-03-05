using UnityEngine;
using UnityEngine.EventSystems;

public class NodeControl : MonoBehaviour
{
    public Vector3 positionOffset; // Offset for turret placement

    private Renderer rend;
    private Color originalColor;
    private GameObject turret; 
    private BuildManager buildManager;
    private BlinkEffect blinkEffect;
    private TowerPlacement turretPlacement;

    private static NodeControl[] allNodes; // Cache all nodes for efficiency

    private void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
        
        buildManager = BuildManager.instance;
        turretPlacement = GetComponent<TowerPlacement>();

        if (buildManager == null)
        {
            Debug.LogError("BuildManager instance is missing!");
            enabled = false; // Disable script if BuildManager is missing
            return;
        }

        // Cache BlinkEffect if available
        if (!TryGetComponent(out blinkEffect))
        {
            Debug.LogError($"BlinkEffect component missing on {gameObject.name}");
        }

        rend.enabled = false; // Disable renderer at start

        // Cache all nodes once
        if (allNodes == null || allNodes.Length == 0)
        {
            allNodes = FindObjectsOfType<NodeControl>();
        }
    }

    public Vector3 GetBuildPosition() => transform.position + positionOffset;
    public bool HasTurret() => turret != null;

    public void StartBlinking()
    {
        if (turret == null && buildManager.GetTurretToBuild() != null) 
        {
            blinkEffect?.ToggleBlinking(true); // Uses a single toggle method for blinking
        }
    }

    public void StopBlinking()
    {
        blinkEffect?.ToggleBlinking(false); // Stops blinking if running
    }

    private void OnMouseDown()
    {
        if (buildManager.GetTurretToBuild() == null)
        {
            buildManager.SelectNode(this);
            return;
        }

        if (turret != null || EventSystem.current.IsPointerOverGameObject())
            return; // Prevent placing if occupied or UI clicked

        // Place turret
        turret = turretPlacement.PlaceTurret(GetBuildPosition(), transform.rotation, this);
        if (turret == null) return;

        buildManager.SetTurretToBuild(null);
        StopBlinking();
        DisableAllNodeRenderers();
    }

    private void OnMouseExit()
    {
        if (!(blinkEffect?.IsBlinking() ?? false)) // If blinkEffect is null or not blinking
        {
            rend.enabled = false;
        }
    }

    private void DisableAllNodeRenderers()
    {
        // Refresh node cache if new nodes are added
        allNodes = FindObjectsOfType<NodeControl>();

        foreach (var node in allNodes)
        {
            node.rend.enabled = false;
            node.StopBlinking();
        }
    }
}