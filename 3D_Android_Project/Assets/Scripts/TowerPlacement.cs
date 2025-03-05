using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    public GameObject PlaceTurret(Vector3 position, Quaternion rotation, NodeControl node)
    {
        GameObject turretToBuild = BuildManager.instance.GetTurretToBuild();
        if (turretToBuild == null)
            return null;

        GameObject turret = Instantiate(turretToBuild, position, rotation);
        turret.layer = LayerMask.NameToLayer("Ignore Raycast");

        // Removed laneID assignment since it's no longer needed
        return turret;
    }
}