using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Interactable _closestInteractable;
    public List<Interactable> InteractableObjects;
    
    public void UpdateClosestInteractable()
    {
        _closestInteractable?.HighlightActive(false);
        _closestInteractable = null;
        float closestDistance = float.MaxValue;
        foreach (var interactable in InteractableObjects)
        {
            float distance = Vector3.Distance(transform.position, interactable.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                _closestInteractable = interactable;
            }
        }
        _closestInteractable?.HighlightActive(true);
    }
}
