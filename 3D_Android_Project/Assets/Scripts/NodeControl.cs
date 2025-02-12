using UnityEngine;
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
    }

    void OnMouseEnter()
    {
        if (buildManager.GetTurretToBuild() == null || turret != null) // Prevents hover if no turret is selected
        {
            return;
        }

        if (rend != null)
        {
            rend.enabled = true; // Ensure the MeshRenderer is active
            if (!isBlinking)
            {
                blinkCoroutine = StartCoroutine(BlinkEffect());
                isBlinking = true;
            }
        }
    }

    private void OnMouseDown()
    {
        if (buildManager.GetTurretToBuild() == null)
        {
            return;
        }

        if (turret != null)
        {
            Debug.Log("Can't Place Here!");
            return;
        }

        GameObject turretToBuild = buildManager.GetTurretToBuild();
        turret = Instantiate(turretToBuild, transform.position + postionOffet, transform.rotation);

        // Determine the closest lane
        int closestLane = GetClosestLane(transform.position);

        // Assign lane to the turret
        TowerBehavior turretScript = turret.GetComponent<TowerBehavior>();
        if (turretScript != null)
        {
            turretScript.laneID = closestLane;
        }

        // **Reset turret selection after building**
        buildManager.SetTurretToBuild(null);
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
        if (rend != null)
        {
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
        while (true)
        {
            rend.material.color = hoverColor;
            yield return new WaitForSeconds(blinkSpeed);
            rend.material.color = originalColor;
            yield return new WaitForSeconds(blinkSpeed);
        }
    }
}
