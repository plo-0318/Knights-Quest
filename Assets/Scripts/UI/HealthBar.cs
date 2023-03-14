using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    [Header("Player HP")] 
    [SerializeField] private int currentHp;
    [SerializeField] private int maxHp;
    [SerializeField] private Slider currentHealthBar;
    [SerializeField] private Slider updateBar;
    [SerializeField] private TextMeshProUGUI hpLabel;
    
    [Header("Damage Buffer")]
    [SerializeField] private float damageTimeBuffer = 1f;
    private float currentDamageTimeBuffer;
    private bool startDamageBuffer;
    
    [Header("Heal Buffer")]
    [SerializeField] private float healTimeBuffer = 1f;
    private float currentHealTimeBuffer;
    private bool startHealBuffer;

    // Start is called before the first frame update
    void Start() {
        UpdateHpLabel();
        currentHealthBar.value = 1f;
        updateBar.value = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        // Damage Buffer Animation
        if (startDamageBuffer) {
            if (currentDamageTimeBuffer <= 0f) {
                UpdateBufferHpBar();
                currentDamageTimeBuffer = damageTimeBuffer;
                startDamageBuffer = false;
            }
            
            currentDamageTimeBuffer -= Time.deltaTime;
        }
        
        // Heal Buffer Animation
        if (startHealBuffer) {
            if (currentHealTimeBuffer <= 0f) {
                UpdateHpBar();
                currentHealTimeBuffer = healTimeBuffer;
                startHealBuffer = false;
            }
            
            currentHealTimeBuffer -= Time.deltaTime;
        }
    }

    public void DecreaseHealth(int value) 
    {
        startDamageBuffer = true;
        currentDamageTimeBuffer = damageTimeBuffer;
        
        if (currentHp <= value) 
        {
            currentHp = 0;
        }
        else {
            currentHp -= value;
        }

        UpdateHpLabel();
        UpdateHpBar();
    }

    public void IncreaseHealth(int value) 
    {
        startHealBuffer = true;
        currentHealTimeBuffer = healTimeBuffer;
        
        if (value >= maxHp || value + currentHp > maxHp) 
        {
            currentHp = maxHp;
        }
        else 
        {
            currentHp += value;
        }
        
        UpdateHpLabel();
        UpdateBufferHpBar();
    }

    private void UpdateHpLabel() {
        hpLabel.text = $"{currentHp}/{maxHp} HP";
    }
    
    // Calculate the length of the HP bar relative to the max HP and the current HP
    private void UpdateHpBar() 
    {
        LeanTween.cancel(gameObject);
        
        // Current Hp
        LeanTween.value(gameObject, currentHealthBar.value, (float) currentHp/maxHp, 0.5f).setOnUpdate((float val) => {
            currentHealthBar.value = val;
        }).setEaseOutExpo();
    }

    private void UpdateBufferHpBar() {
        LeanTween.cancel(gameObject);
        
        // Animation Buffer Hp Bar
        LeanTween.value(gameObject, updateBar.value, (float) currentHp/maxHp, 0.5f).setOnUpdate((float val) => {
            updateBar.value = val;
        }).setEaseOutExpo();
    }
}
