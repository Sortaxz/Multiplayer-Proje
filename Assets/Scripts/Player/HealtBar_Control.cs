using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealtBar_Control : MonoBehaviour
{
    [SerializeField] private Image healtBar;
    private void Awake() 
    {
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void OtherHealtBar(float otherHealt)
    {
        healtBar.fillAmount = otherHealt / 100;
    }
}

