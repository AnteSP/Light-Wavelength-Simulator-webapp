using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameIndic : MonoBehaviour
{
    static TextMeshProUGUI T;

    static float iniX;
    // Start is called before the first frame update
    void Start()
    {
        T = GetComponent<TextMeshProUGUI>();
        iniX = transform.localPosition.x;
    }

    private void Update()
    {
        transform.parent.position = Input.mousePosition;
    }

    public static void Indicate(string S)
    {

        T.text = S;

        T.transform.localPosition = new Vector3((Screen.width / 2) - Input.mousePosition.x < 0 ? -iniX : iniX, T.transform.localPosition.y, T.transform.localPosition.z);

        T.alignment = (Screen.width / 2) - Input.mousePosition.x < 0 ? TextAlignmentOptions.Right : TextAlignmentOptions.Left;
    }
}
