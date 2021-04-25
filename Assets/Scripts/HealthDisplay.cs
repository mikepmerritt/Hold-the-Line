using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{

    private Unit Parent;
    public GameObject HealthPrefab;
    private GameObject HealthPoint1, HealthPoint2;
    private bool Started = false;

    public void StartUp()
    {
        Started = true;
        Parent = GetComponentInParent<Unit>();

        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
        canvas.sortingOrder = 5;
        RectTransform rt = canvas.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(0.25f, 0.25f);
        
        GameObject container = new GameObject("Container");
        container.transform.SetParent(canvas.transform);
        HorizontalLayoutGroup grp = container.AddComponent<HorizontalLayoutGroup>();
        RectTransform containerRT = container.GetComponent<RectTransform>();
        containerRT.position = new Vector3(containerRT.position.x, containerRT.position.y - 0.4384f, containerRT.position.z);
        containerRT.sizeDelta = new Vector2(0.75f, 0.25f);

        grp.childAlignment = TextAnchor.MiddleCenter;
        grp.spacing = 0.125f;
        grp.childControlHeight = false;
        grp.childControlWidth = false;
        grp.childForceExpandHeight = false;
        grp.childForceExpandWidth = false;

        HealthPoint1 = Instantiate(HealthPrefab);
        HealthPoint2 = Instantiate(HealthPrefab);
        HealthPoint1.transform.SetParent(container.transform);
        HealthPoint2.transform.SetParent(container.transform);
        HealthPoint1.SetActive(false);
        HealthPoint2.SetActive(false);
    }

    private void Update()
    {
        if (Started) {
            if (Parent.Health == 1)
            {
                HealthPoint1.SetActive(true);
                HealthPoint2.SetActive(false);
            }
            else 
            {
                HealthPoint1.SetActive(true);
                HealthPoint2.SetActive(true);
            }
        }
    }

}