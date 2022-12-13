using UnityEngine;


public class GameManager : MonoBehaviour
{
/*     [SerializeField] private Camera mainCamera;

    [SerializeField] private BoxCollider2D topWall;
    [SerializeField] private BoxCollider2D bottomWall;
    [SerializeField] private BoxCollider2D leftWall;
    [SerializeField] private BoxCollider2D rightWall; */

    public GameObject Player01;


    void Awake()
    {
        /*         Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined; */
        Player01.SetActive(true);
    }


    // Update is called once per frame
   /*  void Update()
    {
        //Move walls to edge locations
        topWall.size = new Vector2(mainCamera.ScreenToWorldPoint(new Vector3(Screen.width * 2f, 0f, 0f)).x, 1f);
        topWall.offset = new Vector2(0f, mainCamera.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f)).y + 0.5f);

        bottomWall.size = new Vector2(mainCamera.ScreenToWorldPoint(new Vector3(Screen.width * 2f, 0f, 0f)).x, 1f);
        bottomWall.offset = new Vector2(0f, mainCamera.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).y - 0.5f);

        leftWall.size = new Vector2(1f, mainCamera.ScreenToWorldPoint(new Vector3(0f, Screen.height * 2f, 0f)).y);
        leftWall.offset = new Vector2(mainCamera.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).x - 0.5f, 0f);

        rightWall.size = new Vector2(1f, mainCamera.ScreenToWorldPoint(new Vector3(0f, Screen.height * 2f, 0f)).y);
        rightWall.offset = new Vector2(mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x + 0.5f, 0f);
    } */


    /*     // Function to check if the game window is focused or not
        void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
                Time.timeScale = 1;
                Debug.Log("Application is focused");
            }
            else
            {
                Cursor.visible = true;  // Show cursor
                Time.timeScale = 0;     // Freeze Game
                Debug.Log("Application lost focus");
            }
        } */
}

