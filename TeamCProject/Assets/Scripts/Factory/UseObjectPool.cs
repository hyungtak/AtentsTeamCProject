using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseObjectPool : MonoBehaviour
{
    public Action onDeactivate;


    protected virtual void OnDisable()
    {
        onDeactivate?.Invoke();
    }

    

    protected IEnumerator LifeOver(float delay = 0.0f)
    {
        yield return new WaitForSeconds(delay); // delay만큼 대기하고
        this.gameObject.SetActive(false);       // 비활성화 시키기        
    }
}
