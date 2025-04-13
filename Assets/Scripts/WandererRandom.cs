using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class WandererRandom : MonoBehaviour
{
    NavMeshAgent agent;


    public string animatorParam;
    Animator animator;

    float watchDogTimer;
    float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1f)
        {
            CheckStufffAndSelectTarget();
            timer = 0f;
        }
    }

    void CheckStufffAndSelectTarget()
    {
        if (Time.time - watchDogTimer > 0)
        {
            if (agent.enabled) agent.SetDestination(NavManager.Instance.RandomPointInsideBounds());
            watchDogTimer = Random.Range(5f, 20f);
        }
        animator.SetFloat(animatorParam, agent.velocity.sqrMagnitude);
    }
}
