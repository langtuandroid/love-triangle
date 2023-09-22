using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ClickableText : MonoBehaviour, IPointerClickHandler
{
    private TMP_Text textComponent;

    private void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int index = TMP_TextUtilities.FindIntersectingLink(textComponent, Input.mousePosition, null); 
        Debug.LogError(index);


        if (index > -1)
        {
            Application.OpenURL(textComponent.textInfo.linkInfo[index].GetLinkID());
        }

    }
       
} 
