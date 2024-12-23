using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEditor;

public class SavedTime
{
    public DateTime today;
    public string _job, _time;

    public SavedTime(string time, string job)
    {
        today = DateTime.Now;
        _time = time;
        _job = job;
    }
}


public class savingScript : MonoBehaviour
{

    public TimerScript ts;
    public List<SavedTime> savedWorkHours = new List<SavedTime>();
    public void Saving(string time, string job)
    {
        savedWorkHours.Add(new SavedTime(time, job));
        StreamWriter ki = new StreamWriter(Application.persistentDataPath + "/out.txt", true);

        Debug.Log("\n[" + savedWorkHours[savedWorkHours.Count - 1].today.ToString("MM/dd/yyyy HH:mm") + "]\t" + savedWorkHours[savedWorkHours.Count - 1]._job + ", " + savedWorkHours[savedWorkHours.Count - 1]._time);
        ki.WriteLine(savedWorkHours[savedWorkHours.Count - 1].today.ToString("MM/dd/yyyy HH:mm") + "#" + savedWorkHours[savedWorkHours.Count - 1]._job + "#" + savedWorkHours[savedWorkHours.Count - 1]._time);
        ki.Close();

    }

    public void Loading()
    {
        StreamReader be = new StreamReader(Application.persistentDataPath + "/out.txt");
        try
        {
            do
            {
                string funnyText = be.ReadLine();
                string help = ("\n[" + funnyText.Split('#')[0] + "]\n" + funnyText.Split('#')[1] + " -> " + funnyText.Split('#')[2]);
                ts.LoadWrite(help);
            } while (!be.EndOfStream);
        }
        catch
        {
            ts.LoadWrite("No saves yet");
        }

        be.Close();
    }

    public void ClearSaves()
    {
        StreamWriter torol = new StreamWriter(Application.persistentDataPath + "/out.txt", false);
        torol.Write("");
        torol.Close();
    }
}