using System;
using System.Linq;
using System.Xml.Linq;

namespace CVARC.Network
{
    public enum Side
    {
        Left,
        Right,
        Random
    }

    public class HelloPackage 
    {
        public string AccessKey { get; private set; }
        public Side Side { get; private set; }
        public string Opponent { get; private set; }
        public int MapSeed { get; set; }

        private HelloPackage Parse(XDocument doc)
        {
            var content = doc.Elements().Where(z => z.Name.LocalName == "Hello").FirstOrDefault();
            AccessKey = content.Elements().Where(z => z.Name.LocalName == "AccessKey").Select(z=>z.Value).FirstOrDefault();
            Side = (Side)Enum.Parse(typeof(Side),content.Elements().Where(z => z.Name.LocalName == "Side").FirstOrDefault().Value);
            int rand;
            if (!int.TryParse(content.Elements().Where(z => z.Name.LocalName == "Map").FirstOrDefault().Value, out rand))
                rand = -1;
            MapSeed = rand;
            Opponent = content.Elements().Where(z => z.Name.LocalName == "Opponent").Select(z => z.Value).FirstOrDefault();
            return this;
        }

        public HelloPackage Deserialize(string line)
        {
            return Parse(new XDocument(line));
        }

        public string Serialize()
        {
            return string.Format("<Hello><AccessKey>1234</AccessKey><Side>Random</Side><Opponent>MolagBal</Opponent></Hello>\r\n", AccessKey, Side, MapSeed, Opponent);
        }
    }
}
