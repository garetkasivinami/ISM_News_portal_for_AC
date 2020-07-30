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
        public Dictionary<Type, XmlDocument> documents;

        private string folderPath;

        public bool Disposed { get; private set; }

        public XMLContex(string folderPath)
        {
            this.folderPath = folderPath;

            documents = new Dictionary<Type, XmlDocument>();
        }

        public void CreateRange<T>(params T[] items) where T : Model
        {
            Type type = typeof(T);
            XmlDocument document = GetDocument(type);
            int lastId;
            XmlNode infoNode = document.GetElementsByTagName("info").Item(0);
            XmlAttribute xmlAttribute = infoNode.Attributes["lastId"];
            lastId = int.Parse(xmlAttribute.Value);
            XmlNode root = document.SelectNodes("store/items").Item(0);
            ReflectionParse reflectionParse = new ReflectionParse();
            foreach (T item in items)
            {
                XmlElement xmlElement = document.CreateElement("item");
                List<PropertyValue> values = reflectionParse.GetProperties(item);
                values.Where(u => u.Name == "Id").Single().SetValue(lastId);
                lastId++;
                UpdateXmlElement(xmlElement, values);
                root.AppendChild(xmlElement);
            }
            xmlAttribute.Value = lastId.ToString();
        }

        public int GetLastId<T>()
        {
            Type type = typeof(T);
            XmlDocument document = GetDocument(type);
            int lastId;
            XmlNode infoNode = document.GetElementsByTagName("info").Item(0);
            XmlAttribute xmlAttribute = infoNode.Attributes["lastId"];
            lastId = int.Parse(xmlAttribute.Value);
            lastId++;
            xmlAttribute.Value = lastId.ToString();
            return lastId - 1;
        }

        public int Count<T>()
        {
            Type type = typeof(T);
            XmlDocument document = GetDocument(type);
            XmlNode root = document.SelectNodes("store/items").Item(0);
            return root.ChildNodes.Count;
        }

        public T Get<T>(int id) where T : Model
        {
            Type type = typeof(T);
            XmlDocument document = GetDocument(type);
            XmlNode root = document.SelectNodes("store/items").Item(0);
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
            XmlDocument document = GetDocument(type);
            XmlNode root = document.SelectNodes("store/items").Item(0);
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
            XmlDocument document = GetDocument(type);
            XmlNode root = document.SelectNodes("store/items").Item(0);
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
            XmlDocument document = GetDocument(type);
            XmlNode root = document.SelectNodes("store/items").Item(0);
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

        public XmlDocument GetDocument(Type type)
        {
            if (documents.ContainsKey(type))
            {
                return documents[type];
            }
            else
            {
                return FindPhysicalDocument(type);
            }
        }

        public XmlDocument FindPhysicalDocument(Type type)
        {
            string path = Path.Combine(folderPath, type.Name + ".xml");
            XmlDocument xmlDocument = new XmlDocument();
            if (File.Exists(path))
            {
                xmlDocument.Load(path);
            }
            else
            {
                ConfigDocument(xmlDocument, type);
            }
            documents.Add(type, xmlDocument);
            return xmlDocument;
        }

        public void ConfigDocument(XmlDocument document, Type type)
        {
            string typeString = type.Name;

            XmlDeclaration XmlDec = document.CreateXmlDeclaration("1.0", "utf-8", null);
            document.AppendChild(XmlDec);

            XmlElement rootElement = document.CreateElement("store");
            XmlElement infoElement = document.CreateElement("info");

            infoElement.SetAttribute("type", type.Name);
            infoElement.SetAttribute("lastId", "0");

            XmlElement itemsElement = document.CreateElement("items");

            rootElement.AppendChild(infoElement);
            rootElement.AppendChild(itemsElement);
            document.AppendChild(rootElement);
        }

        public void Dispose()
        {
            Save();
            Disposed = true;
        }
        public void Save()
        {
            foreach (Type key in documents.Keys)
            {
                string path = Path.Combine(folderPath, key.Name + ".xml");
                documents[key].Save(path);
            }
        }
    }
}
