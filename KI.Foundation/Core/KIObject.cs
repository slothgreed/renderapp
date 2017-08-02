using KI.Foundation.Utility;
namespace KI.Foundation.Core
{
    public abstract class KIObject
    {
        public KIObject()
        {
            Name = this.GetType().ToString();
            Logger.Log(Logger.LogLevel.Debug, "create kiobject " + Name + ":");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
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

        public override string ToString()
        {
            return Name;
        }
    }
}
