using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using System;

public class RandomWalker : MonoBehaviour
{
    CharacterMovement movement;
    private Func<bool> IsReachedPosFunc;
   
    Vector3 targetPos;
    System.Random random;

    bool forceExit = false;

    void Start()
    {
        movement = GetComponent<CharacterMovement>();
        random = new System.Random();

        targetPos = transform.position;

        StartCoroutine(RandomWalk());
    }

    private void Update()
    {
        forceExit = Input.GetKey(KeyCode.C);   
    }

    private IEnumerator RandomWalk()
    {
        while (true && !forceExit)
        {
            int randomNum = random.Next(0, 3);
            switch (randomNum)
            {
                case 0:
                    targetPos += Vector3.forward * 1f;
                    break;
                case 1:
                    targetPos += Vector3.back * 1f;
                    break;
                case 2:
                    targetPos += Vector3.right * 1f;
                    break;
                case 3:
                    targetPos += Vector3.left * 1f;
                    break;
                default:
                    break;
            }
            Debug.Log(targetPos);
            movement.MoveCharacter(targetPos);

            yield return new WaitUntil(() => movement.IsReachedPosition(targetPos, 1f));
        }
        
    }

}
