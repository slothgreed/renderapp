﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.GLUtil;
namespace RenderApp
{
    public class RenderQueue
    {
        List<PostProcess> list = new List<PostProcess>();

        public RenderQueue()
        {
           
        }
        public void AddPass(PostProcess pass)
        {
            list.Add(pass);
        }
        public void RemovePass(PostProcess pass)
        {
            list.Remove(pass);
        }
        public int Num
        {
            get
            {
                return list.Count;
            }
        }
        public void Dispose()
        {
            foreach(var loop in list)
            {
                loop.Dispose();
            }
            list.Clear();
        }

        internal void SizeChanged(int width, int height)
        {
            foreach (var loop in list)
            {
                loop.SizeChanged(width, height);
            }
        }

        public IEnumerable<PostProcess> Items()
        {
            foreach(var loop in list)
            {
                yield return loop;
            }
        }

        internal void Render()
        {
            foreach(var loop in list)
            {
            }
        }
    }
}
