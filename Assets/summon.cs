using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class summon : MonoBehaviour {
    class summonNote
    {

    }



    static float frameTime = 0.016f;

    GameObject rhythmPanel;
    GameObject pointer;
    ArrayList notes = new ArrayList();
    GameObject noteShot = null;
    float timeSinceKeyDown = 1;
    float timeSinceLastNote = 1;

    // Use this for initialization
    void Start () {

        rhythmPanel = new GameObject("rhythmPanel");
        rhythmPanel.transform.localPosition = new Vector3(-7, -1, 0);

        pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pointer.transform.parent = rhythmPanel.transform;
        pointer.transform.localScale = new Vector3(0.2f, 1, 0.2f);
        pointer.transform.localPosition = new Vector3(0, 0, 0);
        pointer.transform.Translate(new Vector3(0, 0, 0.1f));
        //rhythmPanel.transform.parent = GameObject.tra;
    }
	

	// Update is called once per frame
	void Update () {
        //指针变黑
        if (Input.GetKeyDown(KeyCode.Space)
            || Input.GetKeyDown(KeyCode.Mouse0))
        {
            timeSinceKeyDown = 0;

            //创建noteShot 
            if (noteShot != null)
            {
                Destroy(noteShot);
                noteShot = null;
            }
            foreach (GameObject note in notes)
            {
                if (noteShot == null
                    || System.Math.Abs(noteShot.transform.localPosition.x)
                        > System.Math.Abs(note.transform.localPosition.x))
                    noteShot = note;
            }
            notes.Remove(noteShot);
        }
        if (timeSinceKeyDown<1)
            timeSinceKeyDown += frameTime / 0.5f;//0.5 变黑时间
        else
            timeSinceKeyDown = 1;
        pointer.GetComponent<Renderer>().material.color =
            new Color(timeSinceKeyDown, timeSinceKeyDown, timeSinceKeyDown, 1);

        //noteShot移动
        if (noteShot!=null)
        {
            noteShot.transform.Translate(new Vector3(0, frameTime, 0));
            if (noteShot.transform.localPosition.y > 0.5f)
            {
                Destroy(noteShot);
                noteShot = null;
            }
            /*Material noteShotMaterial = noteShot.GetComponent<Renderer>().material;
            noteShotMaterial.color =
                new Color(noteShotMaterial.color.r, noteShotMaterial.color.g, noteShotMaterial.color.b, 0);/**///改透明度需要贴图
        }

        //创建召唤音符
        if (timeSinceLastNote<1)
            timeSinceLastNote += frameTime / 1f;//1秒一个召唤音符
        else
        {
            timeSinceLastNote = 0;
            GameObject newNote = GameObject.CreatePrimitive(PrimitiveType.Cube);
            newNote.transform.localScale = new Vector3(0.2f, 0.5f, 0.2f);
            newNote.transform.parent = rhythmPanel.transform;
            newNote.transform.localPosition = new Vector3(2, 0, 0);
            notes.Add(newNote);
        }

        //召唤音符移动并消失
        ArrayList destroyList = new ArrayList();
        foreach (GameObject note in notes)
        {
            note.transform.Translate(new Vector3(-frameTime*2, 0, 0));
            if (note.transform.localPosition.x < -1)
            {
                destroyList.Add(note);
            }
        }
        foreach (GameObject note in destroyList)
        {
            notes.Remove(note);
            Destroy(note);
        }/**/

    }
}
