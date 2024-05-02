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

    // 方法：取得目前玩家按方向鍵上下左右的數值
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(jumpKey) && readyToJump)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown); // 如果跳躍過後，就會依照設定的限制時間倒數，時間到了才能往上跳躍
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

    // 方法：重新設定變數readyToJump為true的方法
    private void ResetJump()
    {
        readyToJump = true;
    }
}
