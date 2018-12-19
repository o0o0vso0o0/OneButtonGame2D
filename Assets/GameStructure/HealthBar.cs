using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.GameStructure
{
    class HealthBar
    {
        public GameObject bar;
        public GameObject healthbar;

        public HealthBar(GameObject parentObject, float length, float HPpercentage = 1.0f)
        {
            bar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bar.transform.localScale = new Vector3(length, 0.3f, 0.1f);

            healthbar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            healthbar.GetComponent<Renderer>().material.color =
                            new Color(0,1,0);
            healthbar.transform.parent = bar.transform;
            setHP(HPpercentage);

            bar.transform.parent = parentObject.transform;
        }
        public void setHP(float HPpercentage)
        {
            healthbar.transform.localScale = new Vector3(0.9f * HPpercentage,0.8f, 1.1f);
            healthbar.transform.localPosition = new Vector3((HPpercentage - 1f) / 2 * 0.9f, 0, 0);
        }
    }
}
