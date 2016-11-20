using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.GLUtil;
namespace RenderApp.Render_System
{
    public class RenderQueue
    {
        List<PostPlane> list = new List<PostPlane>();

        public RenderQueue()
        {
           
        }
        public void AddPass(PostPlane pass)
        {
            list.Add(pass);
        }
        public void RemovePass(PostPlane pass)
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
        public PostPlane FindPostProcess(int index)
        {
            if(index < 0 || index > Num)
            {
                return null;
            }
            return list[index];
        }
        internal void SizeChanged(int width, int height)
        {
            foreach (var loop in list)
            {
                loop.SizeChanged(width, height);
            }
        }

        public IEnumerable<PostPlane> Items()
        {
            foreach(var loop in list)
            {
                yield return loop;
            }
        }
        public PostPlane Items(int index)
        {
            if(index < 0 || index > Num)
            {
                return null;
            }
            return list[index];
        }
        internal void ClearBuffer()
        {
            foreach(var loop in list)
            {
                loop.FrameBufferItem.ClearBuffer();
            }

        }
        internal void Render()
        {
            foreach(var loop in list)
            {
                loop.Render();
            }
        }
    }
}
