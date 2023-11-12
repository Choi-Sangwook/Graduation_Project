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

    private void CameraRotation()       //���콺�� ī�޶� ȸ��
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");             //���콺 �����ӿ� ���� y�� �Է�
        float _cameraRotationX = _xRotation * lookSensitivity;      //ī�޶� x������ ȸ���Ҿ�

        currentCameraRotationX -= _cameraRotationX;                 //ī�޶� x�� ȸ����
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);        //ī�޶� ȸ�� ����

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);                             //ī�޶� ȸ���� ����
    }

    private void CharacterRotation()  // �¿� ĳ���� ȸ��
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");             //���콺 �����ӿ� ���� X �� �Է�
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;        //ĳ���� �¿� ȸ�� ����
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));          //ĳ���� ȸ���� ����
    }
}
