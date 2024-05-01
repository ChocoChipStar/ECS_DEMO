using System.Collections;
using UnityEngine;

namespace ExplosionSample
{
    public class Explosion : MonoBehaviour
    {
        // 爆風に当たったときに吹っ飛ぶ力の強さ
        private float explosionPower;

        // 爆風の判定が実際に発生するまでのディレイ
        private float startDelaySeconds = 0.1f;

        // 爆風の持続フレーム数
        private int durationFrameCount = 1;

        // エフェクト含めすべての再生が終了するまでの時間
        private float stopSeconds = 2f;

        //[SerializeField] private ParticleSystem _effect;

        [SerializeField] private AudioSource sfx;

        [SerializeField] private SphereCollider collider;

        // 初期化
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
        /// 爆破する
        /// </summary>
        public void Explode()
        {
            // 当たり判定管理のコルーチン
            StartCoroutine(ExplodeCoroutine());
            // 爆発エフェクト含めてもろもろを消すコルーチン
            StartCoroutine(StopCoroutine());

            // エフェクトと効果音再生
            //_effect.Play();
            sfx.Play();
        }

        private IEnumerator ExplodeCoroutine()
        {
            // 指定秒数が経過するまでFixedUpdate上で待つ
            var delayCount = Mathf.Max(0, startDelaySeconds);
            while (delayCount > 0)
            {
                yield return new WaitForFixedUpdate();
                delayCount -= Time.fixedDeltaTime;
            }

            // 時間経過したらコライダを有効化して爆発の当たり判定が出る
            collider.enabled = true;

            // 一定フレーム数有効化
            for (var i = 0; i < durationFrameCount; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            // 当たり判定無効化
            collider.enabled = false;
        }

        private IEnumerator StopCoroutine()
        {
            // 時間経過後に消す
            yield return new WaitForSeconds(stopSeconds);
            //_effect.Stop();
            sfx.Stop();
            collider.enabled = false;

            Destroy(gameObject);
        }

        /// <summary>
        /// 爆風にヒットしたときに相手をふっとばす処理
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                // 衝突対象がRigidbodyの配下であるかを調べる
                var rigidBody = other.GetComponentInParent<Rigidbody>();

                // Rigidbodyがついてないなら吹っ飛ばないの終わり
                if (rigidBody == null) return;

                // 爆風によって爆発中央から吹き飛ぶ方向のベクトルを作る
                var direction = (other.transform.position - transform.position).normalized;

                // 吹っ飛ばす
                // ForceModeを変えると挙動が変わる（今回は質量無視）
                rigidBody.AddForce(direction * explosionPower, ForceMode.Impulse);
            }
        }
    }
}
