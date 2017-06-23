using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NObjects.Persistent;

public class HowToPlayMenu : MonoBehaviour
{
    [Header ("Parameters")]
    [SerializeField]        private RectTransform panel;
    [SerializeField]        private ElementEntry entryPrefab;
    [SerializeField]        private SpaceObjectDataHandler objectList;

	void Start ()
    {
        ElementEntry entry;

        foreach (SpaceObjectData obj in objectList.GetObjects())
        {
            entry = Instantiate(entryPrefab, panel);
            entry.Populate(obj);
        }
	}
}
