using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodAreaGizmosDraw : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.yellow;

        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, transform.localScale.x / 2.0f);
    }
}
