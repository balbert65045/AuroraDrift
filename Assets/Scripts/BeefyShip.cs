using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeefyShip : Ship
{
    [SerializeField] GameObject MisselPrefab;
    protected override void Attack()
    {
        base.Attack();
        GameObject missel1 = Instantiate(MisselPrefab, transform.position, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90));
        GameObject missel2 = Instantiate(MisselPrefab, transform.position + transform.right * 2f, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z));
        GameObject missel3 = Instantiate(MisselPrefab, transform.position - transform.right * 2f, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 180));
        missel1.GetComponent<Missel>().SetCreator(this.gameObject);
        missel2.GetComponent<Missel>().SetCreator(this.gameObject);
        missel3.GetComponent<Missel>().SetCreator(this.gameObject);
    }
}
