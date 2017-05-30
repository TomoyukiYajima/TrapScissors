using UnityEngine;
using System.Collections;

public class RayCircularSector : MonoBehaviour {
    public GameObject m_MeshObject = null;  // メッシュオブジェクト

    private float m_Radius = 5.0f;          // 扇の半径
    private int m_StartDegree = 0;          // 最初に生成する角度
    private int m_EndDegree = 90;           // 最後に生成する角度
    private int m_TriangleCount = 5;        // 生成する三角形の数
    private MeshFilter m_MeshFilter;        // 編集するメッシュ

    // Use this for initialization
    void Start () {
        // 
        var obj = this.transform.parent.parent;
        var animal = obj.GetComponent<Enemy3D>();
        if (animal != null)
        {
            var status = animal.GetRayStatus();
            m_Radius = status.x;
            // 中心から加算した角度にする
            m_StartDegree = 90 - (int)status.y;
            m_EndDegree = 90 + (int)status.y;
            // 角度の数だけ生成する
            m_TriangleCount = (int)Mathf.Abs(status.y) * 10;
        }

        // メッシュの生成
        //MeshFilter m = this.GetComponent<MeshFilter>();
        //m.mesh = CreateSolidArcMesh();

        m_MeshFilter = m_MeshObject.GetComponent<MeshFilter>();
        m_MeshFilter.mesh = CreateSolidArcMesh();
        //m.mesh = CreateSolidArcMesh();
    }

    // Update is called once per frame
    void Update()
    {
        // メッシュの編集
        m_MeshObject.transform.localRotation = Quaternion.Euler(
            new Vector3(
            0.0f,
            -transform.parent.rotation.eulerAngles.y,
            0.0f)
            );
        //m_MeshFilter.mesh = CreateSolidArcMesh();

        m_MeshFilter.mesh = ChangeVertices(m_MeshFilter.mesh);
    }

    // 扇形メッシュの作成
    private Mesh CreateSolidArcMesh()
    {
        Mesh mesh = new Mesh();
        // 頂点の設定
        SetVertices(mesh);

        //// 頂点座標の計算
        //Vector3[] vertices = new Vector3[2 + m_TriangleCount];
        //Vector2[] uv = new Vector2[2 + m_TriangleCount];
        //// 0番目を円の中心座標にする
        //vertices[0] = Vector3.zero;
        //uv[0] = Vector2.one * 0.5f;

        //float radius = Mathf.Deg2Rad *
        //    ((m_EndDegree - m_StartDegree) / (float)m_TriangleCount
        //    );
        //// 前方ベクトルから、角度の取得
        //var angle = Mathf.Atan2(transform.forward.z, transform.forward.x);
        //var addRad = 270 * Mathf.Deg2Rad;

        //// 頂点データの計算
        //for (int i = 0; i != m_TriangleCount + 1; i++)
        //{
        //    // 角度の設定
        //    var rad = radius * i + (Mathf.Deg2Rad * m_StartDegree);
        //    float x = Mathf.Cos(rad + angle + addRad);
        //    float z = Mathf.Sin(rad + angle + addRad);
        //    // 二次元の頂点を設定する
        //    var vec = new Vector3(x * m_Radius * 2, 0.0f, z * m_Radius * 2);

        //    var point = vec + this.transform.position;
        //    // レイポイントからオブジェクトの位置までのレイを伸ばす
        //    Ray ray = new Ray(this.transform.position, point - transform.position);
        //    RaycastHit hitInfo;
        //    var hit = Physics.Raycast(ray, out hitInfo);

        //    // 壁に衝突した場合は、位置の補正
        //    if (hit && hitInfo.collider.tag == "Wall")
        //    {
        //        if(hitInfo.distance < m_Radius / 2)
        //        {
        //            vec = new Vector3(
        //            x * (hitInfo.distance * 2),
        //            0.0f, z * (hitInfo.distance * 2));
        //        }
        //    }
        //    vertices[i + 1] = vec;
        //    uv[i + 1] = new Vector2(x * 0.5f + 0.5f, z * 0.5f + 0.5f);
        //}
        //// 頂点座標の変更
        //mesh.vertices = vertices;
        //mesh.uv = uv;

        // 三角形ポリゴンの生成(板ポリゴン)
        int[] triangles = new int[3 * m_TriangleCount];
        for (int i = 0; i != m_TriangleCount; i++)
        {
            var point = i * 3;
            // 生成する三角形ポリゴンの原点
            triangles[point] = 0;
            // 他２点の設定
            for (int j = 1; j != 3; j++)
            {
                triangles[point + j] = i + j;
            }
        }
        // メッシュの設定
        mesh.triangles = triangles;
        return mesh;
    }

    // 
    private void SetVertices(Mesh mesh)
    {
        // 頂点座標の計算
        Vector3[] vertices = new Vector3[2 + m_TriangleCount];
        Vector2[] uv = new Vector2[2 + m_TriangleCount];
        // 0番目を円の中心座標にする
        vertices[0] = Vector3.zero;
        uv[0] = Vector2.one * 0.5f;

        float radius = Mathf.Deg2Rad *
            ((m_EndDegree - m_StartDegree) / (float)m_TriangleCount
            );
        // 前方ベクトルから、角度の取得
        var angle = Mathf.Atan2(transform.forward.z, transform.forward.x);
        var addRad = 270 * Mathf.Deg2Rad;

        // 頂点データの計算
        for (int i = 0; i != m_TriangleCount + 1; i++)
        {
            // 角度の設定
            var rad = radius * i + (Mathf.Deg2Rad * m_StartDegree);
            float x = Mathf.Cos(rad + angle + addRad);
            float z = Mathf.Sin(rad + angle + addRad);
            // 二次元の頂点を設定する
            var vec = new Vector3(x * m_Radius * 2, 0.0f, z * m_Radius * 2);

            var point = vec + this.transform.position;
            // レイポイントからオブジェクトの位置までのレイを伸ばす
            Ray ray = new Ray(this.transform.position, point - transform.position);
            RaycastHit hitInfo;
            var hit = Physics.Raycast(ray, out hitInfo);

            // 壁に衝突した場合は、位置の補正
            if (hit && hitInfo.collider.tag == "Wall")
            {
                if (hitInfo.distance < m_Radius)
                {
                    vec = new Vector3(
                    x * (hitInfo.distance * 2),
                    0.0f, z * (hitInfo.distance * 2));
                }
            }
            vertices[i + 1] = vec;
            uv[i + 1] = new Vector2(x * 0.5f + 0.5f, z * 0.5f + 0.5f);
        }
        // 頂点座標の変更
        mesh.vertices = vertices;
        mesh.uv = uv;
    }

    private Mesh ChangeVertices(Mesh mesh)
    {
        SetVertices(mesh);
        return mesh;
    }

    //public void OnDrawGizmos()
    //{
    //    var obj = this.transform.parent.parent;
    //    var animal = obj.GetComponent<Enemy3D>();
    //    if (animal != null)
    //    {
    //        var status = animal.GetRayStatus();
    //        m_Radius = status.x;
    //        m_StartDegree = 0;
    //        m_EndDegree = (int)status.y;
    //        //m_TriangleCount = 5 + (int)status.y / 20;
    //        m_TriangleCount = 100;
    //    }

    //    float radius = Mathf.Deg2Rad *
    //        ((m_EndDegree - m_StartDegree) / (float)m_TriangleCount
    //        );
    //    // 前方ベクトルから、角度の取得
    //    var angle = Mathf.Atan2(transform.forward.z, transform.forward.x);
    //    var addRad = 90 * Mathf.Deg2Rad;

    //    // 頂点データの計算
    //    for (int i = 0; i != m_TriangleCount + 1; i++)
    //    {
    //        // 角度の設定
    //        var rad = radius * i + (Mathf.Deg2Rad * m_StartDegree);
    //        float x = Mathf.Cos(rad + angle - addRad);
    //        float z = Mathf.Sin(rad + angle - addRad);
    //        // 二次元の頂点を設定する
    //        var vec = new Vector3(x * m_Radius / 2, 0.0f, z * m_Radius / 2);

    //        var point = vec + this.transform.position;
    //        // レイポイントからオブジェクトの位置までのレイを伸ばす
    //        Ray ray = new Ray(this.transform.position, (point - transform.position));
    //        RaycastHit hitInfo;
    //        var hit = Physics.Raycast(ray, out hitInfo);

    //        // レイの描画
    //        if (hit && hitInfo.collider.tag == "Wall")
    //        {
    //            if (hitInfo.distance < m_Radius / 2) Gizmos.DrawLine(transform.position, hitInfo.point);
    //            else Gizmos.DrawLine(transform.position, point);

    //        }
    //        else Gizmos.DrawLine(transform.position, point);
    //    }

    //}
}
