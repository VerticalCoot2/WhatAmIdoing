using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class TimerScript : MonoBehaviour
{

    [SerializeField] float faszom = 1f;

    //buttons
    [Header("\nButtons")]
    [SerializeField] Button startBtn;
    [SerializeField] Button stopBtn;
    [SerializeField] Button saveButton;
    [SerializeField] Button savedButtonButton;
    [SerializeField] Button deleteButtonButton;

    //text
    [Header("\nText")]
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text jobText;
    [SerializeField] TMP_Text uHaveSMTG;
    [SerializeField] TMP_InputField inf;

    //time
    //[Header("\nTime")]
    int[] time = { 0, 0, 0 };
    string finalTime;

    //gameObjects
    [Header("\nGameObjects")]
    [SerializeField] GameObject popupPanel;
    [SerializeField] GameObject savedThingsPanel;
    [SerializeField] GameObject saveTextPrefab;
    [SerializeField] GameObject deletePanelGO;

    [SerializeField] GameObject saveShitButton;
    [SerializeField] GameObject deleteSaveShitButton;

    //save
    [SerializeField] GameObject saveTextParent;

    //Animations
    [Header("\nAnimations")]
    [SerializeField] Animator savePanel;
    [SerializeField] Animator savedPanel;
    [SerializeField] Animator savedButton;
    [SerializeField] Animator deleteButton;
    [SerializeField] Animator deletePanel;
    [Header("\n")]

    public bool _timing = false;

    public savingScript sScrpt;

    
    void Awake()
    {
        deletePanelGO.SetActive(false);
        saveShitButton.SetActive(true);
        deleteSaveShitButton.SetActive(false);

        popupPanel.SetActive(false);
        savedThingsPanel.SetActive(false);
        visualizeTime();
        startBtn.interactable = true;
        stopBtn.interactable = false;
        saveButton.interactable = false;
    }

    public void StartTiming()
    {        
        startBtn.interactable = false;
        stopBtn.interactable = true;
        _timing = true;
        StartCoroutine(Timing());
    }

    public void StopTiming()
    {
        //finalTime = time[0] + ":" + time[1] + ":" + time[2];
        finalTime = timerText.text;
        _timing = false;
        stopBtn.interactable = false;
        StopCoroutine(Timing());
        popupPanel.SetActive(true);
        uHaveSMTG.text = "You've worked:\n" + finalTime;
        saveShitButton.SetActive(false);
    }

    public void Typed()
    {
        if(saveButton.interactable == false)
        {
            saveButton.interactable = true;
        }
    }

    public void openSavedThings()
    {
        switch(savedThingsPanel.activeSelf)
        {
            case false:
                sScrpt.Loading();
                StartCoroutine(openSaveThings());
                break;

            case true:
                StartCoroutine(CloseSaveThings());
                break;
        }
    }

    IEnumerator openSaveThings()
    {
        savedThingsPanel.SetActive(true);
        deleteSaveShitButton.SetActive(true);
        savedButton.SetBool("menuOpen", true);
        deleteButtonButton.interactable = false;
        savedButtonButton.interactable = false;
        deletePanelGO.SetActive(false);
        yield return new WaitForSecondsRealtime(0.2f);
        deleteButtonButton.interactable = true;
        savedButtonButton.interactable = true;

    }

    IEnumerator CloseSaveThings()
    {
        savedPanel.SetTrigger("end");
        deleteButton.SetTrigger("end");
        savedButton.SetBool("menuOpen", false);
        yield return new WaitForSecondsRealtime(0.2f);
        deleteSaveShitButton.SetActive(false);
        savedThingsPanel.SetActive(false);
        foreach (Transform child in saveTextParent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void SaveBTN()
    {
        sScrpt.Saving(finalTime, jobText.text);
        inf.text = "";
        StartCoroutine(SaveButtonCloseShit());
    }

    //saveButton
    IEnumerator SaveButtonCloseShit()
    {
        savePanel.SetTrigger("end");
        yield return new WaitForSecondsRealtime(0.2f);
        //deleteSaveShitButton.SetActive(false);
        startBtn.interactable = true;
        popupPanel.SetActive(false);
        saveShitButton.SetActive(true);
    }

    public void LoadWrite(string line)
    {
        GameObject teksz = Instantiate(saveTextPrefab, saveTextParent.transform);
        teksz.GetComponent<TMP_Text>().text = line;
    }

    public void ClearSavedTimes()
    {
        sScrpt.ClearSaves();
        foreach (Transform child in saveTextParent.transform)
        {
            Destroy(child.gameObject);
        }
        LoadWrite("No saves yet");
        StartCoroutine(CloseDeletePanel_());
    }



    public void DeleteButton()
    {
        deletePanelGO.SetActive(true);
    }

    public void CloseDeletePanel()
    {
        StartCoroutine(CloseDeletePanel_());
    }

    IEnumerator CloseDeletePanel_()
    {
        deletePanel.SetTrigger("end");
        yield return new WaitForSecondsRealtime(0.2f);
        deletePanelGO.SetActive(false);
    }




    public IEnumerator Timing()
    {
        while (_timing)
        {
            yield return new WaitForSecondsRealtime(faszom);
            
            time[2]++;

            if (time[2]>=60)
            {
                time[2] = 0;
                time[1]++;

                if(time[1] >= 60)
                {
                    time[1] = 0;
                    time[0]++;
                }
            }

            visualizeTime();
        }
        for (int i = 0; i < 3; i++)
        {
            time[i] = 0;
        }
        visualizeTime();
    }

    public void visualizeTime()
    {
        string help = "";
        for(int i = 0;i < 3; i++)
        {
            if (time[i] >= 10)
            {
                help += time[i];
            }else
            {
                help += "0" + time[i];
            }
            switch(i)
            {
                case 2:
                    break;

                default:
                    help += ':';
                    break;
            }
                
        }
        timerText.text = help;
    }
    

}
