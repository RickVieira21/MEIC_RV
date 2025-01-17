using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;
using TMPro;

public class ObjectManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] interactableObjects;
    [SerializeField]
    InputActionReference aButton;
    [SerializeField]
    InputActionReference xButton;
    [SerializeField]
    InputActionReference bButton;
    [SerializeField]
    InputActionReference yButton;
    [SerializeField]
    GameObject CSV;
    [SerializeField]
    GameObject canvas;

    Dictionary<GameObject, Vector3> objectsInitialLocation;
    int activeObject = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        aButton.action.started += ResetInteractableObjectsPosition;
        xButton.action.started += ResetInteractableObjectsPosition;
        bButton.action.started += StartTest;
        yButton.action.started += StartTest;


        objectsInitialLocation = new Dictionary<GameObject, Vector3>();

        foreach (GameObject obj in interactableObjects)
        {
            Debug.Log(obj.name);
            objectsInitialLocation.Add(obj, obj.transform.position);
        }
    }

    public void EnableSecondaryButtons()
    {
        bButton.action.started += ContinueTest;
        yButton.action.started += ContinueTest;

        bButton.action.Enable();
        yButton.action.Enable();
    }

    public void DisableSecondaryButtons()
    {
        bButton.action.Disable();
        yButton.action.Disable();
    }

    public void ResetInteractableObjectsPosition(InputAction.CallbackContext context)
    {
        Debug.Log("Objects Reseted");
        foreach (GameObject obj in objectsInitialLocation.Keys)
        {
            if (obj.activeSelf)
            {
                Rigidbody rb = obj.GetComponent<Rigidbody>();

                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.Sleep(); // Put Rigidbody into a "sleep" state to prevent further updates

                // Synchronize Rigidbody with the new transform position
                rb.MovePosition(objectsInitialLocation[obj]);
                obj.transform.position = objectsInitialLocation[obj];
            }
        }
    }

    public bool isEndTest()
    {
        return activeObject == interactableObjects.Length - 1;
    }

    public void ContinueTest(InputAction.CallbackContext context)
    {
        interactableObjects[activeObject].SetActive(false);
        Debug.Log(interactableObjects[activeObject].name + " disabled");

        activeObject++;

        canvas.SetActive(false);
        Time.timeScale = 1.0f;

        CSV.GetComponent<CSV>().StartCoroutine(CSV.GetComponent<CSV>().WrtieCSV());

        interactableObjects[activeObject].SetActive(true);
        Debug.Log(interactableObjects[activeObject].name + " active");

        Transform locomotion = transform.Find("Locomotion");
        locomotion.gameObject.SetActive(true);
        Debug.Log("Locomotion ON");

        DisableSecondaryButtons();
    }

    public void StartTest(InputAction.CallbackContext context)
    {
        Time.timeScale = 1.0f;
        canvas.SetActive(false);

        Transform locomotion = transform.Find("Locomotion");
        locomotion.gameObject.SetActive(true);

        Debug.Log(locomotion.name);
        DisableSecondaryButtons();

        CSV.SetActive(true);
        CSV.GetComponent<CSV>().StartCoroutine(CSV.GetComponent<CSV>().WrtieCSV());

        Debug.Log("Test Start");

    }
}
