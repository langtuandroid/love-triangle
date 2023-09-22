using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.UI;

public class AC_EnumPegging : MonoBehaviour
{

    public Text _text;

    public List<string> AllEnum;
    public Button NextBtn, BackBtn;
    private int currentIndex = 0;

    public Action<string> _OnValueChange;
    // Start is called before the first frame update
   public void Initialize(List<string> contant, Action<string> _onValueChange)
    {

        _OnValueChange = _onValueChange;

        AllEnum  = contant;
        NextBtn.onClick.AddListener(NextBtnPresses);
        BackBtn.onClick.AddListener(BackBtnPresses);

        NextBtnPresses();
    }

    private void OnValueChange()
    {
        _text.text = AllEnum[currentIndex].ToString();
        _OnValueChange?.Invoke(AllEnum[currentIndex]);
    }

    private void NextBtnPresses()
    {
        OnValueChange();

        currentIndex++;

        if (currentIndex > AllEnum.Count - 1)
        {
            currentIndex =0;
        }


    }
    private void BackBtnPresses()
    {
        OnValueChange();

        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = AllEnum.Count - 1;
        }

    }
}
