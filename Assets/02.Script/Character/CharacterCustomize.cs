using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomize : MonoBehaviour
{
    [SerializeField] Sprite[] emotionSprites;
    [SerializeField] SpriteRenderer FaceSR;
    [SerializeField] int myFaceSprite;
    Coroutine myEmotionCoroutine;

    public void SetSprite(int _index, Color _color)
    {
        if (_index >= 0)
            FaceSR.sprite = emotionSprites[_index];
        else
            FaceSR.sprite = emotionSprites[0];
        FaceSR.color = Color.white;
    }

    public void SetEmotionFace(int emoIndex)
    {
        if (myEmotionCoroutine != null)
            StopCoroutine(myEmotionCoroutine);
        SetSprite(emoIndex, Color.white);
        myEmotionCoroutine = StartCoroutine(EmotionCoroutine());
    }
    public void ForceEmotionFace(int emoIndex)
    {
        if (myEmotionCoroutine != null)
            StopCoroutine(myEmotionCoroutine);
        SetSprite(emoIndex, Color.white);
    }

    IEnumerator EmotionCoroutine()
    {
        //Sprite _curFace = myFaceSprite;
        yield return new WaitForSeconds(3f);
        SetSprite(myFaceSprite, Color.white);
    }
}
