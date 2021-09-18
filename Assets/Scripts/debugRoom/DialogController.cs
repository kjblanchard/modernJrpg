using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    public TMP_Text textBoxToUpdate;

    public string currentText { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void UpdateDialog()
    {
        textBoxToUpdate.text = currentText;
    }
}
