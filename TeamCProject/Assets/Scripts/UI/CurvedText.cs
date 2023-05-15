using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class CurvedText : Text
{
    /// <summary>
    /// 반지름
    /// </summary>
    public float radius = 0.5f;
    
    /// <summary>
    /// 내부 각
    /// </summary>
    public float wrapAngle = 360.0f;

    /// <summary>
    /// 스케일 값
    /// </summary>
    public float scaleFactor = 100.0f;

    float _radius = -1;
    float _scaleFactor = -1;
    float _circumference = -1;

    protected override void Start()
    {
        base.Start();
        
    }

    private float circumference
    {
        get
        {
            if(_radius != radius || _scaleFactor != scaleFactor)
            {
                _circumference = 2.0f * Mathf.PI * radius * scaleFactor;
                _radius = radius;
                _scaleFactor = scaleFactor;
            }
            return _circumference;
        }
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        if (radius <= 0.0f)
            radius = 0.001f;
        if (scaleFactor <= 0.0f)
            scaleFactor = 0.001f;
    }

    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        base.OnPopulateMesh(toFill);

        for (int i = 0; i < toFill.currentVertCount; i++)
        {
            UIVertex v = UIVertex.simpleVert;
            toFill.PopulateUIVertex(ref v, i);
            Vector3 p = v.position;
            float percentCircumference = v.position.x / circumference;
            Vector3 offset = Quaternion.Euler(0.0f, 0.0f, -percentCircumference * 360.0f) * Vector3.up;
            p = offset * radius * scaleFactor + offset * v.position.y;
            v.position = p;
            toFill.SetUIVertex(v, i);
        }
    }

    private void Update()
    {
        if (radius <= 0.0f)
            radius = 0.001f;
        if (scaleFactor <= 0.0f)
            scaleFactor = 0.001f;
        rectTransform.sizeDelta = new Vector2(circumference * wrapAngle / 360.0f, rectTransform.sizeDelta.y);
    }
}
