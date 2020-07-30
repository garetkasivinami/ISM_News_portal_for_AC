using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using ISMNewsPortal.BLL.Models;

namespace ISMNEWSPORTAL.DAL_XML.Repositories
{
    public class XMLContex : IDisposable
    {
        public XmlDocument document;

        private string filePath;

        public bool Disposed { get; private set; }

        public XMLContex(string filePath)
        {
            this.filePath = filePath;

            FindPhysicalDocument();
        }

        public void CreateRange<T>(params T[] items) where T : Model
        {
            Type type = typeof(T);
            XmlNode infoNode = document.GetElementsByTagName("info").Item(0);
            XmlNode root = SelectNode(type, document);
            ReflectionParse reflectionParse = new ReflectionParse();
            foreach (T item in items)
            {
                XmlElement xmlElement = document.CreateElement("item");
                List<PropertyValue> values = reflectionParse.GetProperties(item);
                UpdateXmlElement(xmlElement, values);
                root.AppendChild(xmlElement);
            }
        }

        public XmlNode SelectNode(Type type, XmlDocument document)
        {
            XmlNode result = document.SelectNodes($"store/{type.Name}/items").Item(0);
            if (result != null)
                return result;

            XmlNode rootNode = document.SelectSingleNode("store");
            XmlElement typeNode = document.CreateElement(type.Name);

            XmlElement infoElement = document.CreateElement("info");
            infoElement.SetAttribute("lastId", "0");
            typeNode.AppendChild(infoElement);

            XmlElement itemsElement = document.CreateElement("items");
            typeNode.AppendChild(itemsElement);

            rootNode.AppendChild(typeNode);
            return itemsElement;
        }

        public int GetLastId<T>()
        {
            Type type = typeof(T);
            int lastId;
            XmlNode infoNode = document.SelectNodes($"store/{type.Name}/info").Item(0);
            XmlAttribute xmlAttribute = infoNode.Attributes["lastId"];
            lastId = int.Parse(xmlAttribute.Value);
            lastId++;
            xmlAttribute.Value = lastId.ToString();
            return lastId - 1;
        }

        public int Count<T>()
        {
            Type type = typeof(T);
            XmlNode root = SelectNode(type, document);
            return root.ChildNodes.Count;
        }

        public T Get<T>(int id) where T : Model
        {
            Type type = typeof(T);
            XmlNode root = SelectNode(type, document);
            ReflectionParse reflectionParse = new ReflectionParse();
            for (int i = 0; i < root.ChildNodes.Count; i++)
            {
                XmlNode xmlNode = root.ChildNodes[i];
                if (xmlNode.Attributes["Id"].Value == id.ToString())
                {
                    T item = typeof(T).GetConstructor(Type.EmptyTypes).Invoke(null) as T;
                    var values = GetPropertyValuesFromNode(xmlNode);
                    reflectionParse.SetPropertiesValues<T>(item, values);
                    return item;
                }
            }
            return null;
        }

        public IEnumerable<T> GetAll<T>() where T : Model
        {
            Type type = typeof(T);
            XmlNode root = SelectNode(type, document);
            ReflectionParse reflectionParse = new ReflectionParse();

            List<T> items = new List<T>();
            for (int i = 0; i < root.ChildNodes.Count; i++)
            {
                XmlNode xmlNode = root.ChildNodes[i];
                T item = typeof(T).GetConstructor(Type.EmptyTypes).Invoke(null) as T;
                var values = GetPropertyValuesFromNode(xmlNode);
                reflectionParse.SetPropertiesValues<T>(item, values);
                items.Add(item);
            }
            return items;
        }

        public List<PropertyValue> GetPropertyValuesFromNode(XmlNode xmlNode)
        {
            List<PropertyValue> values = new List<PropertyValue>();
            foreach (XmlAttribute xmlAttribute in xmlNode.Attributes)
            {
                values.Add(new PropertyValue(xmlAttribute.Name, xmlAttribute.Value));
            }
            return values;
        }

        public void UpdateRange<T>(params T[] items) where T : Model
        {
            Type type = typeof(T);
            XmlNode root = SelectNode(type, document);
            ReflectionParse reflectionParse = new ReflectionParse();
            foreach (T item in items)
            {
                List<PropertyValue> values = reflectionParse.GetProperties(item);
                int id = item.Id;
                for (int i = 0; i < root.ChildNodes.Count; i++)
                {
                    XmlNode xmlNode = root.ChildNodes[i];
                    if (xmlNode.Attributes["Id"].Value == id.ToString())
                    {
                        UpdateXmlNode(xmlNode, values);
                        break;
                    }
                }
            }
        }

        public void DeleteRange<T>(params int[] ids) where T : Model
        {
            Type type = typeof(T);
            XmlNode root = SelectNode(type, document);
            foreach (int id in ids)
            {
                for (int i = 0; i < root.ChildNodes.Count; i++)
                {
                    XmlNode xmlNode = root.ChildNodes[i];
                    if (xmlNode.Attributes["Id"].Value == id.ToString())
                    {
                        root.RemoveChild(xmlNode);
                        break;
                    }
                }
            }
        }

        private void UpdateXmlElement(XmlElement xmlElement, List<PropertyValue> properties)
        {
            foreach (PropertyValue value in properties)
            {
                xmlElement.SetAttribute(value.Name, value.Value?.ToString() ?? "");
            }
        }
        private void UpdateXmlNode(XmlNode xmlNode, List<PropertyValue> properties)
        {
            foreach (PropertyValue value in properties)
            {
                xmlNode.Attributes[value.Name].Value = value.Value?.ToString() ?? "";
            }
        }

        public void FindPhysicalDocument()
        {
            string path = filePath;
            XmlDocument xmlDocument = new XmlDocument();
            if (File.Exists(filePath))
            {
                xmlDocument.Load(path);
            }
            else
            {
                ConfigDocument(xmlDocument);
            }
            document = xmlDocument;
        }

        public void ConfigDocument(XmlDocument document)
        {

            XmlDeclaration XmlDec = document.CreateXmlDeclaration("1.0", "utf-8", null);
            document.AppendChild(XmlDec);

            XmlElement rootElement = document.CreateElement("store");

            document.AppendChild(rootElement);
        }

        public void Dispose()
        {
            Save();
            Disposed = true;
        }
        public void Save()
        {
            document.Save(filePath);
        }
    }
}
