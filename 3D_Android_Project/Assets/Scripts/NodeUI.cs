using UnityEngine;

public class NodeUI : MonoBehaviour
{
    private NodeControl target;

    public GameObject ui;

    public void SetTarget(NodeControl target)
    {
        this.target = target;

        transform.position = target.GetBuildPosition();
        
        ui.SetActive(true);
    }

    public void Hide()
    {
        ui.SetActive(false);
    }
}
