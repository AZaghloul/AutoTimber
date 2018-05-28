using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bim.Domain.Configuration
{
    public class Setup :  Dictionary<string, object> 
    {
        public Setup()
        {
            
        }
        public T Get<T>(string key)
        {
            try
            {
                return (T)base[key];
            }
            catch (Exception)
            {

                return default(T);
            }
            
        }
        public void Set(string key,object value)
        {
            base[key]=value;
        }
    }
}
