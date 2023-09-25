using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    [SerializeField]
    private Transform _startPoint;

    private bool _canClone = true;
    [SerializeField]   //【fix】
    public float _cloneSkillPoint = 10f;
    public int goastSkillPoint = 3;
    [SerializeField]
    private SpriteRenderer _render;
    private float _transparency ;

    //不用属性了。。
    public int _jumpSkillPoint = 3;
    private bool _canJump = true;
    public bool _cloneSkill = false;
    public bool _goastSkill = false;
    public float normalRunSpeed = 7f;
    public float normalJumpForce = 16f;
    public float goastSpeed = 10f;
    public float goastJumpForce = 25f;
    public int gemNumber = 0;

    public bool _firstGem = true;
    public bool _secondGem = false;
    public LogicController logicController;


    private Rigidbody2D _body;
    [SerializeField] private GameObject _cameraZone;
    [SerializeField] private List<GameObject> gemSpawnPoint;

    public Canvas _canvas;
    public bool endingChoose = false;
    private void Start()
    { 
        _body = GetComponent<Rigidbody2D>();
        
    }




    public bool CanClone
    {
        get { return _canClone; }
        set { _canClone = value; }
    }

    public Vector2 StartPoint
    {
        get { return _startPoint.position; }
    }

    public float CloneSkillPoint
    {
        get { return _cloneSkillPoint; }
        set { _cloneSkillPoint = value; }
    }

    //设置透明度
    public float Transpancy
    {
        get { return _render.color.a;  }
        set { _render.color = new Color(_render.color.r, _render.color.g, _render.color.b, value); }
    }

    public bool CanJump
    {
        get { return _canJump; }
        set { _canJump = value; }
    }

    private GameObject gem;
    void OnTriggerEnter2D(Collider2D collider)
    {
        //不销毁trigger
        if (collider.CompareTag("startPlat"))   
        {
            //刷新全息
            _canClone = true;
            _cloneSkillPoint = 10f;
            //刷新疾步
            goastSkillPoint = 3;
            _goastSkill = true;
            if (gem!= null)
            {
               
                gem.SetActive(true);
                int n = Random.Range(0, 4);
                gem.transform.position = gemSpawnPoint[n].transform.position;
            }

            if (endingChoose == true)
            {
                StartCoroutine(Ending2());
                
            
            }
            return;
        }
        if (collider.CompareTag("item2"))   //gem 疾步宝石
        {
            
            gemNumber++;
            goastSkillPoint = 3;
            goastJumpForce += gemNumber;   //+1 +2 +3 +4 增加速度递增
            gem = collider.gameObject;
            _goastSkill = true;
            //Debug.Log("item2 got");
            if (_firstGem)    //第一次拿道具 播放道具说明
            {
                logicController.StartOneDialogue(2);
                _firstGem = false;
                _secondGem = true;
            }
            else if(_secondGem)
            {
                logicController.StartOneDialogue(7);
                _secondGem = false;
            }
            gem.SetActive(false);
            return;
        }
        if (collider.CompareTag("border") && !logicController.gameOverStatus)
        {
            transform.position = _startPoint.position;
            return;
        }


        //销毁trigger 
        if (collider.CompareTag("item1"))   //CLONE 
        {
            
            _cloneSkill = true;
            //Debug.Log("item1 got"); //【delete later】
            logicController.StartOneDialogue(1);
            _canvas.enabled = true;
            
        }

        if(collider.CompareTag("borderL"))
        {
            logicController.StartOneDialogue(6);
            endingChoose = true;
        }
        if (collider.CompareTag("borderF"))
        {
           // Debug.Log("gameover");
            //_body.gravityScale = 0;
            //_cameraZone.SetActive(false);
            
            logicController.gameOverStatus = true;
            //【切换场景】
            AudioController.Instance.ChangeMusic(1);

            SceneManager.LoadScene("FinalScene");
            

        }
        Destroy(collider.gameObject);




    }
    IEnumerator Ending2()
    {
        yield return new WaitForSeconds(2f);

        AudioController.Instance.ChangeMusic(2);
        SceneManager.LoadScene("FinalScene2");
    }

}
