using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CVARC.Basic.Core
{
    public class CompetitionsBundle
    {
        public readonly Competitions Competitions;
        public readonly ICvarcRules CvarcRules;
        protected CompetitionsBundle(Competitions c, ICvarcRules r)
        {
            Competitions = c;
            CvarcRules = r;
        }

        public static CompetitionsBundle Load(string competitionsName, string levelName)
        {
            if (string.IsNullOrEmpty(competitionsName) || !File.Exists(competitionsName))
                throw new Exception(string.Format("Файл соревнований {0} не был найден. Проверьте правильность пути CompetitionsName.", competitionsName));

            var ass = Assembly.LoadFrom(competitionsName);
            var competitions = ass.GetExportedTypes().SingleOrDefault(a => a.IsSubclassOf(typeof(CompetitionsBundle)) && a.Name == levelName);
            if (competitions == null)
                throw new Exception(string.Format("Уровень {0} не был найден в {1}", levelName, competitionsName));
            var ctor = competitions.GetConstructor(new Type[] { });
            return ctor.Invoke(new object[] { }) as CompetitionsBundle;
        }
    }
}
