using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

using static CustomTools;

public class MovableObject : MonoBehaviour
{
    [Header("Target for the navMeshAgent")]
    [SerializeField] private GameObject target;
    [Header("Properties")]
    [SerializeField] private protected float baseSpeed;
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] private protected float health;
    public GameObject Target => target;
    public float BaseSpeed => baseSpeed;
    public float Health => health;

    public bool isDead;


    public void SetTarget(GameObject newTarget)
    {
        if (newTarget == null)
        {
            Debug.LogError("New target is null.");
            return;
        }
        target = newTarget;
        agent.SetDestination(newTarget.transform.position);
    }
    public void SetTarget(Vector2 newTarget)
    {
        agent.SetDestination(newTarget);
    }

    private protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>() ?? gameObject.AddComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private protected void OnEnable()
    {
        if (target != null)
        {
            agent.SetDestination(target.transform.position);
        }
    }

    private protected void Update()
    {
        //agent.SetDestination(target.transform.position);

        if (target == null || isDead) return;

        if (agent.Raycast(agent.transform.position, out var hit))
        {
            agent.speed = baseSpeed / agent.GetAreaCost(FirstBitIndex(hit.mask));
        }
    }
    private protected void ChangeSpeed()
    {
        agent.Raycast(agent.transform.position, out var hit);
        agent.speed = BaseSpeed / agent.GetAreaCost(FirstBitIndex(hit.mask));
    }

    public void ApplyDamage(float damage)
    {
        health -= damage;
        if (health < 0)
        {
            Die();
        }
    }
    private protected void Die()
    {
        isDead = true;
        StartCoroutine(DieTimer());
        agent.enabled = false;
        enabled = false;
        transform.position = -Vector3.one;
    }

    protected IEnumerator DieTimer()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
