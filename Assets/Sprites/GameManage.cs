using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class GameManage : MonoBehaviour
{
    //最高分文本
    public Text highScoreText;
    //总分数
    public int Score=0;
    //分数文本
    public Text scoreText;
    public GameObject effect;
    public AudioClip combineClip;
    public AudioClip combineClip1;
    public GameObject loseUI;
    private bool gameOver = false;
    public LinkedList<Fruit> fruitList = new LinkedList<Fruit>();
    public GameObject readyFruit=null;
    public Transform fruitSpwan;
    public static GameManage instance;
    public GameObject[] fruitPres;
    
    // Start is called before the first frame update
     void Awake()
    {
        instance = this;
        //设置分辨率
        Screen.SetResolution(540, 960, false);
    }
    void Start()
    {
        //CreateFruit();
        int highscore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "最高分：" + highscore.ToString();
        
    }

    // Update is called once per frame
    void Update()
    {
       if(gameOver)
        {
            //SceneManager.LoadScene("PlayGame");//求换游戏页面
            //显示失败界面
            loseUI.SetActive(true);
            return;
        }
        MoveFruit(); 
        CreateFruit();
        CombineFruit();
        GameOverCheck();
    }
    //产生水果
    void CreateFruit()
    {
        foreach(Fruit fruit1 in fruitList)
        {
            if (fruit1.state == FruitState.ready|| fruit1.state == FruitState.dropping)
            {
                return;
            }
        }
        int index=UnityEngine.Random.Range(0,5);
        GameObject fruitObj= Instantiate(fruitPres[index],
            fruitSpwan.position,
            fruitSpwan.rotation);
        Rigidbody2D rigid= fruitObj.GetComponent<Rigidbody2D>();
        rigid.gravityScale = 0;
        // readyFruit = fruitObj;
        //获取fruit脚本
        Fruit fruit = fruitObj.GetComponent<Fruit>();
        fruitList.AddLast(fruit);
        fruit.state = FruitState.ready;
        GameOverCheck();
    }
    //移动水果
    void MoveFruit()
    {
        Fruit readyFruit = null;
        foreach(Fruit fruit in fruitList)
        {
            if (fruit.state == FruitState.ready)
            {
                readyFruit = fruit;
                break;
            }
        }
        if (null == readyFruit)
        {
            return;
        }
        if (Input.GetMouseButton(0))
        {
           Vector3 pos= Camera.main.ScreenToWorldPoint(Input.mousePosition);
           //float x=  Mathf.Clamp(pos.x, -2, 2);
            float x = Mathf.Clamp(pos.x, readyFruit.minX, readyFruit.maxX);
            readyFruit.transform.position = new Vector3(x,
                readyFruit.transform.position.y,
                readyFruit.transform.position.z);
             
        }
        if (Input.GetMouseButtonUp(0))
        {
            Rigidbody2D rigid = readyFruit.GetComponent<Rigidbody2D>();
            rigid.gravityScale = 1;
            // Invoke("CreateFruit",0.5f);//隔5秒创建新水果
            readyFruit.state = FruitState.dropping;
            
        }
    }
    void GameOverCheck()
    {
        foreach (Fruit fruit2 in fruitList)
        {
            if(fruit2.state == FruitState.touchline)
            {
                gameOver = true;
                //更新最高分
                int highScore = PlayerPrefs.GetInt("HighScore", 0);
                if(Score> highScore)
                {
                    PlayerPrefs.SetInt("HighScore", Score);
                }
                break;
            }
        }
    }
    public void OnRestartClick()
    {
        //重新加载场景
        SceneManager.LoadScene("PlayGame");
    }
    //合并水果
    void CombineFruit()
    {
        //检测状态为combine的水果
        Fruit fruit1 = null;
        foreach (Fruit fruit in fruitList)
        {
            if (fruit.state == FruitState.combine)
            {
                fruit1 = fruit;
                break;
            }
        }
            if (fruit1 != null)
            {
            //分数累加
            Score += fruit1.score;
            scoreText.text ="分数："+ Score.ToString();
                //找到另外一种水果
                Fruit fruit2 = fruit1.other;
                Vector3 pos = (fruit1.transform.position 
                    + fruit2.transform.position) / 2;
                //获取新水果的下标
                int index = (int)fruit1.type + 1;
                //销毁要合并的水果
                Destroy(fruit1.gameObject);
                Destroy(fruit2.gameObject);
                fruitList.Remove(fruit1);
                fruitList.Remove(fruit2);
                //实例化新水果
                GameObject fruitObj=
                Instantiate(fruitPres[index], pos,fruitSpwan.rotation);
                //将水果添加到链表
                Fruit fruit3 = fruitObj.GetComponent<Fruit>();
                fruitList.AddLast(fruit3);
                fruit3.state = FruitState.collision;
            //新水果由小变大的动画
            fruitObj.transform.DOScale(0.1f, 0.3f).From();
            //播放一个声音
            AudioSource.PlayClipAtPoint(combineClip, pos);
            //实例化效果
            Instantiate(effect, pos, fruitSpwan.rotation);
        }
    }
}
