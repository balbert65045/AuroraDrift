using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleShip : Ship
{
    [SerializeField] GameObject MisselPrefab;
    protected override void Attack()
    {
        base.Attack();
        GameObject missel = Instantiate(MisselPrefab, transform.position, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90));
        missel.GetComponent<Missel>().SetCreator(this.gameObject);
    }
}
