using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Warrior : MovableObject
{
    [SerializeField] public GameObject MotherTower;
    [SerializeField] List<GameObject> enemiesInRange;
    [SerializeField] CircleCollider2D rangeOfVision;

    [Header("Animation coefs")]
    [SerializeField] private AnimationCurve jigglingCoef; //Частота шага в зависимости от скорости
    [SerializeField] private AnimationCurve jigglingAmp;  //Амплитуда покачиваний во время ходьбы
    private float stepTime; // Фаза ходьбы 

    private GameObject _target;

    [Header("Attack stats")]
    [SerializeField] float attackDamage;
    [SerializeField] float attackSpeed;
    [SerializeField] float attackRange;
    [SerializeField] float patrolTime;
    [SerializeField] Vector2 patrolTarget;

    [SerializeField] AbilityHolder abilityHolder;

    enum State
    {
        None = 0,
        Stalking,
        Patrolling
    }

    [SerializeField] State state;


    new void Awake()
    {
        state = State.None;
        enemiesInRange = new List<GameObject>();
        rangeOfVision = GetComponent<CircleCollider2D>() ?? gameObject.AddComponent<CircleCollider2D>();
        abilityHolder = GetComponent<AbilityHolder>() ?? gameObject.AddComponent<AbilityHolder>();
    }

    new void Update()
    {
        UpdateEnemyList();

        if (isDead)
        {
            StopAllCoroutines();
            enabled = false;
            return;
        }

        ChangeSpeed();
        ApplyJiggling();
        Patrol();
        Attack();
    }
    void ApplyJiggling()
    {
        stepTime += Time.deltaTime * jigglingCoef.Evaluate(agent.velocity.magnitude);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(stepTime * Mathf.PI) * jigglingAmp.Evaluate(agent.velocity.magnitude));
    }

    void Attack()
    {
        var enemiesInPatrolRange = enemiesInRange.
                                    Where(x => x != null && (x.transform.position - MotherTower.transform.position).magnitude < MotherTower.GetComponent<WarriorTower>().PatrolDistance);
        if (enemiesInPatrolRange.Count() > 0)
        {
            _target = enemiesInPatrolRange.
                OrderBy(x => (x.transform.position - transform.position).magnitude).
                First();

            SetTarget(_target);
            if (state != State.Stalking)
            {
                state = State.Stalking;
                StartCoroutine(AttackCycle());
            }
        }
        else if (state == State.Stalking)
        {

            state = State.None;
            _target = null;
        }
    }

    void Patrol()
    {
        if (enemiesInRange.Count == 0 && (state == State.None || state == State.Stalking))
        {
            state = State.Patrolling;
            StartCoroutine(PatrolCycle());
        }
    }

    IEnumerator AttackCycle()
    {
        yield return new WaitForSeconds(Random.value * attackSpeed);
        while (_target != null && state == State.Stalking)
        {
            if (Vector2.Distance(_target.transform.position, (Vector2)transform.position + rangeOfVision.offset) < attackRange)
            {
                abilityHolder.ActivateAbility(_target);
            }
            Debug.Log("Attack!");
            yield return null;
        }
    }
    IEnumerator PatrolCycle()
    {
        yield return new WaitForSeconds(Random.value * patrolTime);
        while (state == State.Patrolling && agent.isOnNavMesh)
        {
            yield return new WaitForEndOfFrame();
            patrolTarget = (Vector2)MotherTower.transform.position + (Random.insideUnitCircle * (MotherTower.GetComponent<WarriorTower>().PatrolDistance + 1));
            SetTarget(patrolTarget);
            yield return new WaitForSeconds(patrolTime);
        }
    }

    #region Update list of enemies
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, является ли объект врагом
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Добавляем врага в список
            enemiesInRange.Add(other.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        // Проверяем, является ли объект врагом
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Добавляем врага в список
            enemiesInRange.Remove(other.gameObject);
        }
    }

    public void UpdateEnemyList()
    {
        for (int i = enemiesInRange.Count - 1; i >= 0; i--)
        {
            if (enemiesInRange[i].GetComponent<MovableObject>().isDead)
            {
                enemiesInRange.RemoveAt(i);
            }
        }
    }
    #endregion

  
}