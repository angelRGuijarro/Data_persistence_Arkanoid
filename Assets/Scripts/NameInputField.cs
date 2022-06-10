using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[RequireComponent(typeof(TMP_InputField))]
public class NameInputField : MonoBehaviour
{
    private void Start()
    {
        TMP_InputField inputField = GetComponent<TMP_InputField>();
        inputField.onEndEdit.AddListener(nameChanged);
    }
    
    public void nameChanged(string name)
    {
        ApplicationManager.userName = name;        
    }
}
