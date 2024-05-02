using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed;
    public KeyCode jumpKey = KeyCode.Space;

    public float jumpForce;
    public float jumpCooldown;

    public Transform PlayerCamera;

    private bool readyToJump;
    private float horizontalInput;   
    private float verticalInput;  
    
    private Vector3 moveDirection;
    private Rigidbody rbFirstPerson;
    // Start is called before the first frame update
    void Start()
    {
        rbFirstPerson = GetComponent<Rigidbody>();
        rbFirstPerson.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        SpeedControl(); 
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    // ��k�G���o�ثe���a����V��W�U���k���ƭ�
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(jumpKey) && readyToJump)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown); // �p�G���D�L��A�N�|�̷ӳ]�w������ɶ��˼ơA�ɶ���F�~�੹�W���D
        }
    }

    private void MovePlayer()
    {
        moveDirection = PlayerCamera.forward * verticalInput + PlayerCamera.right * horizontalInput;
        rbFirstPerson.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rbFirstPerson.velocity.x, 0f, rbFirstPerson.velocity.z);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rbFirstPerson.velocity = new Vector3(limitedVel.x, rbFirstPerson.velocity.y, limitedVel.z);
        }
    }
    private void Jump()
    {
        rbFirstPerson.velocity = new Vector3(rbFirstPerson.velocity.x, 0f, rbFirstPerson.velocity.z);
        rbFirstPerson.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    // ��k�G���s�]�w�ܼ�readyToJump��true����k
    private void ResetJump()
    {
        readyToJump = true;
    }
}
