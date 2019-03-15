using UnityEngine;
using System.Collections;

public class UiEffectTest : MonoBehaviour {

    public UIBaseProgress m_uieffect;

    private float m_precitent;
    private Material m_material;
    private float m_precitentTemp;

    void Start()
    {
        m_material = m_uieffect.GetMaterial();
        m_precitent = m_material.GetFloat("_Percent");
        m_precitentTemp = m_precitent;
        m_uieffect.SetPercent(m_precitent);
    }

	void Update () {
        m_precitent = m_material.GetFloat("_Percent");

        if (m_precitent == m_precitentTemp)
            return;

        m_precitentTemp = m_precitent;
        m_uieffect.SetPercent(m_precitent);
	}
}
