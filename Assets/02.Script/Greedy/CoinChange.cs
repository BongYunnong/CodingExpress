using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinChange : CustomGreedy
{
    public int compare(CoinInfo a, CoinInfo b)
    {
        return b.ChangeValue.CompareTo(a.ChangeValue);
    }

    [System.Serializable]
    public class CoinInfo {
        public GameObject CoinPrefab;
        public int ChangeValue;
    }
    [SerializeField] int randomCoinCount;
    [SerializeField] Vector2 randomCoinValMinMax;
    [SerializeField] Vector2 randomChangeMinMax;

    [SerializeField] List<CoinInfo> coinInfos;

    [SerializeField] GameObject initialCoinObj;
    List<GameObject> CoinObjs=new List<GameObject>();
    [SerializeField] int initialChange = 20;
    private int remainChange = 20;
    const float coinSpacing = 1.5f;


    [SerializeField] Transform initialCoinTR;
    [SerializeField] Transform finishedTR;

    [SerializeField] GameObject initialTable;
    [SerializeField] GameObject finishTable;

    [SerializeField] Text InfoTxt;

    public override void InitializeGreedy(Character _Character)
    {
        base.InitializeGreedy(_Character);
        StopCoroutine("ProceedCoroutine");
        StopCoroutine("InitializeCoroutine");
        StartCoroutine("InitializeCoroutine");
    }
    IEnumerator InitializeCoroutine()
    {
        if (randomize)
        {
            initialChange = (int)Random.Range(randomChangeMinMax.x, randomChangeMinMax.y); 
            coinInfos.Clear();
            for(int i=0;i< randomCoinCount; i++)
            {
                CoinInfo tmpCoinInfo = new CoinInfo();
                tmpCoinInfo.ChangeValue = (int)Random.Range(randomCoinValMinMax.x, randomCoinValMinMax.y);
                coinInfos.Add(tmpCoinInfo);
            }
        }
        remainChange = initialChange;
        InfoTxt.text = "[RemainChange]\n\n" + remainChange;

        initialTable.SetActive(true);
        finishTable.SetActive(true);

        coinInfos.Sort(compare);
        initialCoinTR.position = new Vector3(-(coinInfos.Count / 2f) * coinSpacing, initialCoinTR.position.y, initialCoinTR.position.z);
        finishedTR.position = new Vector3(-(coinInfos.Count / 2f) * coinSpacing, finishedTR.position.y, finishedTR.position.z);
        for (int i=0;i< coinInfos.Count; i++)
        {
            GameObject tmpCoin = Instantiate(initialCoinObj,
                initialCoinTR.position + Vector3.up * 1.5f + Vector3.right * coinSpacing * i
                , Quaternion.identity);
            tmpCoin.transform.localScale = Vector3.one * Mathf.Sqrt(Mathf.Sqrt(coinInfos[i].ChangeValue));
            tmpCoin.transform.localPosition += Vector3.up * Mathf.Sqrt(Mathf.Sqrt(coinInfos[i].ChangeValue)) / 2f;
            tmpCoin.GetComponentInChildren<Text>().text = coinInfos[i].ChangeValue.ToString();
            tmpCoin.GetComponentInChildren<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value);

            tmpCoin.transform.name = coinInfos[i].ChangeValue.ToString();
            tmpCoin.gameObject.SetActive(true);
            coinInfos[i].CoinPrefab = tmpCoin;
            CoinObjs.Add(tmpCoin);
            yield return new WaitForSeconds(0.5f);
        }

        initialized = true;
    }
    public override void StartGreedy()
    {
        base.StartGreedy();
        StartCoroutine("ProceedCoroutine");
    }

    IEnumerator ProceedCoroutine()
    {
        yield return new WaitForSeconds(1f);

        int count = 0;

        int depth = 0;
        for(int i=0; i< coinInfos.Count; i++)
        {
            if(remainChange/coinInfos[i].ChangeValue > 0)
            {
                depth++;
                int mul = (remainChange / coinInfos[i].ChangeValue);
                remainChange -= mul * coinInfos[i].ChangeValue;
                InfoTxt.text += "\n- " + mul + " * "+ coinInfos[i].ChangeValue +" = "+remainChange;
                count += mul;

                for (int j = 0; j < mul; j++)
                {
                    myCharacter.transform.position = coinInfos[i].CoinPrefab.transform.position;
                    GameObject tmpCoin = Instantiate(
                        coinInfos[i].CoinPrefab,
                        finishedTR.position +Vector3.up*1.5f+ Vector3.back * coinSpacing * (depth) + Vector3.right * coinSpacing * j,
                        Quaternion.identity);
                    tmpCoin.SetActive(true);

                    CoinObjs.Add(tmpCoin);
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }
        print(count);
        proceeding = false;
    }

    public override void ResetGreedy()
    {
        base.ResetGreedy();
        StopCoroutine("ProceedCoroutine");
        StopCoroutine("InitializeCoroutine");
        for (int i=0;i< CoinObjs.Count; i++)
        {
            Destroy(CoinObjs[i]);
        }
        initialTable.SetActive(false);
        finishTable.SetActive(false);

        CoinObjs.Clear();
    }
}
