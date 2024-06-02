using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lab1_Script : MonoBehaviour
{
    [SerializeField] private GameObject triggerObject;
    [SerializeField] private GameObject sphere;
    private Rigidbody sphereRigidbody;
    private Renderer sphereRenderer;

    private void Start()
    {
        sphereRigidbody = sphere.GetComponent<Rigidbody>();
        sphereRenderer = sphere.GetComponent<Renderer>();
        sphereRigidbody.useGravity = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            sphereRigidbody.useGravity = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == sphere)
        {
            sphereRenderer.material.color = Color.black;
            Debug.Log("Trigger enter");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == sphere)
        {
            sphereRenderer.material.color = Color.red;
            Debug.Log("Trigger exit");
        }
    }
    public void onExitButtonClick()
    {
        SceneManager.LoadSceneAsync(0);
    }
}