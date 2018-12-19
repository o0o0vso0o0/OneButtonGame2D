using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.GameStructure;

public class MonstersManager : MonoBehaviour {
    static float frameTime ;


    Team friendlyTeam = new Team();
    Team enemyTeam = new Team();

    // Use this for initialization
    void Start ()
    {
        frameTime = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
    }

    int frameSinceLastSummonEnemy = 1000;
    void FixedUpdate()
    {
        if (frameSinceLastSummonEnemy> 1000)
        {
            frameSinceLastSummonEnemy = 0;
            summon((UnitID)Random.Range(1,4),5,true);
        }
        frameSinceLastSummonEnemy++;


        //Unit closestFriendlyUnit = friendlyTeam.getEastestUnit();
        //Unit closestEnemyUnit = enemyTeam.getEastestUnit();

        for (int i = 0; i < friendlyTeam.units.Count; ++i)
        {
            Unit friendlyUnit = (Unit)friendlyTeam.units[i];
            ArrayList enemyInRange = enemyTeam.getUnitsInRange(friendlyUnit, friendlyUnit.attackRange);
            if (enemyInRange.Count == 0)
                friendlyUnit.move();
            else foreach (Unit enemy in enemyInRange)
                {
                    if (!friendlyUnit.attack(enemy))
                    {
                        UnityEngine.Object.Destroy(enemy.gameObject);
                        enemyTeam.units.Remove(enemy);
                    }
                }
        }
        for (int i = 0; i < enemyTeam.units.Count; ++i)
        {
            Unit enemyUnit = (Unit)enemyTeam.units[i];
            ArrayList friendlyUnitInRange = friendlyTeam.getUnitsInRange(enemyUnit, enemyUnit.attackRange);
            if (friendlyUnitInRange.Count == 0)
                enemyUnit.move(true);
            else
                foreach (Unit friendlyUnit in friendlyUnitInRange)
                {
                    if (!enemyUnit.attack(friendlyUnit))
                    {
                        UnityEngine.Object.Destroy(friendlyUnit.gameObject);
                        friendlyTeam.units.Remove(friendlyUnit);
                    }
                }
        }
    }

    public void summon(UnitID id, float power, bool enemy = false)
    {
        if (enemy)
        {
            Unit newUnit = new Unit(id, power);
            newUnit.gameObject.transform.parent = gameObject.transform;
            newUnit.gameObject.transform.localPosition = new Vector3(16, 0, 0);
            enemyTeam.units.Add(newUnit);
        }
        else
        {
            Unit newUnit = new Unit(id, power);
            newUnit.gameObject.transform.parent = gameObject.transform;
            newUnit.gameObject.transform.localPosition = new Vector3(0, 0, 0);
            friendlyTeam.units.Add(newUnit);
        }
    }
}
