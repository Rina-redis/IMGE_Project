using UnityEngine;
using Unity.Cinemachine;

public class PanAndZoom : MonoBehaviour
{

    [SerializeField]
    private float panSpeed = 2f;

    [SerializeField]
    private float zoomSpeed = 3f;

    [SerializeField]
    private float zoomInMax = 40f;

    [SerializeField]
    private float zoomOutMax = 90f;

    //need be attacht to vm
    private CinemachineInputProvider inputProvider;
    private CinemachineCamera virtualCamera;
    private Transform cameraTransform;

    private void Awake()
    {
        inputProvider = GetComponent<CinemachineInputProvider>();
        virtualCamera = GetComponent<CinemachineCamera>();
        cameraTransform = virtualCamera.gameObject.transform;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void ZoomScreen(float increment){
        float fov = virtualCamera.Lens.FieldOfView;
        float target = Mathf.Clamp(fov + increment, zoomInMax, zoomOutMax);
        virtualCamera.Lens.FieldOfView = Mathf.Lerp(fov, target, zoomSpeed * Time.deltaTime);
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = inputProvider.GetAxisValue(0);
        float y = inputProvider.GetAxisValue(1);
        float z = inputProvider.GetAxisValue(2);

        if (x != 0 || y != 0)
        {
            PanScreen_(x, y);
        }
        if(z != 0){
            ZoomScreen(z);
        }

    }

    public Vector2 PanDirection(float x, float y)
    {
        Vector2 direction = Vector2.zero;
        if (y >= Screen.height * .95f)
        {
            direction.y += 1;
        }
        else if (y <= Screen.height * .05f)
        {
            direction.y -= 1;
        }

        if (x >= Screen.width * .95f)
        {
            direction.x += 1;
        }
        else if (x <= Screen.width * .05f)
        {
            direction.x -= 1;
        }

        return direction;
    }

    public void PanScreen_(float x, float y)
    {
        Vector2 direction = PanDirection(x, y);
        cameraTransform.position = Vector3.Lerp(cameraTransform.position,
        cameraTransform.position + (Vector3)(direction * panSpeed),
        Time.deltaTime);

    }
}
