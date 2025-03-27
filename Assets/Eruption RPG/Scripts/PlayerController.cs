using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;

    Rigidbody2D rigid;
    SpriteRenderer playerSpriter;
    Animator anim;

    public SpriteRenderer shadowSpriter;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerSpriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude);
        anim.SetFloat("InputY", inputVec.y);

        //inputVec.x가 0보다 작으면 좌우 반전
        if (inputVec.x != 0)
        {
            playerSpriter.flipX = inputVec.x < 0;
            shadowSpriter.flipX = inputVec.x < 0;
        }
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
}
