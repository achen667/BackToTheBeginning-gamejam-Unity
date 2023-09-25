using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerObject;

    private Player _player;
    private Player _clonedPlayer;


    private float runSpeed;
    private float jumpForce;


    private Rigidbody2D _body;
    private BoxCollider2D _collider;
    private Collider2D _hit;
   // private SpriteRenderer _render;   //ȫϢ������͸���� 
    private Transform _playerTsf;

    private GameObject _clonedPlayerObject;

    [SerializeField]
    private PlatformEffector2D[] platform;
 //   private bool _onIce = false;
    private Animator _animator;

    private bool _goastStatus = false;   //����״̬
    private int _goastSkillPoint = 3;


    public bool canControl = true;  //�Ƿ���Կ������  ���Ի�ʱ��ֹ���ƣ�

    public LogicController logicController;

    [SerializeField] private List<AudioClip> audioClipList;

    private bool _firstBeenChecked = true;
    private bool _firstRunOutCloneTime = true;

    [SerializeField] private Slider _slider;
    //private bool _canClone = true;

    //[SerializeField]
    //private Vector2 _startPoint;


    // Start is called before the first frame update
    void Start()
    {
        _player = _playerObject.GetComponent<Player>();       //��ȡplayer�ű����
        _body = _playerObject.GetComponent<Rigidbody2D>();
    //    _render = _playerObject.GetComponent<SpriteRenderer>();
        _playerTsf = _playerObject.GetComponent<Transform>();
        _collider = _playerObject.GetComponent<BoxCollider2D>();
        _animator = _playerObject.GetComponent<Animator>();
        runSpeed = _player.normalRunSpeed;
        jumpForce = _player.normalJumpForce;
        SwitchGoast(false);
        _player._canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canControl)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _playerTsf.position += Vector3.left * Time.deltaTime * runSpeed;
                //_body.velocity = new Vector2(-runSpeed*Time.deltaTime, _body.velocity.y);
                //_body.velocity = new Vector2(0, _body.velocity.y);
                _playerObject.transform.localScale = new Vector3(-1, 1, 1);
                _animator.SetBool("IsRunning", true);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                _playerTsf.position += Vector3.right * Time.deltaTime * runSpeed;
                //_body.velocity = new Vector2(runSpeed * Time.deltaTime, _body.velocity.y);
                //_body.velocity = new Vector2(0, _body.velocity.y);
                _animator.SetBool("IsRunning", true);
                _playerObject.transform.localScale = new Vector3(1, 1, 1);
            }
            else
                _animator.SetBool("IsRunning", false);


            if (Input.GetKeyDown(KeyCode.Space))
            {
                //if (_body.velocity.y == 0)
                if (IsOnGround())
                {
                    _body.velocity = new Vector2(0, 0);
                    _body.velocity += Vector2.up * jumpForce;
                    if (_goastStatus)   //������Ծ
                    {
                        _player.goastSkillPoint--;
                        if (_player.goastSkillPoint <= 0)   //���ܵ��þ���
                        {

                            SwitchGoast(false);
                            _goastStatus = false;
                        }

                    }
                    else if (_player._jumpSkillPoint > 0)   //��ͨ��Ծ
                    {
                        _player._jumpSkillPoint--;
                        PlaySound(_player._jumpSkillPoint);
                    }
                    else if (_player._jumpSkillPoint <=0)
                        PlaySound(0);
                }

            }
            if (Input.GetKeyDown(KeyCode.G) && _player._goastSkill)
            {
                if(_goastStatus)  //������� ��ô����
                {
                    SwitchGoast(false);
                    _goastStatus = false;
                }
                else if(_player.goastSkillPoint >0 )  //������� �����м��ܵ� ���
                {
                    SwitchGoast(true);
                    _goastStatus = true;
                }
                //����û��Ӧ


            }
            ClonePlayer();  //��E
        }
        else
        {
            _animator.SetBool("IsRunning", true);
            _animator.SetBool("IsJumpUp", false);
        }
        //if(Input.GetKeyDown(KeyCode.E))
        //{
        //    ClonePlayer();

        //}
        //ClonePlayer();  //��E
        ChangeCloneTranspancy();
        CloneExistTime();

        DevsCheck();

        //CheckCloneEnableDistance();
        //����
        if (IsOnGround())
        {
            _animator.SetBool("IsJump", false);

        }
        else
        {
            _animator.SetBool("IsJump", true);
            if (_body.velocity.y >0 )
                _animator.SetBool("IsJumpUp", true);
            else
                _animator.SetBool("IsJumpUp", false);
        }

    }



    private bool IsOnGround()
    {
        //Vector2 center = _collider.bounds.center;
        //Vector2 size = _collider.bounds.size + new Vector3(0,1,0);
        //LayerMask ignoreMask = ~(1 << 6);

        //_hit = Physics2D.OverlapCapsule(_collider.bounds.center,size, CapsuleDirection2D.Vertical, 0f,ignoreMask);
        //return (_hit != null);

        Vector3 max = _collider.bounds.max;
        Vector3 min = _collider.bounds.min;
        Vector2 corner1 = new Vector2(max.x, min.y - .1f);
        Vector2 corner2 = new Vector2(min.x, min.y - .1f);

        Collider2D hit = Physics2D.OverlapArea(corner1, corner2);


        if (hit != null)
        {
            return true;
        }

        return false;
    }

    private void ClonePlayer()
    {
        if (_body.velocity != new Vector2(0, 0))
            return;
        if (Input.GetKeyDown(KeyCode.C) && _player._cloneSkill)
        {
            if(!CheckCloneEnableDistance())//�����Ƿ��ڿ��÷�Χ��
            {
                logicController.StartOneDialogue(3);
                return;
            }
            if (_player.CanClone && _clonedPlayerObject==null)
            {
                _clonedPlayerObject = Instantiate(_playerObject, _player.StartPoint, Quaternion.identity);
                // _clonedPlayerObject.GetComponent<Animator>().enabled = false;
                _clonedPlayer = _clonedPlayerObject.GetComponent<Player>();
                _clonedPlayer.Transpancy = 0;
            }
            else if (!_player.CanClone)
            {
                logicController.StartOneDialogue(4);

            }
            else 
            {
                Destroy(_clonedPlayerObject);
                Destroy(_clonedPlayer);
                
            }
        }
    }
    private void ChangeCloneTranspancy()
    {
        if (_clonedPlayerObject != null)
            if (_clonedPlayer.Transpancy >= 1)
                return;
            else
                StartCoroutine(IncreaseTrans());
    }
    IEnumerator IncreaseTrans()
    {
        if (_clonedPlayer.Transpancy < 0.7)
            _clonedPlayer.Transpancy += 0.3f * Time.deltaTime;
        yield return null;
    }

    //ȫϢ��¡ʣ�����ʱ��
    private void CloneExistTime()
    {
        if (_clonedPlayerObject != null )
        {
            _player.CloneSkillPoint -= Time.deltaTime;
            //_slider.value = 1 - _player.CloneSkillPoint / 10f;
        }
        if (_player.CloneSkillPoint <= 0)
        {
            _player.CanClone = false;
            Destroy(_clonedPlayerObject);
            Destroy(_clonedPlayer);
          //  _player.CloneSkillPoint = 10f;  //����������ϼ���ʱ�� ��������ʹ�ÿ�¡��
        }
        _slider.value = 1 - _player.CloneSkillPoint / 10f;
    }

    private void DevsCheck()
    {
        if ((_player._jumpSkillPoint <= 0) )
            StartCoroutine(CheckAroundStart());

    }

    //����Ƿ��ڳ����㸽��
    IEnumerator CheckAroundStart()
    {
        yield return new WaitForSeconds(1.0f);
        AudioController.Instance.PlaySoundLow(audioClipList[3]);
        yield return new WaitForSeconds(1.0f);   //��Ծ���ܵ�ľ��� �ӳ�һ��� ���м��

        float dis = Vector3.Distance(_player.StartPoint, _player.transform.position);


        if (dis < 5 || _clonedPlayer != null)
        {
            _player._jumpSkillPoint = 3;
            //return;
        }
        else if ((_player._jumpSkillPoint <= 0) )   //�ܵ��ͷ�
        {
            //�ر�ȫϢ
            if (_clonedPlayerObject != null)
            {
                Destroy(_clonedPlayerObject);
                Destroy(_clonedPlayer);
                _player.CanClone = true;
            }

            _body.velocity = new Vector2(0, 0);
            _player.transform.position = _player.StartPoint;

            if (_firstBeenChecked)  //��һ�μ������ʱ �����Ի�
            {
                logicController.StartOneDialogue(5);
                _firstBeenChecked = false;
            }

            _player._jumpSkillPoint = 3;    //�ͷ����� ���´���㿪ʼ 
            _player.CanClone = true;
        }
    }
    private bool CheckCloneEnableDistance()
    {

        float dis = _player.transform.position.y -  _player.StartPoint.y  ;
        if(dis > 32)
        {
            return false;
        }
        return true;
    }


    private void SwitchGoast(bool Turn)
    {
        if (Turn)
        {
            
            foreach (PlatformEffector2D plat in platform)
                plat.useOneWay = true;
            jumpForce = _player.goastJumpForce;
            runSpeed = _player.goastSpeed;
            _player.Transpancy = 0.6f;
        }
        else
        {
            
            foreach (PlatformEffector2D plat in platform)
                plat.useOneWay = false;
            runSpeed = _player.normalRunSpeed;
            jumpForce = _player.normalJumpForce;
            _player.Transpancy = 1f;
        }
    }

    private void PlaySound(int index)
    {
        AudioController.Instance.PlaySound(audioClipList[index]);
    }


}
