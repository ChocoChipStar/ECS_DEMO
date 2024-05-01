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
        //垂直方向と水平方向の入力を取得し、それぞれの移動速度をかける。
        float Xvalue = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float Yvalue = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        //現在のX,Zベクトルに上の処理で取得した値を渡す。
        Vector3 movedir = new Vector3(Xvalue, 0, Yvalue);

        //現在地に上で取得をした値を足して移動する。
        transform.position += movedir;

        //進む方向に滑らかに向く。
        transform.forward = Vector3.Slerp(transform.forward, movedir, Time.deltaTime * rotateSpeed);
    }
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (other.gameObject.tag == "Can")
            {

                // 力を発生させる場所
                Vector3 explosionPos = attackRange.transform.position;
                explosionPos.y = 0;
                attackRange.transform.position = explosionPos;

                // 中心点から設定した半径の中にあるcolliderの配列を取得する。
                Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

                foreach (Collider hit in colliders)
                {
                    // 力を及ぼしたいオブジェクトにRigidbodyが付いていることが必要（ポイント）
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        // 取得したRigidbodyに力を加える
                        // ３つの引数（加える力の強さ、力の中心点、力を及ぼす半径）
                        rb.AddExplosionForce(power, explosionPos, 5.0f);
                        Instantiate(hit_particle, explosionPos, Quaternion.identity); //パーティクル用ゲームオブジェクト生成
                    }
                }
            }
        }
    }
}