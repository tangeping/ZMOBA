using KBEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testConfig : MonoBehaviour {

    private const string serverSettingsAssetFile = "FrameSyncConfig";
    // Use this for initialization
    void Start () {

        FrameSyncConfig Config =  Resources.Load<FrameSyncConfig>(serverSettingsAssetFile);
        Debug.Log("Config.syncWindow:" + Config.syncWindow);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
