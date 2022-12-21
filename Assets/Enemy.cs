using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator animator;
    private float health = 1f;
    private float animationTime;
    public float animationEndDelay = 0f;

    public float Health {
        set {
            health = value;
            
            if(health <= 0) {
                Defeated();
            }
        }
        get {
            return health;
        }
    }

    private void Start() {
        animator = GetComponent<Animator>();
    }

    public void Defeated() {
        animator.SetTrigger("isDefeated");
        RemoveEnemy();
    }
    public void RemoveEnemy() {
        animationTime = animator.GetCurrentAnimatorStateInfo(0).length + animationEndDelay;
        print(animationTime);
        Destroy(gameObject, animationTime);
    }
}
