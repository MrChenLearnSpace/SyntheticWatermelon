using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//水果类型（按照合成顺序）
public enum FruitType
{
    shanzhu,//山竹
    pingguo,
    chengzi,
    ningmeng,
    mihoutao,
    fanqie,
    shuimitao,
    boluo,
    yezi,
    xigua,
    daxigua
}
//水果状态
public enum FruitState
{
    ready,//准备
    dropping,//掉落
    collision,//碰撞
    combine,//合并
    touchline//碰线
}
public class Fruit : MonoBehaviour
{
    public int score = 1;
    //public AudioClip combineClip1;
    //水果类型
    public FruitType type = FruitType.shanzhu;
    //合并的水果
    public Fruit other;
    //水果状态
    //最小x范围
    public float minX = 0;
    //最大x范围
    public float maxX = 0;

    public FruitState state = FruitState.ready;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    //检测碰撞函数
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (state == FruitState.dropping || state == FruitState.collision)

            if ("Fruit" == collision.gameObject.tag)
            {
                //修改状态为碰撞
                state = FruitState.collision;
                //判断两个水果的类型是否相同
                if (type != FruitType.daxigua)
                {
                    //获取被碰撞的水果
                    Fruit fruit =
                    collision.gameObject.GetComponent<Fruit>();
                    if ((fruit.state == FruitState.dropping ||
                        fruit.state == FruitState.collision) && type == fruit.type)
                    {
                        //修改两个水果状态为合并
                        state = FruitState.combine;
                        fruit.state = FruitState.combine;
                        //记录合并的水果
                        other = fruit;
                        fruit.other = this;
                    }
                }

            }
            else if ("Ground" == collision.gameObject.tag)
            {
                state = FruitState.collision;
                //AudioSource.PlayClipAtPoint(combineClip1,this.transform.position);
            }
    }
    //检测触发
    void OnTriggerStay2D(Collider2D collider)
    {
        //判断当前状态是否碰撞
        if (state == FruitState.collision)
        {
            //如果碰到线
            if (collider.tag == "Line")
            {
                //将状态变为碰线状态
                state = FruitState.touchline;
                Debug.Log("碰线");
            }
        }

    }
}
