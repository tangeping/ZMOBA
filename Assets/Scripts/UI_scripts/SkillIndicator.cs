using KBEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillIndicator : MonoBehaviour {

    public GameObject indicator;

    void Update()
    {
        var player = SpaceData.Instance.getLocalPlayer().GetComponent<PlayerEntity>();
        if (!player) return;

        indicator.SetActive(player.SyncWantedSkill != -1);
        indicator.transform.position = Input.mousePosition;
    }
}
