using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3 : MonoBehaviour
{
    public GameObject attackRange;

    [SerializeField]
    float moveSpeed = 10f;
    float rotateSpeed = 10f;

    public float radius = 10.0f;
    public float power = 100.0f;

    [SerializeField] ParticleSystem hit_particle;
    void Update()
    {
        //���������Ɛ��������̓��͂��擾���A���ꂼ��̈ړ����x��������B
        float Xvalue = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float Yvalue = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        //���݂�X,Z�x�N�g���ɏ�̏����Ŏ擾�����l��n���B
        Vector3 movedir = new Vector3(Xvalue, 0, Yvalue);

        //���ݒn�ɏ�Ŏ擾�������l�𑫂��Ĉړ�����B
        transform.position += movedir;

        //�i�ޕ����Ɋ��炩�Ɍ����B
        transform.forward = Vector3.Slerp(transform.forward, movedir, Time.deltaTime * rotateSpeed);
    }
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (other.gameObject.tag == "Can")
            {

                // �͂𔭐�������ꏊ
                Vector3 explosionPos = attackRange.transform.position;
                explosionPos.y = 0;
                attackRange.transform.position = explosionPos;

                // ���S�_����ݒ肵�����a�̒��ɂ���collider�̔z����擾����B
                Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

                foreach (Collider hit in colliders)
                {
                    // �͂��y�ڂ������I�u�W�F�N�g��Rigidbody���t���Ă��邱�Ƃ��K�v�i�|�C���g�j
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        // �擾����Rigidbody�ɗ͂�������
                        // �R�̈����i������͂̋����A�͂̒��S�_�A�͂��y�ڂ����a�j
                        rb.AddExplosionForce(power, explosionPos, 5.0f);
                        Instantiate(hit_particle, explosionPos, Quaternion.identity); //�p�[�e�B�N���p�Q�[���I�u�W�F�N�g����
                    }
                }
            }
        }
    }
}