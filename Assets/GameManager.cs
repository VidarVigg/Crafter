using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager INSTANCE;
    [SerializeField]
    private Canvas inventoryCanvas;
    private bool activated;

    private void Awake()
    {
        if (INSTANCE)
        {
            Destroy(gameObject);
        }
        else
        {
            INSTANCE = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!activated)
            {
                inventoryCanvas.enabled = true;
                activated = true;
            }
            else
            {
                inventoryCanvas.enabled = false;
                activated = false;
            }
        }
    }
}
