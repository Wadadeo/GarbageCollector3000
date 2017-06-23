using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NObjects.Persistent;

public class ElementEntry : MonoBehaviour
{
    [Header ("UI Elements")]
    [SerializeField]        private Image icon;
    [SerializeField]        private Text descriptionText;
    
    public void Populate(SpaceObjectData objectData)
    {
        descriptionText.text = objectData.name + ":\n<i>" + objectData.objectDescription + "</i>";
        icon.sprite = objectData.objectIcon;
    }

}
