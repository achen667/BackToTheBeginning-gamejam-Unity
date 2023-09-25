using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//֮�����źŸ�д�����

public class DialogueController : MonoBehaviour
{
    [SerializeField]
    private Canvas _dialogueCanvas;

    public Text textLable;

    public List <TextAsset> textFileList;
    private int currentLine;

    private List<string> _textList = new List<string>();

    [SerializeField] private Image faceImage;
    [SerializeField] private Sprite face1, face2;


    //����LogicController���յ��ź� ��ʶ�Ի�������
    public delegate void dialogueEnd();
    public static event dialogueEnd OnDialogueEnd;



    private void OnEnable()
    {
        LogicController.OnDialogueTiming += StartDialogue;
    }

    // Start is called before the first frame update
    void Start()
    {
        _dialogueCanvas.enabled = false;
        //faceImage.sprite = face1;
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.G))
        //{
        //    textLable.text = _textList[currentLine];
        //    currentLine++;
        //}
    }

    
    void ReadTextFile(int index)
    {
        TextAsset file = textFileList[index]; 
        _textList.Clear();
        currentLine = 0;

        string[] lineData = file.text.Split('\n');
        foreach(var line in lineData)
        {
            _textList.Add(line);
        }
    }


    void StartDialogue(object sender,LogicController.OnDialogueTimingArgs e)
    {
        _dialogueCanvas.enabled = true;
        int index;
        index = e.dialogueNumber;
        ReadTextFile(index);
        //������ʾһ�С�
        switch (_textList[currentLine][0])
        {
            case 'D':
                faceImage.sprite = face1;
                currentLine++;
                break;
            case 'F':
                faceImage.sprite = face2;
                currentLine++;
                break;
            default:
                faceImage.sprite = null;

                break;
        }

        textLable.text = _textList[currentLine];
        currentLine++;
        StartCoroutine(DisplayDialogue());
    }

    private IEnumerator DisplayDialogue()
    {
        //����ֹ���ơ�

        while (currentLine != _textList.Count)
        {
            if (Input.GetKeyDown(KeyCode.Return)|| Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                switch (_textList[currentLine][0])
                {
                    case 'D':
                        faceImage.sprite = face1;
                        currentLine++;
                        break;
                    case 'F':
                        faceImage.sprite = face2;
                        currentLine++;
                        break;
                    default:
                        faceImage.sprite = null;

                        break;
                }

                textLable.text = _textList[currentLine];
                currentLine++;
            }
            yield return null;
        }
        while (!(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        { 
            yield return null; 
        }
        _dialogueCanvas.enabled = false;
        OnDialogueEnd?.Invoke(); //�Ի����� ֪ͨLogicController

        //��������ơ�


    }

}
