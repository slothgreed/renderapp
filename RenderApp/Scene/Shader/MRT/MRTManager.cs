﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using OpenTK;
//using OpenTK.Graphics.OpenGL;
//namespace RenderApp.GLUtil
//{
//    public static class MRTManager
//    {
//        public static readonly string FrameName = "MRT";
//        public static readonly string Posit = "Posit";
//        public static readonly string Normal = "Normal";
//        public static readonly string Color = "Color";
//        public static readonly string Light = "Light";

//        public static void Initialize(int width, int height)
//        {
//            string[] textureName = new string[4];
//            textureName[0] = Posit;
//            textureName[1] = Normal;
//            textureName[2] = Color;
//            textureName[3] = Light;
//            FramebufferAttachment[] attachment = new FramebufferAttachment[5];

//            attachment[0] = FramebufferAttachment.ColorAttachment1;
//            attachment[1] = FramebufferAttachment.ColorAttachment2;
//            attachment[2] = FramebufferAttachment.ColorAttachment3;
//            attachment[3] = FramebufferAttachment.ColorAttachment4;

//            FrameBufferManager.CreateFrameBuffer(FrameName, width, height, textureName, attachment);
//        }
      

//    }
//}
