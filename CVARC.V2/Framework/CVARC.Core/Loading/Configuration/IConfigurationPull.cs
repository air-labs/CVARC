using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CVARC.V2
{
    public interface IConfigurationPull
    {
        void Pull(Settings configuration, string[] fieldsToPull);
    }

    public static class IConfigurationPullExtensions
    {
        public static void PullOnly(
            this IConfigurationPull pull, 
            Settings configuration, 
            params Expression<Func<Settings, object>>[] fields)
        {
            var names=fields.Select(z => z.Body as MemberExpression).Select(z => z.Member.Name).ToArray();
            pull.Pull(configuration, names);
        }

        public static void PullAll(this IConfigurationPull pull, Settings configuration)
        {
            pull.Pull(configuration, null);
        }
    }
}
