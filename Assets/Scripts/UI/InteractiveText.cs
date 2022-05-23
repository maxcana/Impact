using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractiveText : MonoBehaviour
{
    TextMeshProUGUI textMesh;
    TextMeshProUGUI currentLink;
    private void Awake()
    {
        currentLink = GameObject.Find("Current Link").GetComponent<TextMeshProUGUI>();
        textMesh = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        int i = TMP_TextUtilities.FindIntersectingLink(textMesh, Input.mousePosition, null);
        if (i != -1)
        {
            TMP_LinkInfo info = textMesh.textInfo.linkInfo[i];
            string link = info.GetLinkID();
            currentLink.text = link;
            currentLink.color += new Color(0, 0, 0, functions.valueMoveTowards(currentLink.color.a, 0.24f, 10));
            if (Input.GetMouseButtonDown(0))
            {
                if (link.Contains("https://"))
                {
                    Application.OpenURL(link);
                }
                else
                {
                    Debug.LogError("i almost ran a maybe virus (uh oh)");
                }

            }
        } else {
            currentLink.color += new Color(0, 0, 0, functions.valueMoveTowards(currentLink.color.a, 0f, 10));
        }
    }
}
