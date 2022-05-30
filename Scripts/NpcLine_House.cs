using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcLine_House : MonoBehaviour
{
    public float lineOfSite;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(gameObject.transform.position, lineOfSite);
    }
}
