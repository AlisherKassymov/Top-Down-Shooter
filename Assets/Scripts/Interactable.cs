using System;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _highlightMaterial;
    private Material _defaultMaterial;

    private void Start()
    {
        if (_meshRenderer == null)
        {
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
        }
        _defaultMaterial = _meshRenderer.material;
    }

    public void HighlightActive(bool active)
    {
        if (active)
        {
            _meshRenderer.material = _highlightMaterial;
        }
        else
        {
            _meshRenderer.material = _defaultMaterial;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();
        if (playerInteraction == null)
        {
            return;
        }
        
        playerInteraction.InteractableObjects.Add(this);
        playerInteraction.UpdateClosestInteractable();
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();
        if (playerInteraction == null)
        {
            return;
        }
        playerInteraction.InteractableObjects.Remove(this);
        playerInteraction.UpdateClosestInteractable();
    }
}
