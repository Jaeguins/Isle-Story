using UnityEngine;
using UnityEngine.UI;
using System.Collections;
[SerializeField]
public interface ISummary {
    Sprite GetProductSprite();
    int GetTotalPeople();
    int GetNowPeople();
    int GetSparePeople();
    float GetProdPercentage();
    bool IsProducing();
}
public class EntitySummary : MonoBehaviour {
    public Entity entity;
    public Coroutine coroutine;
    ISummary Target;
    public Text PeopleIndicator;
    public Image ProdSprite;
    public Progressbar progressbar;
    public IEnumerator Routine() {
        while (gameObject.activeInHierarchy) {
            if (Target == null) {
                if (entity && entity is ISummary) Target = entity as ISummary;
                yield return new WaitForSeconds(0.33f);
                continue;
            }
            int spare = Target.GetSparePeople(), now = Target.GetNowPeople(), total = Target.GetTotalPeople();
            PeopleIndicator.text = string.Concat(new object[] { spare==-1?"":spare as object, " / ", now == -1 ? "" : now as object, " / ", total == -1 ? "" : total as object });
            bool res = Target.IsProducing();
            ProdSprite.gameObject.SetActive(res);
            progressbar.gameObject.SetActive((entity as Building).UnderConstruct);
            if (res) ProdSprite.sprite = Target.GetProductSprite();
            if((entity as Building).UnderConstruct) progressbar.Value = Target.GetProdPercentage();
            yield return new WaitForSeconds(0.33f);
        }
    }
    public void OnEnable() {
        coroutine=StartCoroutine(Routine());
    }
    public void OnDisable() {
        StopCoroutine(coroutine);
    }
}
