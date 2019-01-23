using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceWindow : MonoBehaviour {
    public List<ResourceDiff> diffs;
    private void LateUpdate() {
        foreach (ResourceDiff diff in diffs) {
            Resource resource;
            switch (diff.cat) {
                case 1:
                    resource = ResourceManager.Instance.itemResources[diff.type];
                    diff.Bind((ItemType)diff.type, resource.Amount, resource.MaxAmount);
                    break;
                case 2:
                    resource = ResourceManager.Instance.totalResources[diff.type];
                    diff.Bind((ResourceType)diff.type, resource.Amount, resource.MaxAmount);
                    break;
                default:
                    Debug.LogError("Invalid resource type");
                    break;
            }
        }
    }
}
