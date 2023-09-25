using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstStepTrigger : MonoBehaviour
{
    public delegate void FirstTrigger();
    public static event FirstTrigger onFirstTrigger;


    public LogicController logicController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            logicController.StartOneDialogue(0);
            //onFirstTrigger?.Invoke();
        }
    }


}
