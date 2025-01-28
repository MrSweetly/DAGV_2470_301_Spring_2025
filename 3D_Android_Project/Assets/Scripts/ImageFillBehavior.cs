using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ImageFillBehavior : MonoBehaviour
{
    public Image image;
    
    [Range(0f, 1f)]
    public float fillAmount = 1f;
    
    public List<TagFillChange> tagFillChanges;
    
    private void Update()
    {
        image.fillAmount = Mathf.Clamp(fillAmount, 0f, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check each tag in tagFillChange list, then send tag data to ChangeFillAmount if called
        foreach (var tagFillChange in tagFillChanges)
        {
            if (other.CompareTag(tagFillChange.tag))
            {
                ChangeFillAmount(tagFillChange.fillChange);
                break;
            }
        }
    }

    // Update fillAmount with changeValue
    private void ChangeFillAmount(float changeValue)
    {
        fillAmount += changeValue;
    }

    // Inspector view
    [System.Serializable]
    public class TagFillChange
    {
        public string tag;
        public float fillChange;
    }
}