using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool Passable;
        public CellObject ContainedObject;
    }
    public FoodObject FoodPrefab;
    public WallObject WallPrefab;
    public EXPObject ExpPrefab;
    public EnemyController EnemyObjectControllerPrefab;
    public ExitCellObject ExitCellPrefab;
    private Vector2Int exitCell;
    public ShopObject ShopNpcPrefab;
    [Range(0f, 1f)] public float shopNpcChancePerLevel = 0.30f;
    private CellData[,] m_BoardData;
    private Tilemap m_Tilemap;
    private Grid m_Grid;
    public int Width;
    public int Height;
    public float wallDensity = 0.2f;
    public float foodDensity = 0.1f;
    public float enemyDensity = 0.005f;
    public float expDensity = 0.02f;
    private int area;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;
    public List<Vector2Int> m_EmptyCellsList;
  
   public void Init()
    {
        if(Width>30 || Height>30)
        {
            Width = 30;
            Height = 30;
        }
        area = Width * Height;
        exitCell = new Vector2Int(Width - 2, Height - 2);
        m_Tilemap = GetComponentInChildren<Tilemap>();
        m_Grid = GetComponentInChildren<Grid>();
        //Initialize the list
        m_EmptyCellsList = new List<Vector2Int>();
        
        m_BoardData = new CellData[Width, Height];

        for (int y = 0; y < Height; ++y)
        {
            for(int x = 0; x < Width; ++x)
            {
                Tile tile;
                m_BoardData[x, y] = new CellData();
                
                if(x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
                {
                    tile = WallTiles[Random.Range(0, WallTiles.Length)];
                    m_BoardData[x, y].Passable = false;
                }
                else
                {
                    tile = GroundTiles[Random.Range(0, GroundTiles.Length)];
                    m_BoardData[x, y].Passable = true;
                    
                    //this is a passable empty cell, add it to the list
                    m_EmptyCellsList.Add(new Vector2Int(x, y));
                }
                m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }

        ReserveCells();

        AddObject(Instantiate(ExitCellPrefab), exitCell);

        GenerateWall();
        GenerateFood();
        GenerateEnemy();
        GenerateExp();
        ShopNpcSpawn();
    }

    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
       return m_Grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }
    public Tile GetCellTile(Vector2Int cellIndex)
    {
        return m_Tilemap.GetTile<Tile>(new Vector3Int(cellIndex.x, cellIndex.y, 0));
    }
    public CellData GetCellData(Vector2Int cellIndex)
    {
        if (cellIndex.x < 0 || cellIndex.x >= Width || cellIndex.y < 0 || cellIndex.y >= Height)
        {
           return null;
        }

        return m_BoardData[cellIndex.x, cellIndex.y];
    }
    void GenerateFood()
    {
        int foodCount =  Mathf.Max(1,Mathf.RoundToInt(area * foodDensity));
        
        for (int i = 0; i < foodCount; ++i)
        {
            int randomIndex = Random.Range(0, m_EmptyCellsList.Count);
            Vector2Int coord = m_EmptyCellsList[randomIndex];

            m_EmptyCellsList.RemoveAt(randomIndex);
            FoodObject newFood = Instantiate(FoodPrefab);
            AddObject(newFood, coord);
        }
    }
    void GenerateEnemy()
    {
        int enemyCount =  Mathf.Max(1,Mathf.RoundToInt(area * enemyDensity));

        for (int i = 0; i < enemyCount; ++i)
        {
            int randomIndex = Random.Range(0, m_EmptyCellsList.Count);
            Vector2Int coord = m_EmptyCellsList[randomIndex];

            m_EmptyCellsList.RemoveAt(randomIndex);
            EnemyController newenemy = Instantiate(EnemyObjectControllerPrefab);
            AddObject(newenemy, coord);
            
        }
    }
    void GenerateWall()
    {
        int wallCount = Mathf.RoundToInt(area * wallDensity);

        for (int i = 0; i < wallCount; ++i)
        {
            int randomIndex = Random.Range(0, m_EmptyCellsList.Count);
            Vector2Int coord = m_EmptyCellsList[randomIndex];

            m_EmptyCellsList.RemoveAt(randomIndex);

            WallObject newWall = Instantiate(WallPrefab);
            AddObject(newWall, coord);
        }

    }
    void GenerateExp() 
    {
        int expCount = Mathf.Max(1, Mathf.RoundToInt(area * expDensity));

        for (int i = 0; i < expCount; ++i)
        {
            int randomIndex = Random.Range(0, m_EmptyCellsList.Count);
            Vector2Int coord = m_EmptyCellsList[randomIndex];

            m_EmptyCellsList.RemoveAt(randomIndex);
            EXPObject newEXP = Instantiate(ExpPrefab);
            AddObject(newEXP, coord);

        }
    }
    void ShopNpcSpawn()
    {
        if (Random.value > shopNpcChancePerLevel) return;

        Vector2Int topLeft = new Vector2Int(1, Height - 2);
        AddObject(Instantiate(ShopNpcPrefab), topLeft);
    }
    public void SetCellTile(Vector2Int cellIndex, Tile tile)
    {
        m_Tilemap.SetTile(new Vector3Int(cellIndex.x, cellIndex.y, 0), tile);
    }
    void AddObject(CellObject obj, Vector2Int coord, bool passable = true)
    {
        CellData data = m_BoardData[coord.x, coord.y];

        if(passable)
            data.Passable = true;    
        else
            data.Passable = false;

        obj.transform.position = CellToWorld(coord);
        data.ContainedObject = obj;
        obj.Init(coord);
    }
    void ReserveCells()
    {
        Vector2Int playerSpawn = new Vector2Int(1, 1);
        Vector2Int shopTopLeft = new Vector2Int(1, Height - 2);

        m_EmptyCellsList.Remove(playerSpawn);
        m_EmptyCellsList.Remove(exitCell);
        m_EmptyCellsList.Remove(shopTopLeft);
    }
    public void Clean()
    {
        if(m_BoardData == null) return;

        for (int y = 0; y < Height; ++y)
        {
            for (int x = 0; x < Width; ++x)
            {
                var cellData = m_BoardData[x, y];
                if (cellData.ContainedObject != null)
                {
                    Destroy(cellData.ContainedObject.gameObject);
                }
                SetCellTile(new Vector2Int(x,y), null);
            }
        }
    }
}