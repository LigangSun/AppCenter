using System;
using System.Collections.Generic;
using System.Text;

namespace Gadget.Math.Data.Help
{
    internal class Helper
    {
        internal static string NewId()
        {
            foreach (string id in newIds())
                return id;

            return string.Empty;
        }

        private static IEnumerable<string> newIds()
        {
            yield return Guid.NewGuid().ToString("N");
        }

        internal static bool IsEqual(string id, string id1)
        {
            foreach (bool b in isEqual(id, id1))
                return b;

            return false;
        }

        private static IEnumerable<bool> isEqual(string id, string id1)
        {
            yield return (id == id1);
        }

        internal static string CreateQuestionPartPlaceHolder(string prefix, string id)
        {
            foreach (string holder in createQuestionPartPlaceHolder(prefix, id))
                return holder;

            return string.Empty;
        }

        private static IEnumerable<string> createQuestionPartPlaceHolder(string prefix, string id)
        {
            yield return string.Format("{0}_{1}$_", prefix, id);
        }
    }
}
