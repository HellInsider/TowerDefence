using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Enemy : MovableObject
{
    [Header("Animation coefs")]
    [SerializeField] private AnimationCurve jigglingCoef; //������� ���� � ����������� �� ��������
    [SerializeField] private AnimationCurve jigglingAmp;  //��������� ����������� �� ����� ������

    [Header("Flipping sprite")]
    [SerializeField] private bool isFlipping; //��������� ������� � ����������� �� ����������� ��������
    [SerializeField] private float velocityForFlipping; //��������� �������� ��������, ��� ������� �������� �����������

    [Header("Characteristics")]
    [SerializeField] private float health = 1f;
    [SerializeField] private float distanceToEnter = 1f; //��������� ��� ������� ���� �������� �� ����
    [Header("Active effects")]
    private List<Effect> effects = new List<Effect>();
    public bool isDead;

    [Header("Attack stats")]
    [SerializeField] private List<GameObject> unitsInRange = new List<GameObject>();
    [SerializeField] CircleCollider2D rangeOfAttack;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float attackSpeed = 1f;
    private bool isAttacking;
    private GameObject _target;

    private float stepTime; // ���� ������ (��� ����, ����� ��� ����� �� ��� � ���� ����)

    private SpriteRenderer spriteRenderer;

    new void Awake()
    {
        base.Awake(); // ����� Awake �� MovableObject
        stepTime = Random.value;

        spriteRenderer = GetComponent<SpriteRenderer>() ?? gameObject.AddComponent<SpriteRenderer>();
    }

    new void Update()
    {
        base.Update(); // ����� Update �� MovableObject

        if (Target != null)
        {
            ChangeSpeed();
            ApplyJiggling();
        }
        else
        {
            FindNewTarget();
        }

        Attack();


        if (isFlipping && Mathf.Abs(agent.velocity.x) > velocityForFlipping) //������� �������� ��� ��������� �������������� ��������
        {
            spriteRenderer.flipX = agent.velocity.x > 0;
        }
    }

    void FindNewTarget()
    {
        SetTarget(GameObject.FindGameObjectsWithTag("Base").OrderBy(x => (x.transform.position - transform.position).magnitude).FirstOrDefault());
    }



    void ApplyJiggling()
    {
        stepTime += Time.deltaTime * jigglingCoef.Evaluate(agent.velocity.magnitude);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(stepTime * Mathf.PI) * jigglingAmp.Evaluate(agent.velocity.magnitude));
    }

    public void ApplyDamage(float damage, GameObject owner)
    {
        health -= damage;
        if (health < 0)
        {
            if (owner.CompareTag("Tower"))
            {
                owner.GetComponent<ArcherTower>().RemoveEnemyFromList(gameObject);
            }
            if (owner.CompareTag("Warrior"))
            {
                owner.GetComponent<Warrior>().RemoveEnemyFromList(gameObject);
            }
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }



    #region Update list of units
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���������, �������� �� ������ ������
        if (other.gameObject.CompareTag("Unit") || other.gameObject.CompareTag("Warrior"))
        {
            // ��������� ����� � ������
            unitsInRange.Add(other.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        // ���������, �������� �� ������ ������
        if (other.gameObject.CompareTag("Unit") || other.gameObject.CompareTag("Warrior"))
        {
            // ������� ����� �� ������
            unitsInRange.Remove(other.gameObject);
        }
    }
    public void RemoveUnitFromList(GameObject unit)
    {
        unitsInRange.Remove(unit);
    }

    void OnDrawGizmos()
    {
        if (rangeOfAttack != null) Handles.DrawWireDisc(transform.position + (Vector3)rangeOfAttack.offset, Vector3.forward, rangeOfAttack.radius);
    }

    #endregion

    #region Attack nearest unit

    void Attack()
    {
        if (unitsInRange.Count > 0)
        {
            _target = unitsInRange.OrderBy(x => (x.transform.position - transform.position).magnitude).FirstOrDefault();
            if (_target == null) return;
            if (!isAttacking)
            {
                isAttacking = true;
                StartCoroutine(AttackCycle());
            }
        }
        else
        {
            isAttacking = false;
            StopCoroutine(AttackCycle());
        }
        
    }

    IEnumerator AttackCycle()
    {
        while (isAttacking)
        {
            switch (_target.tag)
            {
                case "Unit":
                    _target.GetComponent<Unit>().ApplyDamage(damage, gameObject);
                    break;
                case "Warrior":
                    _target.GetComponent<Warrior>().ApplyDamage(damage, gameObject);
                    break;
            }    
            yield return new WaitForSeconds(attackSpeed);
        }
    }


    #endregion

    //TODO
    #region Effects 
    public void AddEffect(Effect effect)
    {
        effects.Add(effect);
        effect.Apply();
    }

    public void RemoveEffect(Effect effect)
    {
        effect.Remove();
        effects.Remove(effect);
    }

    public void UpdateEffects()
    {
        foreach (var effect in effects.ToList())
        {
            effect.Duration -= Time.deltaTime;
            if (effect.Duration <= 0)
            {
                RemoveEffect(effect);
            }
        }
    }
    #endregion
}
