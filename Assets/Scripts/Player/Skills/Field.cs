using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
 
  private float damage;

[SerializeField]
  private float size;

   public void Init(float damage, float size)
    {
        this.damage = damage;
        this.size = size;
    }

}
