using System.Collections;
using System.IO;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace resource.preview
{
    internal class VSPreview : extension.AnyPreview
    {
        protected override void _Execute(atom.Trace context, int level, string url, string file)
        {
            var a_Context = new Deserializer();
            try
            {
                var a_Context1 = "";
                {
                    __Execute(context, level - 1, a_Context.Deserialize<dynamic>(new StringReader(File.ReadAllText(file))) as IEnumerable, "", NAME.TYPE.PARAMETER, ref a_Context1);
                }
            }
            catch (YamlException ex)
            {
                context.
                    SetUrl(file, ex.Start.Line, ex.Start.Column).
                    Send(NAME.SOURCE.PREVIEW, NAME.TYPE.EXCEPTION, level, __GetErrorMessage(ex.Message)).
                    SendPreview(NAME.TYPE.EXCEPTION, url);
            }
        }

        private static void __Execute(atom.Trace context, int level, object data, string name, string type, ref string trail)
        {
            if (data == null)
            {
                return;
            }
            if (GetState() == NAME.STATE.CANCEL)
            {
                return;
            }
            if (string.IsNullOrEmpty(name) == false)
            {
                context.
                    SetComment(__GetComment(data, type), "[[[Data Type]]]").
                    Send(NAME.SOURCE.PREVIEW, __GetType(data, type), level, name, __GetValue(data));
            }
            {
                var a_Context = data.GetHashCode() + ";";
                if (trail.Contains(a_Context))
                {
                    return;
                }
                {
                    trail += a_Context;
                }
            }
            if (data is IList)
            {
                var a_Context = data as IList;
                var a_Index = 0;
                foreach (var a_Context1 in a_Context)
                {
                    {
                        a_Index++;
                    }
                    {
                        __Execute(context, level + 1, a_Context1, "[" + a_Index.ToString() + "]", NAME.TYPE.PARAMETER, ref trail);
                    }
                }
            }
            if (data is IDictionary)
            {
                var a_Context = data as IDictionary;
                foreach (string a_Context1 in a_Context.Keys)
                {
                    __Execute(context, level + 1, a_Context[a_Context1], a_Context1, NAME.TYPE.PARAMETER, ref trail);
                }
            }
        }

        private static string __GetErrorMessage(string data)
        {
            var a_Index = data.IndexOf("): ");
            if (a_Index > 0)
            {
                return GetFinalText(data.Substring(a_Index + 3));
            }
            return data;
        }

        private static string __GetValue(object data)
        {
            if ((data is IList) || (data is IDictionary))
            {
                return "";
            }
            return GetFinalText(data.ToString());
        }

        private static string __GetComment(object data, string type)
        {
            if (data is IList)
            {
                return "[[[Array]]]";
            }
            if (data is IDictionary)
            {
                return "[[[Object]]]";
            }
            if (type == NAME.TYPE.PARAMETER)
            {
                return "[[[Property]]]";
            }
            return "[[[Item]]]";
        }

        private static string __GetType(object data, string type)
        {
            if ((data is IList) || (data is IDictionary))
            {
                return NAME.TYPE.PARAMETER;
            }
            return type;
        }
    };
}
