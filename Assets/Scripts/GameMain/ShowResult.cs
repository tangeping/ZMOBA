using KBEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowResult : MonoBehaviour {

    private GameObject VictoryPerfab;
    private GameObject DefeatedPerfab;
    public Transform showPos;
    // Use this for initialization
    void Start () {
        VictoryPerfab = Resources.Load<GameObject>("UI/effect/ui_v2_jiesuan_shengli");
        DefeatedPerfab = Resources.Load<GameObject>("UI/effect/ui_v2_jiesuan_shibai");

        if(showPos && SpaceData.Instance.GameOverResult != GameOverType.UNKOWN)
        {
            var showPerfab = SpaceData.Instance.GameOverResult == GameOverType.WIN ? VictoryPerfab : DefeatedPerfab;
            Instantiate(showPerfab, showPos.position, showPos.rotation);
        }
    }

}
