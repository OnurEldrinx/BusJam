using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class LevelEditorWindow : EditorWindow
    {
        private int _rows = 4;
        private int _columns = 5;
        private GameObject[,] _grid;
        private GameObject _tilePrefab;
        private Tile _selectedTile;
        private Object _newContent;
        private TileContentTypes _contentType;
        private ColorOptions _passengerColor;
        private Obstacle _obstaclePrefab;
        private Passenger _passengerPrefab;
        private Bus _busPrefab;
        private ColorOptions _busColor;

        private Level _level;
        
        [MenuItem("Window/Level Editor")]
        public static void ShowWindow()
        {
            GetWindow<LevelEditorWindow>("Level Editor");
        }

        private void OnGUI()
        {
            
            GUILayout.Label("Game Components", EditorStyles.boldLabel);
            _passengerPrefab = (Passenger)EditorGUILayout.ObjectField("Passenger Prefab", _passengerPrefab, typeof(Passenger), false);
            _obstaclePrefab = (Obstacle)EditorGUILayout.ObjectField("Obstacle Prefab",_obstaclePrefab,typeof(Obstacle),true);
            _tilePrefab = (GameObject)EditorGUILayout.ObjectField("Tile Prefab", _tilePrefab, typeof(GameObject), false);
            _busPrefab = (Bus)EditorGUILayout.ObjectField("Bus Prefab",_busPrefab,typeof(Bus),true);

            GUILayout.Space(10);
            
            GUILayout.Label("Grid Settings", EditorStyles.boldLabel);
            _rows = EditorGUILayout.IntField("Rows", _rows);
            _columns = EditorGUILayout.IntField("Columns", _columns);
            
            
            if (GUILayout.Button("Generate Grid"))
            {
                GenerateGrid();
            }
            
            GUILayout.Space(10);
            
            
            if (_grid != null)
            {
                
                GUILayout.Label("Grid Map", EditorStyles.boldLabel);

                
                for (int i = 0; i < _rows; i++)
                {
                    GUILayout.BeginHorizontal();
                    for (int j = 0; j < _columns; j++)
                    {
                        if (GUILayout.Button($"Tile {i},{j}"))
                        {
                            _selectedTile = _grid[i, j].GetComponent<Tile>();
                            _newContent = _selectedTile.GetContent();
                        }
                    }
                    GUILayout.EndHorizontal();
                }

            }

            GUILayout.Space(10);
            
            if (_selectedTile != null)
            {

                GUILayout.Label("Tile Content Settings", EditorStyles.boldLabel);

                GUILayout.Label($"Selected Tile: {_selectedTile.gameObject.name}", EditorStyles.boldLabel);

                if (_selectedTile.GetContent() != null)
                {
                    GUILayout.Label($"Selected Tile's Content: {_selectedTile.GetContentInfo()}", EditorStyles.boldLabel);
                }
                

                _contentType = (TileContentTypes)EditorGUILayout.EnumPopup("Content Type",_contentType);

                switch (_contentType)
                {
                    case TileContentTypes.Passenger:
                        _passengerColor = (ColorOptions)EditorGUILayout.EnumPopup("Color",_passengerColor);
                        break;
                    case TileContentTypes.Obstacle:
                        break;
                        
                }

                
                //_newContent = EditorGUILayout.ObjectField("Tile Content", _newContent, typeof(TileContent), true);
                if (GUILayout.Button("Apply Content"))
                {
                    ApplyContentToSelectedTile();
                }
            }
            
            GUILayout.Space(10);

            if (_level != null)
            {
                GUILayout.Label("Bus Order Settings", EditorStyles.boldLabel);
            
                _busColor = (ColorOptions)EditorGUILayout.EnumPopup("Bus Color",_busColor);
                if (GUILayout.Button("Insert Bus to Queue"))
                {
                    Bus b = Instantiate(_busPrefab);
                    b.SetColor(_busColor);

                    GameObject busQueue = GameObject.Find("BusQueue");
                    if (busQueue is null)
                    {
                        busQueue = new GameObject("BusQueue");
                        busQueue.transform.parent = _level.transform;
                    }

                    b.transform.parent = busQueue.transform;
                    _level.InsertBus(b);

                }
            }
            
            



        }

        private void GenerateGrid()
        {
            if (_tilePrefab == null)
            {
                Debug.LogError("Please assign a tile prefab.");
                return;
            }

            // Clear the existing grid
            if (_grid != null)
            {
                foreach (var tile in _grid)
                {
                    if (tile != null)
                    {
                        DestroyImmediate(tile.transform.root.gameObject);
                        break;
                    }
                }
            }

            GameObject levelObject = new GameObject("Level");
            _level = levelObject.AddComponent<Level>();
            
            _grid = new GameObject[_rows, _columns];

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    GameObject tileInstance = (GameObject)PrefabUtility.InstantiatePrefab(_tilePrefab);
                    tileInstance.transform.position = new Vector3(j, 0, -i)*0.8f; // Adjust position as needed
                    tileInstance.name = $"Tile_{i}_{j}";
                    _grid[i, j] = tileInstance;
                    tileInstance.transform.parent = levelObject.transform;
                    _level.InsertTile(tileInstance.GetComponent<Tile>());
                }
            }

            levelObject.transform.position -= new Vector3(0, 0.25f, 0);
            levelObject.transform.rotation = Quaternion.Euler(0,-90,0);
        }

        private void ApplyContentToSelectedTile()
        {
            if (_selectedTile != null)
            {
                Undo.RecordObject(_selectedTile, "Change Tile Content");
                //_selectedTile.SetContent((TileContent)_newContent);
                //_selectedTile.GetContent().colorData = _passengerColor;
                //_selectedTile.GetContent().type = _contentType;

                switch (_contentType)
                {
                    case TileContentTypes.Passenger:
                        Passenger p = Instantiate(_passengerPrefab, _selectedTile.transform, true);
                        p.SetColor(_passengerColor);
                        p.type = _contentType;
                        _selectedTile.SetContent(p);

                        var passengerTransform = p.transform;
                        var defaultPos = passengerTransform.localPosition;
                        passengerTransform.localPosition = new Vector3(0,defaultPos.y,0);
                        p.transform.parent = _selectedTile.transform.root;
                        break;
                    case TileContentTypes.Obstacle:
                        Obstacle o = Instantiate(_obstaclePrefab, _selectedTile.transform, true);
                        _selectedTile.gameObject.SetActive(false);
                        _selectedTile.SetContent(o);
                        o.transform.localPosition = Vector3.zero;
                        o.transform.parent = _selectedTile.transform.root;

                        break;
                }
                
                
                
                EditorUtility.SetDirty(_selectedTile);
            }
        }
    }
}
