using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    //This Node class is used to build the tree structure 

    //Every tree has a root node and that root node will have child nodes.

    //Every child node has a parent node and the parent node can only have 2 children

    private List<Node> childrenNodeList;

    public List<Node> ChildrenNodeList { get => childrenNodeList; }

    public bool Visted {get; set;}
    public Vector2Int BottomLeftAreaCorner { get; set; }

    public Vector2Int BottomRightAreaCorner { get; set; }

    public Vector2Int TopLeftAreaCorner { get; set; }

    public Vector2Int TopRightAreaCorner { get; set; }


    public Node Parent { get; set; }

    public int TreeLayerIndex { get; set; }

    public Node(Node parentNode) 
    { 
        childrenNodeList = new List<Node>();
        this.Parent = parentNode;
        if (parentNode != null)
        {
            parentNode.AddChild(this);
        }
    }

    public void AddChild(Node node)
    {
        childrenNodeList.Add(node);
    }

    public void RemoveChild(Node node)
    {
        childrenNodeList.Remove(node); 
    }
} 