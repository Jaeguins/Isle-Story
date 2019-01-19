using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityScroll : MonoBehaviour
{
    public Scrollbar scrollbar;
    private void OnEnable() {
        scrollbar.value = 1;
    }
}
