using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class NodeControl : MonoBehaviour
{
    public Color hoverColor;
    public float blinkSpeed = 0.5f; // Speed of the blinking effect
    public Vector3 postionOffet;

    private Renderer rend;
    private Color originalColor;
    private bool isBlinking = false;
    private Coroutine blinkCoroutine;
    private GameObject turret;

    BuildManager buildManager;

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;

        buildManager = BuildManager.instance;
        if (BuildManager.instance == null)
        {
            Debug.LogError("BuildManager instance is missing!");
        }

        // Ensure MeshRenderer is disabled at the start (for your setup)
        rend.enabled = false;
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position + postionOffet;
    }

    // Method to start blinking when triggered by the Shop
    public void StartBlinking()
    {
        if (buildManager.GetTurretToBuild() == null || turret != null) // Prevent blinking if no turret is selected or turret is already placed
        {
            return;
        }

        if (rend != null && !isBlinking) // Ensure it's not already blinking
        {
            rend.enabled = true; // Enable the MeshRenderer
            isBlinking = true;
            blinkCoroutine = StartCoroutine(BlinkEffect());
        }
    }


    private void OnMouseDown()
    {
        if (buildManager.GetTurretToBuild() == null)
        {
            buildManager.SelectNode(this);
            return;
        }

        if (turret != null)
        {
            return;
        }
        
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Click ignored: UI element was clicked.");
            return; // Prevent placing a turret when clicking UI
        }

        // LayerMask to check if we are clicking on the correct layer for nodes only
        int layerMask = 1 << LayerMask.NameToLayer("Node"); // Ensure this layer is set to "Node" for your node objects
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            // Proceed with placing the turret
            GameObject turretToBuild = buildManager.GetTurretToBuild();
            turret = Instantiate(turretToBuild, transform.position + postionOffet, transform.rotation);
            
            // **Set turret to Ignore Raycast layer**
            turret.layer = LayerMask.NameToLayer("Ignore Raycast");

            // Optionally, assign lane if required
            int closestLane = GetClosestLane(transform.position);
            TowerBehavior turretScript = turret.GetComponent<TowerBehavior>();
            if (turretScript != null)
            {
                turretScript.laneID = closestLane;
            }

            // **Reset turret selection after building**
            buildManager.SetTurretToBuild(null);

            // Stop blinking after placing the turret
            StopBlinking();

            // Disable all node renderers after placement
            DisableAllNodeRenderers();
        }
    }

    // Finds the closest lane to the node
    private int GetClosestLane(Vector3 position)
    {
        int closestLane = 0;
        float minDistance = float.MaxValue;

        for (int i = 0; i < EnemySpawner.instance.spawnPositions.Count; i++)
        {
            float distance = Vector3.Distance(position, EnemySpawner.instance.spawnPositions[i].value);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestLane = i;
            }
        }

        return closestLane;
    }

    void OnMouseExit()
    {
        if (rend != null && !isBlinking) // Only turn off when not blinking
        {
            // Disable MeshRenderer when mouse exits
            rend.enabled = false;
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
            }
            isBlinking = false;
            rend.material.color = originalColor; // Reset color
        }
    }

    IEnumerator BlinkEffect()
    {
        while (isBlinking)
        {
            rend.material.color = hoverColor;
            yield return new WaitForSeconds(blinkSpeed);
            rend.material.color = originalColor;
            yield return new WaitForSeconds(blinkSpeed);
        }
    }

    // Method to stop blinking when a turret is placed
    public void StopBlinking()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        isBlinking = false;
        rend.material.color = originalColor; // Reset color
        rend.enabled = false; // Disable renderer after stopping blinking
    }

    // Disable MeshRenderers on all nodes after turret is placed
    private void DisableAllNodeRenderers()
    {
        GameObject nodesParent = GameObject.Find("Nodes"); // Find the "Nodes" parent object in the scene
        if (nodesParent == null)
        {
            Debug.LogError("No Nodes parent found in the scene!");
            return;
        }

        // Find all NodeControl components in the children of the "Nodes" parent object
        NodeControl[] nodes = nodesParent.GetComponentsInChildren<NodeControl>();

        foreach (NodeControl node in nodes)
        {
            node.rend.enabled = false; // Disable renderer on all nodes
            node.StopBlinking(); // Ensure blinking is stopped on all nodes
        }
    }
}