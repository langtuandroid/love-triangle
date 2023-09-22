using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;
public class UnlockableItem : MonoBehaviour {

    public static Action onUpdateMoney;
    public static Action<Transform> onUnlock;
    [SerializeField] protected MeshRenderer m_renderer;
    [SerializeField] protected TextMeshProUGUI m_txtPrice;

    public int price;
    protected bool m_isUnlocked;
    private int m_originalPrice;

    public Image imgFill;
    public Image imgBG;
    private string m_saveName => transform.parent.gameObject.name;
    public bool IsUnlocked => m_isUnlocked;

	private void Start() {
        Load();
        UpdatePrice(false);
    }
	public virtual void UnlockItem(bool p_playSFX = true) {
        m_isUnlocked = true;
        m_renderer.gameObject.SetActive(false);
        m_txtPrice.gameObject.SetActive(false);
        price = 0;
        Save();
    }

    private void OnTriggerEnter(Collider other) {
        if (!m_isUnlocked && other.GetComponent<CarCollider>() != null) {
            StartCoroutine(DoBuy());
        }
    }

    private void OnTriggerExit(Collider other) {
        if (!m_isUnlocked && other.GetComponent<CarCollider>() != null) {
            StopAllCoroutines();
            MakeTextWhite();
        }
    }

    IEnumerator DoBuy() {
        float speed = 0.00005f;
        float incrementor = 1;
        int clampAmount = PlayerData.CurrentMoney - price;
        if (clampAmount < 0) {
            clampAmount = 0;
        }
        while (PlayerData.CurrentMoney > 0 && !m_isUnlocked) {
            PlayerData.DecreaseMoney((int)incrementor, true, clampAmount);
            price -= (int)incrementor;
            UpdatePrice();
            if (price <= 0) {
                UnlockItem();
            }
            Save();
            onUpdateMoney?.Invoke();
            yield return new WaitForSeconds(speed);
            speed -= 0.5f;
            speed = Mathf.Clamp(speed, 0f, 10000000f);
            incrementor += 1f;
        }
        if (!m_isUnlocked) {
            if (PlayerData.CurrentMoney <= 0) {
                MakeTextRed();
            }
            else {
                MakeTextWhite();
            }
        }
    }

    public void MakeTextRed() {
        m_txtPrice.color = Color.red;
        imgFill.color = Color.red;
        imgBG.color = Color.red;
    }

    public void MakeTextGreen() {
        m_txtPrice.color = Color.green;
        imgFill.color = Color.white;
        imgBG.color = Color.white;
    }

    public void MakeTextWhite() {
        m_txtPrice.color = Color.white;
        imgFill.color = Color.white;
        imgBG.color = Color.white;
    }

    void UpdatePrice(bool p_playSFX = true) {
        if (m_isUnlocked) {
            UnlockItem(p_playSFX);
        }
        else {
            m_txtPrice.text = LGGUtility.FormatMoney(price);
            imgFill.fillAmount = 1f - ((float)price / (float)m_originalPrice);
        }
        
    }

    public virtual void Load() {
        int inspectorPrice = price;
        price = PlayerPrefs.GetInt(m_saveName, 0);
        if (price == -1) {
            m_isUnlocked = true;
            price = 0;
        } else if (price <= 0 && !m_isUnlocked) {
            m_originalPrice = price = inspectorPrice;
        }
    }

    public virtual void Save() {
        if (price <= 0) {
            PlayerPrefs.SetInt(m_saveName, -1);
            PlayerPrefs.SetInt(m_saveName + "orig", -1);
        }
        else {
            PlayerPrefs.SetInt(m_saveName, price);
            PlayerPrefs.SetInt(m_saveName + "orig", m_originalPrice);
        }
        
    }
}
