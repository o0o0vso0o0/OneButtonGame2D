using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesRecord : MonoBehaviour {

    GameObject notesRecordPanel;
    ArrayList notes = new ArrayList();
    int frameCountSinceAdd = 0;

	// Use this for initialization
	void Start () {
        notesRecordPanel = new GameObject("notesRecordPanel");
        notesRecordPanel.transform.localPosition = new Vector3(-7, -2, 0);
    }
	
	// Update is called once per frame
	void Update () {
        if (frameCountSinceAdd>150)
        {
            summon();
            frameCountSinceAdd = 0;
        }

        frameCountSinceAdd++;
    }

    public void addNote(SummonRing.Note note)
    {
        note.noteObject.transform.parent = notesRecordPanel.transform;
        note.noteObject.transform.localPosition = new Vector3(notes.Count/3.0f, 0, 0);
        note.noteObject.transform.localRotation = new Quaternion(0,0,0,0);
        notes.Add(note);

        frameCountSinceAdd = 0;
    }
    public void summon()
    {
        if (notes.Count == 0)
            return;

        if (notes.Count>=2)
        {
            gameObject.GetComponent<MonstersManager>().summon(((SummonRing.Note)notes[0]).type,  - 0.3f + notes.Count * 0.3f);
        }
        foreach (SummonRing.Note i in notes)
        {
            Destroy(i.noteObject);
        }
        notes.Clear();
    }
}
