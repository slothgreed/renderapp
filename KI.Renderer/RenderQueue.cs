using System;
using System.Collections.Generic;
using KI.Gfx.KITexture;
using KI.Asset.Technique;

namespace KI.Asset
{
    /// <summary>
    /// レンダーキューイベント
    /// </summary>
    public class RenderQueueEventArgs : EventArgs
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="technique">テクニック</param>
        public RenderQueueEventArgs(RenderTechnique technique)
        {
            Technique = technique;
        }

        /// <summary>
        /// テクニック
        /// </summary>
        public RenderTechnique Technique { get; private set; }
    }

    /// <summary>
    /// レンダーキュー
    /// </summary>
    public class RenderQueue
    {
        /// <summary>
        /// レンダーテクニックリスト
        /// </summary>
        private List<RenderTechnique> techniques = new List<RenderTechnique>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RenderQueue()
        {
        }

        /// <summary>
        /// テクニックの追加後イベント
        /// </summary>
        public event EventHandler<RenderQueueEventArgs> TechniqueAdded;

        /// <summary>
        /// テクニックの削除後イベント
        /// </summary>
        public event EventHandler<RenderQueueEventArgs> TechniqueRemoved;

        /// <summary>
        /// アイテム
        /// </summary>
        public IEnumerable<RenderTechnique> Items
        {
            get
            {
                foreach (var loop in techniques)
                {
                    yield return loop;
                }
            }
        }

        /// <summary>
        /// レンダーテクニックの追加
        /// </summary>
        /// <param name="technique">レンダーテクニック</param>
        public void AddTechnique(RenderTechnique technique)
        {
            techniques.Add(technique);
            OnTechniqueAdded(technique);
        }

        /// <summary>
        /// レンダーテクニックの削除
        /// </summary>
        /// <param name="technique">レンダーテクニック</param>
        public void RemoveTechnique(RenderTechnique technique)
        {
            technique.Dispose();
            techniques.Remove(technique);
            OnTechniqueRemoved(technique);
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public void Dispose()
        {
            foreach (var loop in techniques)
            {
                loop.Dispose();
            }

            techniques.Clear();
        }

        /// <summary>
        /// サイズ変更
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public void SizeChanged(int width, int height)
        {
            foreach (var loop in techniques)
            {
                loop.SizeChanged(width, height);
            }
        }

        /// <summary>
        /// バッファのクリア
        /// </summary>
        public void ClearBuffer()
        {
            foreach (var loop in techniques)
            {
                loop.RenderTarget.ClearBuffer();
            }
        }

        /// <summary>
        /// レンダリング
        /// </summary>
        /// <param name="scene">シーン</param>
        public void Render(Scene scene)
        {
            foreach (var loop in techniques)
            {
                loop.Render(scene);
            }
        }

        /// <summary>
        /// 指定したレンダーテクニックのテクスチャを取得
        /// </summary>
        /// <param name="type">テクニックのタイプ</param>
        /// <returns>出力テクスチャ</returns>
        public Texture[] OutputTexture(RenderTechniqueType type)
        {
            foreach (var technique in techniques)
            {
                if (technique.Technique == type)
                {
                    return technique.OutputTexture;
                }
            }

            return null;
        }

        /// <summary>
        /// 全ての出力テクスチャの取得
        /// </summary>
        /// <returns>出力テクスチャ</returns>
        public IEnumerable<Texture> OutputTextures()
        {
            foreach (var technique in techniques)
            {
                //出力用のは返さない
                if (technique is OutputBuffer)
                {
                    continue;
                }

                foreach (var texture in technique.OutputTexture)
                {
                    yield return texture;
                }
            }
        }

        /// <summary>
        /// テクニックの追加後イベント発行
        /// </summary>
        /// <param name="technique">レンダーテクニック</param>
        private void OnTechniqueAdded(RenderTechnique technique)
        {
            TechniqueAdded?.Invoke(this, new RenderQueueEventArgs(technique));
        }

        /// <summary>
        /// テクニックの削除後イベント発行
        /// </summary>
        /// <param name="technique">レンダーテクニック</param>
        private void OnTechniqueRemoved(RenderTechnique technique)
        {
            TechniqueRemoved?.Invoke(this, new RenderQueueEventArgs(technique));
        }
    }
}
