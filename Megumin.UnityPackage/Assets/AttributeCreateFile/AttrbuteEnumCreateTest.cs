using System;
using System.Collections;
using System.Collections.Generic;
using Megumin;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System.ComponentModel;

public class AttrbuteEnumCreateTest : ScriptableObject
{
    [Button]
    public void GenericCode()
    {
        CSCodeGenerator generator = new CSCodeGenerator();
        generator.Push(@$"using System;");
        generator.Push(@$"using System.Collections;");
        generator.Push(@$"using System.Collections.Generic;");
        generator.Push(@$"using System.ComponentModel;");
        generator.Push(@$"using UnityEngine;");
        generator.Push("");
        generator.Push(@$"namespace $(NameSpace)");
        using (generator.NewScope)
        {
            HashSet<Type> safe = new HashSet<Type>();
            Assembly[] assemblies = new Assembly[]
            {
                typeof(CategoryAttribute).Assembly,
                typeof(DescriptionAttribute).Assembly,
            };

            var ambs = AppDomain.CurrentDomain.GetAssemblies().Concat(assemblies);
            var debugambs = ambs.ToArray();
            var attrs = from amb in ambs
                        let all = amb.CustomAttributes
                        from attr in all
                        select attr;

            foreach (var attr in attrs)
            {
                safe.Add(attr.AttributeType);
            }

            var attrs2 = from amb in ambs
                         let all = amb.ExportedTypes
                         from type in all
                         where type.IsSubclassOf(typeof(System.Attribute))
                         select type;

            var debugattrs2 = attrs2.ToList();

            foreach (var attr in attrs2)
            {
                safe.Add(attr);
            }

            var list = safe.ToList();
            list.RemoveAll(ele => ele.FullName.StartsWith("AOT"));
            list.RemoveAll(ele => ele.FullName.StartsWith("JetBrains.Annotations"));
            list.RemoveAll(ele => ele.FullName.StartsWith("log4net"));
            list.RemoveAll(ele => ele.FullName.StartsWith("Microsoft.SqlServer"));
            list.RemoveAll(ele => ele.FullName.StartsWith("NUnit"));
            list.RemoveAll(ele => ele.FullName.StartsWith("System.Configuration"));
            list.RemoveAll(ele => ele.FullName.StartsWith("System.Data"));
            list.RemoveAll(ele => ele.FullName.StartsWith("System.Management"));
            list.RemoveAll(ele => ele.FullName.StartsWith("System.Runtime.InteropServices.WindowsRuntime"));
            list.RemoveAll(ele => ele.FullName.StartsWith("System.Runtime.Remoting"));
            list.RemoveAll(ele => ele.FullName.StartsWith("System.Security.Permissions"));
            list.RemoveAll(ele => ele.FullName.StartsWith("System.Web"));
            list.RemoveAll(ele => ele.FullName.StartsWith("UnityEditor.SettingsManagement"));
            list.RemoveAll(ele => ele.FullName.StartsWith("UnityEngine.TestTools"));

            list.Sort((l, r) => l.FullName.CompareTo(r.FullName));

            foreach (var item in list)
            {
                AttributeUsageAttribute attributeUsage = item.GetCustomAttribute<AttributeUsageAttribute>();
                generator.Push(@$"[{item.FullName}] // AllowMultiple = {attributeUsage.AllowMultiple} Inherited = {attributeUsage.Inherited}");
            }

            generator.PushComment($"见识见识常见Attribute");
            generator.Push(@$"public partial class AllAttributeList");
            using (generator.NewScope)
            {
                var enums = from amb in ambs
                            let all = amb.ExportedTypes
                            from type in all
                            where type.IsSubclassOf(typeof(System.Enum))
                            select type;

                var debugaenums = enums.ToList();

                foreach (var item in debugaenums)
                {
                    generator.Push(@$"public {item.FullName.Replace("+", ".")} {item.Name}_Test;");
                }
                generator.Push(@$"public int TestProperty {{ get; set; }}");
            }
        }

        generator.Macro["NameSpace"] = "Megumin.Test";
        generator.GenerateNear(this);
    }
}
