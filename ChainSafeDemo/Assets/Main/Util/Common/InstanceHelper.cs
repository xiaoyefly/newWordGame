
namespace Main.Utility
{
    public class Singleton<T> where T : class, new()
    {
        protected static T m_Instance;

        public static T I
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new T();
                }

                return m_Instance;
            }
        }

        public static void FreePtr()
        {
            m_Instance = null;
        }
    }
}