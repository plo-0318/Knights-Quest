using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillField : Skill
{
    private Field field;
    private GameObject FieldPrefab;
    private readonly float BASE_DAMAGE;
    private readonly float BASE_COOLDOWN_TIME;
    private float damage;
    private bool hasField;

    public SkillField()
    {
        name = "field";
        FieldPrefab = Resources.Load<GameObject>("field");

        BASE_DAMAGE = GameManager.GetSkillData(name).Damage;
        BASE_COOLDOWN_TIME = GameManager.GetSkillData(name).Cooldown;

        level = 1;
        damage = BASE_DAMAGE;

        hasField = false;

        type = Type.ATTACK;
    }

    protected override void OnLevelUp()
    {
        if (level == 2)
        {
            damage = BASE_DAMAGE * 1.25f;
            field.SetDamage(damage);
            field.SetScale(1.25f);
        }

        if (level == 3)
        {
            damage = BASE_DAMAGE * 1.5f;
            field.SetDamage(damage);
            field.SetScale(1.5f);
        }

        if (level == 4)
        {
            damage = BASE_DAMAGE * 1.75f;
            field.SetDamage(damage);
            field.SetScale(1.75f);
        }

        if (level == 5)
        {
            damage = BASE_DAMAGE * 2f;
            field.SetDamage(damage);
            field.SetScale(2f);
            field.SetSlowEnemy(true);
        }
    }

    public override void Use()
    {
        //Instantiate Forcefield here, but only if there isn't one already.
        if (!hasField)
        {
            Transform player = GameManager.PlayerMovement().transform;

            field = GameObject
                .Instantiate(FieldPrefab, player.position, Quaternion.identity, player)
                .GetComponent<Field>();

            field.Init(damage, BASE_COOLDOWN_TIME);

            hasField = true;
        }
    }
}
