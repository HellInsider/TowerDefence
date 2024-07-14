using System.Collections;
using UnityEngine;
using static CustomTools;

public class Projectile : MonoBehaviour
{
    public GameObject owner;
    [SerializeField] public float damage;
    [SerializeField] float despawnTime;
    [SerializeField] public Team team;

    private void Awake()
    {
        StartCoroutine(Despawn());
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(despawnTime);
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other is BoxCollider2D)
        {
            switch (team)
            {
                case Team.none: break;
                case Team.friend:
                    if (other.gameObject.CompareTag("Enemy"))
                    {
                        other.gameObject.GetComponent<MovableObject>().ApplyDamage(damage);
                        Destroy(gameObject);
                    }
                    break;
                case Team.enemy:
                    if (other.gameObject.CompareTag("Unit") || other.gameObject.CompareTag("Warrior"))
                    {
                        other.gameObject.GetComponent<MovableObject>().ApplyDamage(damage);
                        Destroy(gameObject);
                    }
                    break;
            }
        }
    }
}
