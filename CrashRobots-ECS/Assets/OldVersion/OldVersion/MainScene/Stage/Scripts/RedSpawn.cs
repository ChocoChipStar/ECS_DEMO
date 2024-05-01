using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSpawn : MonoBehaviour
{
    public MeshRenderer mr;

    //�I�u�W�F�N�g�v���n�u
    public GameObject PlayerObject;

    public GameObject refrigeratorPrefab;
    public GameObject carPrefab;

    GameObject[] tagObjects;

    //�G�������ԊԊu
    [SerializeField]
    private float interval = 3;
    //�o�ߎ���
    private float time = 0f;

    public int upperlimit = 30;

    private float carpetSizX;
    private float carpetSizZ;

    private float mww = 3.5f; // �ǂƂ̗]��(margin with wall)

    private float cameraX = 16;
    private float cameraY = 9;

    float cameraDistance;

    private bool dis;
    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        mr.enabled = false;

        carpetSizX = transform.lossyScale.x;
        carpetSizZ = transform.lossyScale.z;

        cameraDistance = Mathf.Sqrt(Mathf.Pow(cameraX, 2.0f) + Mathf.Pow(cameraY, 2)); // ��(cameraX^2 + cameraY^2)

    }

    // Update is called once per frame
    void Update()
    {
        tagObjects = GameObject.FindGameObjectsWithTag("CanBreakTrash");
        if (tagObjects.Length <= upperlimit)
        {
            //���Ԍv��
            time += Time.deltaTime;

            //�o�ߎ��Ԃ��������ԂɂȂ����Ƃ�(�������Ԃ��傫���Ȃ����Ƃ�)
            if (time >= interval)
            {
                // �����|�W�V����
                Vector3 pos = Vector3.zero;

                // �I�u�W�F�N�g���J���������ǂ���
                if (IsObjectOnScreenOll()) dis = false;
                else
                {
                    // �J�����O�ɂȂ�܂Ń����_���T������
                    while (IsInScreen(pos))
                        pos = RandomPos(pos);

                    dis = true;
                }

                if (dis)
                {
                    RedSpownInstatiate(pos);
                }
                dis = false;
                //�o�ߎ��Ԃ����������čēx���Ԍv�����n�߂�
                time = 0f;
            }
        }
    }

    // ���̃I�u�W�F�N�g���[����[�܂ŃJ���������̔���
    private bool IsObjectOnScreenOll()
    {
        // �I�u�W�F�N�g��position�擾
        var positon = transform.position;
        // �I�u�W�F�N�g�̊p�܂ł̋���
        var cornerDistance = transform.lossyScale / 2;

        // �E��̊p����
        var righitUpCorner = positon + cornerDistance;
        righitUpCorner.x -= mww;
        righitUpCorner.y -= mww;
        var isRightUpCornerOnScreen = IsInScreen(righitUpCorner);
        // �����̊p����
        var leftDownCorner = positon - cornerDistance;
        leftDownCorner.x += mww;
        leftDownCorner.y = -leftDownCorner.y + mww;
        var isLeftDownCornerOnScreen = IsInScreen(leftDownCorner);

        // ���̃I�u�W�F�N�g���[����[�܂ŃJ���������̔���
        var isObjectOnScreenOll = isRightUpCornerOnScreen && isLeftDownCornerOnScreen;

        return isObjectOnScreenOll;
    }

    // �����_���ȃX�|�[���|�W�V�����K��
    private Vector3 RandomPos(Vector3 pos)
    {
        float carpetX = Random.Range(-carpetSizX / 2 + mww, carpetSizX / 2 - mww);
        float carpetZ = Random.Range(-carpetSizZ / 2 + mww, carpetSizZ / 2 - mww);
        pos = new Vector3(transform.position.x + carpetX, transform.position.y, transform.position.z + carpetZ);
        //float distance = Vector3.Distance(PlayerObject.transform.position, pos);

        return pos;
    }

    // �J����������
    private bool IsInScreen(Vector3 pos)
    {
        // �J����position�擾
        var camera = Camera.main.transform;
        // �J�����̌���
        var cameraDir = camera.forward;

        // pos���X�N���[�����W�ɂ���
        var screenPos = Camera.main.WorldToScreenPoint(pos);
        // ��ʓ������肷��
        bool ouScreenX = 0 <= screenPos.x && screenPos.x <= Screen.width;
        bool ouScreenY = 0 <= screenPos.y && screenPos.y <= Screen.height;
        bool onScreen = ouScreenY && ouScreenX;

        // �J��������I�u�W�F�N�g�ւ̌���
        var posDir = pos - camera.position;
        // pos���J�����̑O�ɂ��邩�̔���
        bool cameraFront = Vector3.Dot(cameraDir, posDir) > 0;

        // ��ʓ����J�����̑O����
        var onScreenAndCameraFront = onScreen && cameraFront;

        return onScreenAndCameraFront;
    }

    // �X�|�[��������
    private void RedSpownInstatiate(Vector3 pos)
    {
        float rotateY = Random.Range(0, 360);

        float probability = Random.Range(1, 11);

        if (probability <= 4)
        {
            pos.y = 1.5f;
            //enemy���C���X�^���X������(��������)
            GameObject trash = Instantiate(refrigeratorPrefab, pos, Quaternion.Euler(-90, rotateY, 0));
        }
        else
        {
            //enemy���C���X�^���X������(��������)
            GameObject trash = Instantiate(carPrefab, pos, Quaternion.Euler(0, rotateY, 0));
        }
    }

}
