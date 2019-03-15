using UnityEngine;
using KBEngine;

public class TextMeshHealthBar : MonoBehaviour {
    [SerializeField] Color teamColor = Color.green;
    [SerializeField] Color enemyColor = Color.red;
    [SerializeField] Color backgroundColor = Color.black;

    [SerializeField] GameEntity self; // own entity component, probably in parents

    string GenerateHealthString(int n, Color color) {
        return "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" +
               new string('_', n) +
               "</color>" +
               "<color=#" + ColorUtility.ToHtmlStringRGBA(backgroundColor) + ">" +
               new string('_', 10-n) +
               "</color>";
    }

    void Update() {

        // set color based on same team or not
        int localTeam = (int)SpaceData.Instance.getLocalTeam();
        var color = (localTeam  == self.teamID ? teamColor : enemyColor);

        // draw health as _ _ _
        // -> one _ per 10% in health color, the rest in black as background
        // -> "" while dead, looks best
        if (self.health > 0) {
            int n = Mathf.RoundToInt(self.HealthPercent() * 10);
            GetComponent<TextMesh>().text = GenerateHealthString(n, color);
        } else GetComponent<TextMesh>().text = "";
        
    }
}
