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
            // ������������� ���� ��� Gizmos
            Gizmos.color = Color.red;

            // �������� ������ � ������� ����������
            Vector3 size = collider.size;
            Vector3 center = collider.offset;

            // ����������� � ������� ����������
            Vector3 worldCenter = transform.TransformPoint(center);
            Vector3 worldSize = transform.TransformVector(size);

            // ������ ���
            Gizmos.DrawWireCube(worldCenter, worldSize);
        }
        else
        {
            Debug.LogWarning("BoxCollider2D �� ������ �� ������� " + gameObject.name);
        }
    }
}
