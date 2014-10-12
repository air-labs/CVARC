//using System;
//using System.IO;
//using System.Linq;
//using System.Reflection;

//namespace CVARC.Basic
//{
//    public class CompetitionsBundle
//    {
//        public readonly Competitions competitions;
//        public readonly ICvarcRules Rules;
//        public CompetitionsBundle(Competitions c, ICvarcRules r)
//        {
//            competitions = c;
//            Rules = r;
//        }

//        public static CompetitionsBundle Load(string competitionsName, string levelName)
//        {

//            if (string.IsNullOrEmpty(competitionsName) || !File.Exists(competitionsName))
//                throw new Exception(string.Format("Файл соревнований {0} не был найден. Проверьте правильность пути CompetitionsName.", competitionsName));

//            var ass = Assembly.LoadFrom(competitionsName);
//            var list = ass.GetExportedTypes().ToList();
//            var competitions = list.SingleOrDefault(a => a.IsSubclassOf(typeof(CompetitionsBundle)) && a.Name == levelName);
//            if (competitions == null)
//                throw new Exception(string.Format("Уровень {0} не был найден в {1}", levelName, competitionsName));
//            var ctor = competitions.GetConstructor(new Type[] { });
//            return ctor.Invoke(new object[] { }) as CompetitionsBundle;
//        }
//    }
//}
