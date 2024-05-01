using Unity.VisualScripting;
using UnityEngine;

namespace ExplosionSample
{
    public class Bomb : MonoBehaviour
    {
        // シングルトン化
        public static Bomb instance;

        [SerializeField, Header("爆発までの時間[s]を設定してください")]
        private float time = 3.0f;

        [SerializeField, Header("爆風のPrefabをアタッチしてください")]
        private Explosion explosionPrefab;

        #region explosion初期化用変数

        [SerializeField, Header("爆風範囲を設定してください")]
        private float explodeRadius;

        [SerializeField,Header("爆風に当たったときに吹っ飛ぶ力の強さを設定してください")]
        private float explosionPower;

        [SerializeField, Header("爆風の判定が実際に発生するまでのディレイを設定してください")]
        private float startDelaySeconds = 0.1f;

        [SerializeField, Header("爆風の持続フレーム数を設定してください")]
        private int durationFrameCount = 1;

        [SerializeField, Header("エフェクト含めすべての再生が終了するまでの時間を設定してください")]
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
            // 爆発を生成
            var explosion = Instantiate(explosionPrefab, explodePos, Quaternion.identity);
            explosion.Inisialized(explosionPower, startDelaySeconds, durationFrameCount, stopSeconds, explodeRadius);
            explosion.Explode();
        }
    }    
}
