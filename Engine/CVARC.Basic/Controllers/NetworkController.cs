using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CVARC.Basic.Controllers;

namespace CVARC.Basic
{
    public abstract class NetworkController
    {
        public abstract Command ParseRequest(string request);



        public T GetValue<T>(XElement parent, string name, Func<string,T> convert)

        {
            var element = parent.Elements().Where(z => z.Name.LocalName == name).FirstOrDefault();
            if (element == null) return default(T);
            return convert(element.Value);
        }

        public string GetString(XElement parent, string name)
        {
            return GetValue<string>(parent, name, z => z);
        }

        public int GetInt(XElement parent, string name)
        {
            return GetValue<int>(parent, name, z=> int.Parse(z));
        }

        public double GetDouble(XElement parent, string name)
        {
            return GetValue<double>(parent, name, str =>
                {
                    str = str.Replace(',', '.');
                    return double.Parse(str, CultureInfo.InvariantCulture);

                });
        }


    }
}
