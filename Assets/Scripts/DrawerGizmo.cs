using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerGizmo : MonoBehaviour
{
    void OnDrawGizmos()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            // Устанавливаем цвет для Gizmos
            Gizmos.color = Color.red;

            // Получаем размер и позицию коллайдера
            Vector3 size = collider.size;
            Vector3 center = collider.offset;

            // Преобразуем в мировые координаты
            Vector3 worldCenter = transform.TransformPoint(center);
            Vector3 worldSize = transform.TransformVector(size);

            // Рисуем куб
            Gizmos.DrawWireCube(worldCenter, worldSize);
        }
        else
        {
            Debug.LogWarning("BoxCollider2D не найден на объекте " + gameObject.name);
        }
    }
}
