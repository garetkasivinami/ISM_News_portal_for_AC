using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.DAL_XML.Reflection;

namespace ISMNewsPortal.DAL_XML.Repositories
{
    public class XMLContex : IDisposable
    {
        public XmlDocument document;

        private string filePath;

        public bool Disposed { get; private set; }

        public XMLContex(string filePath)
        {
            this.filePath = filePath;

            GetDocument();
        }

        public void CreateRange<T>(params T[] items) where T : Model
        {
            Type type = typeof(T);
            XmlNode infoNode = document.GetElementsByTagName("info").Item(0);
            XmlNode root = GetTypeNodeItemsSection(type, document);
            foreach (T item in items)
            {
                XmlElement xmlElement = document.CreateElement("item");
                List<PropertyValue> values = ReflectionParser.GetProperties(item);

                SetXmlElementAttributes(xmlElement, values);

                root.AppendChild(xmlElement);
            }
        }

        public XmlNode GetTypeNodeInfoSection(Type type, XmlDocument document)
        {
            XmlNode result = document.SelectNodes($"store/{type.Name}/info").Item(0);
            if (result != null)
                return result;

            XmlElement typeNode = CreateTypeParentNode(type, document);

            var sections = CreateTypeNodeSections(typeNode, document);

            return sections[0];
        }

        public XmlNode GetTypeNodeItemsSection(Type type, XmlDocument document)
        {
            XmlNode result = document.SelectNodes($"store/{type.Name}/items").Item(0);
            if (result != null)
                return result;

            XmlElement typeNode = CreateTypeParentNode(type, document);

            var sections = CreateTypeNodeSections(typeNode, document);

            return sections[1];
        }

        private XmlElement[] CreateTypeNodeSections(XmlElement rootNode, XmlDocument document)
        {
            XmlElement[] xmlElements = new XmlElement[2];

            xmlElements[0] = document.CreateElement("info");
            xmlElements[0].SetAttribute("lastId", "0");

            xmlElements[1] = document.CreateElement("items");

            foreach (XmlElement xmlElement in xmlElements)
                rootNode.AppendChild(xmlElement);

            return xmlElements;
        }

        private XmlElement CreateTypeParentNode(Type type, XmlDocument document)
        {
            XmlNode rootNode = document.SelectSingleNode("store");
            XmlElement typeNode = document.CreateElement(type.Name);
            rootNode.AppendChild(typeNode);
            return typeNode;
        }

        public void SetNewItemId<T>(T item) where T: Model
        {
            Type type = typeof(T);
            int lastId;
            XmlNode infoNode = GetTypeNodeInfoSection(type, document);
            XmlAttribute xmlAttribute = infoNode.Attributes["lastId"];
            lastId = int.Parse(xmlAttribute.Value);
            item.Id = lastId;
            lastId++;
            xmlAttribute.Value = lastId.ToString();
        }

        public int Count<T>()
        {
            Type type = typeof(T);
            XmlNode root = GetTypeNodeItemsSection(type, document);
            return root.ChildNodes.Count;
        }

        public T Get<T>(int id) where T : Model
        {
            Type type = typeof(T);
            XmlNode root = GetTypeNodeItemsSection(type, document);

            XmlNode itemNode = GetNodeById(id, root.ChildNodes);
            if (itemNode != null)
            {
                T item = typeof(T).GetConstructor(Type.EmptyTypes).Invoke(null) as T;
                var values = GetPropertyValuesFromNode(itemNode);
                ReflectionParser.SetPropertiesValues<T>(item, values);
                return item;
            }
            return null;
        }

        public XmlNode GetNodeById(int id, XmlNodeList nodes)
        {
            foreach(XmlNode xmlNode in nodes)
            {
                if (xmlNode.Attributes["Id"].Value == id.ToString())
                {
                    return xmlNode;
                }
            }
            return null;
        }

        public IEnumerable<T> GetAll<T>() where T : Model
        {
            Type type = typeof(T);
            XmlNode root = GetTypeNodeItemsSection(type, document);
            List<T> items = new List<T>();
            for (int i = 0; i < root.ChildNodes.Count; i++)
            {
                XmlNode xmlNode = root.ChildNodes[i];
                T item = typeof(T).GetConstructor(Type.EmptyTypes).Invoke(null) as T;
                var values = GetPropertyValuesFromNode(xmlNode);
                ReflectionParser.SetPropertiesValues<T>(item, values);
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
            XmlNode root = GetTypeNodeItemsSection(type, document);
            foreach (T item in items)
            {
                List<PropertyValue> values = ReflectionParser.GetProperties(item);
                int id = item.Id;
                XmlNode itemNode = GetNodeById(id, root.ChildNodes);
                if (itemNode != null)
                {
                    UpdateXmlNode(itemNode, values);
                }
            }
        }

        public void DeleteRange<T>(params int[] ids) where T : Model
        {
            Type type = typeof(T);
            XmlNode root = GetTypeNodeItemsSection(type, document);
            foreach (int id in ids)
            {
                XmlNode itemNode = GetNodeById(id, root.ChildNodes);
                if (itemNode != null)
                {
                    root.RemoveChild(itemNode);
                }
            }
        }

        private void SetXmlElementAttributes(XmlElement xmlElement, List<PropertyValue> properties)
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

        public void GetDocument()
        {
            string path = filePath;
            XmlDocument xmlDocument = new XmlDocument();

            if (File.Exists(filePath))
                xmlDocument.Load(path);
            else
                ConfigDocument(xmlDocument);

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
