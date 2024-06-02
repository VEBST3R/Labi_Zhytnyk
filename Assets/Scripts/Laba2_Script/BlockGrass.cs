using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Додайте цей рядок

public class BlockGrass : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // Замініть "Player" на тег вашого гравця
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}