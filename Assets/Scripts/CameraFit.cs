using UnityEngine;

public class CameraFit : MonoBehaviour
{
    // Slightly larger than the walls (17.5 x 10) so the edges show.
    [SerializeField] private float roomWidth = 18.5f;
    [SerializeField] private float roomHeight = 10.5f;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        // Orthographic size is half the visible height.
        float sizeForHeight = roomHeight / 2f;
        float sizeForWidth = roomWidth / (2f * cam.aspect);

        // Use whichever is bigger, so the whole room always fits.
        cam.orthographicSize = Mathf.Max(sizeForHeight, sizeForWidth);
    }
}