using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleControl : MonoBehaviour
{
    [SerializeField] private GameObject scrollBar;
    [SerializeField]private Scrollbar handleScrollBar;
    [SerializeField] private CheatController cheatController;
    private bool findHandle = false;
    int i = 0;
    void Start()
    {
        StartCoroutine(HandleMove());
    
    }

    private void Update() 
    {
           
    }

    public IEnumerator HandleMove()
    {
        if(!findHandle)
        {
            while (true && scrollBar == null && cheatController == null)
            {
                yield return new WaitForSeconds(.1f);
                
                cheatController = FindObjectOfType<CheatController>();
                scrollBar = GameObject.FindWithTag("Scrollbar Vertical").gameObject;
                handleScrollBar = scrollBar.GetComponent<Scrollbar>();


                if(scrollBar != null  && cheatController != null)
                {
                    findHandle = true;
                }
            }
        }
        
        else
        {
            while(true)
            {
                yield return new WaitForSeconds(.1f);
                print("cheatController Count : "+ cheatController.PlayerMessageObjects.Count);

                if(cheatController.PlayerMessageObjects.Count > 4 && i < 4)
                {
                    handleScrollBar.value -= .1f;
                    i++;
                }
                else if( i > 3)
                {
                    yield return new WaitForSeconds(.5f);
                    foreach (GameObject gameObject in cheatController.PlayerMessageObjects)
                    {
                        Destroy(gameObject);
                    }
                    cheatController.PlayerMessageObjects.Clear();
                    handleScrollBar.value = 1;
                    i = 0;
                    StopCoroutine(HandleMove());
                    
                }
            }
        }
        
    }

}
