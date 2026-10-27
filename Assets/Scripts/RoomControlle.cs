using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private GameObject exit; // hidden until the room is cleared

    private bool cleared;

    private void Start()
    {
        exit.SetActive(false);
    }

    private void Update()
    {
        if (cleared) return;

        // No enemies left in the scene means the room is cleared.
        if (FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length == 0)
        {
            cleared = true;
            exit.SetActive(true);
        }
    }
}