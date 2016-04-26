using System;
using System.Collections.Generic;
using System.Xml;

namespace BF.Common.Helper
{
    public class XmlHelper<T, P>
        where T : new()
        where P : new()
    {
        /// <summary>
        /// Xml文件转成对象实例集
        /// </summary>
        /// <param name="xml">xml</param>
        /// <returns></returns>
        public static IList<T> XmlFileToEntityList(string xmlPath)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(xmlPath);
            }
            catch
            {
                return null;
            }
            if (doc.ChildNodes.Count != 1)
                return null;

            IList<T> items = new List<T>();
            foreach (XmlNode node in doc.ChildNodes)
            {
                items.Add(XmlNodeToHeaderEntity(node));
                foreach (XmlNode child in node.ChildNodes)
                {
                    items.Add(XmlNodeToHeaderEntity(child));
                }
            }
            return items;
        }
        /// <summary>
        /// Xml转成对象实例集
        /// </summary>
        /// <param name="xml">xml</param>
        /// <returns></returns>
        public static IList<T> XmlToEntityList(string xml)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(xml);
            }
            catch
            {
                return null;
            }
            if (doc.ChildNodes.Count != 1)
                return null;

            IList<T> items = new List<T>();
            foreach (XmlNode node in doc.ChildNodes)
            {
                items.Add(XmlNodeToHeaderEntity(node));
            }
            return items;
        }

        /// <summary>
        /// Xml转成对象实例集
        /// </summary>
        /// <param name="xml">xml</param>
        /// <returns></returns>
        public static T XmlToEntity(string xml)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(xml);
            }
            catch
            {
                return new T();
            }
            if (doc.ChildNodes.Count != 1)
                return new T();

            XmlNode node = doc.ChildNodes[0];

            return XmlNodeToHeaderEntity(node);
        }

        private static T XmlNodeToHeaderEntity(XmlNode node)
        {
            T item = new T();

            if (node.NodeType == XmlNodeType.Element)
            {
                System.Reflection.PropertyInfo[] propertyInfo = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                foreach (XmlNode child in node.ChildNodes)
                {
                    string attrName = child.Name.ToLower();
                    string attrValue = child.InnerText.ToString().Trim();

                    #region
                    foreach (System.Reflection.PropertyInfo pinfo in propertyInfo)
                    {
                        if (pinfo != null)
                        {
                            string name = pinfo.Name.ToLower();
                            Type dbType = pinfo.PropertyType;
                            if (name == attrName)
                            {
                                if (String.IsNullOrEmpty(attrValue))
                                    continue;
                                switch (dbType.ToString())
                                {
                                    case "System.Int32":
                                        pinfo.SetValue(item, Convert.ToInt32(attrValue), null);
                                        break;
                                    case "System.Int64":
                                        pinfo.SetValue(item, Convert.ToInt64(attrValue), null);
                                        break;
                                    case "System.Boolean":
                                        pinfo.SetValue(item, Convert.ToBoolean(attrValue), null);
                                        break;
                                    case "System.DateTime":
                                        pinfo.SetValue(item, Convert.ToDateTime(attrValue), null);
                                        break;
                                    case "System.Decimal":
                                        pinfo.SetValue(item, Convert.ToDecimal(attrValue), null);
                                        break;
                                    case "System.Double":
                                        pinfo.SetValue(item, Convert.ToDouble(attrValue), null);
                                        break;
                                    case "System.String":
                                        pinfo.SetValue(item, attrValue, null);
                                        break;
                                    default:
                                        pinfo.SetValue(item, XmlNodeToList(child), null);
                                        break;
                                }
                                continue;
                            }
                        }
                    }
                    #endregion
                }
            }
            return item;
        }

        private static List<P> XmlNodeToList(XmlNode node)
        {
            List<P> list = new List<P>();
            if (node.NodeType == XmlNodeType.Element)
            {
                P item = new P();
                if (node.FirstChild.Name.ToLower() == "record")
                {
                    foreach (XmlNode child in node.ChildNodes) //参数有多个列
                    {
                        item = XmlNodeToEntity(child);
                        list.Add(item);
                    }
                }
                else
                {
                    item = XmlNodeToEntity(node);
                    list.Add(item);
                }
            }

            return list;
        }

        private static P XmlNodeToEntity(XmlNode node)
        {
            P item = new P();
            if (node.NodeType == XmlNodeType.Element)
            {
                System.Reflection.PropertyInfo[] propertyInfo = typeof(P).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                foreach (XmlNode child in node.ChildNodes)
                {
                    string attrName = child.Name.ToLower();
                    string attrValue = child.InnerText.ToString().Trim();
                    foreach (System.Reflection.PropertyInfo pinfo in propertyInfo)
                    {
                        if (pinfo != null)
                        {
                            string name = pinfo.Name.ToLower();
                            Type dbType = pinfo.PropertyType;
                            if (name == attrName)
                            {
                                if (String.IsNullOrEmpty(attrValue))
                                    continue;
                                switch (dbType.ToString())
                                {
                                    case "System.Int32":
                                        pinfo.SetValue(item, Convert.ToInt32(attrValue), null);
                                        break;
                                    case "System.Boolean":
                                        pinfo.SetValue(item, Convert.ToBoolean(attrValue), null);
                                        break;
                                    case "System.DateTime":
                                        pinfo.SetValue(item, Convert.ToDateTime(attrValue), null);
                                        break;
                                    case "System.Decimal":
                                        pinfo.SetValue(item, Convert.ToDecimal(attrValue), null);
                                        break;
                                    case "System.Double":
                                        pinfo.SetValue(item, Convert.ToDouble(attrValue), null);
                                        break;
                                    default:
                                        pinfo.SetValue(item, attrValue, null);
                                        break;
                                }
                                continue;
                            }
                        }
                    }
                }
            }

            return item;
        }

    }
}
