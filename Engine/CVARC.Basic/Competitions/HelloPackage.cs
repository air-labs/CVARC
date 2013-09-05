using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.Network
{
    public class HelloPackage 
    {
        public string AccessKey { get; private set; }
        public bool LeftSide { get; private set; }
        public string Opponent { get; private set; }

        public void Parse(System.Xml.Linq.XDocument doc)
        {
            var content = doc.Elements().Where(z => z.Name.LocalName == "Hello").FirstOrDefault();
            AccessKey = content.Elements().Where(z => z.Name.LocalName == "AccessKey").Select(z=>z.Value).FirstOrDefault();
            LeftSide = content.Elements().Where(z => z.Name.LocalName == "Side").Select(z=>z.Value=="Left").FirstOrDefault();
            Opponent = content.Elements().Where(z => z.Name.LocalName == "Opponent").Select(z => z.Value).FirstOrDefault();
        }
    }
}
