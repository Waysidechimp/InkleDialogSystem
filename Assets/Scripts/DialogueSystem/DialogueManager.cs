using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    PlayerInteraction playerInteraction;
    AudioManager audioManager;
    [SerializeField] InkScript currentScript;

    [Header("Dialog Gameobjects")]
    [SerializeField] GameObject dialogGameobject;
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] GameObject buttonPlacement;

    [Header("Dialog Settings")]
    [SerializeField] bool inDialog = false;
    [SerializeField] bool canContinue = true;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] int lastChoiceMade;
    [SerializeField] float talkSpeed;
    [SerializeField] bool isPrintingLine = false;
    [SerializeField] bool instantPrintLine = false;

    private void Awake()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
        playerInteraction = FindFirstObjectByType<PlayerInteraction>();
        playerInteraction.interactionEvent += BeginDialog;
    }

    //The method that marks the beginning of the dialog process. Activated by an event called in PlayerInteraction
    void BeginDialog(object inkScript, System.EventArgs e)
    {
        PlayerState.currentState = PlayerState.PlayerStates.Reading;
        currentScript = (InkScript)inkScript;
        inDialog = true;
        dialogText.text = "";
        currentScript.Start();
        audioManager.InsertSoundEffect(currentScript.GetAudioClip());
        StartCoroutine(AdvanceDialog());
        dialogGameobject.SetActive(true);
    }

    //The method that marks the end of the dialog process. Is called during the OnClick event and can be called by other scripts.
    public void EndDialog()
    {
        PlayerState.currentState = PlayerState.PlayerStates.Playing;
        inDialog = false;
        currentScript.End();
        dialogText.text = "";
        currentScript = null;
        dialogGameobject.SetActive(false);
    }

    //The method that is called whenever the player clicks the Mouse1 button.
    //Features advancment of dialog, ending of dialog, and instantly printing of dialog to the screen
    void OnClick(InputValue value)
    {
        if (PlayerState.currentState != PlayerState.PlayerStates.Reading)
        {
            return;
        }
        else
        {
            if(isPrintingLine && instantPrintLine == false)
            {
                instantPrintLine = true;
            }
            else if (currentScript.CheckContinue() == true && !isPrintingLine)
            {
                canContinue = true;
                StartCoroutine(AdvanceDialog());
            }
            else if (currentScript.CheckContinue() == false && currentScript.CheckChoices() == 0)
            {
                canContinue = false;
                EndDialog();
            }
        }
    }

    //The method that handles the printing of dialog onto the screen.
    IEnumerator AdvanceDialog()
    {
        isPrintingLine = true;
        if (currentScript.CheckContinue() == false && currentScript.CheckChoices() == 0)
        {
            canContinue = false;
            EndDialog();
        }
        string dialog = currentScript.AdvanceDialog();
        string[] split = dialog.Split(':');
        nameText.text = split[0];
        dialogText.text = "";
        char[] dialogSplit = split[1].ToCharArray();
        //Scrolling text fuctionality. Click again to get instant text to screen.
        foreach (char c in dialogSplit)
        {
            if(instantPrintLine)
            {
                dialogText.text = split[1];
                break;
            }
            dialogText.text += c;
            audioManager.SFXRandomizePitch();
            audioManager.PlaySoundEffect();
            yield return new WaitForSeconds(talkSpeed);
            audioManager.StopSoundEffect();
        }
        //check if there are any choices after printing text.
        if (currentScript.CheckChoices() > 0)
        {
            DoChoices();
        }
        instantPrintLine = false;
        isPrintingLine = false;
    }

    //The method that handles choices and their buttons. Uses the AddButtonListener to associate buttons to numbers
    void DoChoices()
    {
        int choiceAmount = currentScript.CheckChoices();
        Debug.Log(choiceAmount);
        for(int i = 0; i < choiceAmount; i++)
        {
            lastChoiceMade = i;
            GameObject button = Instantiate(buttonPrefab, buttonPlacement.transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = currentScript.GetChoices()[i];
            AddButtonListener(i, button);
        }
    }

    //A helper method for the DoChoices method. Associated a different number with each button.
    void AddButtonListener(int i, GameObject button)
    {
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(delegate { EnterChoice(i); });
    }

    //The method that is called when a button is pressed. the int passed in is based on the button pressed.
    public void EnterChoice(int choice)
    {
        lastChoiceMade = choice;
        currentScript.SetChoice(choice);
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("OptionButton");
        foreach(GameObject button in buttons)
        {
            Destroy(button);
        }
        StartCoroutine(AdvanceDialog());
    }
}
