using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class LogicController : MonoBehaviour
{
    //public delegate void firstDialogue();
    //public static event firstDialogue dialogue;
    public static event EventHandler<OnDialogueTimingArgs> OnDialogueTiming;
    public class OnDialogueTimingArgs :EventArgs
    {
        public int dialogueNumber;
    }
    OnDialogueTimingArgs dialogueTimingArgs;

    [SerializeField]
    private PlayerController _playerController;

    private bool firstTimeInTrigger = true;

    public bool gameOverStatus = false;

    private void OnEnable()
    {
        //FirstStepTrigger.onFirstTrigger += OnFirstStepTrigger;
        DialogueController.OnDialogueEnd += ToggleCanControl;

    }

    private void OnDisable()
    {
        //FirstStepTrigger.onFirstTrigger -= OnFirstStepTrigger;
        DialogueController.OnDialogueEnd -= ToggleCanControl;
    }

    private void Start()
    {
        dialogueTimingArgs = new OnDialogueTimingArgs();
        
    }

    //private void OnFirstStepTrigger()
    //{
    //    if (firstTimeInTrigger)
    //    {
    //        ToggleCanControl();
    //        dialogueTimingArgs.dialogueNumber = 0;   //第一段
    //        firstTimeInTrigger = false;
    //        //开始第一段对话
    //        //对话控制器将接收信号
    //        OnDialogueTiming?.Invoke(this, dialogueTimingArgs);
    //    }
    //}

    private void ToggleCanControl()
    {
        _playerController.canControl = !_playerController.canControl; //    禁止/允许控制
    }

    public void StartOneDialogue(int index)
    {
        ToggleCanControl();
        dialogueTimingArgs.dialogueNumber = index;   //第index段
        firstTimeInTrigger = false;
        //开始第一段对话
        //对话控制器将接收信号
        OnDialogueTiming?.Invoke(this, dialogueTimingArgs);
    }
}
