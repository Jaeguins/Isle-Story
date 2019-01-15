using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FixedPanel : EntityPanel{
    public Text Name, Type, Status;
    public override void Bind(Entity entity) {
        base.Bind(entity);
        Name.text = target.UIName;
        Type.text = target.UIType;
        Status.text = target.UIStatus;
    }
    public override void Clear() {
        base.Clear();
        Name.text = Strings.NaN;
        Type.text = Strings.NaN;
        Status.text = Strings.NaN;
    }
}
