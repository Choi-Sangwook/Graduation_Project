using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private float cameraRotationLimit;

    private float currentCameraRotationX;
    public Camera theCamera;
    private Rigidbody myRigid;



    public EditManager manager;

    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (manager.isSubCamera.isOn && !(manager.inputField.isFocused))
        {
            Move();
            if (Input.GetMouseButton(1))
            {
                CameraRotation();
                CharacterRotation();
            }
        }
    }

    private void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirZ = Input.GetAxisRaw("Vertical");
        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;
        Vector3 velocity = (moveHorizontal + moveVertical).normalized * walkSpeed;
        myRigid.MovePosition(transform.position + velocity * Time.deltaTime);
    }

    private void CameraRotation()       //마우스로 카메라 회전
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");             //마우스 움직임에 따른 y값 입력
        float _cameraRotationX = _xRotation * lookSensitivity;      //카메라가 x축으로 회전할양

        currentCameraRotationX -= _cameraRotationX;                 //카메라 x축 회전값
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);        //카메라가 회전 제한

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);                             //카메라 회전값 설정
    }

    private void CharacterRotation()  // 좌우 캐릭터 회전
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");             //마우스 움직임에 따른 X 값 입력
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;        //캐릭터 좌우 회전 제한
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));          //캐릭터 회전값 설정
    }
}
