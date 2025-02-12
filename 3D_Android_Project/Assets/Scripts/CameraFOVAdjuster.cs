using UnityEngine;

public class CameraFOVAdjuster : MonoBehaviour
{
    public float targetAspect = 16f / 9f;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        AdjustFOV();
    }

    void AdjustFOV()
    {
        float screenAspect = (float)Screen.width / Screen.height;
        if (screenAspect < targetAspect)
        {
            cam.fieldOfView *= targetAspect / screenAspect;
        }
    }
}