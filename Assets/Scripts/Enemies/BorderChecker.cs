using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BorderChecker : MonoBehaviour
{
    [SerializeField]
    public GameObject player;
    public Tilemap tileMap;

    
    // Start is called before the first frame update
    void Start()
    {
        BoundsInt mapBounds = tileMap.cellBounds;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
