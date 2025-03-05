using UnityEngine;
using System.Collections;

public class BlinkEffect : MonoBehaviour
{
    public Color hoverColor;
    public float blinkSpeed = 0.5f;

    private Renderer rend;
    private Color originalColor;
    private Coroutine blinkCoroutine;

    private void Awake()
    {
        if (!TryGetComponent(out rend))
        {
            Debug.LogError($"Renderer component missing on {gameObject.name}");
            enabled = false; // Disable script if no renderer is found
            return;
        }

        originalColor = rend.material.color;
    }

    public void ToggleBlinking(bool enable)
    {
        if (enable)
        {
            if (blinkCoroutine == null) // Prevent redundant coroutine starts
            {
                rend.enabled = true;
                blinkCoroutine = StartCoroutine(BlinkCoroutine());
            }
        }
        else
        {
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
                blinkCoroutine = null;
            }
            rend.material.color = originalColor;
            rend.enabled = false;
        }
    }

    public bool IsBlinking() => blinkCoroutine != null;

    private IEnumerator BlinkCoroutine()
    {
        while (true) // Infinite loop until stopped externally
        {
            rend.material.color = hoverColor;
            yield return new WaitForSeconds(blinkSpeed);
            rend.material.color = originalColor;
            yield return new WaitForSeconds(blinkSpeed);
        }
    }
}