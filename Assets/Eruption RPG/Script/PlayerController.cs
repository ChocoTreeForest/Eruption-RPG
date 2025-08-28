using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerController : MonoBehaviour
{
    public TitleDirector titleDirector;
    public Vector2 inputVec;
    public float speed;

    TouchControls touchControls;
    Rigidbody2D rigid;
    SpriteRenderer playerSpriter;
    Animator anim;

    public SpriteRenderer shadowSpriter;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerSpriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        touchControls = new TouchControls();
    }

    void OnEnable()
    {
        touchControls.Enable();

        touchControls.Touch.TouchPress.performed += ctx => OnTouchStart();
        touchControls.Touch.TouchPress.canceled += ctx => OnTouchEnd();
    }

    void OnDisable()
    {
        touchControls.Touch.TouchPress.performed -= ctx => OnTouchStart();
        touchControls.Touch.TouchPress.canceled -= ctx => OnTouchEnd();

        touchControls.Disable();
    }

    void OnTouchStart()
    {
        // 터치 좌표 -> 월드 좌표 변환
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(touchControls.Touch.TouchPosition.ReadValue<Vector2>());
        inputVec = (touchPos - (Vector2)transform.position).normalized;
    }

    void OnTouchEnd()
    {
        inputVec = Vector2.zero; // 손가락을 떼면 이동 멈춤
    }

    void FixedUpdate()
    {
        if (BattleManager.Instance != null && MenuUIManager.Instance != null)
        {
            if (BattleManager.Instance.isInBattle || MenuUIManager.Instance.isPanelOpen) return;
        }

        if (touchControls.Touch.TouchPress.IsPressed())
        {
            // 이동 방향 계속 업데이트
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touchControls.Touch.TouchPosition.ReadValue<Vector2>());
            inputVec = (touchPos - (Vector2)transform.position).normalized;
        }

        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    void LateUpdate()
    {
        if (BattleManager.Instance != null && MenuUIManager.Instance != null)
        {
            if (BattleManager.Instance.isInBattle || MenuUIManager.Instance.isPanelOpen)
            {
                anim.SetFloat("Speed", 0f);
                return;
            }
        }

        anim.SetFloat("Speed", inputVec.magnitude);
        anim.SetFloat("InputY", inputVec.y);

        if (titleDirector != null)
        {
            if (titleDirector.isMoving)
            {
                anim.SetFloat("Speed", titleDirector.speed);
                anim.SetFloat("InputY", 1f);
            }
        }

        //inputVec.x가 0보다 작으면 좌우 반전
        if (inputVec.x != 0)
        {
            playerSpriter.flipX = inputVec.x < 0;
            shadowSpriter.flipX = inputVec.x < 0;
        }
    }
}
