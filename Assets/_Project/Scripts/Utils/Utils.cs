using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static void RemoveAllChild(Transform component) {
        int childCount = component.childCount;
        for(int i = childCount - 1; i >= 0; i--) {
            DestroyImmediate(component.GetChild(i).gameObject);
        }
    }
}
