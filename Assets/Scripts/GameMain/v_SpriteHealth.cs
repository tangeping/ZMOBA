using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using KBEngine;

public class v_SpriteHealth : MonoBehaviour
{
    private GameEntity iChar;
    public Slider healthSlider;
    public Slider damageDelay;    
    public float smoothDamageDelay;
    public Text damageCounter;
    public float damageCounterTimer = 1.5f;
    public Image FillImage;

    private bool inDelay;
    private float damage;
    private float currentSmoothDamage;
    private bool Init = false;

    void Start()
    {       
	    iChar = transform.GetComponentInParent<GameEntity>();
        if (iChar == null)
        {
            Debug.LogWarning("The character must have a ICharacter Interface");
            Destroy(this.gameObject);
        }
    }

    void InitAtribute()
    {
        if(iChar&& iChar.teamID >0 && iChar.healthMax>1 && !Init)
        {
            Init = true;

            healthSlider.maxValue = iChar.healthMax;
            healthSlider.value = healthSlider.maxValue;
            string imageFile = (int)SpaceData.Instance.getLocalTeam() != iChar.teamID ? "UI/dfxuetiao" : "UI/dfxuetiao1";
            FillImage.sprite = Resources.Load<Sprite>(imageFile);

            damageDelay.maxValue = iChar.healthMax;
            damageDelay.value = healthSlider.maxValue;
            damageCounter.text = string.Empty;

            Debug.Log(iChar.name + ".team:" + iChar.teamID + ",getLocalTeam:" + SpaceData.Instance.getLocalTeam());
        }
    }

    void Update()
    {
        InitAtribute();
        SpriteBehaviour();
    }

    void SpriteBehaviour()
    {
        if (Camera.main != null && Init)
        {
            transform.LookAt(Camera.main.transform.position, Vector3.up);

            if (iChar == null || iChar.health <= 0)
            {
                /*Destroy(gameObject);*/
            }

            healthSlider.value = iChar.health;
            damageCounter.text = iChar.health.ToString();
        }
    }

    public void Damage(float value)
    {
        try
        {
            healthSlider.value -= value;

            this.damage += value;
            damageCounter.text = damage.ToString("00");
            if (!inDelay)
                StartCoroutine(DamageDelay());
        }
        catch
        {
            Destroy(this);
        }        
    }

    IEnumerator DamageDelay()
    {
        inDelay = true;       
        
        while(damageDelay.value > healthSlider.value)
        {           
            damageDelay.value -= smoothDamageDelay;
            yield return null;
        }
        inDelay = false;        
        damage = 0;
        yield return new WaitForSeconds(damageCounterTimer);
        damageCounter.text = string.Empty;
    }
}
