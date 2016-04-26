using System;
using System.Reflection;

namespace BF.Common.Extensions
{
    public static class EnumExtension
    {
        /// <summary>
        /// 获得枚举字段的Attribute
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this Enum enumValue) where T : Attribute
        {

            FieldInfo field = enumValue.GetType().GetField(enumValue.ToString());

            return field == null ? null : Attribute.GetCustomAttribute(field, typeof(T)) as T;
        }
        /// <summary>
        /// 获取枚举字段的描述，如果没有则返回枚举本身
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Description(this Enum s)
        {
            var des = s.GetAttribute<System.ComponentModel.DescriptionAttribute>();
            return des == null ? s.ToString() : des.Description;
        }
        /// 获取枚举值
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetValue(this Enum s)
        {
            var des = s.GetAttribute<System.Runtime.Serialization.EnumMemberAttribute>();
            return des == null ? s.ToString() : des.Value;
        }

    }
}
