using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField]
    private Transform dialogueContainer, interactText, yesNoContainer;
    [SerializeField]
    private TextMeshProUGUI dialogueText;
    private Animator animator;
    private Audio3D audio;

    private bool dialogueActive, dialogueShown, dialogueWriting;
    private int dialogueIndex, textIndex;
    private string[] dialogues = new string[] {
        "Welcome 23-2! Congratulations on your previous achievement.",
        "Are you prepared to defend the jungle's heart from the destroyers?"
    };

    public static NPCDialogue Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        animator = GetComponent<Animator>();
        audio = GetComponent<Audio3D>();
    }

    public void Activate(bool x)
    {
        dialogueActive = x;
    }

    void Update()
    {
        if (TowerDefense.Instance.GetGameStatus()) return;

        if(!dialogueShown && dialogueActive)
        {
            interactText.gameObject.SetActive(true);
        }
        else
        {
            interactText.gameObject.SetActive(false);
        }

        if(dialogueActive && Input.GetKeyDown(KeyCode.F))
        {
            DeleteDialogue();
            dialogueIndex = 0;
            dialogueShown = true;
            dialogueContainer.gameObject.SetActive(true);
            audio.Dialogue("Dialogue1");
            animator.SetBool("isTalking", true);
            StartCoroutine(WriteDialogue());
        }
        else if(!dialogueWriting && dialogueIndex == 1 && Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(NextDialogue());
        }

        if (!dialogueActive)
        {
            dialogueShown = false;
            dialogueContainer.gameObject.SetActive(false);
            DeleteDialogue();
        }
    }

    private IEnumerator NextDialogue()
    {
        dialogueWriting = true;
        StopCoroutine(WriteDialogue());
        animator.SetBool("isTalking", false);
        while (dialogueText.text.Length > 0)
        {
            dialogueText.text = dialogueText.text[0..^1];
            yield return new WaitForSeconds(0.00005f);
        }
        yield return null;
        audio.Dialogue("Dialogue2");
        yield return StartCoroutine(WriteDialogue());
        yesNoContainer.gameObject.SetActive(true);
        UIController.Instance.ShowCursor(true);
    }

    private void DeleteDialogue()
    {
        animator.SetBool("isTalking", false);
        StopAllCoroutines();
        dialogueText.text = "";
        dialogueIndex = 0;
        textIndex = 0;
        dialogueWriting = false;
        yesNoContainer.gameObject.SetActive(false);
    }

    private IEnumerator WriteDialogue()
    {
        animator.SetBool("isTalking", true);
        dialogueWriting = true;
        textIndex = 0;
        while(textIndex < dialogues[dialogueIndex].Length)
        {
            dialogueText.text += dialogues[dialogueIndex][textIndex];
            textIndex++;
            yield return new WaitForSeconds(0.007f);
        }
        dialogueWriting = false;
        dialogueIndex++;
    }

    public void YesQuest()
    {
        dialogueShown = false;
        dialogueContainer.gameObject.SetActive(false);
        DeleteDialogue();
        TowerDefense.Instance.StartGame();
        AudioManager.Instance.SwitchBGM("TdBGM");
        UIController.Instance.ShowCursor(false);
    }

    public void NoQuest()
    {
        dialogueShown = false;
        dialogueContainer.gameObject.SetActive(false);
        DeleteDialogue();
        UIController.Instance.ShowCursor(false);
    }
}
