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

        public KIObject(string name)
        {
            Name = name;
            Logger.Log(Logger.LogLevel.Debug, "create kiobject " + Name + ":");
        }

        private string _name = null;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

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
