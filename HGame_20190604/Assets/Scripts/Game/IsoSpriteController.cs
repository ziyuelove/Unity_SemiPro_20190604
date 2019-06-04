using UnityEngine;
using System;
using System.Collections;
[ExecuteInEditMode]
public class IsoSpriteController : MonoBehaviour {
    private float m_OverlayLayerZ, m_GroundOverlayLayerZ;
    // Use this for initialization
    void Start () 
    {
        m_OverlayLayerZ = -1;
        m_GroundOverlayLayerZ = 0.5f;
    }

	void LateUpdate()
	{
        Setlayer_Z();
    }

    //根据Object的Y值来指定深度
    void Setlayer_Z()
    {
        Vector3 vPos = transform.position;
        //float f0 = Mathf.Abs(m_OverlayLayerZ - m_GroundOverlayLayerZ);
        // float f1 = 0;
        //if (transform.position.y>0)
        ///     f1 = Mathf.Abs(0 - transform.position.y) / 24;
        //else
        //    f1 = Mathf.Abs(transform.position.y) / 24;
        float f1 = transform.position.y;
        vPos.z = f1/1000;
        transform.position = vPos;
    }

}