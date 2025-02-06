using Ink.Parsed;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] CircleCollider2D interactionSphere;
    [SerializeField] ContactFilter2D interactionFilter;
    List<Collider2D> interactables;
    [SerializeField] GameObject focusedInteractable;

    public event EventHandler interactionEvent;

    private void Awake()
    {
        interactables = new List<Collider2D>();
    }

    private void Update()
    {
        FindClosestInteractable();
    }

    void FindClosestInteractable()
    {
        if (interactionSphere.Overlap(interactionFilter, interactables) == 0)
        {
            //Debug.Log("No Interactables: " +  interactables.Count);
            focusedInteractable = null;
            return;
        }
        else
        {
            //Debug.Log("Interactables Found: " + interactables.Count);
            float currentClosestDistance = float.MaxValue;
            GameObject currentClosestInteractable = null;
            foreach(Collider2D interactable in interactables)
            {
                Transform interactTransform = interactable.transform;
                float distanceFromPlayer = Vector2.Distance(interactTransform.position, transform.position);
                if (distanceFromPlayer < currentClosestDistance)
                {
                    currentClosestDistance = distanceFromPlayer;
                    currentClosestInteractable = interactable.gameObject;
                }
            }
            focusedInteractable = currentClosestInteractable;
            //Debug.Log(focusedInteractable.name + ", Dist: " + currentClosestDistance);
        }
    }

    void OnInteract(InputValue value)
    {
        if(focusedInteractable != null && PlayerState.currentState == PlayerState.PlayerStates.Playing)
        {
            string dataPath = "ScriptableObjects/" + focusedInteractable.name + "_InkScript";
            InkScript inkScript = Resources.Load<InkScript>(dataPath);
            interactionEvent?.Invoke(inkScript, EventArgs.Empty);
        }
    }
}
