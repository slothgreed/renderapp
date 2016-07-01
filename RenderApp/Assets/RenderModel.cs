using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using RenderApp.Utility;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using RenderApp.Analize;
namespace RenderApp.Object
{
    /// <summary>
    /// 全描画形状の親クラス
    /// 同種形状は、一つのモデル形状を使いまわす。同じ形状のインスタンスを複数生成しないこと。
    /// 同種形状は、モデルビュー行列のリストで、形状位置等を設定すること。
    /// </summary>
    public abstract class RenderModel
    {
        #region [メンバ変数]

        /// <summary>
        /// バイアス行列
        /// </summary>
        public static Matrix4 m_BiasMatrix = new Matrix4(0.5f, 0, 0, 0,
                                       0, 0.5f, 0, 0,
                                       0, 0, 0.5f, 0,
                                       0.5f, 0.5f, 0.5f, 1);

        /// <summary>
        /// モデルビュー行列、Shaderに投げる用
        /// </summary>
        protected Matrix4 m_ModelMatrix = new Matrix4();
        /// <summary>
        /// VertexBufferObject
        /// </summary>                   
        protected int[] m_vbo = new int[(int)CEnum.Attrib.Num];
        /// <summary>
        /// レンダリングpreRender();を最初に書くこと
        /// </summary>
        public abstract void ObjectRender();
        /// <summary>
        /// 頂点情報の格納
        /// </summary>
        public List<Vector3> m_position = new List<Vector3>();
        /// <summary>
        /// 法線情報の格納
        /// </summary>
        public List<Vector3> m_normal = new List<Vector3>();
        /// <summary>
        /// 色情報の格納
        /// </summary>
        public List<Vector3> m_color = new List<Vector3>();
        /// <summary>
        /// テクスチャコードの格納
        /// </summary>
        public List<Vector2> m_texcoord = new List<Vector2>();
        /// <summary>
        /// 波動方程式用テスト 
        /// </summary>
        public List<float> m_Velocity = new List<float>();
        /// <summary>
        /// 波動方程式用テスト 
        /// </summary>
        public List<float> m_Current = new List<float>();
        /// <summary>
        /// テクスチャ
        /// </summary>
        protected List<string> m_Texture = new List<string>();
        /// <summary>
        /// インデックスの格納
        /// </summary>
        protected List<int> m_Index = new List<int>();
        /// <summary>
        /// テクスチャバインドId
        /// </summary>
        protected List<int> m_TexBindId = new List<int>();
        /// <summary>
        /// 解析用
        /// </summary>
        public CAnalize m_Analize;
        /// <summary>
        /// 選択した三角形
        /// </summary>
        private Polygon m_SelectPolygon;

        struct Polygon
        {
            public Vector3 color1;
            public Vector3 color2;
            public Vector3 color3;
            public int tri1;
            public int tri2;
            public int tri3;
        }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RenderModel()
        {
            m_ModelMatrix = Matrix4.Identity;
        }
        public void Render(int positionId, int normalId, int colorId, int texcoordId)
        {
            preRender(positionId, normalId, colorId, texcoordId);
            ObjectRender();
            endRender(positionId, normalId, colorId, texcoordId);
        }

        #region [テクスチャ周り]
        /// <summary>
        /// テクスチャのバインドID
        /// </summary>
        /// <returns></returns>
        public List<int> GetTexBindId()
        {
            return m_TexBindId;
        }
        #endregion
        #region[Getter関数]
        /// <summary>
        /// モデル行列のげった
        /// </summary>
        /// <returns></returns>
        public Matrix4 GetModelMatrix()
        {

            return m_ModelMatrix;
        }

        #region 頂点のゲッター
        /// <summary>
        /// モデルビューを適用した頂点位置
        /// </summary>
        /// <param name="i"></param>
        /// <param name="init">初期値か否か</param>
        /// <returns></returns>
        public Vector3 GetVertex(int i, bool init = false)
        {
            if (init)
            {
                return m_position[m_Index[i]];
            }
            //return CCalc.Multiply(m_ModelMatrix, m_position[m_Index[i]]);
            return CCalc.Multiply(m_ModelMatrix, m_position[i]);
        }
        /// <summary>
        /// 三角形
        /// </summary>
        public void GetTriangle(int i, out Vector3 tri1, out Vector3 tri2, out Vector3 tri3)
        {
            tri1 = m_position[m_Index[3 * i]];
            tri2 = m_position[m_Index[3 * i + 1]];
            tri3 = m_position[m_Index[3 * i + 2]];
        }
        /// <summary>
        /// 三角形法線
        /// </summary>
        public void GetTriangleNormal(int i, out Vector3 nor1, out Vector3 nor2, out Vector3 nor3)
        {
            nor1 = m_normal[m_Index[3 * i]];
            nor2 = m_normal[m_Index[3 * i + 1]];
            nor3 = m_normal[m_Index[3 * i + 2]];
        }
        /// <summary>
        /// 頂点数
        /// </summary>
        public int GetVertexNum()
        {
            return m_position.Count;
        }
        /// <summary>
        /// 三角形数
        /// </summary>
        public int GetTriangleNum()
        {
            return m_Index.Count / 3;
        }
        #endregion
        #endregion
        #region [バウンディングボックス]
        /// <summary>
        /// バウンディングボックスの最大値
        /// </summary>
        /// <param name="init"></param>
        /// <returns></returns>
        public Vector3 GetBoundBoxMax(bool init = false)
        {
            if(init)
            {
                return m_Analize.BoundBox.BoundMax;
            }
            return CCalc.Multiply(m_ModelMatrix, m_Analize.BoundBox.BoundMax);
        }
        /// <summary>
        /// バウンディングボックスの最小値
        /// </summary>
        /// <param name="init"></param>
        /// <returns></returns>
        public Vector3 GetBoundBoxMin(bool init = false)
        {
            if (init)
            {
                return m_Analize.BoundBox.BoundMin;
            }
            return CCalc.Multiply(m_ModelMatrix, m_Analize.BoundBox.BoundMin);
        }
        /// <summary>
        /// バウンディングボックスの中央
        /// </summary>
        /// <param name="init"></param>
        /// <returns></returns>
        public Vector3 GetBoundBoxCenter(bool init = false)
        {
            if (init)
            {
                return m_Analize.BoundBox.BoundCenter;
            }
            return CCalc.Multiply(m_ModelMatrix, m_Analize.BoundBox.BoundCenter);
        }
        #endregion
        #region [Setter関数]
        /// <summary>
        /// モデル行列のせった
        /// </summary>
        /// <returns></returns>
        public void SetModelMatrix(Matrix4 mat)
        {
            m_ModelMatrix = mat;
        }
        #endregion
        #region [レンダリング処理]
        /// <summary>
        /// レンダリングの後処理
        /// </summary>
        protected void endRender(int positionId, int normalId, int colorId, int texcoordId)
        {

            if (positionId != -1)
            {
                GL.DisableVertexAttribArray(positionId);
            
            }
            if (normalId != -1)
            {
                GL.DisableVertexAttribArray(normalId);

            }
            if (colorId != -1)
            {
                GL.DisableVertexAttribArray(colorId);

            }
            if (texcoordId != -1)
            {
                GL.DisableVertexAttribArray(texcoordId);

            }
            
        }
        /// <summary>
        ///　頂点、法線、色の設定レンダリングの前処理
        /// </summary>
        protected void preRender(int positionId, int normalId, int colorId, int texcoordId)
        {

            
            if (m_position.Count != 0 && positionId != -1)
            {
                GL.EnableVertexAttribArray(positionId);
                GL.BindBuffer(BufferTarget.ArrayBuffer, m_vbo[(int)CEnum.Attrib.Vertex]);
                GL.VertexAttribPointer(positionId, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            }
                
            if (m_normal.Count != 0 && normalId != -1)
            {
                GL.EnableVertexAttribArray(normalId);
                GL.BindBuffer(BufferTarget.ArrayBuffer, m_vbo[(int)CEnum.Attrib.Normal]);
                GL.VertexAttribPointer(normalId, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            }
                
            if (m_color.Count != 0 && colorId != -1)
            {
                GL.EnableVertexAttribArray(colorId);
                GL.BindBuffer(BufferTarget.ArrayBuffer, m_vbo[(int)CEnum.Attrib.Color]);
                GL.VertexAttribPointer(colorId, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            }
                
            if (m_texcoord.Count != 0 && texcoordId != -1)
            {

                GL.EnableVertexAttribArray(texcoordId);
                GL.BindBuffer(BufferTarget.ArrayBuffer, m_vbo[(int)CEnum.Attrib.Texture]);
                GL.VertexAttribPointer(texcoordId, 2, VertexAttribPointerType.Float, false, Vector2.SizeInBytes, 0);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            }
                
            if (m_Index.Count != 0 && positionId != -1)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, m_vbo[(int)CEnum.Attrib.Index]);
            }
                
        }
        #endregion
        #region [Uniform関数]
        /// <summary>
        /// MVP行列
        /// </summary>
        public void SetUniformMVP(int location,Matrix4 ProjMatrix,Matrix4 CameraMatrix)
        {
            Matrix4 mvp = m_ModelMatrix * CameraMatrix * ProjMatrix;
            GL.UniformMatrix4(location, false, ref mvp);
            
            }
        
        /// <summary>
        /// モデルビューのユニフォームの設定、法線行列も入れる
        /// </summary>
        public void SetUniformModelMatrix(int locationModel, int locationNormal)
        {
            GL.UniformMatrix4(locationModel, false, ref m_ModelMatrix);
            Matrix4 norm = m_ModelMatrix.ClearScale();
            Matrix3 NormalMatrix = new Matrix3(norm);
            GL.UniformMatrix3(locationNormal, false, ref NormalMatrix);
        }

        /// <summary>
        /// BiasをかけたMVP(Biasはメソッド内でかける)
        /// </summary>
        public void SetUniformMVPBiasMatrix(int location, Matrix4 ProjMatrix, Matrix4 CameraMatrix)
        {
            Matrix4 MVP = m_ModelMatrix * CameraMatrix * ProjMatrix;
            Matrix4 BMVP = MVP *m_BiasMatrix;
            GL.UniformMatrix4(location, false, ref BMVP);
        }
        #endregion
        #region [bind等のGL周り]

        /// <summary>
        /// 頂点、色、法線のBind
        /// </summary>
        protected void BindBuffer()
        {
            if (m_position.Count != 0)
            {
                if (m_vbo[0] == 0)
                {
                    GL.GenBuffers((int)CEnum.Attrib.Num, m_vbo);
                }
                bindPosition();

                if (m_normal.Count != 0)
                {
                    bindNormal();
                }
                if (m_color.Count != 0)
                {
                    bindColor();
                }
                if (m_texcoord.Count != 0)
                {
                    bindTexcoord();
                }
                if (m_Index.Count != 0)
                {
                    bindIndex();
                }
            }
        }
        /// <summary>
        /// 頂点配列のバインド
        /// </summary>
        private void bindIndex()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, m_vbo[(int)CEnum.Attrib.Index]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(m_Index.Count * sizeof(int)), m_Index.ToArray(), BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }
        /// <summary>
        /// テクスチャのバインド
        /// </summary>
        private void bindTexcoord()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, m_vbo[(int)CEnum.Attrib.Texture]);
            GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, new IntPtr(m_texcoord.Count * Vector2.SizeInBytes), m_texcoord.ToArray(), BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        /// <summary>
        /// 頂点のバインド
        /// </summary>
        protected void bindPosition()
        {
            // VBO作成

            GL.BindBuffer(BufferTarget.ArrayBuffer, m_vbo[(int)CEnum.Attrib.Vertex]);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(m_position.Count * Vector3.SizeInBytes), m_position.ToArray(), BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        /// <summary>
        /// 色のバインド
        /// </summary>
        protected void bindColor()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, m_vbo[(int)CEnum.Attrib.Color]);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(m_color.Count * Vector3.SizeInBytes), m_color.ToArray(), BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        /// <summary>
        /// 法線のバインド
        /// </summary>
        protected void bindNormal()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, m_vbo[(int)CEnum.Attrib.Normal]);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(m_normal.Count * Vector3.SizeInBytes), m_normal.ToArray(), BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        /// <summary>
        /// バッファの削除
        /// </summary>
        public void DeleteBuffer()
        {
            GL.DeleteBuffers((int)CEnum.Attrib.Num, m_vbo);

        }
        /// <summary>
        /// バインドしたデータと頂点色法線情報削除
        /// </summary>
        protected void ClearData()
        {
            if (m_position.Count != 0)
            {
                DeleteBuffer();
            }
            m_position.Clear();
            m_normal.Clear();
            m_color.Clear();
            m_texcoord.Clear();

        }
        #endregion
        #region [幾何変換]
        //基本的に各関数では、変換処理ができない場合形状位置を変えないようにしています。
        //段階を踏んで変換を行いたい場合は、各変換行列を生成し、掛け合わせた値を
        //Transformation()関数で変換しましょう
        //なお、Transformation関数でも、変換できない場合は、変換しません。


        #region [平行移動]

        /// <summary>
        ///　モデルビューに平行移動を適用
        /// </summary>
        /// <param name="move"></param>
        public void Translate(Vector3 move)
        {

            m_ModelMatrix = m_ModelMatrix.ClearTranslation();
            Matrix4 translate = Matrix4.CreateTranslation(move);
            m_ModelMatrix *= translate;
        }
        #endregion
        #region [拡大縮小]

        /// <summary>
        ///　モデルビューに拡大縮小を適用
        /// </summary>
        public void Scale(Vector3 scale, bool init = true)
        {
            m_ModelMatrix = m_ModelMatrix.ClearScale();
            Vector3 translate = m_ModelMatrix.ExtractTranslation();
            Quaternion quart = m_ModelMatrix.ExtractRotation();
            m_ModelMatrix = Matrix4.Identity;


            m_ModelMatrix *= Matrix4.CreateScale(scale);
            Matrix4 quartMat = Matrix4.CreateFromQuaternion(quart);
            m_ModelMatrix *= quartMat;
            m_ModelMatrix = m_ModelMatrix.ClearTranslation();
            m_ModelMatrix *= Matrix4.CreateTranslation(translate);
        }
        #endregion
        #region [回転]
        /// <summary>
        ///　形状に回転を適用(初期の向きに対して)
        /// </summary>
        /// 
        public bool SetModelViewRotateXYZ(Matrix4 quart, bool init)
        {
            //移動量を消して、回転後移動
            Vector3 translate = m_ModelMatrix.ExtractTranslation();
            //初期形状に対しての場合は、回転量も削除
            if (init)
            {
                m_ModelMatrix = m_ModelMatrix.ClearRotation();
            }
            m_ModelMatrix = m_ModelMatrix.ClearTranslation();

            m_ModelMatrix *= quart;
            //基点分元に戻す
            m_ModelMatrix = m_ModelMatrix.ClearTranslation();
            m_ModelMatrix *= Matrix4.CreateTranslation(translate);

            return true;
        }


        /// <summary>
        /// X軸で回転
        /// </summary>
        /// <param name="angle">Degree</param>
        /// <param name="init">初期形状に対してか否か</param>
        public virtual bool RotateX(float angle, bool init = false)
        {
            Matrix4 rotate = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(angle));
            return SetModelViewRotateXYZ(rotate, init);
        }

        /// <summary>
        /// Y軸で回転
        /// </summary>
        /// <param name="angle">Degree</param>
        /// <param name="init">初期形状に対してか否か</param>
        public virtual bool RotateY(float angle, bool init = false)
        {
            Matrix4 rotate = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(angle));
            return SetModelViewRotateXYZ(rotate, init);
        }
        /// <summary>
        /// Z軸で回転
        /// </summary>
        /// <param name="angle">Degree</param>
        /// <param name="init">初期形状に対してか否か</param>
        public virtual bool RotateZ(float angle, bool init = false)
        {
            Matrix4 rotate = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(angle));
            return SetModelViewRotateXYZ(rotate, init);
        }
        public bool RotateXYZ(float angleX,float angleY,float angleZ,bool init = false)
        {
            Matrix4 rotateX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(angleX));
            Matrix4 rotateY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(angleY));
            Matrix4 rotateZ = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(angleZ));
            return SetModelViewRotateXYZ(rotateX * rotateY * rotateZ, init);

        }
        /// <summary>
        /// vector1をvector2に回転させる。
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <param name="init">初期形状に対してか否か</param>
        public virtual bool RotateQuaternion(Vector3 vector1, Vector3 vector2, bool init = false)
        {
            Vector3 Ex = Vector3.Cross(vector1, vector2);
            if (vector1.Z == -1.0f)
            {
                RotateY(180, init);
                return true;
            }
            else if (vector1.Z == 1.0f)
            {
                RotateY(0, init);
                return true;
            }
            if (Ex.Length != 0)
            {
                Ex.Normalize();
                float angle = -CCalc.Angle(vector1, vector2, Ex);
                Matrix4 mat = Matrix4.CreateFromAxisAngle(Ex, angle);
                return SetModelViewRotateXYZ(mat, init);
            }
            return false;
        }
        #endregion
        #region [Transformation]

        /// <summary>
        /// 初期形状から一括変換する用
        /// </summary>
        public bool Transformation(Matrix4 matrix)
        {
            m_ModelMatrix = Matrix4.Identity;
            m_ModelMatrix *= matrix;
            return true;
        }
        #endregion
        #region[各ゲッタ]
        /// <summary>
        /// 平行移動ベクトルのゲッタ
        /// </summary>
        public Vector3 GetTranslate()
        {
            return m_ModelMatrix.ExtractTranslation();
        }
        /// <summary>
        /// 拡大縮小ベクトルのゲッタ
        /// </summary>
        public Vector3 GetScale()
        {
            return m_ModelMatrix.ExtractScale();
        }
        /// <summary>
        /// 回転行列のゲッタ
        /// </summary>
        public Quaternion GetRotate()
        {
            return m_ModelMatrix.ExtractRotation();
        }
        #endregion
        #endregion

        /// <summary>
        /// 形状の正規化
        /// </summary>
        protected void NormalizeObject()
        {
            Vector3 pos;
            Vector3 m_Max = GetBoundBoxMax();
            Vector3 m_Min = GetBoundBoxMin();
            Vector3 scale = (m_Max - m_Min);
            float length = scale.X;
            if (length < scale.Y)
            {
                length = scale.Y;
            }
            if (length < scale.Z)
            {
                length = scale.Z;
            }
            for (int i = 0; i < m_position.Count; i++)
            {
                pos = (m_position[i] - m_Min);
                pos.X /= length;
                pos.Y /= length;
                pos.Z /= length;
                m_position[i] = new Vector3(pos);
            }
        }
       
    }
}
