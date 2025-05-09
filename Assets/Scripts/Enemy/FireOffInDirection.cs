using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireOffInDirection : MonoBehaviour
{
    [SerializeField] float MoveSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        float randX = Random.Range(-1f, 1f);
        float randY = Random.Range(-1f, 1f);

        GetComponent<Rigidbody2D>().velocity = new Vector2(randX, randY).normalized * MoveSpeed;
        //GetComponent<R>
    }
}
