using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Character : MonoBehaviour
{
    enum MoveType
    {
        GetInput,
        FollowTarget
    }
    [SerializeField] Transform CharacterBody;
    [SerializeField] MoveType moveType;
    [SerializeField] Transform target;
    [SerializeField] float moveSpeed=3;
    [SerializeField] float jumpPower=10;
    Vector3 input;
    Rigidbody rb;
    Animator anim;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (moveType)
        {
            case MoveType.GetInput:
                input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                EmotionFunction();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    input.y = jumpPower;
                    anim.SetTrigger("Jump");
                }
                else if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    anim.SetTrigger("Dodge");
                }

                break;
            case MoveType.FollowTarget:
                if(target)
                    input = (target.position - this.transform.position);
                input.y = 0;
                input.Normalize();
                break;
            default:
                break;
        }

        rb.velocity = input* moveSpeed + rb.velocity.y*Vector3.up;

        if (input.x > 0.01f)
            CharacterBody.transform.localScale = new Vector3(1, 1, 1);
        else if (input.x < -0.01f)
            CharacterBody.transform.localScale = new Vector3(-1, 1, 1);
        anim.SetFloat("Vel", input.magnitude*10);
    }

    private void EmotionFunction()
    {
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Keypad0 + i))
            {
                SetEmotion(i);
            }
        }
    }

    public void SetEmotion(int _index)
    {
        anim.SetInteger("EmotionIndex", _index);
        anim.SetTrigger("EmotionTrigger");
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
    }
    public void Attacked()
    {
        anim.SetTrigger("Attacked");
    }
    public void Die()
    {
        anim.SetTrigger("Die");
    }

}
