using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillField : Skill
{
    private Field field;
    private GameObject FieldPrefab;
    private const float BASE_DAMAGE = 1f;
    private float damage;
    private bool hasField;

    public SkillField()
    {
        name = "Field";
        FieldPrefab = Resources.Load<GameObject>("Field");

        level = 1;
        damage = BASE_DAMAGE;

        hasField = false;
    }

    public override void Upgrade()
    {
        base.Upgrade();

        if (level == 2)
        {
            damage = BASE_DAMAGE * 2f;
            field.SetScale(1.2f);
        }

        if (level == 3)
        {
            damage = BASE_DAMAGE * 3f;
            field.SetScale(1.4f);
        }

        if (level == 4)
        {
            damage = BASE_DAMAGE * 4f;
            field.SetScale(1.6f);
        }
    }

    public override void Use()
    {
        //Instantiate Forcefield here, but only if there isn't one already.
        if (!hasField)
        {
            Vector2 playerPos = GameManager.PlayerMovement().transform.position;
            field = GameObject
                .Instantiate(FieldPrefab, playerPos, Quaternion.identity)
                .GetComponent<Field>();
            hasField = true;
            field.transform.parent = GameManager.PlayerMovement().transform;
        }
    }
}
