using KI.Foundation.Utility;

namespace KI.Foundation.Core
{
    /// <summary>
    /// オブジェクト
    /// </summary>
    public abstract class KIObject
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public KIObject()
        {
            Name = this.GetType().ToString();
            Logger.Log(Logger.LogLevel.Debug, "create kiobject " + Name + ":");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        public KIObject(string name)
        {
            Name = name;
            Logger.Log(Logger.LogLevel.Debug, "create kiobject " + Name + ":");
        }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 解放処理
        /// </summary>
        public virtual void Dispose()
        {
        }

        /// <summary>
        /// 文字列
        /// </summary>
        /// <returns>文字列</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
