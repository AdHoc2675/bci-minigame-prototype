using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoorAreaGizmosDraw : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.green;

        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, transform.localScale.x / 2.0f);
    }
}
