using System;
using System.Reflection;

public static class MethodInfoExtensions
{
    public static string GetBackingFieldName(this MethodInfo method)
    {
        var body = method.GetMethodBody().GetILAsByteArray();
        // Check for matching instructions: ldarg.0 ldfld <field>
        if (body.Length < 7 || body[body.Length - 7] != 0x02 || body[body.Length - 6] != 0x7B)
            return ""; // return blank if no match

        // Get token
        var token = BitConverter.ToInt32(body, body.Length - 5);

        // Return field name
        return method.DeclaringType.Module.ResolveField(token).Name;
    }
}
