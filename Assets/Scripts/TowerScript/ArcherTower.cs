using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArcherTower : Tower
{
    private List<GameObject> enemiesInRange; // Список врагов в диапазоне атаки
    [SerializeField] private CircleCollider2D rangeOfAttack;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float attackRate;
    [SerializeField] private float attackTimer;
    [SerializeField] private float projSpeed;
    [SerializeField] private bool isFire;
    [SerializeField] private TargetType type;
    [SerializeField] private float efficiency_coef;
    [SerializeField] private AbilityHolder abilityHolder;

    private enum TargetType
    {
        First = 0,
        Last,
        Nearest,
        Farthest
    }

    private GameObject SelectTarget(TargetType type)
    {
        switch (type)
        {
            case TargetType.First:
                return enemiesInRange.First();
            case TargetType.Last:
                return enemiesInRange.Last();
            case TargetType.Nearest:
                return enemiesInRange.OrderBy(x => (x.transform.position - transform.position).sqrMagnitude).First();
            case TargetType.Farthest:
                return enemiesInRange.OrderBy(x => (x.transform.position - transform.position).sqrMagnitude).Last();
            default:
                return null;
        }
    }

    private void Awake()
    {
        enemiesInRange = new List<GameObject>();
        rangeOfAttack = GetComponent<CircleCollider2D>() ?? gameObject.AddComponent<CircleCollider2D>();
    }

    #region Find enemies in range of attack
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    public void RemoveEnemyFromList(GameObject enemy)
    {
        enemiesInRange.Remove(enemy);
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

    private void Update()
    {
        UpdateEnemyList();
        if (enemiesInRange.Count != 0)
        {
            var target = SelectTarget(type);
            abilityHolder.ActivateAbility(target);
        }
    }

    private void OnDrawGizmos()
    {
        if (rangeOfAttack != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + (Vector3)rangeOfAttack.offset, rangeOfAttack.radius);
        }

        if (enemiesInRange != null && enemiesInRange.Count != 0)
        {
            Gizmos.color = Color.yellow;
            foreach (GameObject enemy in enemiesInRange)
            {
                if (enemy != null)
                {
                    var collider = enemy.GetComponent<BoxCollider2D>();
                    if (collider != null)
                    {
                        Gizmos.DrawWireCube(enemy.transform.position + (Vector3)collider.offset, collider.size);
                    }
                }
            }
        }
    }
}
