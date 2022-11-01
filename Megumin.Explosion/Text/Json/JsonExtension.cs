using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace System.Text.Json
{
    public static class JsonExtension_EE1B5F4D7BDD4EC2B5CFEED6ABC10AF9
    {
#if NET5_0_OR_GREATER

        public static void SetSpacesPerIndent(int space = 4)
        {
            //const 编译时常量，改不了的 sad
            var type = typeof(JsonDocument).Assembly.GetType("JsonConstants");
            var fieldMember = type.GetProperty("SpacesPerIndent", BindingFlags.Static | BindingFlags.Public);
            fieldMember.SetValue(null, space);
        }

#endif
    }
}
