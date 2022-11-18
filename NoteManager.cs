using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BadApple;

public static class NoteManager
{
    // The max we actually expect to see is 170. Doubling that ensures some
    // notes from the next frame are always rendered, preventing flickering.
    private const int DefaultMaxActiveNotes = 400;
    
    private static Queue<float[]> futureNotes;
    private static Queue<ActiveNote> activeNotes;
    private static Stack<GameObject> inactiveNotes;

    public static void Init(List<float[]> levelData)
    {
        futureNotes = new Queue<float[]>();
        foreach (float[] note in levelData.OrderBy(note => note[0]))
        {
            futureNotes.Enqueue(note);
        }

        activeNotes = new Queue<ActiveNote>();
        inactiveNotes = new Stack<GameObject>();
    }

    public static void Update(GameController __instance, float noteHolderX)
    {
        while (activeNotes.Count > 0 && activeNotes.Peek().DestroyPosition + noteHolderX <= 0)
        {
            GameObject go = activeNotes.Dequeue().GameObject;
            go.SetActive(false);
            inactiveNotes.Push(go);
        }

        // We allow exceeding the default by using TrombLoader configs
        int maxActiveNotes = Math.Max(DefaultMaxActiveNotes, __instance.beatstoshow);
        
        while (activeNotes.Count < maxActiveNotes && futureNotes.Count > 0)
        {
            float[] note = futureNotes.Dequeue();
            BuildNote(__instance, note);
        }
    }

    private record struct ActiveNote(GameObject GameObject, float DestroyPosition);

    private static void BuildNote(GameController __instance, float[] note)
    {
        float noteStartMeasure = note[0];
        float noteLength = note[1];
        float noteStartPitch = note[2];
        float notePitchDelta = note[3];
        float noteEndPitch = note[4];
        float noteEndMeasure = noteStartMeasure + noteLength;
        float noteStartPosition = noteStartMeasure * __instance.defaultnotelength;
        float noteLengthPosition = noteLength * __instance.defaultnotelength;
        float noteEndPosition = noteEndMeasure * __instance.defaultnotelength;

        GameObject noteObject;
        if (inactiveNotes.Count > 0)
        {
            noteObject = inactiveNotes.Pop();
            noteObject.SetActive(true);
        }
        else
        {
            noteObject = UnityEngine.Object.Instantiate(
                __instance.singlenote,
                new Vector3(0, 0, 0),
                Quaternion.identity,
                __instance.noteholder.transform
            );
            __instance.allnotes.Add(noteObject);

            // Set color
            NoteDesigner component1 = noteObject.GetComponent<NoteDesigner>();
            component1.setColorScheme(
                __instance.note_c_start[0], __instance.note_c_start[1], __instance.note_c_start[2],
                __instance.note_c_end[0], __instance.note_c_end[1], __instance.note_c_end[2]
            );

            // Disable start and end circles
            noteObject.transform.GetChild(0).gameObject.SetActive(false);
            noteObject.transform.GetChild(1).gameObject.SetActive(false);
        }

        noteObject.GetComponent<RectTransform>().anchoredPosition3D =
            new Vector3(noteStartPosition, noteStartPitch, 0.0f);

        __instance.allnotevals.Add(new float[]
        {
            noteStartPosition,
            noteEndPosition,
            noteStartPitch,
            notePitchDelta,
            noteEndPitch
        });

        // Draw note with lineRenderer
        LineRenderer[] componentsInChildren = noteObject.GetComponentsInChildren<LineRenderer>();
        foreach (LineRenderer lineRenderer in componentsInChildren)
        {
            if (notePitchDelta == 0)
            {
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
                lineRenderer.SetPosition(1, new Vector3(noteLengthPosition, 0, 0));
            }
            else
            {
                const int lineSegmentCount = 8;
                lineRenderer.positionCount = lineSegmentCount;
                lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
                for (int i = 1; i < lineSegmentCount; i++)
                {
                    float x = noteLengthPosition * i / (lineSegmentCount - 1);
                    float y = __instance.easeInOutVal(i, 0, notePitchDelta, lineSegmentCount - 1);
                    lineRenderer.SetPosition(i, new Vector3(x, y, 0));
                }
            }
        }

        activeNotes.Enqueue(new ActiveNote(noteObject, noteEndPosition + 5));
    }
}
