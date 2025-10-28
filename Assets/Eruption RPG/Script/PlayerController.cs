using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.SearchService;
#endif
using UnityEngine;
using UnityEngine.EventSystems;
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

    void OnEnable() => touchControls.Enable();
    void OnDisable() => touchControls.Disable();

    void Update()
    {
        if (!CanMove())
        {
            return;
        }

        if (EventSystem.current.currentSelectedGameObject != null && titleDirector == null)
        {
            // UI 버튼을 누르는 중이라면 이동 안 함
            inputVec = Vector2.zero;
            return;
        }

        if (touchControls.Touch.TouchPress.IsPressed() && !IsPointerOverUI())
        {
            // 이동 방향 계속 업데이트
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touchControls.Touch.TouchPosition.ReadValue<Vector2>());
            inputVec = (touchPos - (Vector2)transform.position).normalized;
        }
        else
        {
            inputVec = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        if (!CanMove())
        {
            return;
        }

        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    void LateUpdate()
    {
        if (titleDirector != null)
        {
            HandleTitleMovement();
            return;
        }

        if (!CanMove())
        {
            anim.SetFloat("Speed", 0f);
            return;
        }

        anim.SetFloat("Speed", inputVec.magnitude);
        anim.SetFloat("InputY", inputVec.y);

        //inputVec.x가 0보다 작으면 좌우 반전
        if (inputVec.x != 0)
        {
            playerSpriter.flipX = inputVec.x < 0;
            shadowSpriter.flipX = inputVec.x < 0;
        }
    }

    private bool CanMove()
    {
        if (PlayerStatus.Instance != null && PlayerStatus.Instance.gameOver)
            return false;

        if (MenuUIManager.Instance != null && MenuUIManager.Instance.isFading)
            return false;

        if (titleDirector != null)
            return false;

        if (BattleManager.Instance != null && MenuUIManager.Instance != null)
        {
            if (BattleManager.Instance.isInBattle ||
                MenuUIManager.Instance.isPanelOpen ||
                BattleManager.Instance.sceneChanging)
                return false;
        }

        return true;
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current != null &&
               EventSystem.current.IsPointerOverGameObject(0);
    }

    private void HandleTitleMovement()
    {
        if (titleDirector.isMoving)
        {
            anim.SetFloat("Speed", titleDirector.speed);
            anim.SetFloat("InputY", 1f);
        }
        else
        {
            anim.SetFloat("Speed", 0f);
        }
    }
}
