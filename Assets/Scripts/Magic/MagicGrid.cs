using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MagicGrid : MonoBehaviour, INodeObserver
{
    [SerializeField]
    public Canvas magicCanvas;

    [SerializeField]
    private int height = 5;
    [SerializeField]
    private int width = 5;
    [SerializeField]
    private LineRenderer lineRenderer;
    public static Node firstNode;
    [SerializeField]
    private Node centerNode;
    [SerializeField]
    private List<Node> activeNodes = new List<Node>();
    [SerializeField]
    private List<int> activatedNodes = new List<int>();
    [SerializeField]
    private Transform nodeGridParent;
    private static int nodeCount = 0;
    [SerializeField]
    public SpellDatabase spellDatabase = new SpellDatabase();
    public string combination;

    private void Start()
    {
        DrawNodes();
        nodeGridParent.gameObject.SetActive(false);
    }
    internal void ActivateGrid()
    {
        nodeGridParent.position = InputManager.INSTANCE.GetMousePosition();
        nodeGridParent.transform.position = InputManager.INSTANCE.GetMousePosition();
        nodeGridParent.gameObject.SetActive(true);
    }


    private void DrawNodes()
    {

        int startX = width / 2;
        int startY = height / 2;


        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {

                Node clone = Instantiate(centerNode, Vector3.zero, Quaternion.identity, nodeGridParent);
                clone.transform.position = new Vector3((nodeGridParent.position.x - startX) + i, (nodeGridParent.position.y - startY) + j, 0);
                clone.Observer = this;
                activeNodes.Add(clone);
                Node.index += 1;
                clone.ShowIndex(Node.index);

            }

        }
    }

    internal void DisableGrid()
    {
        CheckCombo();
        nodeGridParent.gameObject.SetActive(false);
        firstNode = null;
        nodeCount = 0;
        lineRenderer.positionCount = 1;
        activatedNodes.Clear();
        combination = null;

        for (int i = 0; i < activeNodes.Count; i++)
        {
            activeNodes[i].activated = false;
        }
    }
    public void Interacted(Node node)
    {
        combination += node.indexForShow;
        lineRenderer.positionCount++;
        if (firstNode == null)
        {
            firstNode = node;
            lineRenderer.SetPosition(0, firstNode.transform.position);
        }
        nodeCount++;
        lineRenderer.SetPosition(nodeCount, node.transform.position);

    }
    public void CheckCombo()
    {
        for (int i = 0; i < spellDatabase.spells.Length; i++)
        {
            if(combination == spellDatabase.spells[i].combination)
            {
                Debug.Log(spellDatabase.spells[i].name + " was Cast ");
            }
        } 

    }
}
