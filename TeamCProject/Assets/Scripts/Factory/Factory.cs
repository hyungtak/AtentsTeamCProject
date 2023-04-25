using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PoolObjectType
{
    Stone = 0,
    Fireball,
    Goblin,
    Wizard

}

public class Factory : Singleton<Factory>
{
    StonePool stonePool;

    //GoblinPool goblinPool;


    protected override void PreInitialize()
    {
        // 자식으로 붙어있는 풀들 다 찾아놓기
        stonePool = GetComponentInChildren<StonePool>();
    }
    protected override void Initialize()
    {
        stonePool?.Initialize();
    }

    public GameObject GetObject(PoolObjectType type)
    {
        GameObject result = null;
        switch (type)       // type에 맞게 꺼내서 result에 저장
        {
            case PoolObjectType.Stone:
                result = GetStone().gameObject;
                break;
        
        }
        return result;      // result를 리턴. 타입이 없는 타입이면 null
    }


    public Stone GetStone() => stonePool?.GetObject();

}
