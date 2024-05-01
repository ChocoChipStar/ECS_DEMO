using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public partial class LineUpEditor : EditorWindow
{
    [Tooltip("����̃G�l�~�[������")]
    private int enemiesArray = 0;

    [Tooltip("����̃G�l�~�[��ނ����@/�@�v�f���͑���G�l�~�[��")]
    private int[] enemyTypeIndex = new int[0];

    [Tooltip("����̃G�l�~�[�I�u�W�F�N�g����")]
    private GameObject[] lineUpObjects = new GameObject[0];

    [Tooltip("����̃G�l�~�[�������W����")]
    private Vector3[] GeneratePositions = new Vector3[0];

    [Tooltip("�G�f�B�^�E�B���h�E���A�N�e�B�u��Ԃ��̔���")]
    private bool isWindowActive = false;

    [Tooltip("�G�f�B�^�E�B���h�E�̍��W")]
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
    private string[] type = { "���P�b�g�p���`�G�l�~�[", "��]�U���G�l�~�[", "�������G�l�~�[" };

    [MenuItem("Generate Settings/����ݒ�y�G�l�~�[�z")] // �w�b�_���j���[��/�w�b�_�ȉ��̃��j���[��
    private static void ShowWindow()
    {
        var window = GetWindow<LineUpEditor>("UIElements");
        window.titleContent = new GUIContent("�G�l�~�[����v���p�e�B"); // �G�f�B�^�g���E�B���h�E�̃^�C�g��
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
    /// �w�肵���v�f���yenemiesIndex�z����ɑ���v���p�e�B�x�[�X�̏������������s���܂�
    /// </summary>
    private void InitializeLineUpSystem()
    {
        EditorGUILayout.LabelField("������쐬");

        enemiesArray = EditorGUILayout.IntField("����̃G�l�~�[��", enemyTypeIndex != null ? enemyTypeIndex.Length : 0);

        if (enemiesArray <= 0)
            enemiesArray = EditorPrefs.GetInt("EnemiesIndexKey", enemiesArray); // EditorWindow���ċN���������ɑO��ۑ����Ă����l�ɕύX

        if (enemyTypeIndex == null || enemyTypeIndex.Length != enemiesArray) // IntField�̒l���X�V���ꂽ�����m�F
        {
            enemyTypeIndex = new int[enemiesArray];

            DestroyLineUpObjects(lineUpObjects); // �Â��v�f���ƃI�u�W�F�N�g��������
            lineUpObjects = new GameObject[enemiesArray]; // �V���ȗv�f������

            GeneratePositions = new Vector3[enemiesArray];
            instantiatePrefabs = new GameObject[type.Length];

            GeneratePositionBaseObjects();

            SetSavedPrefabs();

            SetSavedPositionsAndTypes();
        }
    }

    /// <summary>
    /// ���W�̃x�[�X�ƂȂ�I�u�W�F�N�g���쐬���鏈�����s���܂�
    /// </summary>
    private void GeneratePositionBaseObjects()
    {
        for (int i = 0; i < enemiesArray; i++)
        {
            // �G�l�~�[������ɐ������W�I�u�W�F�N�g�𐶐�
            lineUpObjects[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            lineUpObjects[i].transform.SetParent(GameObject.Find("LineUpManager").transform.GetChild(0).transform);
            lineUpObjects[i].name = "Base LineUp" + i;
        }
    }

    /// <summary>
    /// �Q�[���ɓo�ꂷ��v���t�@�u���đ�����鏈�����s���܂�
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
    /// �O����͂������W�Ǝ�ނ��đ�����鏈�����s���܂�
    /// </summary>
    private void SetSavedPositionsAndTypes()
    {
        for (int i = 0; i < enemiesArray; ++i)
        {
            // �G�l�~�[�������Vector3�̍��W�o�^�t�B�[���h�𐶐�
            GeneratePositions[i].x = EditorPrefs.GetFloat($"PositionXKey{i}", GeneratePositions[i].x);
            GeneratePositions[i].y = EditorPrefs.GetFloat($"PositionYKey{i}", GeneratePositions[i].y);
            GeneratePositions[i].z = EditorPrefs.GetFloat($"PositionZKey{i}", GeneratePositions[i].z);

            // �ۑ����ꂽ�G�l�~�[�^�C�v����
            enemyTypeIndex[i] = EditorPrefs.GetInt($"SelectTypeValue{i}", 100);

            lineUpObjects[i].transform.position = GeneratePositions[i];
        }
    }

    /// <summary>
    /// ����v���p�e�B�̓��̓t�B�[���h���쐬���܂�
    /// </summary>
    private void InputPropertySystem()
    {
        GUILayout.Space(20f);

        for (int i = 0; i < 3; ++i)
            instantiatePrefabs[i] = EditorGUILayout.ObjectField("�Q�[���ɓo�ꂳ���G����", instantiatePrefabs[i], typeof(GameObject), false) as GameObject;

        GUILayout.Space(20f);

        for (int i = 0; i < enemiesArray; i++)
        {
            GUILayout.Label("����G�l�~�[" + (i + 1) + "�̖�");

            EditorGUILayout.BeginHorizontal(); // Begin��End�ň͂����Ԃ̍s�͈�s�ŕ\�������

            GUILayout.Label("���W���w��");

            GeneratePositions[i] = EditorGUILayout.Vector3Field("", GeneratePositions[i]);

            enemyTypeIndex[i] = EditorGUILayout.Popup("", enemyTypeIndex[i], type);

            EditorGUILayout.EndHorizontal(); // End

            GUILayout.Space(20f);
        }

        if (GUILayout.Button("�O���b�h�ʒu�ɃI�u�W�F�N�g���ړ�����"))
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

        if (GUILayout.Button("�I���I�u�W�F�N�g�̍��W�����Z�b�g"))
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

        if(GUILayout.Button("�\�������v���t�@�u��ۑ�"))
        {
            isSaved = true;
            displayedObj = new GameObject[enemiesArray];
        }
    }

    /// <summary>
    /// �I�����ꂽ�I�u�W�F�N�g�̍��W�����Z�b�g���鏈�������s���܂�
    /// </summary>
    /// <param name="selectedObject">�I�����ꂽ�I�u�W�F�N�g����</param>
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
    /// �v���t�@�u��\�����鏈�����s���܂�
    /// </summary>
    private void DrawPrefabsSystem()
    {
        if (GUILayout.Button("�v���t�@�u��\��"))
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

        if (GUILayout.Button("�v���t�@�u���\��"))
        {
            if (!isDisplayed)
                return;

            instanceCounter = 0;
            isDisplayed     = false;

            DestroyLineUpObjects(displayedObj);
        }
    }

    /// <summary>
    /// �G�f�B�^�E�B���h�E��ŕύX���������ۂɓo�^���W���X�V���܂�
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
    /// �G�f�B�^��ŕύX�������W���G�f�B�^�E�B���h�E�ɔ��f���鏈�������s���܂�
    /// </summary>
    private void FixedEditorInputPosition()
    {
        if (Selection.activeTransform == null || Selection.activeGameObject == null)
            return;

        Repaint();

        if (Selection.gameObjects.Length > 1) // �����I�u�W�F�N�g��I�����Ă����ꍇ
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
        else // �P��̃I�u�W�F�N�g��I�����Ă����ꍇ
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
    /// �G�f�B�^�E�B���h�E�I�����̏��������s���܂�
    /// </summary>
    private void ExitAutoSave()
    {
        EditorPrefs.SetInt("EnemiesIndexKey", enemiesArray); // ����G�l�~�[�̐���ۑ�

        for (int i = 0; i <  enemiesArray; ++i)
            EditorPrefs.SetInt($"SelectTypeValue{i}", enemyTypeIndex[i]);

        for (int i = 0; i < instantiatePrefabs.Length; i++) // ��������G�l�~�[�v���t�@�u��ۑ�
            EditorPrefs.SetString($"GeneratePath{i}", AssetDatabase.GetAssetPath(instantiatePrefabs[i]));

        DestroyLineUpObjects(lineUpObjects);
        DestroyLineUpObjects(displayedObj);
    }

    /// <summary>
    /// ����I�u�W�F�N�g�̍폜�������s���܂�
    /// </summary>
    private void DestroyLineUpObjects(GameObject[] destroyObjects)
    {
        if (destroyObjects != null)
        {
            // �G�l�~�[������ɑ���I�u�W�F�N�g���폜
            for (int i = 0; i < destroyObjects.Length; i++)
            {
                if (destroyObjects[i] != null)
                    GameObject.DestroyImmediate(destroyObjects[i]); // GameObject�̓R�[�h��ɕK�v
            }
        }
    }
}