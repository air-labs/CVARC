using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace CVARC.Core.Replay
{
    public static class ConverterToJavaScript
    {
            private static Dictionary<int, List<object>> existing;
            private static List<string> iter;
            private static List<object> result;

            public static string Convert(string input)
            {
                var replayPlayer = new ReplayPlayer(input);
                Body rootBody = replayPlayer.RootBody;
                existing = new Dictionary<int, List<object>>();
                result = new List<object>();

                result.Add(replayPlayer.DT);
                rootBody.ChildAdded += BodyAdded;

                while (!replayPlayer.IsAtEnd)
                {
                    iter = new List<string>();
                    replayPlayer.Update();
                    result.Add(iter);
                }

                return (new JavaScriptSerializer()).Serialize(result);
            }

            public static string Convert(SerializationRoot input)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                var replayPlayer = new ReplayPlayer(input);
                Body rootBody = replayPlayer.RootBody;
                existing = new Dictionary<int, List<object>>();
                result = new List<object>();

                result.Add(replayPlayer.DT);
                rootBody.ChildAdded += BodyAdded;

                while (!replayPlayer.IsAtEnd)
                {
                    iter = new List<string>();
                    replayPlayer.Update();
                    result.Add(iter);
                }
                var res = (new JavaScriptSerializer()).Serialize(result);
                //replace id of root body to 0
                res = res.Replace(","+rootBody.Id+",", ",0,");
                return res;
            }

            private static void BodyAdded(Body body)
            {
                iter.Add(Convert(body.getCreateJSON()));
                existing.Add(body.Id, body.getEditJSON());

                body.PropertyChanged += BodyLocationChanged;
            }

            static void BodyLocationChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
            {
                if (e.PropertyName.Equals(Body.LocationPropertyName))
                {
                    var body = (sender as Body);
                    var current_position = body.getEditJSON();
                    var old_position = existing[body.Id];
                    var elem = RemoveDuplicate(current_position, old_position);

                    elem[0] = body.Id;  // idшник должен затереться - восстанавливаем

                    var str = Convert(elem);

                    if (str != null)
                        iter.Add(str);

                    existing[body.Id] = current_position;
                }
            }

            static List<object> RemoveDuplicate(List<object> first, List<object> second)
            {
                List<object> result = new List<object>(first);

                if (first.Count == second.Count)
                    for (int i = result.Count - 1; i >= 0; --i)
                        if (result[i].Equals(second[i]))
                            result[i] = null;

                return result;
            }

            static string Convert(List<object> array)
            {
                int count = array.Count;
                while (count > 0 && array[count - 1] == null)
                    count--;

                if (count < 2) // idшник будет всегда
                    return null;
                else
                    return string.Join(",", array.GetRange(0, count)); //- я не знаю, эквивалентно ли это
                    //return string.Join(",", array.GetRange(0, count).Select(z=>z.ToString()).ToArray()); 
            }

    }
}
