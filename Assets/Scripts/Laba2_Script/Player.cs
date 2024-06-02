using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Text ItemsAddedText;
    public Text ItemsDeletedText;
    public int ItemsAdded = 0;
    public int ItemsDeleted = 0;
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;
    public float jumpForce = 2.0f;
    public bool rotateX = true;
    public bool rotateY = true;
    public LayerMask groundLayer;
    public Camera myCamera;
    public int BlockType = 1;
    public GameObject inventoryUI;
    public int selectedInventoryIndex = 0;
    private Transform cameraTransform;
    private float currentCameraRotationX = 0f;
    private Rigidbody rb;
    private bool isJumping = false;
    public bool invertY = false;
    [SerializeField] private AudioSource[] audioSources;
    public WorldGeneration worldGeneration;
    public LevelManager levelManager;

    public GameObject[] inventory; //інвертар гравця
    public GameObject[] blocks; //блоки, які може створити гравець
    public float spawnDistance = 5f; // відстань на якій буде створено об'єкт
    // Start is called before the first frame update
    void Start()
    {
        // Get the transform of the child camera
        cameraTransform = GetComponentInChildren<Camera>().transform;
        rb = GetComponent<Rigidbody>();

        ItemsDeletedText.text = "Об'єктів видалено: " + ItemsDeleted.ToString();
        ItemsAddedText.text = "Об'єктів встановленно: " + ItemsAdded.ToString();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R) && levelManager.isPaused == false)
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            if (!inventoryUI.activeSelf)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
            }

        }
        if (levelManager.isPaused == false)
        {

            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            transform.Translate(movement * speed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && Mathf.Abs(rb.velocity.y) < 0.001f)
            {
                rb.velocity = Vector3.up * jumpForce;
            }
            // Camera rotation
            float rotationX = 0;
            float rotationY = 0;

            if (rotateX)
            {
                rotationX = Input.GetAxisRaw("Mouse X") * rotationSpeed * Time.deltaTime;
            }

            if (rotateY)
            {
                rotationY = Input.GetAxisRaw("Mouse Y") * (invertY ? -1 : 1) * rotationSpeed * Time.deltaTime;
            }

            // Update the current camera rotation
            currentCameraRotationX -= rotationY;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -90f, 90f);

            // Apply the rotation to the camera
            cameraTransform.localRotation = Quaternion.Euler(currentCameraRotationX, 0f, 0f);
            transform.Rotate(Vector3.up * rotationX);
        }


        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            // Deactivate the currently selected object
            if (inventory[selectedInventoryIndex] != null)
            {
                inventory[selectedInventoryIndex].SetActive(false);
            }

            if (scroll > 0) // scroll up
            {
                selectedInventoryIndex++;
                if (selectedInventoryIndex >= inventory.Length)
                {
                    selectedInventoryIndex = 0; // wrap around to the start of the array
                }
            }
            else // scroll down
            {
                selectedInventoryIndex--;
                if (selectedInventoryIndex < 0)
                {
                    selectedInventoryIndex = inventory.Length - 1; // wrap around to the end of the array
                }
            }

            // Activate the newly selected object
            if (inventory[selectedInventoryIndex] != null)
            {
                inventory[selectedInventoryIndex].SetActive(true);
            }
        }
        switch (selectedInventoryIndex)
        {
            case 0: //кірка

                if (Input.GetMouseButtonDown(0) && levelManager.isPaused == false)
                {
                    Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.gameObject.CompareTag("Block_Stone"))
                        {
                            audioSources[2].Play();
                            ItemsDeleted++;
                            ItemsDeletedText.text = "Об'єктів видалено: " + ItemsDeleted.ToString();
                            Destroy(hit.transform.gameObject);
                            worldGeneration.blocksInfo.Remove(hit.transform.gameObject);
                        }
                    }
                }

                break;

            case 1: //лопата

                if (Input.GetMouseButtonDown(0) && levelManager.isPaused == false)
                {
                    Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.gameObject.CompareTag("Block_Grass") || hit.transform.gameObject.CompareTag("Block_Dirt"))
                        {
                            audioSources[1].Play();
                            ItemsDeleted++;
                            ItemsDeletedText.text = "Об'єктів видалено: " + ItemsDeleted.ToString();
                            Destroy(hit.transform.gameObject);
                            worldGeneration.blocksInfo.Remove(hit.transform.gameObject);
                        }
                    }
                }

                break;

            case 2: //сокира

                if (Input.GetMouseButtonDown(0) && levelManager.isPaused == false)
                {
                    Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.gameObject.CompareTag("Block_Wood"))
                        {
                            audioSources[3].Play();
                            ItemsDeleted++;
                            ItemsDeletedText.text = "Об'єктів видалено: " + ItemsDeleted.ToString();
                            Destroy(hit.transform.gameObject);
                            worldGeneration.blocksInfo.Remove(hit.transform.gameObject);
                        }
                    }
                }
                break;

            case 3: //блок трави
                if (Input.GetMouseButtonDown(0) && levelManager.isPaused == false)
                {
                    // визначення місця, куди гравець дивиться
                    Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        // створення нового блоку в місці, куди гравець дивиться
                        Vector3 blockPosition = hit.point + hit.normal / 2.0f; // додати половину довжини блоку, щоб він з'явився на поверхні, а не в ній
                        blockPosition.x = Mathf.RoundToInt(blockPosition.x);
                        blockPosition.y = Mathf.RoundToInt(blockPosition.y);
                        blockPosition.z = Mathf.RoundToInt(blockPosition.z);
                        audioSources[0].Play();
                        GameObject obj = Instantiate(blocks[0], blockPosition, Quaternion.identity);
                        worldGeneration.blocksInfo.Add(obj);
                        ItemsAdded++;
                        ItemsAddedText.text = "Об'єктів встановленно: " + ItemsAdded.ToString();

                    }
                }
                break;
            case 4: //блок землі
                if (Input.GetMouseButtonDown(0) && levelManager.isPaused == false)
                {
                    // визначення місця, куди гравець дивиться
                    Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        // створення нового блоку в місці, куди гравець дивиться
                        audioSources[1].Play();
                        Vector3 blockPosition = hit.point + hit.normal / 2.0f; // додати половину довжини блоку, щоб він з'явився на поверхні, а не в ній
                        blockPosition.x = Mathf.RoundToInt(blockPosition.x);
                        blockPosition.y = Mathf.RoundToInt(blockPosition.y);
                        blockPosition.z = Mathf.RoundToInt(blockPosition.z);
                        GameObject obj = Instantiate(blocks[1], blockPosition, Quaternion.identity);
                        worldGeneration.blocksInfo.Add(obj);
                        ItemsAdded++;
                        ItemsAddedText.text = "Об'єктів встановленно: " + ItemsAdded.ToString();

                    }
                }
                break;
            case 5: //блок каменю
                if (Input.GetMouseButtonDown(0) && levelManager.isPaused == false)
                {
                    // визначення місця, куди гравець дивиться
                    Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        // створення нового блоку в місці, куди гравець дивиться
                        audioSources[2].Play();
                        Vector3 blockPosition = hit.point + hit.normal / 2.0f; // додати половину довжини блоку, щоб він з'явився на поверхні, а не в ній
                        blockPosition.x = Mathf.RoundToInt(blockPosition.x);
                        blockPosition.y = Mathf.RoundToInt(blockPosition.y);
                        blockPosition.z = Mathf.RoundToInt(blockPosition.z);
                        GameObject obj = Instantiate(blocks[2], blockPosition, Quaternion.identity);
                        worldGeneration.blocksInfo.Add(obj);
                        ItemsAdded++;
                        ItemsAddedText.text = "Об'єктів встановленно: " + ItemsAdded.ToString();
                    }
                }
                break;
            case 6: //блок дерева
                if (Input.GetMouseButtonDown(0) && levelManager.isPaused == false)
                {
                    // визначення місця, куди гравець дивиться
                    Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        // створення нового блоку в місці, куди гравець дивиться
                        audioSources[3].Play();
                        Vector3 blockPosition = hit.point + hit.normal / 2.0f; // додати половину довжини блоку, щоб він з'явився на поверхні, а не в ній
                        blockPosition.x = Mathf.RoundToInt(blockPosition.x);
                        blockPosition.y = Mathf.RoundToInt(blockPosition.y);
                        blockPosition.z = Mathf.RoundToInt(blockPosition.z);
                        GameObject obj = Instantiate(blocks[3], blockPosition, Quaternion.identity);
                        worldGeneration.blocksInfo.Add(obj);
                        ItemsAdded++;
                        ItemsAddedText.text = "Об'єктів встановленно: " + ItemsAdded.ToString();
                    }
                }
                break;

        }
    }


}