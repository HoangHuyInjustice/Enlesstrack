using _Game.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour //class PlayerController điều khiển nhân vật
                                              //Kế thừa MonoBehaviour, giúp gán script này vào GameObject trong Unity.
{
    [SerializeField]
    private Rigidbody _rb; //Điều khiển vật lý của nhân vật.

    private Animator _animator; //Điều khiển animation của nhân vật.

    private BoxCollider _boxCollider;//Phát hiện va chạm.

    [SerializeField]
    private float _speed = 10f; //Tốc độ di chuyển của nhân vật.

    private float _horizontalInput; //Lưu giá trị di chuyển ngang(A hoặc D).

    [SerializeField]
    private float jumpForce = 100f;//Lực nhảy của nhân vật.

    private bool isGrounded = false; //Kiểm tra xem nhân vật có đang đứng trên mặt đất không.

    public bool IsAlive = true;//rạng thái sống/chết của nhân vật.

    public int CurrentHP; //Lượng máu hiện tại.

    private int _maxHP = 3; //Máu tối đa.

    #region CurrentItem

    private int _currentBonusSpeed;

    private int _currentDef;

    #endregion

    private void Awake()
    {
        _maxHP += GameManager.Instance.HP;
         CurrentHP = _maxHP;
        GameManager.Instance.HP = 0;
        GameManager.Instance.SaveShopHP();
    }

    private void Start() //Khởi tạo nhân vật
    {
        _rb = GetComponent<Rigidbody>();//Lấy Rigidbody, BoxCollider, Animator từ GameObject.
        _boxCollider = GetComponent<BoxCollider>();
        _animator = gameObject.GetComponentInChildren<Animator>();

        GameUI.Instance.UpdateHP(CurrentHP, _maxHP);//Cập nhật UI máu
        StartCoroutine(IncreaseSpeedOverTime());//Bắt đầu tăng tốc theo thời gian
    }

    private void Update()//Xử lý nhập từ bàn phím
    {
        _horizontalInput = Input.GetAxis("Horizontal"); //Di chuyển nhân vật//Nhận tín hiệu di chuyển ngang nhận input từ A/ D 
        {

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded) //Nhảy khi nhấn Space và nhân vật chạm đất
                Jump();
        }

        if (transform.position.y < -5)//Chết nếu rơi khỏi bản đồ
        {
            Die();
        }
        return;
    }
    private void FixedUpdate() //Di chuyển nhân vật
    {
        if (IsAlive == false) return; //Chỉ di chuyển khi còn sống
           
        Vector3 forwardMove = transform.forward * _speed * Time.fixedDeltaTime;      //Tạo vector di chuyển forwardMove: tiến về phía trước.
        Vector3 horizontalMove = transform.right * _horizontalInput * _speed * Time.fixedDeltaTime;     //horizontalMove: di chuyển ngang.
        _rb.MovePosition(_rb.position + horizontalMove + forwardMove);        // Di chuyển bằng Rigidbody _rb.MovePosition(): giúp di chuyển mượt hơn so với transform.position.
    }

    private void OnCollisionEnter(Collision collision) //Xử lý va chạm
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; //Chạm đất
        }
        else if (collision.gameObject.CompareTag("RaoChan"))
        {
            SoundManager.Instance.PlaySFX(3); //Phát âm thanh
            TakeDamage(1);                            // mất máu
            Destroy(collision.gameObject);//Hủy rào chắn
            
        }
    }
    private void TakeDamage(int damage) //Gây sát thương cho nhân vật
    {
        CurrentHP -= damage; // Giảm máu nhân vật theo lượng sát thương nhận vào
        CurrentHP = Mathf.Clamp(CurrentHP, 0, _maxHP); // Đảm bảo máu không nhỏ hơn 0 và không vượt quá _maxHP

        GameUI.Instance.UpdateHP(CurrentHP, _maxHP); //// Cập nhật thanh máu trên giao diện UI

        if (CurrentHP <= 0) // Nếu máu giảm về 0 hoặc thấp hơn
        {
            Die(); // Gọi hàm Die() để xử lý cái chết của nhân vật
        }
    }

    private void OnCollisionStay(Collision collision) //Xác định khi nhân vật đang đứng trên mặt đất
    {
        if (collision.gameObject.CompareTag("Ground")) //// Kiểm tra nếu va chạm với mặt đất
        {
            isGrounded = true; // Đánh dấu rằng nhân vật đang đứng trên mặt đất
            //Khi nhân vật đứng trên bề mặt lâu, Unity có thể không liên tục kích hoạt OnCollisionEnter, nên dùng OnCollisionStay để giữ trạng thái isGrounded = true.
        }
    }

    private void OnCollisionExit(Collision collision) //Xác định khi nhân vật rời khỏi mặt đất
    {
        if (collision.gameObject.CompareTag("Ground")) //// Kiểm tra nếu nhân vật rời khỏi mặt đất
        {
            isGrounded = false; // Đánh dấu rằng nhân vật đang ở trên không 
            //Giúp kiểm soát việc nhảy của nhân vật – chỉ cho phép nhảy khi đang chạm đất.
        }
    }

    private IEnumerator IncreaseSpeedOverTime() //Tăng tốc độ nhân vật theo thời gian
    {
        while (IsAlive) // Lặp lại miễn là nhân vật còn sống
        {
            yield return new WaitForSeconds(30f);// Chờ 30 giây
            _speed += 5f; // Tăng tốc độ thêm 5 đơn vị

            Debug.Log("Tốc độ hiện tại: " + _speed); // Hiển thị tốc độ mới trên console
        }
    }

    void Jump() //Nhảy
    {
        _animator.SetTrigger("Jump");
        _rb.AddForce(Vector3.up * jumpForce);
    }

    public void Die()// chết 
    {
        _animator.SetTrigger("IsAlive"); //Kích hoạt animation chết
        IsAlive = false; //Đánh dấu nhân vật đã chết, ngăn cản di chuyển.
        StartCoroutine(ResetIsAlive()); // Bắt đầu coroutine để reset trạng thái nhân vật sau 3 giây
        if (transform.position.y < -5) return;//Nếu nhân vật đã rơi khỏi bản đồ, thoát khỏi hàm ngay.
        GameUI.Instance.OnPlayerDeath();// khi chết thì gọi hàm này để add timer + coin hiện tại đã đạt được là bao nhiêu
        GameUI.Instance.CoinDasbBoard = 0; //Đặt lại số coin về 0.
    }
    private IEnumerator ResetIsAlive()
    {
        GameUI.Instance.StopCoroutine(GameUI.Instance.TimerCountdown());//Dừng bộ đếm thời gian của UI.
        yield return new WaitForSeconds(3); //Chờ 3 giây trước khi chuyển sang màn hình game over.
        GameOver(); //reset game.

    }
    void GameOver() //Reset game
    {
        GameUI.Instance.Timer = 120f; //Đặt lại bộ đếm thời gian về 120 giây khi game bắt đầu lại
        SceneManager.LoadScene("GamePlay");//bắt đầu lại từ đầu.
    }

    #region AddItem

    public void IncreaseHP(int amount)// Tăng HP
    {
        CurrentHP += amount; //Tăng lượng máu hiện tại.
        Debug.Log("Tăng máu" + amount);                //In ra console số máu được tăng
        CurrentHP = Mathf.Clamp(CurrentHP, 0, _maxHP); //Đảm bảo máu không vượt quá giới hạn

        GameUI.Instance.UpdateHP(CurrentHP, _maxHP);   //Cập nhật thanh máu trên giao diện UI.
    }

    public void ActivateSpeedBonus(float amount, float duration) // Tăng tốc độ tạm thời
    {
        if (_currentBonusSpeed == 0)          //Kiểm tra nếu chưa có buff tốc độ
        {
            _speed += amount;                 // Tăng tốc độ nhân vật.
            Debug.Log("Tăng tốc độ" + amount); 
            _currentBonusSpeed = 1;
            StartCoroutine(ResetSpeedBonus(amount, duration)); //Gọi coroutine để reset tốc độ sau một khoảng thời gian
            GameUI.Instance.ShowSpeedTimer(duration);          //Hiển thị bộ đếm thời gian trên UI.
        }
    }

    public void ActivateDefBonus(int amount, float duration) // Tăng giáp 
    {
        if (_currentDef == 0)
        {
            _currentDef += amount;
            _rb.useGravity = false;        // kiểm tra rigibodi, nếu đã ăn giáp thì độ rơi = 0 , là không có rơi
            _boxCollider.isTrigger = true; // Kiểm tra trigger của Player = true thì đi xuyên qua đc vật thể
            _currentDef = 1;
            StartCoroutine(ResetDefBonus(duration));
            GameUI.Instance.ShowDefTimer(duration);
        }
    }

    private IEnumerator ResetSpeedBonus(float amount, float duration) // Kết thúc buff tốc độ
    {
        yield return new WaitForSeconds(duration);
        _speed -= amount;
        _currentBonusSpeed = 0;
        GameUI.Instance.HideSpeedTimer();
    }

    private IEnumerator ResetDefBonus(float duration) //kết thúc buff giáp
    {
        yield return new WaitForSeconds(duration);
        _rb.useGravity = true;      // nó sẽ không rơi
        _boxCollider.isTrigger = false;// thì nó sẽ không đi xuyên qua đc
        _currentDef = 0;
        GameUI.Instance.HideDefTimer();
    }

    #endregion
}
