using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortestJobFirst : CustomGreedy
{
    public int compare(CustomerInfo a, CustomerInfo b)
    {
        return a.burstTime.CompareTo(b.burstTime);
    }

    [System.Serializable]
    public class CustomerInfo
    {
        public GameObject CustomerObj;
        public int burstTime;
        public float waitingTime;
    }
    [SerializeField] int randomCustomerCount;
    [SerializeField] Vector2 randomBurstTimeMinMax;

    public GameObject CustomerPrefab;
    [SerializeField] List<CustomerInfo> customerInfos;

    const float customerSpacing = 1.5f;

    [SerializeField] Transform workingTR;
    [SerializeField] Transform finishedTR;

    public override void InitializeGreedy(Character _Character)
    {
        base.InitializeGreedy(_Character);
        StopCoroutine("ProceedCoroutine");
        StopCoroutine("InitializeCoroutine");
        StartCoroutine("InitializeCoroutine");
    }
    IEnumerator InitializeCoroutine()
    {
        workingTR.gameObject.SetActive(true);
        if (randomize)
        {
            customerInfos.Clear();
            for (int i = 0; i < randomCustomerCount; i++)
            {
                CustomerInfo tmpCustomerInfo = new CustomerInfo();
                tmpCustomerInfo.burstTime = (int)Random.Range(randomBurstTimeMinMax.x, randomBurstTimeMinMax.y);
                customerInfos.Add(tmpCustomerInfo);
            }
        }

        for (int i = 0; i < customerInfos.Count; i++)
        {
            customerInfos[i].waitingTime = customerInfos[i].burstTime;
            customerInfos[i].CustomerObj = Instantiate(CustomerPrefab, this.transform.position + Vector3.left * customerSpacing * i, Quaternion.identity);
            customerInfos[i].CustomerObj.GetComponent<CharacterCanvas>().SetActiveInfoPanel(true);
            customerInfos[i].CustomerObj.GetComponent<CharacterCanvas>().SetInfoTxt(customerInfos[i].waitingTime.ToString());
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
        customerInfos.Sort(compare);

        yield return new WaitForSeconds(1f);

        customerInfos[0].CustomerObj.transform.position = workingTR.position;
        float count = customerInfos[0].waitingTime;

        float tmpTime = customerInfos[0].waitingTime;
        while (tmpTime > 0)
        {
            tmpTime -= Time.deltaTime;
            customerInfos[0].CustomerObj.GetComponent<CharacterCanvas>().SetInfoTxt(tmpTime.ToString("F2"));
            yield return new WaitForSeconds(Time.deltaTime);
        }
        customerInfos[0].CustomerObj.transform.position = finishedTR.position;
        customerInfos[0].CustomerObj.GetComponent<CharacterCanvas>().SetActiveInfoPanel(false);

        for (int i = 1; i < customerInfos.Count; i++)
        {
            customerInfos[i].CustomerObj.transform.position = workingTR.position;
            tmpTime = customerInfos[i].waitingTime;
            while (tmpTime > 0)
            {
                tmpTime -= Time.deltaTime;
                customerInfos[i].CustomerObj.GetComponent<CharacterCanvas>().SetInfoTxt(tmpTime.ToString("F2"));
                yield return new WaitForSeconds(Time.deltaTime);
            }
            customerInfos[i].CustomerObj.GetComponent<CharacterCanvas>().SetActiveInfoPanel(false);
            customerInfos[i].waitingTime = customerInfos[i-1].waitingTime + customerInfos[i].waitingTime;
            count += customerInfos[i].waitingTime;
            customerInfos[i].CustomerObj.transform.position = finishedTR.position+Vector3.back*i* customerSpacing;

            yield return new WaitForSeconds(1f);
        }

        print(count);
        proceeding = false;
    }
    public override void ResetGreedy()
    {
        base.ResetGreedy();
        StopCoroutine("ProceedCoroutine");
        StopCoroutine("InitializeCoroutine");
        workingTR.gameObject.SetActive(false);
        for (int i = 0; i < customerInfos.Count; i++)
        {
            Destroy(customerInfos[i].CustomerObj);
            customerInfos[i].waitingTime = 0;
        }
    }
}
