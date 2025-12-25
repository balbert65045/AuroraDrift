using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public Action OnTakeDamage {  get; set; }
    public void TakeDamge(GameObject fromWhat, float damage, Vector2 force, DamageType damageType) {}
    public void Stunned() { }
    public void UnStunn() { }
}
