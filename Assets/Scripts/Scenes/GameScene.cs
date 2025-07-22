using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    Coroutine co;

    IEnumerator CoExplodeAfterSeconds(float seconds)
    {
        Debug.Log("Explode Enter");

        yield return new WaitForSeconds(seconds);

        Debug.Log("Explode Execute");
        co = null;
    }

    IEnumerator CoStopExplodeAfterSecnods(float seconds)
    {
        Debug.Log("Stop Enter");

        yield return new WaitForSeconds(seconds);

        Debug.Log("Stop Execute");
        if (co != null)
        {
            StopCoroutine(co);
            co = null;
        }
    }

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;

        Managers.UI.ShowSceneUI<UI_Inven>();

        // PoolManager 실습
        //for (int i = 0; i < 5; i++)
        //{
        //    Managers.Resource.Instantiate("UnityChan");
        //}

        // Coroutine 실습
        co = StartCoroutine("CoExplodeAfterSeconds", 4.0f);
        StartCoroutine("CoStopExplodeAfterSecnods", 2.0f);

        // DataManager 실습
        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;

    }

    void Update()
    {
        
    }
    public override void Clear()
    {
        
    }
}
