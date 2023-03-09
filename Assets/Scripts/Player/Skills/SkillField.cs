using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillField : Skill
{
    private GameObject Field;
    [SerializeField] public GameObject Player;
    private const float BASE_DAMAGE = 1f;
    private  Vector2 BASE_SIZE = new Vector2(4f,4f);
    private float damage;
    private Vector2 size;
    

    public SkillField()
    {
        name = "Field";
        Field = Resources.Load<GameObject>("field");

        level = 1;
        damage = BASE_DAMAGE;

        size = BASE_SIZE;

    }


    public override void Upgrade()
    {
        base.Upgrade();

        if(level == 2)
        {
            damage = BASE_DAMAGE * 2f;
            size = BASE_SIZE * 1.25f;
        }

        if(level == 3)
        {
            damage = BASE_DAMAGE * 3f;
           size = BASE_SIZE * 2f;
        }

        if(level == 4)
        {
            damage = BASE_DAMAGE * 4f;
            size = BASE_SIZE * 3f;
    }
}
private void OnTriggerEnter2D(Collider2D other)
{
    if(other.CompareTag("Player"))
    {
        other.transform.parent = Player.transform;
    }
}
}
