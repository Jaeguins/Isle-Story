using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public interface ISummary {
    Sprite GetProductSprite();
    int GetTotalPeople();
    int GetNowPeople();
    int GetSparePeople();
    float GetProdPercentage();
    bool IsProducing();
}
public class EntitySummary : MonoBehaviour {
    public ISummary Target;
    public Text PeopleIndicator;
    public Image ProdSprite;
    public Progressbar progressbar;
    public IEnumerator Routine() {
        while (gameObject.activeInHierarchy) {
            PeopleIndicator.text = string.Concat(new object[] { Target.GetSparePeople(), " / ", Target.GetNowPeople(), " / ", Target.GetTotalPeople() });
            bool res = Target.IsProducing();
            ProdSprite.gameObject.SetActive(res);
            progressbar.gameObject.SetActive(res);
            if (res) {
                ProdSprite.sprite = Target.GetProductSprite();
                progressbar.Value = Target.GetProdPercentage();
            }
            yield return new WaitForSeconds(0.33f);
        }
    }
    public void OnEnable() {
        StartCoroutine(Routine());
    }
}
