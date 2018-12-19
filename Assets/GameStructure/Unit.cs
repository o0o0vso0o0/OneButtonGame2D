using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.GameStructure
{
    public enum UnitID
    {
        empty,
        waterElement,//1
        electricityElement,//2
        soilElement,//3
        fireElement//4
    }

    class Unit
    {
        UnitID id;
        public ArmorType body;
        public float bodyWidth;
        public AttackType skill;
        public GameObject gameObject;
        public HealthBar healthBar;
        public float movementSpeed;//per second
        public float attackRange;
        public float attackSpeed;//per second
        public float timeSinceLastAttack = 0;//second
        public float standDistance;//distance stand between enemy and the unit //not used

        public bool attack(Unit unit)
        {
            if (timeSinceLastAttack>=1/ attackSpeed)
            {
                timeSinceLastAttack = 0;
                unit.body.attacked(skill);
                float HPpercentage = unit.body.getHPpercentage();
                unit.healthBar.setHP(HPpercentage);
                return HPpercentage > 0;
            }

            timeSinceLastAttack += Time.fixedDeltaTime;
            return true;//enemy not killed
        }

        public Unit(ArmorType body, AttackType attack, GameObject gameObject)
        {
        }
        public Unit(UnitID id, float power)
        {
            this.id = id;
            float powAtSize = (float)Math.Pow(power, 1.0f / 3.0f);
            switch (id)
            {
                default:
                    {
                        ArmorAttribute[] armorAtt = { new ArmorAttribute(EArmorType.flesh, powAtSize, 100 * powAtSize) };
                        body = new ArmorType(armorAtt);
                        AttackAttribute[] attackAtt = { new AttackAttribute(EAttackType.hit, 10 * powAtSize) };
                        skill = new AttackType(attackAtt);

                        gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        gameObject.transform.localScale = new Vector3(powAtSize * 0.75f, powAtSize, powAtSize);
                        gameObject.GetComponent<Renderer>().material.color =
                            new Color(1, 1, 1);
                        movementSpeed = 0.5f / powAtSize;
                        attackRange = 0.5f * powAtSize;
                        standDistance = attackRange * 0.9f;
                        bodyWidth = powAtSize/2;
                        attackSpeed = 2f / powAtSize;
                    }
                    break;
                case UnitID.waterElement:
                    {
                        ArmorAttribute[] armorAtt = { new ArmorAttribute(EArmorType.water, powAtSize, 100 * powAtSize) };
                        body = new ArmorType(armorAtt);
                        AttackAttribute[] attackAtt = { new AttackAttribute(EAttackType.cold, 10 * powAtSize),
                                new AttackAttribute(EAttackType.hit, 10 * powAtSize)};
                        skill = new AttackType(attackAtt);

                        gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        gameObject.transform.localScale = new Vector3(powAtSize * 0.75f, powAtSize, powAtSize);
                        gameObject.GetComponent<Renderer>().material.color =
                            new Color(0.5f, 0.9f, 1f);
                        movementSpeed = 0.5f / powAtSize;
                        attackRange = 2.0f * powAtSize;
                        standDistance = attackRange * 0.9f;
                        bodyWidth = powAtSize / 2;
                        attackSpeed = 1f / powAtSize;
                    }
                    break;
                case UnitID.electricityElement:
                    {
                        ArmorAttribute[] armorAtt = { new ArmorAttribute(EArmorType.electricity, powAtSize, 100 * powAtSize) };
                        body = new ArmorType(armorAtt);
                        AttackAttribute[] attackAtt = { new AttackAttribute(EAttackType.electricity, 10 * powAtSize) };
                        skill = new AttackType(attackAtt);

                        gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        gameObject.transform.localScale = new Vector3(powAtSize * 0.75f, powAtSize, powAtSize);
                        gameObject.GetComponent<Renderer>().material.color =
                            new Color(0.9f, 0.9f, 1f);
                        movementSpeed = 0.8f / powAtSize;
                        attackRange = 0.8f * powAtSize;
                        standDistance = attackRange * 0.9f;
                        bodyWidth = powAtSize / 2;
                        attackSpeed = 2f / powAtSize;
                    }
                    break;
                case UnitID.soilElement:
                    {
                        ArmorAttribute[] armorAtt = { new ArmorAttribute(EArmorType.soil, powAtSize, 100 * powAtSize) };
                        body = new ArmorType(armorAtt);
                        AttackAttribute[] attackAtt = { new AttackAttribute(EAttackType.hit, 10 * powAtSize) };
                        skill = new AttackType(attackAtt);

                        gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        gameObject.transform.localScale = new Vector3(powAtSize * 0.75f, powAtSize, powAtSize);
                        gameObject.GetComponent<Renderer>().material.color =
                            new Color(0.9f, 0.7f, 0.4f);
                        movementSpeed = 0.3f / powAtSize;
                        attackRange = 0.5f * powAtSize;
                        standDistance = attackRange * 0.9f;
                        bodyWidth = powAtSize / 2;
                        attackSpeed = 0.5f / powAtSize;
                    }
                    break;
            }
            healthBar = new HealthBar(gameObject, gameObject.transform.localScale.x);
            healthBar.bar.transform.localPosition = new Vector3(0, 1, 0);
        }

        public void move(bool enemy = false)
        {
            if (enemy)
                gameObject.transform.Translate(new Vector3(-movementSpeed * Time.fixedDeltaTime, 0, 0));
            else
                gameObject.transform.Translate(new Vector3(movementSpeed * Time.fixedDeltaTime, 0, 0));

            timeSinceLastAttack += Time.fixedDeltaTime;
        }
        public float distance(Unit unit)
        {
            return Math.Abs(gameObject.transform.position.x - unit.gameObject.transform.position.x) - bodyWidth - unit.bodyWidth;
        }
    }

    class Team
    {
        public ArrayList units = new ArrayList();

        public Team()
        {

        }

        /*public void update(float time)
        {
            foreach (Unit unit in units)
            {
                unit.gameObject.transform.Translate(new Vector3(time, 0,0));
            }
        }*/
        /*
        public Unit getEastestUnit()
        {
            Unit unit = null;
            for (int i = 0; i < units.Count; i++)
            {
                if (unit == null
                    || unit.gameObject.transform.position.x > ((Unit)units[i]).gameObject.transform.position.x)
                    unit = (Unit)units[i];
            }
            return unit;
        }
        public Unit getWestestUnit()
        {
            Unit unit = null;
            for (int i = 0; i < units.Count; i++)
            {
                if (unit == null
                    || unit.gameObject.transform.position.x < ((Unit)units[i]).gameObject.transform.position.x)
                    unit = (Unit)units[i];
            }
            return unit;
        }*/
        /*public Unit getClosestUnit(Unit unit)
        {
            Unit closestUnit = null;
            float shortestDistance = 0f;
            for (int i = 0; i < units.Count; i++)
            {
                float distance = ((Unit)units[i]).distance(unit);
                if (closestUnit == null
                    || shortestDistance > distance)
                {
                    closestUnit = (Unit)units[i];
                    shortestDistance = distance;
                }
            }
            return closestUnit;
        }/**/
        public Unit getClosestUnitInRange(Unit unit, float range)
        {
            Unit closestUnit = null;
            float shortestDistance = 0f;
            for (int i = 0; i < units.Count; i++)
            {
                float distance = ((Unit)units[i]).distance(unit);
                if (closestUnit == null
                    || shortestDistance > distance)
                {
                    closestUnit = (Unit)units[i];
                    shortestDistance = distance;
                }
            }
            if (shortestDistance > range)
            {
                return null;
            }
            return closestUnit;
        }
        public ArrayList getUnitsInRange(Unit unit, float range)
        {
            ArrayList unitsInRange = new ArrayList();
            for (int i = 0; i < units.Count; i++)
            {
                float distance = ((Unit)units[i]).distance(unit);
                if (range > distance)
                {
                    unitsInRange.Add(units[i]);
                }
            }
            return unitsInRange;
        }
    }


}
