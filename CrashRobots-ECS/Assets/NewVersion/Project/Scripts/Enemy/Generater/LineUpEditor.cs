using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public partial class LineUpEditor : EditorWindow
{
    [Tooltip("隊列のエネミー数を代入")]
    private int enemiesArray = 0;

    [Tooltip("隊列のエネミー種類を代入　/　要素数は隊列エネミー数")]
    private int[] enemyTypeIndex = new int[0];

    [Tooltip("隊列のエネミーオブジェクトを代入")]
    private GameObject[] lineUpObjects = new GameObject[0];

    [Tooltip("隊列のエネミー生成座標を代入")]
    private Vector3[] GeneratePositions = new Vector3[0];

    [Tooltip("エディタウィンドウがアクティブ状態かの判定")]
    private bool isWindowActive = false;

    [Tooltip("エディタウィンドウの座標")]
    private Vector2 scrollPosition = Vector2.zero;
    
    private GameObject[] instantiatePrefabs = new GameObject[0];

    [Tooltip("")]
    private GameObject[] displayedObj = new GameObject[0];
    [Tooltip("")]
    private bool isDisplayed = false;
    [Tooltip("")]
    private bool isSaved = false;

    private int instanceCounter = 0;

    private float gridSize = 1f;
    private string[] type = { "ロケットパンチエネミー", "回転攻撃エネミー", "盾持ちエネミー" };

    [MenuItem("Generate Settings/隊列設定【エネミー】")] // ヘッダメニュー名/ヘッダ以下のメニュー名
    private static void ShowWindow()
    {
        var window = GetWindow<LineUpEditor>("UIElements");
        window.titleContent = new GUIContent("エネミー隊列プロパティ"); // エディタ拡張ウィンドウのタイトル
        window.Show();
    }

    [SerializeField] private VisualTreeAsset _rootVisualTreeAsset;
    [SerializeField] private StyleSheet _rootStyleSheet;

    private void OnEnable()
    {
        EditorApplication.update += OnEditorUpdate;
    }

    private void OnDisable()
    {
        EditorApplication.update -= OnEditorUpdate;
    }

    private void OnGUI()
    {
        using (var scrollView = new EditorGUILayout.ScrollViewScope(scrollPosition))
        {
            scrollPosition = scrollView.scrollPosition;

            InitializeLineUpSystem();

            InputPropertySystem();

            if (GUI.changed)
                UpdateEditorWindowValue();

            if (!isWindowActive)
                FixedEditorInputPosition();
        }
    }

    private void OnDestroy()
    {
        ExitAutoSave();
    }

    private void OnEditorUpdate()
    {
        isWindowActive = this == EditorWindow.focusedWindow;
    }

    /// <summary>
    /// 指定した要素数【enemiesIndex】を基に隊列プロパティベースの初期化処理を行います
    /// </summary>
    private void InitializeLineUpSystem()
    {
        EditorGUILayout.LabelField("隊列を作成");

        enemiesArray = EditorGUILayout.IntField("隊列のエネミー数", enemyTypeIndex != null ? enemyTypeIndex.Length : 0);

        if (enemiesArray <= 0)
            enemiesArray = EditorPrefs.GetInt("EnemiesIndexKey", enemiesArray); // EditorWindowを再起動した時に前回保存していた値に変更

        if (enemyTypeIndex == null || enemyTypeIndex.Length != enemiesArray) // IntFieldの値が更新されたかを確認
        {
            enemyTypeIndex = new int[enemiesArray];

            DestroyLineUpObjects(lineUpObjects); // 古い要素数とオブジェクトを初期化
            lineUpObjects = new GameObject[enemiesArray]; // 新たな要素数を代入

            GeneratePositions = new Vector3[enemiesArray];
            instantiatePrefabs = new GameObject[type.Length];

            GeneratePositionBaseObjects();

            SetSavedPrefabs();

            SetSavedPositionsAndTypes();
        }
    }

    /// <summary>
    /// 座標のベースとなるオブジェクトを作成する処理を行います
    /// </summary>
    private void GeneratePositionBaseObjects()
    {
        for (int i = 0; i < enemiesArray; i++)
        {
            // エネミー数を基に生成座標オブジェクトを生成
            lineUpObjects[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            lineUpObjects[i].transform.SetParent(GameObject.Find("LineUpManager").transform.GetChild(0).transform);
            lineUpObjects[i].name = "Base LineUp" + i;
        }
    }

    /// <summary>
    /// ゲームに登場するプレファブを再代入する処理を行います
    /// </summary>
    private void SetSavedPrefabs()
    {
        for (int i = 0; i < instantiatePrefabs.Length; i++)
        {
            string savedObjectPath = EditorPrefs.GetString($"GeneratePath{i}", "");
            instantiatePrefabs[i] = AssetDatabase.LoadAssetAtPath<GameObject>(savedObjectPath);
        }
    }

    /// <summary>
    /// 前回入力した座標と種類を再代入する処理を行います
    /// </summary>
    private void SetSavedPositionsAndTypes()
    {
        for (int i = 0; i < enemiesArray; ++i)
        {
            // エネミー数を基にVector3の座標登録フィールドを生成
            GeneratePositions[i].x = EditorPrefs.GetFloat($"PositionXKey{i}", GeneratePositions[i].x);
            GeneratePositions[i].y = EditorPrefs.GetFloat($"PositionYKey{i}", GeneratePositions[i].y);
            GeneratePositions[i].z = EditorPrefs.GetFloat($"PositionZKey{i}", GeneratePositions[i].z);

            // 保存されたエネミータイプを代入
            enemyTypeIndex[i] = EditorPrefs.GetInt($"SelectTypeValue{i}", 100);

            lineUpObjects[i].transform.position = GeneratePositions[i];
        }
    }

    /// <summary>
    /// 隊列プロパティの入力フィールドを作成します
    /// </summary>
    private void InputPropertySystem()
    {
        GUILayout.Space(20f);

        for (int i = 0; i < 3; ++i)
            instantiatePrefabs[i] = EditorGUILayout.ObjectField("ゲームに登場される敵を代入", instantiatePrefabs[i], typeof(GameObject), false) as GameObject;

        GUILayout.Space(20f);

        for (int i = 0; i < enemiesArray; i++)
        {
            GUILayout.Label("隊列エネミー" + (i + 1) + "体目");

            EditorGUILayout.BeginHorizontal(); // BeginとEndで囲った間の行は一行で表示される

            GUILayout.Label("座標を指定");

            GeneratePositions[i] = EditorGUILayout.Vector3Field("", GeneratePositions[i]);

            enemyTypeIndex[i] = EditorGUILayout.Popup("", enemyTypeIndex[i], type);

            EditorGUILayout.EndHorizontal(); // End

            GUILayout.Space(20f);
        }

        if (GUILayout.Button("グリッド位置にオブジェクトを移動する"))
        {
            for (int i = 0; i < enemiesArray; ++i)
            {
                lineUpObjects[i].transform.position = new Vector3(
                    Mathf.Round(lineUpObjects[i].transform.position.x / gridSize) * gridSize,
                    Mathf.Round(lineUpObjects[i].transform.position.y / gridSize) * gridSize,
                    Mathf.Round(lineUpObjects[i].transform.position.z / gridSize) * gridSize
                );

                GeneratePositions[i] = lineUpObjects[i].transform.position;
            }
        }

        if (GUILayout.Button("選択オブジェクトの座標をリセット"))
        {
            if (Selection.activeGameObject == null)
                return;

            if (Selection.gameObjects.Length > 1)
            {
                foreach (GameObject selectedObject in Selection.gameObjects)
                    ResetSelectObjectsPosition(selectedObject);

                return;
            }

            ResetSelectObjectsPosition(Selection.activeGameObject);
        }

        DrawPrefabsSystem();

        if(GUILayout.Button("表示したプレファブを保存"))
        {
            isSaved = true;
            displayedObj = new GameObject[enemiesArray];
        }
    }

    /// <summary>
    /// 選択されたオブジェクトの座標をリセットする処理を実行します
    /// </summary>
    /// <param name="selectedObject">選択されたオブジェクトを代入</param>
    private void ResetSelectObjectsPosition(GameObject selectedObject)
    {
        for (int i = 0; i < lineUpObjects.Length; ++i)
        {
            if (selectedObject.name == "Base LineUp" + i)
            {
                lineUpObjects[i].transform.position = Vector3.zero;
                GeneratePositions[i] = Vector3.zero;
            }
        }
    }

    /// <summary>
    /// プレファブを表示する処理を行います
    /// </summary>
    private void DrawPrefabsSystem()
    {
        if (GUILayout.Button("プレファブを表示"))
        {
            if (isDisplayed)
                return;

            displayedObj = new GameObject[enemiesArray];
            isDisplayed = true;

            for (int i = 0; i < displayedObj.Length; ++i)
            {
                displayedObj[instanceCounter] = PrefabUtility.InstantiatePrefab(instantiatePrefabs[enemyTypeIndex[i]]) as GameObject;
                displayedObj[instanceCounter].transform.position = GeneratePositions[i];
                instanceCounter++;
            }
        }

        if (GUILayout.Button("プレファブを非表示"))
        {
            if (!isDisplayed)
                return;

            instanceCounter = 0;
            isDisplayed     = false;

            DestroyLineUpObjects(displayedObj);
        }
    }

    /// <summary>
    /// エディタウィンドウ上で変更が入った際に登録座標を更新します
    /// </summary>
    private void UpdateEditorWindowValue()
    {
        for (int i = 0; i < enemiesArray; i++)
            lineUpObjects[i].transform.position = GeneratePositions[i];

        for (int i = 0; i < enemiesArray; ++i)
        {
            EditorPrefs.SetFloat($"PositionXKey{i}", GeneratePositions[i].x);
            EditorPrefs.SetFloat($"PositionYKey{i}", GeneratePositions[i].y);
            EditorPrefs.SetFloat($"PositionZKey{i}", GeneratePositions[i].z);

            GeneratePositions[i] = lineUpObjects[i].transform.position;
        }
    }

    /// <summary>
    /// エディタ上で変更した座標をエディタウィンドウに反映する処理を実行します
    /// </summary>
    private void FixedEditorInputPosition()
    {
        if (Selection.activeTransform == null || Selection.activeGameObject == null)
            return;

        Repaint();

        if (Selection.gameObjects.Length > 1) // 複数オブジェクトを選択していた場合
        {
            foreach(GameObject selectedObject in Selection.gameObjects)
            {
                for (int i = 0; i < lineUpObjects.Length; ++i)
                {
                    if (selectedObject.name == "Base LineUp" + i)
                    {
                        lineUpObjects[i].transform.position = selectedObject.transform.position;
                        GeneratePositions[i] = selectedObject.transform.position;
                    }
                }
            }
        }
        else // 単一のオブジェクトを選択していた場合
        {
            for (int i = 0; i < lineUpObjects.Length; ++i)
            {
                if (Selection.activeGameObject.name == "Base LineUp" + i)
                {
                    lineUpObjects[i].transform.position = Selection.activeGameObject.transform.position;
                    GeneratePositions[i] = Selection.activeGameObject.transform.position;
                }
            }
        }
    }

    /// <summary>
    /// エディタウィンドウ終了時の処理を実行します
    /// </summary>
    private void ExitAutoSave()
    {
        EditorPrefs.SetInt("EnemiesIndexKey", enemiesArray); // 隊列エネミーの数を保存

        for (int i = 0; i <  enemiesArray; ++i)
            EditorPrefs.SetInt($"SelectTypeValue{i}", enemyTypeIndex[i]);

        for (int i = 0; i < instantiatePrefabs.Length; i++) // 代入したエネミープレファブを保存
            EditorPrefs.SetString($"GeneratePath{i}", AssetDatabase.GetAssetPath(instantiatePrefabs[i]));

        DestroyLineUpObjects(lineUpObjects);
        DestroyLineUpObjects(displayedObj);
    }

    /// <summary>
    /// 隊列オブジェクトの削除処理を行います
    /// </summary>
    private void DestroyLineUpObjects(GameObject[] destroyObjects)
    {
        if (destroyObjects != null)
        {
            // エネミー数を基に隊列オブジェクトを削除
            for (int i = 0; i < destroyObjects.Length; i++)
            {
                if (destroyObjects[i] != null)
                    GameObject.DestroyImmediate(destroyObjects[i]); // GameObjectはコード上に必要
            }
        }
    }
}