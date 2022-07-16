using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomize : MonoBehaviour
{
    public enum CustomType {
        Hair,
        BackHair,
        Face
    }

    [SerializeField] bool randomCustom;

    [SerializeField] Sprite[] hairSprites;
    [SerializeField] Sprite[] backHairSprites;
    [SerializeField] Sprite[] emotionSprites;
    [SerializeField] SpriteRenderer HairSR;
    [SerializeField] SpriteRenderer BackHairSR;
    [SerializeField] SpriteRenderer FaceSR;
    [SerializeField] int myFaceSprite;
    Coroutine myEmotionCoroutine;

    private void Start()
    {
        if (randomCustom)
        {
            Color hairColor = new Color(Random.value, Random.value, Random.value);
            SetSprite(CustomType.Hair, Random.Range(0, hairSprites.Length), hairColor);
            hairColor -= Color.white * 0.1f;
            SetSprite(CustomType.BackHair, Random.Range(0, backHairSprites.Length), hairColor);
        }
    }

    public void SetSprite(CustomType customType,int _index, Color _color)
    {
        switch (customType)
        {
            case CustomType.Hair:
                if (_index >= 0)
                    HairSR.sprite = hairSprites[_index];
                else
                    HairSR.sprite = hairSprites[0];
                HairSR.color = _color;
                break;
            case CustomType.BackHair:
                if (_index >= 0)
                    BackHairSR.sprite = backHairSprites[_index];
                else
                    BackHairSR.sprite = backHairSprites[0];
                BackHairSR.color = _color;
                break;
            case CustomType.Face:
                if (_index >= 0)
                    FaceSR.sprite = emotionSprites[_index];
                else
                    FaceSR.sprite = emotionSprites[0];
                FaceSR.color = Color.white;
                break;
        }
    }

    public void SetEmotionFace(int emoIndex)
    {
        if (myEmotionCoroutine != null)
            StopCoroutine(myEmotionCoroutine);
        SetSprite(CustomType.Face,emoIndex, Color.white);
        myEmotionCoroutine = StartCoroutine(EmotionCoroutine());
    }
    public void ForceEmotionFace(int emoIndex)
    {
        if (myEmotionCoroutine != null)
            StopCoroutine(myEmotionCoroutine);
        SetSprite(CustomType.Face,emoIndex, Color.white);
    }

    IEnumerator EmotionCoroutine()
    {
        //Sprite _curFace = myFaceSprite;
        yield return new WaitForSeconds(3f);
        SetSprite(CustomType.Face,myFaceSprite, Color.white);
    }
}
