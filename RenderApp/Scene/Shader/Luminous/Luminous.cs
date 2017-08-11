using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using RenderApp.Utility;

namespace RenderApp
{
    //public class CLuminous : CShader
    //{
    //    //#region [シングルトン]
    //    //private static CLuminous m_Instance = new CLuminous();
    //    //public static CLuminous GetInstance()
    //    //{
    //    //    return m_Instance;
    //    //}
    //    //#endregion

    //    //int lightColorId;
    //    //private List<CSphere> m_SphereLight = new List<CSphere>();

    //    //private int uMVP;
    //    //private int uModelMatrix;
    //    //private int uNormalMatrix; 
    //    //private int uLightPos ;
    //    //private int uposit_2D ;
    //    //private int unormal_2D; 
    //    //private int uUnProjectMatrix; 
    //    //private int uScreenW ;
    //    //private int uScreenH;
    //    //private int ulightColorId;

    //    //protected override int LoadShader()
    //    //{
    //    //    string vertShader;
    //    //    string fragShader;
    //    //    vertShader = Encoding.UTF8.GetString(Properties.Resources.Luminous_vert);
    //    //    fragShader = Encoding.UTF8.GetString(Properties.Resources.Luminous_frag);
    //    //    //AddRenderTexture(ETexture.Luminous);
    //    //    return CreateShaderProgram(vertShader, fragShader);
    //    //}

    //    //protected override void SetUniformName()
    //    //{
    //    //    uMVP = GL.GetUniformLocation(mProgram, "MVP");
    //    //    uModelMatrix = GL.GetUniformLocation(mProgram, "ModelMatrix");
    //    //    uNormalMatrix = GL.GetUniformLocation(mProgram, "NormalMatrix");
    //    //    uLightPos = GL.GetUniformLocation(mProgram, "LightPos");
    //    //    uposit_2D = GL.GetUniformLocation(mProgram, "posit_2D");
    //    //    unormal_2D = GL.GetUniformLocation(mProgram, "normal_2D");
    //    //    uUnProjectMatrix = GL.GetUniformLocation(mProgram, "UnProject");
    //    //    uScreenW = GL.GetUniformLocation(mProgram, "ScreenW");
    //    //    uScreenH = GL.GetUniformLocation(mProgram, "ScreenH");
    //    //    ulightColorId = GL.GetUniformLocation(mProgram, "LightColor");
    //    //}
    //    //public override void Initialize()
    //    //{
    //    //    Vector3 center = new Vector3((
    //    //        Global.WorldMax.X + Global.WorldMin.X) / 2,
    //    //        Global.WorldMin.Y,
    //    //        (Global.WorldMax.Z + Global.WorldMin.Z) / 2);
    //    //    int num = 10;
    //    //    double dist = 18;
    //    //    double theta = 360 / num;
    //    //    Vector3 move = new Vector3();
    //    //    CSphere sphere;
    //    //    for(int i = 0; i <= num; i++)
    //    //    {
    //    //        move.X = (float)(Math.Sin(theta * i) * dist);
    //    //        move.Y = 1;
    //    //        move.Z = (float)(Math.Cos(theta * i) * dist);
    //    //        sphere = new CSphere(12, 20, 20, true, Vector3.UnitY);
    //    //        sphere.Translate(move);
    //    //        m_SphereLight.Add(sphere);

    //    //    }
    //    //    sphere = new CSphere(12, 20, 20, true, Vector3.UnitY);
    //    //    sphere.Translate(new Vector3(center.X,0, center.Z));
    //    //    m_SphereLight.Add(sphere);
            
    //    //}
        
    //    //public override void Dispose()
    //    //{
    //    //    base.Dispose();
    //    //    GlobalModel.Sphere.Clear();
    //    //}
        
    //    //public override void AnimationTimer(int timerCount, float angleCount)
    //    //{
    //    //    int num = 10;
    //    //    double dist = 18;
    //    //    double theta = 360 / num;
    //    //    Vector3 move = new Vector3();
    //    //    for (int i = 0; i <= num; i++)
    //    //    {
    //    //        move.X = (float)(Math.Sin(theta * i - MathHelper.DegreesToRadians(timerCount % 360)) * dist);
    //    //        move.Y = 1;
    //    //        move.Z = (float)(Math.Cos(theta * i - MathHelper.DegreesToRadians(timerCount % 360)) * dist);
    //    //        m_SphereLight[i].Translate(new Vector3(move.X, 1, move.Z));
    //    //    }
    //    //}

    //    //public override void StopTimer()
    //    //{
    //    //    int num = 10;
    //    //    double dist = 18;
    //    //    double theta = 360 / num;
    //    //    Vector3 move = new Vector3();
    //    //    for (int i = 0; i <= num; i++)
    //    //    {
    //    //        move.X = (float)(Math.Sin(theta * i) * dist);
    //    //        move.Y = 1;
    //    //        move.Z = (float)(Math.Cos(theta * i) * dist);
    //    //        m_SphereLight[i].Translate(move);
    //    //    }
    //    //}
    //}
}
