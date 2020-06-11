using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPoint : MonoBehaviour {

    //属性
    private SpriteRenderer spriteRenderer;
    public GridState gridState;
    public GridIndex gridIndex;

    //资源
    private Sprite gridSprite;//格子图片资源
    private Sprite monsterPathSprite;//怪物路点图片资源
    public GameObject[] itemPrefabs;//道具数组
    public GameObject currentItem;//当前格子持有道具

    //格子状态
    public struct GridState
    {
        public bool canBuild;
        public bool isMonsterPoint;
        public bool hasItem;
        public int itemID;
    }

    //格子索引
    public struct GridIndex
    {
        public int xIndex;
        public int yIndex;
    }

  

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gridSprite= Resources.Load<Sprite>("Pictures/NormalMordel/Game/Grid");
        monsterPathSprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/1/Monster/6-1");
        itemPrefabs = new GameObject[10];
        string prefabsPath = "Prefabs/Game/" + MapMaker.Instance.bigLevelID.ToString()+"/Item/";
        for (int i = 0; i < itemPrefabs.Length; i++)
        {
            itemPrefabs[i] = Resources.Load<GameObject>(prefabsPath + i);
            if (!itemPrefabs[i])
            {
                Debug.Log("加载失败，失败路径："+prefabsPath+i); 
            }
        }
        InitGrid();
    }

    public void InitGrid()
    {
        gridState.canBuild = true;
        gridState.isMonsterPoint = false;
        gridState.hasItem = false;
        spriteRenderer.enabled = true;
        gridState.itemID = -1;
        spriteRenderer.sprite = gridSprite;
        Destroy(currentItem); 
    }

    private void OnMouseDown()
    {
        //怪物路点
        if (Input.GetKey(KeyCode.P))
        {
            gridState.canBuild = false;
            spriteRenderer.enabled = true;
            gridState.isMonsterPoint = !gridState.isMonsterPoint;
            if (gridState.isMonsterPoint)//是怪物路点
            {
                MapMaker.Instance.monsterPath.Add(gridIndex);
                spriteRenderer.sprite = monsterPathSprite;
            }
            else//不是怪物路点
            {
                MapMaker.Instance.monsterPath.Remove(gridIndex);
                spriteRenderer.sprite = gridSprite;
                gridState.canBuild = true;
            }
        }
        //道具
        else if (Input.GetKey(KeyCode.I))
        {
            gridState.itemID++;
            //当前格子从持有道具状态转化为没有道具
            if (gridState.itemID==itemPrefabs.Length)
            {
                gridState.itemID = -1;
                Destroy(currentItem);
                gridState.hasItem = false;
                return;
            }
            if (currentItem==null)
            {
                //产生道具
                CreateItem();
            }
            else//本身就有道具
            {
                Destroy(currentItem);
                //产生道具
                CreateItem();
            }
            gridState.hasItem = true;
        }


        else if (!gridState.isMonsterPoint)
        {
            gridState.isMonsterPoint = false;
            gridState.canBuild = !gridState.canBuild;
            if (gridState.canBuild)
            {
                spriteRenderer.enabled = true;
            }
            else
            {
                spriteRenderer.enabled = false;
            }
        }
    }

    //生成道具
    private void CreateItem()
    {
        Vector3 creatPos = transform.position;
        if (gridState.itemID<=2)
        {
            creatPos += new Vector3(MapMaker.Instance.gridWidth,-MapMaker.Instance.gridHeight)/2;
        }
        else if (gridState.itemID<=4)
        {
            creatPos += new Vector3(MapMaker.Instance.gridWidth,0)/2;
        }
        GameObject itemGo = Instantiate(itemPrefabs[gridState.itemID],creatPos,Quaternion.identity);
        currentItem = itemGo;
    }

    //更新格子状态
    public void UpdateGrid()
    {
        if (gridState.canBuild)
        {
            spriteRenderer.sprite = gridSprite;
            spriteRenderer.enabled = true;
            if (gridState.hasItem)
            {
                CreateItem();
            }
        }
        else
        {
            if (gridState.isMonsterPoint)
            {
                spriteRenderer.sprite = monsterPathSprite;
            }
            else
            {
                spriteRenderer.enabled = false;
            }
       
        }
    }
}
