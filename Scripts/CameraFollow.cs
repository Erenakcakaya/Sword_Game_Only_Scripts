using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float speed = 2f;
    public float yPos = 1f;
    public float xPos = 1f;
    public Vector3 boundsMax;
    public Vector3 boundsMin;
    public Transform target;

    private void Start()
    {
        speed = 5;
        yPos = 2.4f;
    }
    void Update()
    {

        //Kamerayı belli alanlar arasında tutup oyuncuyu takip ettirir.
        Vector3 pos = new Vector3(Mathf.Clamp(target.position.x, boundsMin.x, boundsMax.x) + xPos, Mathf.Clamp(target.position.y, boundsMin.y, boundsMax.y) + yPos, -10f);
        transform.position = Vector3.Slerp(transform.position, pos, speed * Time.deltaTime);

    }
}
