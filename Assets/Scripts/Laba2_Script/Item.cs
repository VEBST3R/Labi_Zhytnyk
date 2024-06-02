using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Item : MonoBehaviour
{
    public float rotationSpeed = 50f; // швидкість обертання
    public int scoreValue = 1; // кількість очків за предмет
    public Text scoreText; // текстове поле для виведення очків
    private static int score = 0; // загальна кількість очків


    // Update is called once per frame
    void Update()
    {
        // обертання об'єкта навколо своєї осі
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // якщо гравець торкнувся предмета
        if (other.gameObject.CompareTag("Player"))
        {
            // знищити предмет
            Destroy(gameObject);

            // додати очки
            score += scoreValue;

            // вивести очки
            scoreText.text = score.ToString();

        }
    }
}