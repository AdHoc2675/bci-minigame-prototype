using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatAreaGizmosDraw : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.red;

        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, transform.localScale.x / 2.0f);
    }
}
