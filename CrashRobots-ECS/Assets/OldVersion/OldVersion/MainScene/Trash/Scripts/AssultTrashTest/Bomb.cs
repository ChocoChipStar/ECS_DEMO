using Unity.VisualScripting;
using UnityEngine;

namespace ExplosionSample
{
    public class Bomb : MonoBehaviour
    {
        // �V���O���g����
        public static Bomb instance;

        [SerializeField, Header("�����܂ł̎���[s]��ݒ肵�Ă�������")]
        private float time = 3.0f;

        [SerializeField, Header("������Prefab���A�^�b�`���Ă�������")]
        private Explosion explosionPrefab;

        #region explosion�������p�ϐ�

        [SerializeField, Header("�����͈͂�ݒ肵�Ă�������")]
        private float explodeRadius;

        [SerializeField,Header("�����ɓ��������Ƃ��ɐ�����ԗ͂̋�����ݒ肵�Ă�������")]
        private float explosionPower;

        [SerializeField, Header("�����̔��肪���ۂɔ�������܂ł̃f�B���C��ݒ肵�Ă�������")]
        private float startDelaySeconds = 0.1f;

        [SerializeField, Header("�����̎����t���[������ݒ肵�Ă�������")]
        private int durationFrameCount = 1;

        [SerializeField, Header("�G�t�F�N�g�܂߂��ׂĂ̍Đ����I������܂ł̎��Ԃ�ݒ肵�Ă�������")]
        private float stopSeconds = 2f;

        #endregion

        private void Awake()
        {
            //Invoke(nameof(Explode), time);
            Singltonization();
        }

        private void Singltonization()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        public void Explode(Vector3 explodePos)
        {
            // �����𐶐�
            var explosion = Instantiate(explosionPrefab, explodePos, Quaternion.identity);
            explosion.Inisialized(explosionPower, startDelaySeconds, durationFrameCount, stopSeconds, explodeRadius);
            explosion.Explode();
        }
    }    
}
