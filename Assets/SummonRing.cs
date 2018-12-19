using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.GameStructure;

public class SummonRing : MonoBehaviour {
    public class Note
    {
        public class NoteShape
        {
            public static GameObject element()
            {
                GameObject noteObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                /*noteObject.transform.localRotation = new Quaternion((float)Math.Sin(Math.PI / 4), (float)Math.Sin(Math.PI / 4), 0,
                    (float)Math.Cos(Math.PI / 4));/**/
                noteObject.transform.localScale = new Vector3(0.2f, 0.5f, 0.2f);
                return noteObject;
            }
        }

        public GameObject noteObject;
        public UnitID type;

        public Note()
        {

        }
        public Note(UnitID type)
        {
            this.type = type;
            switch (type)
            {
                case UnitID.waterElement:
                    noteObject = NoteShape.element();
                    noteObject.GetComponent<Renderer>().material.color =
                            new Color(0.5f, 0.9f, 1f);
                    break;
                case UnitID.electricityElement:
                    noteObject = NoteShape.element();
                    noteObject.GetComponent<Renderer>().material.color =
                            new Color(0.9f, 0.9f, 1f);
                    break;
                case UnitID.soilElement:
                    noteObject = NoteShape.element();
                    noteObject.GetComponent<Renderer>().material.color =
                            new Color(0.9f, 0.7f, 0.4f);
                    break;
                default:
                    break;
            }
        }
        public Note(UnitID type, GameObject noteObject)
        {
            this.type = type;
            this.noteObject = noteObject;
        }

        public Note copy()
        {
            Note note = new Note();
            note.type = type;
            note.noteObject = GameObject.Instantiate(noteObject);
            return note;
        }
    }

    class SummonNotes
    {
        public GameObject notesNode;
        ArrayList notes = new ArrayList();
        public double rotate = 0;//radian
        public SummonNotes(GameObject parentNote, UnitID[] noteTypes, float radius = 2)
        {
            notesNode = new GameObject();
            notesNode.transform.parent = parentNote.transform;
            notesNode.transform.localPosition = new Vector3(0, 0, 0);
            /*notesNode.transform.localRotation = new Quaternion(0, (float)Math.Sin((double)i / noteNumber * Math.PI), 0,
                (float)Math.Cos((double)i / noteNumber * Math.PI));/**/
            for (int i = 0; i < noteTypes.Length; ++i)
            {
                Note note = new Note(noteTypes[i]);
                note.noteObject.transform.parent = notesNode.transform;

                o0.Vector3 v = new o0.Vector3(0, 0, -2);
                v.selfRotate(new o0.Vector3(0, -1, 0), (double)i / noteTypes.Length * Math.PI * 2);

                note.noteObject.transform.localPosition = new Vector3((float)v.x, (float)v.y, (float)v.z);
                note.noteObject.transform.localRotation = new Quaternion(0, (float)Math.Sin((double)i / noteTypes.Length * Math.PI), 0,
                    (float)Math.Cos((double)i / noteTypes.Length * Math.PI));/**/
                notes.Add(note);
            }
        }
        /*public SummonNotes(GameObject parentNote, float radius = 2, int noteNumber = 8)
        {
        }*/
        public void update(double rotateAngle)
        {
            rotate += rotateAngle;
            notesNode.transform.localRotation = new Quaternion(0, (float)Math.Sin(rotate / 2), 0,
                (float)Math.Cos(rotate/2));/**/
        }
        public Note getClosestNote(Vector3 targetPosition)
        {
            o0.Vector3 o0targetPosition = new o0.Vector3(targetPosition.x, targetPosition.y, targetPosition.z);

            Note closestNote = null;
            double closestDistance = 0;
            foreach (Note note in notes)
            {
                Vector3 notePosition = note.noteObject.transform.localPosition;
                o0.Vector3 v = new o0.Vector3(notePosition.x, notePosition.y, notePosition.z);
                v.selfRotate(new o0.Vector3(0, -1, 0), rotate);
                double distance = (o0targetPosition - v).length();
                if (closestNote == null||
                    closestDistance > distance)
                {
                    closestNote = note;
                    closestDistance = distance;
                }
            }
            return closestNote;
        }/**/
    }

    static float frameTime;

    GameObject rhythmPanel;
    GameObject pointer;
    SummonNotes notes = null;
    float notesRadius = 2;
    double timePerRound = 10; //notes rotate time every round
    GameObject noteShot = null;
    float timeSinceKeyDown = 1;
    //float timeSinceLastNote = 1;

    // Use this for initialization
    void Start()
    {
        frameTime = Time.fixedDeltaTime;

        rhythmPanel = new GameObject("rhythmPanel");
        rhythmPanel.transform.localPosition = new Vector3(-6, -1, 0);

        pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pointer.transform.parent = rhythmPanel.transform;
        pointer.transform.localScale = new Vector3(0.2f, 1, 0.2f);
        pointer.transform.localPosition = new Vector3(0, 0, 0);
        pointer.transform.Translate(new Vector3(0, 0, 0.1f));
        //rhythmPanel.transform.parent = GameObject.tra;

        UnitID[] types = { UnitID.soilElement, UnitID.soilElement,UnitID.soilElement,
            UnitID.waterElement,UnitID.waterElement,UnitID.waterElement,
            UnitID.electricityElement,UnitID.electricityElement,UnitID.electricityElement };
        //创建召唤音符
        notes = new SummonNotes(rhythmPanel, types, notesRadius);
        notes.notesNode.transform.localPosition = new Vector3(0, 0, notesRadius);
    }



    // Update is called once per frame
    void Update()
    {
        //指针变黑
        if (timeSinceKeyDown>=1 
            && (Input.GetKeyDown(KeyCode.Space)
            || Input.GetKeyDown(KeyCode.Mouse0)))
        {
            timeSinceKeyDown = 0;

            //创建noteShot 
            if (noteShot != null)
            {
                Destroy(noteShot);
                noteShot = null;
            }
            Note closestNote = notes.getClosestNote(new Vector3(0, 0, -notesRadius));
            noteShot = GameObject.Instantiate(closestNote.noteObject);
            noteShot.transform.parent = rhythmPanel.transform;
            noteShot.transform.localPosition = closestNote.noteObject.transform.position - rhythmPanel.transform.position;
            noteShot.transform.Rotate(new Vector3(0, 1, 0), (float)(notes.rotate / Math.PI * 180));

            Vector3 distance = noteShot.transform.localPosition - new Vector3(0, 0, -notesRadius);
            if (Math.Pow(distance.x, 2) + Math.Pow(distance.x, 2) + Math.Pow(distance.x, 2) < 0.1)
            {
                gameObject.GetComponent<NotesRecord>().addNote(new Note(closestNote.type,GameObject.Instantiate(noteShot)));
            }
            else
            {
                Destroy(noteShot);
                noteShot = null;
                gameObject.GetComponent<NotesRecord>().summon();
            }
        }
        if (timeSinceKeyDown < 1)
            timeSinceKeyDown += frameTime / 0.5f;//0.5 变黑时间
        else
            timeSinceKeyDown = 1;
        pointer.GetComponent<Renderer>().material.color =
            new Color(timeSinceKeyDown, timeSinceKeyDown, timeSinceKeyDown, 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(Time.fixedDeltaTime);//物理帧时间



        //noteShot移动
        if (noteShot != null)
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

        notes.update(Math.PI*2* frameTime / timePerRound);
        
    }
}
