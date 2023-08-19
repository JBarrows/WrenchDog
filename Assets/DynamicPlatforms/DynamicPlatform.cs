using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WrenchDog/DynamicPlatform")]
public class DynamicPlatform : ScriptableObject
{
    public bool isLocked = false;
    public Sprite sprite;
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D boxCollider2D;
    public Rigidbody2D rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
