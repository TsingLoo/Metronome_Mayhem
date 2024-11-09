using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Group12;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class ActionFx : MonoBehaviour
{
    public Image _image;
    public TMP_Text _text; 

    private Vector3 originalImgPos;
    private Vector3 originalTxtPos;
    
    private void Awake()
    {
        _image.color = new Color(1f, 1f, 1f, 0f);
        originalImgPos = _image.transform.localPosition;
        originalTxtPos = _text.transform.localPosition;
        
    }

    private void Reset()
    {
        _image.color = new Color(1f, 1f, 1f, 0f);
        _image.transform.localScale = 0.01f * Vector3.one;
        _image.transform.DORotate(new Vector3(0f, 0f, 0), 0f, RotateMode.Fast);
        
        _text.transform.localScale = 0.01f * Vector3.one;
        _text.text = "";
    }

    private void OnEnable()
    {

    }

    public void DoFx(timingGrade grade)
    {
        Color color = FxManager.Instance.GetColorByGrade(grade);
        
        
        Reset();
        DOTween.Kill(this.gameObject.GetHashCode());
        Sequence seq = DOTween.Sequence().SetId(this.gameObject.GetHashCode());
        
        seq.Join(_text.transform.DOLocalMove( originalTxtPos + new Vector3((Random.value -0.5f) * 200f, Random.value * 30f, 0f ), 0.4f));
        seq.Join(_text.transform.DORotate(new Vector3(0f, 0f, 730f +  Random.value * 30f ), 0f, RotateMode.Fast));
        _text.text = FxManager.Instance.GetStringByGrade(grade);
        _text.color = color;
        
        seq.Join(_image.DOColor(color, 0.1f));
        seq.Join(_text.transform.DOScale(1f, 0.1f));
        seq.Append(_image.transform.DOScale(0.5f, 0.2f));
        seq.Append(_image.DOFade(1f, 0.1f));
        seq.Join(_image.transform.DORotate(new Vector3(0f, 0f, 710f +  Random.value * 30f ), 0.5f, RotateMode.LocalAxisAdd));
        seq.Join(_image.transform.DOLocalMove( originalImgPos + new Vector3((Random.value -0.5f) * 40f, Random.value * 30f, 0f ), 0.4f));
        seq.Append(_image.DOFade(0f, 0.5f));
        
        seq.Play() .OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

    private void OnDisable()
    {
        DOTween.Kill(this.gameObject.GetHashCode());
    }
}
