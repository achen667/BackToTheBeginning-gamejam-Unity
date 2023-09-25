using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class EndingPerformance : MonoBehaviour
{


    public Text textLable;

    public TextAsset textFile;
    private int currentLine;

    private List<string> _textList = new List<string>();

    private string lineData;

    public float speed = 0.2f;

    private bool _performEnd = false;



    private void Start()
    {
        ReadTextFile();
        DisplayDialogue();
    }

    void ReadTextFile()
    {
        TextAsset file = textFile;
        currentLine = 0;

        lineData = file.text;

    }

    private void DisplayDialogue()
    {

                StartCoroutine(Display());

    }
    IEnumerator Display()
    {
        
        for (int i = 0; i < lineData.Length; i++)
        {
            textLable.text += lineData[i];
            if(lineData[i] != ' ')
                yield return new WaitForSeconds(speed);
        }
        _performEnd = true;
    }


    private void Update()
    {
        //if (_performEnd)
        //    if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        //    {
        //        AudioController.Instance._musicSource.enabled = true;
        //        AudioController.Instance._musicSource.volume = 1;
        //        AudioController.Instance._musicSource.Play();
        //        AudioController.Instance._musicSource2.enabled = false;
        //        AudioController.Instance._musicSource3.enabled = false;
                

        //        SceneManager.LoadScene("StartScene");
                
        //    }
    }
}
