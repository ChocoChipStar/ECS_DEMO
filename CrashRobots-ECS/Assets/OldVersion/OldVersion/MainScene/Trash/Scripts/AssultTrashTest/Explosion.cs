using System.Collections;
using UnityEngine;

namespace ExplosionSample
{
    public class Explosion : MonoBehaviour
    {
        // �����ɓ��������Ƃ��ɐ�����ԗ͂̋���
        private float explosionPower;

        // �����̔��肪���ۂɔ�������܂ł̃f�B���C
        private float startDelaySeconds = 0.1f;

        // �����̎����t���[����
        private int durationFrameCount = 1;

        // �G�t�F�N�g�܂߂��ׂĂ̍Đ����I������܂ł̎���
        private float stopSeconds = 2f;

        //[SerializeField] private ParticleSystem _effect;

        [SerializeField] private AudioSource sfx;

        [SerializeField] private SphereCollider collider;

        // ������
        public void Inisialized(float explosionPower, float startDelaySeconds, int durationFrameCount, float stopSeconds,float explodeRadius)
        {
            collider.radius = explodeRadius;
            this.explosionPower = explosionPower; 
            this.startDelaySeconds = startDelaySeconds; 
            this.durationFrameCount = durationFrameCount; 
            this.stopSeconds = stopSeconds;
        }

        private void Awake()
        {
            //_effect.Stop();
            sfx.Stop();
            collider.enabled = false;
        }

        /// <summary>
        /// ���j����
        /// </summary>
        public void Explode()
        {
            // �����蔻��Ǘ��̃R���[�`��
            StartCoroutine(ExplodeCoroutine());
            // �����G�t�F�N�g�܂߂Ă������������R���[�`��
            StartCoroutine(StopCoroutine());

            // �G�t�F�N�g�ƌ��ʉ��Đ�
            //_effect.Play();
            sfx.Play();
        }

        private IEnumerator ExplodeCoroutine()
        {
            // �w��b�����o�߂���܂�FixedUpdate��ő҂�
            var delayCount = Mathf.Max(0, startDelaySeconds);
            while (delayCount > 0)
            {
                yield return new WaitForFixedUpdate();
                delayCount -= Time.fixedDeltaTime;
            }

            // ���Ԍo�߂�����R���C�_��L�������Ĕ����̓����蔻�肪�o��
            collider.enabled = true;

            // ���t���[�����L����
            for (var i = 0; i < durationFrameCount; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            // �����蔻�薳����
            collider.enabled = false;
        }

        private IEnumerator StopCoroutine()
        {
            // ���Ԍo�ߌ�ɏ���
            yield return new WaitForSeconds(stopSeconds);
            //_effect.Stop();
            sfx.Stop();
            collider.enabled = false;

            Destroy(gameObject);
        }

        /// <summary>
        /// �����Ƀq�b�g�����Ƃ��ɑ�����ӂ��Ƃ΂�����
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                // �ՓˑΏۂ�Rigidbody�̔z���ł��邩�𒲂ׂ�
                var rigidBody = other.GetComponentInParent<Rigidbody>();

                // Rigidbody�����ĂȂ��Ȃ琁����΂Ȃ��̏I���
                if (rigidBody == null) return;

                // �����ɂ���Ĕ����������琁����ԕ����̃x�N�g�������
                var direction = (other.transform.position - transform.position).normalized;

                // ������΂�
                // ForceMode��ς���Ƌ������ς��i����͎��ʖ����j
                rigidBody.AddForce(direction * explosionPower, ForceMode.Impulse);
            }
        }
    }
}
