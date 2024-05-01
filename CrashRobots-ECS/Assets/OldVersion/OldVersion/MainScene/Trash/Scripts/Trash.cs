using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Trash : MonoBehaviour
{
    // Rigitbody�擾�p
    private Rigidbody rigidbody;

    //// velocity�p
    //public Vector3 velocity { get; private set; }

    // TrashOrigin�擾
    [SerializeField, Header("TrashOrigin���A�^�b�`���Ă�������")]
    private GameObject trashOrigin;
    // TrashOrigin�̎q����Transform�擾
    private Transform[] trashOriginChildren;

    // TrashDizision�擾
    [SerializeField, Header("TrashDivision���A�^�b�`���Ă�������")]
    private GameObject[] trashDivision;
    // TrashDivision�̃X�N���v�g�擾
    private TrashDivision[] trashDivisionScript;

    // TrashDirection�����ł����p�x
    [SerializeField, Header("TrashDirection�����ł����p�x�����߂Ă�������")]
    private float flyAngle;
    // TrashDirection�����ł�������
    [SerializeField, Header("TrashDirection�̓����蔻�肪�L���ȋ�����ݒ肵�Ă�������")]
    private float enableColliderDistance;
    // TrashDirection���΂���
    [SerializeField, Header("TrashDirection���΂��͂�ݒ肵�Ă�������")]
    private float flyPower;

    // ��]�̍Œᑬ
    [SerializeField, Header("���[�e�[�V�����̍Œ�X�s�[�h�����߂Ă�������")]
    private float rotationMin;
    // ��]�̍ō���
    [SerializeField, Header("���[�e�[�V�����̍ő�X�s�[�h�����߂Ă�������")]
    private float rotationMax;

    // ����܂ł̎���
    [SerializeField, Header("����܂ł̎��Ԃ�ݒ肵�Ă�������")]
    private float brokenTimer;

    private ScoreManager scoreManager;

    // �q�b�g�������p�擾�p
    public Vector3 hitDirection { get; private set; }

    // Division��Destroy������
    private int divisionDestroyNum;
    // Division��Destroy���Ă���Destroy
    public void DestroyTrash()
    {
        divisionDestroyNum++;
        if (trashDivision.Length == divisionDestroyNum)
            Destroy(gameObject);
    }

    void Start()
    {
        // Rigitbody�擾
        rigidbody = GetComponent<Rigidbody>();

        // ScoreManeger�擾
        scoreManager = ScoreManager.instance;

        // trashOriginChildren�̐������߂�
        trashOriginChildren = new Transform[trashOrigin.transform.childCount];
        // trashOriginChildren�擾
        for (int i = 0; i < trashOrigin.transform.childCount; i++)
        {
            trashOriginChildren[i] = trashOrigin.transform.GetChild(i);
        }

        // trashDivisionScript�̐������߂�
        trashDivisionScript = new TrashDivision[trashDivision.Length];
        // trashDivisionScript�擾
        for (int i = 0; i < trashDivision.Length; i++)
        {
            // TrashDivision�擾
            trashDivisionScript[i] = trashDivision[i].GetComponent<TrashDivision>();

            // ��]���x�ݒ�
            trashDivisionScript[i].RotateSpeed(rotationMax, rotationMin);

            // �����蔻�肪�L���ȋ�����ݒ肵�܂�
            trashDivisionScript[i].SetDistanceFly(enableColliderDistance);

            // ����܂ł̎��Ԃ�ݒ肵�܂�
            trashDivisionScript[i].SetBrokenTime(brokenTimer);

        }
    }

    public void ExplosionTrash()
    {
        // x�������ɑ��x�㏑��
        //var vel = rigidbody.velocity;
        //vel.x = virtualHammer.speed;
        //velocity = vel;

        hitDirection = transform.position - PlayerDataManager.Instance.transform.position;

        // hitDirection���p�x�ɕϊ�
        float hitAngle = Vec3ToAngle(hitDirection);

        // ��ԍŏ��̊p�x�����߂܂�
        float firstAngle = -(trashDivision.Length - 1) * flyAngle * 0.5f;

        // division��origin�Ɠ����ꏊ�A�����p�x�ɂ���
        InitializecTransform();

        for (int i = 0; i < trashDivision.Length; i++)
        {
            float eachAngle = hitAngle + flyAngle * i + firstAngle;

            // ��ԕ����Ɨ͂�ݒ肵�܂�
            trashDivisionScript[i].SetDirection(AngleToVec3(eachAngle), flyPower);
        }

        // trashDivision��ON
        for (int i = 0; i < trashDivision.Length; i++)
            trashDivision[i].SetActive(true);
        // trashOrigin��Off
        trashOrigin.SetActive(false);

        // ��ꂽ����SE�Đ�
        SoundManager.Instance.BrokenSound();
    }

    // division��origin�Ɠ����ꏊ�A�����p�x�ɂ���
    private void InitializecTransform()
    {
        for (int i = 0; i < trashDivision.Length; i++)
        {
            string divisionName = trashDivision[i].transform.name;
            Vector3 divisionPosition = trashDivision[i].transform.position;
            Quaternion divisionRotetion = trashDivision[i].transform.rotation;
            for (int j = 0; j < trashOriginChildren.Length; j++)
            {
                string originName = trashOriginChildren[j].transform.name;
                Vector3 originPosition = trashOriginChildren[i].transform.position;
                Quaternion originRotetion = trashOriginChildren[i].transform.rotation;

                // division��origin�Ɠ����ꏊ�A�����p�x�ɂ���
                if (divisionName == originName)
                {
                    divisionPosition = originPosition;
                    divisionRotetion = originRotetion;
                }
            }
            trashDivision[i].transform.position = divisionPosition;
            trashDivision[i].transform.rotation = divisionRotetion;
        }
    }

        // Vector3���p�x�ɒ����֐�
        float Vec3ToAngle(Vector3 vec)
    {
        float angle = Mathf.Atan2(vec.z, vec.x) * Mathf.Rad2Deg;
        return angle;
    }

    Vector3 AngleToVec3(float angle)
    {
        float rad = angle * Mathf.Deg2Rad; //�p�x�����W�A���p�ɕϊ�

        //rad(���W�A���p)���甭�˗p�x�N�g�����쐬
        Vector3 direction = Vector3.zero;
        direction.z = (float)Math.Sin(rad);
        direction.x = (float)Math.Cos(rad);
        direction.Normalize();

        return direction;
    }
}

