using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.GameStructure
{
    enum EArmorType
    {
        water,
        fire,
        electricity,
        soil,
        stone,
        flesh,
        bone,//for undied
        plant,
        metal
            
    }

    class ArmorAttribute
    {
        public EArmorType type;
        public float thickness;//meter
        public float coverHP;
        public float maxCoverHP;
        public ArmorAttribute(EArmorType type, float thickness, float maxCoverHP)
        {
            this.thickness = thickness;
            this.type = type;
            this.maxCoverHP = maxCoverHP;
            this.coverHP = maxCoverHP;
        }
        public ArmorAttribute(EArmorType type, float thickness, float maxCoverHP, float coverHP)//human's armor
        {
            this.thickness = thickness;
            this.type = type;
            this.maxCoverHP = maxCoverHP;
            this.coverHP = coverHP;
        }
        public float coverProportion()
        {
            return coverHP / maxCoverHP;
        }
    }
    class ArmorType
    {
        public ArmorAttribute[] attributes;//ordered first is outside armor, last is inside armor 
        public ArmorType(ArmorAttribute[] attributes)
        {
            this.attributes = attributes;
            //normalize();
        }
        public float getTotalHP()
        {
            float totalHP = 0;
            foreach (ArmorAttribute attribute in attributes)
            {
                totalHP += attribute.maxCoverHP;
            }
            return totalHP;
        }
        public float getHP()
        {
            float HP = 0;
            foreach (ArmorAttribute attribute in attributes)
            {
                if (attribute.coverHP<=0)
                {
                    continue;
                }
                HP += attribute.coverHP;
            }
            return HP;
        }
        public float getHPpercentage()
        {
            return getHP() / getTotalHP();
        }

        public void attacked(AttackAttribute attack, int armorIndex = 0)
        {
            if (armorIndex >= attributes.Length)
            {
                return;
            }
            ArmorAttribute armor = attributes[armorIndex];
            if (armor.coverHP <= 0)
            {
                attacked(attack, armorIndex + 1);
            }

            switch (armor.type)
            {
                case EArmorType.water:
                    switch (attack.type)
                    {
                        case EAttackType.hit:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.5f, armor.thickness);//穿过的伤害
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 0.5f;//造成伤害比例
                            }
                            break;
                        case EAttackType.cutting:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.5f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 0.2f;
                            }
                            break;
                        case EAttackType.thorn:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.7f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 0.1f;
                            }
                            break;
                        case EAttackType.cold:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.5f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 0.1f;
                            }
                            break;
                        case EAttackType.heat:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.1f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 1.0f;
                            }
                            break;
                        case EAttackType.electricity:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.9f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= attack.power * 2f;
                            }
                            break;
                        case EAttackType.poison:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.9f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= attack.power * 2.0f;
                            }
                            break;
                    }
                    break;
                case EArmorType.fire:
                    switch (attack.type)
                    {
                        case EAttackType.hit:
                            {
                                attacked(attack, armorIndex + 1);
                            }
                            break;
                        case EAttackType.cutting:
                            {
                                attacked(attack, armorIndex + 1);
                            }
                            break;
                        case EAttackType.thorn:
                            {
                                attacked(attack, armorIndex + 1);
                            }
                            break;
                        case EAttackType.cold:
                            {
                                armor.coverHP -= attack.power;
                            }
                            break;
                        case EAttackType.heat:
                            {
                                attacked(attack, armorIndex + 1);
                                armor.coverHP += attack.power;
                            }
                            break;
                        case EAttackType.electricity:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.9f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= attack.power * 0.1f;
                            }
                            break;
                        case EAttackType.poison:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.9f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= attack.power * 0.1f;
                            }
                            break;
                    }
                    break;
                case EArmorType.electricity:
                    switch (attack.type)
                    {
                        case EAttackType.hit:
                            {
                                attacked(attack, armorIndex + 1);
                                armor.coverHP *= 0.9f;
                            }
                            break;
                        case EAttackType.cutting:
                            {
                                attacked(attack, armorIndex + 1);
                                armor.coverHP *= 0.9f;
                            }
                            break;
                        case EAttackType.thorn:
                            {
                                attacked(attack, armorIndex + 1);
                                armor.coverHP *= 0.9f;
                            }
                            break;
                        case EAttackType.cold:
                            {
                                attacked(attack, armorIndex + 1);
                            }
                            break;
                        case EAttackType.heat:
                            {
                                attacked(attack, armorIndex + 1);
                            }
                            break;
                        case EAttackType.electricity:
                            {
                                armor.coverHP -= attack.power;
                            }
                            break;
                        case EAttackType.poison:
                            {
                                attacked(attack, armorIndex + 1);
                                armor.coverHP -= attack.power * 0.1f;
                            }
                            break;
                    }
                    break;
                case EArmorType.soil:
                    switch (attack.type)
                    {
                        case EAttackType.hit:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.5f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 0.8f;
                            }
                            break;
                        case EAttackType.cutting:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.5f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 0.2f;
                            }
                            break;
                        case EAttackType.thorn:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.7f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 0.1f;
                            }
                            break;
                        case EAttackType.cold:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.5f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 0.5f;
                            }
                            break;
                        case EAttackType.heat:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.5f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 0.5f;
                            }
                            break;
                        case EAttackType.electricity:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.3f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= attack.power * 0.3f;
                            }
                            break;
                        case EAttackType.poison:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.9f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= attack.power * 2.0f;
                            }
                            break;
                    }
                    break;
                case EArmorType.stone:
                    switch (attack.type)
                    {
                        case EAttackType.hit:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.9f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 0.3f;
                            }
                            break;
                        case EAttackType.cutting:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.5f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 0.2f;
                            }
                            break;
                        case EAttackType.thorn:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.7f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 0.1f;
                            }
                            break;
                        case EAttackType.cold:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.5f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 0.1f;
                            }
                            break;
                        case EAttackType.heat:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.1f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 1.0f;
                            }
                            break;
                        case EAttackType.electricity:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.9f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= attack.power * 0.1f;
                            }
                            break;
                        case EAttackType.poison:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.9f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= attack.power * 2.0f;
                            }
                            break;
                    }
                    break;
                case EArmorType.flesh:
                    switch (attack.type)
                    {
                        case EAttackType.hit:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.9f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 0.3f;
                            }
                            break;
                        case EAttackType.cutting:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.5f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 0.2f;
                            }
                            break;
                        case EAttackType.thorn:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.7f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 0.1f;
                            }
                            break;
                        case EAttackType.cold:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.5f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 0.1f;
                            }
                            break;
                        case EAttackType.heat:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.1f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 1.0f;
                            }
                            break;
                        case EAttackType.electricity:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.9f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= attack.power * 0.1f;
                            }
                            break;
                        case EAttackType.poison:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.9f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= attack.power * 2.0f;
                            }
                            break;
                    }
                    break;
                case EArmorType.bone:
                    switch (attack.type)
                    {
                        case EAttackType.hit:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.9f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 0.3f;
                            }
                            break;
                        case EAttackType.cutting:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.5f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 0.2f;
                            }
                            break;
                        case EAttackType.thorn:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.7f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 0.1f;
                            }
                            break;
                        case EAttackType.cold:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.5f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 0.1f;
                            }
                            break;
                        case EAttackType.heat:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.1f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 1.0f;
                            }
                            break;
                        case EAttackType.electricity:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.9f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= attack.power * 0.1f;
                            }
                            break;
                        case EAttackType.poison:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.9f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= attack.power * 2.0f;
                            }
                            break;
                    }
                    break;
                case EArmorType.plant:
                    switch (attack.type)
                    {
                        case EAttackType.hit:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.9f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 0.3f;
                            }
                            break;
                        case EAttackType.cutting:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.5f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 0.2f;
                            }
                            break;
                        case EAttackType.thorn:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.7f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 0.1f;
                            }
                            break;
                        case EAttackType.cold:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.5f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 0.1f;
                            }
                            break;
                        case EAttackType.heat:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.1f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= (attack.power - powerLeft) * 1.0f;
                            }
                            break;
                        case EAttackType.electricity:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.9f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= attack.power * 0.1f;
                            }
                            break;
                        case EAttackType.poison:
                            {
                                float powerLeft = attack.power * (float)Math.Pow(0.9f, armor.thickness);
                                attacked(new AttackAttribute(attack.type, powerLeft), armorIndex + 1);
                                armor.coverHP -= attack.power * 2.0f;
                            }
                            break;
                    }
                    break;
            }
        }
        public void attacked(AttackType attack)
        {
            foreach (AttackAttribute attribute in attack.attributes)
            {
                attacked(attribute);
            }
        }

        public void attackedTemp(AttackAttribute attack, int armorIndex = 0)
        {
            if (armorIndex >= attributes.Length)
            {
                return;
            }
            ArmorAttribute armor = attributes[armorIndex];
            if (armor.coverHP <= 0)
            {
                attackedTemp(attack, armorIndex + 1);
            }
            armor.coverHP -= attack.power;
        }

    }

    enum EAttackType
    {
        //fluid,//water and air
        hit,//撞击
        cutting,//切割
        thorn,//刺
        cold,
        heat,
        electricity,
        poison
        //light,
        //dark
    }
    struct AttackAttribute
    {
        public EAttackType type;
        public float power;
        public AttackAttribute(EAttackType type, float power)
        {
            this.type = type;
            this.power = power;
            //normalize();
        }
    }
    class AttackType
    {
        public AttackAttribute[] attributes;
        public AttackType(AttackAttribute[] attributes)
        {
            this.attributes = attributes;
            //normalize();
        }
        /*
        public void normalize()
        {
            float totalProportion = 0;
            foreach (AttackAttribute attribute in attributes)
            {
                totalProportion += attribute.proportion;
            }
            if (totalProportion == 1.0f)
                return;
            for (int i = 0; i < attributes.Length; ++i)
            {
                attributes[i].proportion /= totalProportion;
            }
        }/**/
    }

    /*
    
        physical,
        ice,
        fire,
        electricity,
        poison,
        light,
        dark,
    /**/


    /*static float damageRate(ArmorType armor, AttackType attack)
    {
        foreach(AttackAttribute attackAttribute in attack.attributes)
        {

        }
    }*/

    /*class Race
    {

    }

    class GameStructure
    {
    }*/
}
