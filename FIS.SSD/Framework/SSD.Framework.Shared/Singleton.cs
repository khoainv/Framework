
namespace SSD.Framework
{
    public class Singleton<T> where T: new()
    {
        static object obj = new object();
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (obj)
                    {
                        //chac chan chua duoc khoi tao bo doi tuong khac
                        if (instance == null)
                        {
                            instance = new T();
                        }
                    }
                }
                return instance;
            }
        }
    }
}
