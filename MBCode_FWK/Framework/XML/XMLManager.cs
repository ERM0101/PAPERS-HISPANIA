#region Librerias usadas por la clase

using MBCode.Framework.Managers.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

#endregion

namespace MBCode.Framework.Managers.XML
{
    #region Enumerados

    /// <summary>
    /// Determina la posición en la que se quiere poner el nuevo nodo.
    /// </summary>
    public enum LocationXML
    {
        /// <summary>
        /// Inserta el nuevo nodo como el primer hijo del nodo padre.
        /// </summary>
        Begin,

        /// <summary>
        /// Inserta el nuevo nodo como el último hijo del nodo padre.
        /// </summary>
        End,

        /// <summary>
        /// Inserta el nuevo nodo en la posición indicada del nodo padre.
        /// </summary>
        Specific,
    }

    /// <summary>
    /// Determina la manera como se muestran los valores de un nodo
    /// </summary>
    public enum FormatXML
    {
        /// <summary>
        /// Se muestra como texto propiamente dicho.
        /// </summary>
        Normal,

        /// <summary>
        /// Se añade un Tag value para mejorar la visibilidad.
        /// </summary>
        WithTag,
    }

    public enum ExistResult
    {
        First,
        All,
    }

    #endregion

    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha: 28/02/2012.
    /// Descripción: clase que dota al Framework de las funcionalidades de gestión de ficheros XML.
    /// </summary>
    public static class XMLManager
    {
        #region Constantes

        public const string ROOT_NODE = "Root";

        public const string ROOT_NODE_TEXT = "Root description";

        #endregion

        #region Load Xml Document

        /// <summary>
        /// Método encargado de crear una estructura XmlDocument para el fichero Xml pasado cómo parámetro.
        /// </summary>
        /// <param name="sFileName">Nombre del fichero a analizar.</param>
        /// <returns>XmlDocument asociado al fichero pasado como parámetro, null si error.</returns>
        public static XmlDocument LoadDocumentFromFile(string sFileName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(sFileName);
            return (xmlDoc);
        }

        /// <summary>
        /// Crea la estructura XmlDocument con el valor del string pasado cómo parámetro.
        /// </summary>
        /// <param name="sXml">Texto que contiene la cadena de caráteres que se desea analizar.</param>
        /// <returns>XmlDocument asociado al fichero pasado como parámetro, null si error</returns>
        public static XmlDocument LoadDocumentFromString(string sXml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(sXml);
            return (xmlDoc);
        }

        #endregion

        #region Create XML Document

        /// <summary>
        /// Crea la estructura XmlDocument asociada a los valores pasados cómo parámetro.
        /// </summary>
        /// <param name="bMustClosed">Booleano que indica si se debe cerrar el XMl una vez realizada la operación.</param>
        /// <returns>XmlDocument inicializado, si operación correcta, null, si error.</returns>
        public static XmlDocument CreateDocument(bool bMustClosed = false)
        {
            XmlDocument xmlDoc = new System.Xml.XmlDocument();
            AddDescriptorNode(ref xmlDoc);
            if (bMustClosed) CloseDocument(ref xmlDoc);
            return (xmlDoc);
        }

        /// <summary>
        /// Crea la estructura XmlDocument asociada a los valores pasados cómo parámetro.
        /// </summary>
        /// <param name="sElementoRaiz">Elemento raíz.</param>
        /// <param name="sTextoRaiz">Texto del elemento raíz.</param>
        /// <param name="bMustClosed">Booleano que indica si se debe cerrar el XMl una vez realizada la operación.</param>
        /// <returns>XmlDocument inicializado, si operación correcta, null, si error.</returns>
        public static XmlDocument CreateDocument(string sElementoRaiz, string sTextoRaiz, bool bMustClosed = false)
        {
            XmlDocument xmlDoc = CreateDocument();
            AddRootNode(ref xmlDoc, sElementoRaiz, sTextoRaiz);
            if (bMustClosed) CloseDocument(ref xmlDoc);
            return (xmlDoc);
        }

        #endregion

        #region Close Document

        /// <summary>
        /// Cierra el documento.
        /// </summary>
        /// <param name="xmlDoc">Documento que se desea cerrar.</param>
        public static void CloseDocument(ref XmlDocument xmlDoc)
        {
            xmlDoc.Normalize();
        }

        /// <summary>
        /// Cierra el documento Xml y lo guarda en un fichero.
        /// </summary>
        /// <param name="xmlDoc">Documento que se desea cerrar.</param>
        /// <param name="sPathDocument"></param>
        public static void CloseAndSaveDocument(ref XmlDocument xmlDoc, string sPathDocument)
        {
            CloseDocument(ref xmlDoc);
            xmlDoc.Save(sPathDocument);
        }

        #endregion
        
        #region Nodo Descriptor

        /// <summary>
        /// Escribe en la estructura XmlDocument pasada cómo parámetro el nodo descriptor indicado.
        /// </summary>
        /// <param name="xmlDoc">Documento en el que se quiere añadir un descriptor</param>
        private static void AddDescriptorNode(ref XmlDocument xmlDoc)
        {
            xmlDoc.AppendChild(xmlDoc.CreateNode(XmlNodeType.XmlDeclaration, string.Empty, string.Empty));
        }

        #endregion

        #region Nodo Raíz

        /// <summary>
        /// Escribe en la estructura XmlDocument pasada cómo parámetro el nodo raíz indicado.
        /// </summary>
        /// <param name="xmlDoc">Documento en el que se quiere añadir un nodo raíz</param>
        /// <param name="sElementoRaiz"> Valor del elemento raíz</param>
        /// <param name="sTextoRaiz">Escribe el texo indicado en el nodo raíz</param>
        private static XmlNode AddRootNode(ref XmlDocument xmlDoc, string sElementoRaiz = ROOT_NODE, string sTextoRaiz = ROOT_NODE_TEXT)
        {
            XmlNode xmlNodoRaiz = xmlDoc.CreateNode(XmlNodeType.Element, sElementoRaiz, string.Empty);
            XmlNode xmlAttr = xmlDoc.CreateNode(XmlNodeType.Attribute, "Contains", string.Empty);
            xmlAttr.Value = sTextoRaiz;
            xmlNodoRaiz.Attributes.SetNamedItem(xmlAttr);
            xmlDoc.AppendChild(xmlNodoRaiz);
            return (xmlNodoRaiz);
        }

        #endregion

        #region Nodo de Datos

        #region Add

        /// <summary>
        /// Añade el nodo de nombre sNewNodeName y valor sNewNodeValue en la posición indicada por eLocation e 
        /// iLocationNewNode como hijo del nodo xmlRootNewNode y con los atributos indicados por slAtrib. 
        /// </summary>
        /// <param name="xmlDoc">Documento al que se desea añadir un nuevo nodo.</param>
        /// <param name="sNameRootNewNode">Nombre del nodo raíz del nuevo nodo.</param>
        /// <param name="sNewNodeName">Nombre del nuevo nodo.</param>
        /// <param name="sNewNodeValue">Valor del nuevo nodo.</param>
        /// <param name="bMustClosed">Booleano que indica si se debe cerrar el XMl una vez realizada la operación.</param>
        /// <param name="slAtrib">Atributos del nuevo nodo.</param>
        /// <param name="eFormat">Formato a usar para el nuevo nodo.</param>
        /// <param name="eLocation">Posición que ha de ocupar el nuevo nodo en el documento.</param>
        /// <param name="iLocationNewNode">Posición concreta que ha de ocupar el nuevo nodo en el documento.</param>
        /// <returns>Referencia al nodo creado si la operación es correcta, null sinó</returns>
        public static XmlNode AddNode(ref XmlDocument xmlDoc, string sNameRootNewNode, string sNewNodeName,
                                      string sNewNodeValue, bool bMustClosed = false, SortedList slAtrib = null, 
                                      FormatXML eFormat = FormatXML.Normal, LocationXML eLocation = LocationXML.End, 
                                      int iLocationNewNode = 0)
        {
            //  Comprueba que exista el nodo raíz.
                XmlNode xmlRootNewNode = Exist(xmlDoc, sNameRootNewNode);
                if (xmlRootNewNode == null)
                {
                    throw new ArgumentException(MsgManager.ErrorMsg("MSG_XMLManager_000", sNameRootNewNode));
                }
            //  Comprueba que no exista el nodo que se desea crear.
                return (AddNode(ref xmlDoc, xmlRootNewNode, sNewNodeName, sNewNodeValue, bMustClosed, slAtrib, eFormat, eLocation, iLocationNewNode));
        }

        /// <summary>
        /// Añade el nodo de nombre sNewNodeName y valor sNewNodeValue en la posición indicada por eLocation e 
        /// iLocationNewNode como hijo del nodo xmlRootNewNode y con los atributos indicados por slAtrib. 
        /// </summary>
        /// <param name="xmlDoc">Documento al que se desea añadir un nuevo nodo.</param>
        /// <param name="xmlRootNewNode">Nodo raíz del nuevo nodo.</param>
        /// <param name="sNewNodeName">Nombre del nuevo nodo.</param>
        /// <param name="sNewNodeValue">Valor del nuevo nodo.</param>
        /// <param name="bMustClosed">Booleano que indica si se debe cerrar el XMl una vez realizada la operación.</param>
        /// <param name="slAtrib">Atributos del nuevo nodo.</param>
        /// <param name="eFormat">Formato a usar para el nuevo nodo.</param>
        /// <param name="eLocation">Posición que ha de ocupar el nuevo nodo en el documento.</param>
        /// <param name="iLocationNewNode">Posición concreta que ha de ocupar el nuevo nodo en el documento.</param>
        /// <returns>Referencia al nodo creado si la operación es correcta, null sinó</returns>
        public static XmlNode AddNode(ref XmlDocument xmlDoc, XmlNode xmlRootNewNode, string sNewNodeName,
                                      string sNewNodeValue, bool bMustClosed = false, SortedList slAtrib = null, 
                                      FormatXML eFormat = FormatXML.Normal, LocationXML eLocation = LocationXML.End, 
                                      int iLocationNewNode = 0)
        {
            //  Comprueba que exista el nodo raíz.
                if (xmlRootNewNode == null)
                {
                    throw new ArgumentException(MsgManager.ErrorMsg("MSG_XMLManager_000", "Node root for '" + sNewNodeName + "'."));
                }
            //  Comprueba que no exista el nodo que se desea crear.
                if (Exist(xmlDoc, sNewNodeName) != null)
                {
                    throw new InvalidOperationException(MsgManager.ErrorMsg("MSG_XMLManager_001", sNewNodeName));
                }
            //  Crea el nuevo Nodo.
                XmlNode xmlNode = CreateNode(ref xmlDoc, sNewNodeName, sNewNodeValue, slAtrib, eFormat);
            //  Inserta el nuevo nodo en la posición deseada.
                switch (eLocation)
                {
                    case LocationXML.Begin:
                         xmlRootNewNode.InsertBefore(xmlNode, xmlRootNewNode.FirstChild);
                         break;
                    case LocationXML.Specific:
                         xmlRootNewNode.InsertBefore(xmlNode, xmlRootNewNode.ChildNodes[iLocationNewNode]);
                         break;
                    case LocationXML.End:
                    default:
                         xmlRootNewNode.InsertAfter(xmlNode, xmlRootNewNode.LastChild);
                         break;
                }
            //  Cierra el documento si es necesario.
                if (bMustClosed) CloseDocument(ref xmlDoc);
                return (xmlNode);
        }

        #endregion
        
        #region Edit
        
        /// <summary>
        /// Añade el nodo de nombre sNewNodeName y valor sNewNodeValue en la posición indicada por eLocation e 
        /// iLocationNewNode como hijo del nodo xmlRootNewNode y con los atributos indicados por slAtrib. 
        /// </summary>
        /// <param name="xmlDoc">Documento al que se desea añadir un nuevo nodo.</param>
        /// <param name="sNodeName">Nombre del nodo a modificar.</param>
        /// <param name="sNewNodeValue">Nuevo valor del nuevo nodo.</param>
        /// <param name="bMustClosed">Booleano que indica si se debe cerrar el XMl una vez realizada la operación.</param>
        /// <returns>Referencia al nodo creado si la operación es correcta, null sinó</returns>
        public static XmlNode ChangeValue(ref XmlDocument xmlDoc, string sNodeName, string sNewNodeValue, bool bMustClosed = false)
        {
            //  Comprueba que no exista el nodo que se desea crear.
                XmlNode xmlNode = Exist(xmlDoc, sNodeName);
                if (xmlNode == null)
                {
                    throw new ArgumentException(MsgManager.ErrorMsg("MSG_XMLManager_000", sNodeName));
                }
            //  Modifica el valor del elemento del nodo.
                xmlNode.InnerText = sNewNodeValue;
            //  Cierra el documento si es necesario.
                if (bMustClosed) CloseDocument(ref xmlDoc);
                return (xmlNode);
        }
        
        /// <summary>
        /// Añade el nodo de nombre sNewNodeName y valor sNewNodeValue en la posición indicada por eLocation e 
        /// iLocationNewNode como hijo del nodo xmlRootNewNode y con los atributos indicados por slAtrib. 
        /// </summary>
        /// <param name="xmlDoc">Documento al que se desea añadir un nuevo nodo.</param>
        /// <param name="sNodeName">Nombre del nodo a modificar.</param>
        /// <param name="sAttributeName">Nombre del atributo a modificar.</param>
        /// <param name="sNewAttributeValue">Nuevo valor del atributo.</param>
        /// <param name="bMustClosed">Booleano que indica si se debe cerrar el XMl una vez realizada la operación.</param>
        /// <returns>Referencia al nodo creado si la operación es correcta, null sinó</returns>
        public static XmlNode ChangeAttribute(ref XmlDocument xmlDoc, string sNodeName, string sAttributeName, 
                                              string sNewAttributeValue, bool bMustClosed = false)
        {
            //  Comprueba que no exista el nodo que se desea crear.
                XmlNode xmlNode = Exist(xmlDoc, sNodeName);
                if (xmlNode == null)
                {
                    throw new ArgumentException(MsgManager.ErrorMsg("MSG_XMLManager_000", sNodeName));
                }
            //  Modifica el valor del atributo.
                if (xmlNode.Attributes[sAttributeName] != null)
                xmlNode.Attributes[sAttributeName].Value = sNewAttributeValue;
            //  Cierra el documento si es necesario.
                if (bMustClosed) CloseDocument(ref xmlDoc);
                return (xmlNode);
        }
        
        #endregion

        #region Auxiliares

        /// <summary>
        /// Crea un nuevo nodo con los valores pasados cómo parámetro. 
        /// </summary>
        /// <param name="xmlDoc">Documento al que se desea añadir un nuevo nodo.</param>
        /// <param name="sNewNodeName">Nombre del nuevo nodo.</param>
        /// <param name="sNewNodeValue">Valor del nuevo nodo.</param>
        /// <param name="slAtrib">Atributos del nuevo nodo.</param>
        /// <param name="eFormat">Formato a usar para el nuevo nodo.</param>
        /// <returns>Referencia al nodo creado si la operación es correcta, null sinó</returns>
        private static XmlNode CreateNode(ref XmlDocument xmlDoc, string sNewNodeName, string sNewNodeValue, SortedList slAtrib = null, 
                                          FormatXML eFormat = FormatXML.Normal)
        { 
            //  Crea el nuevo Nodo.
                XmlNode xmlNode = xmlDoc.CreateNode(XmlNodeType.Element, sNewNodeName, string.Empty);
                switch (eFormat)
                {
                    case FormatXML.WithTag:
                         XmlNode xmlInfo = xmlDoc.CreateNode(XmlNodeType.Element, "Value", string.Empty);
                         xmlInfo.AppendChild(xmlDoc.CreateTextNode(sNewNodeValue));
                         xmlNode.AppendChild(xmlInfo);
                         break;
                    case FormatXML.Normal:
                    default:
                         xmlNode.AppendChild(xmlDoc.CreateTextNode(sNewNodeValue));
                         break;
                }
            //  Añade los atributos al nuevo nodo.
                if (slAtrib != null)
                {
                    foreach (string sKey in slAtrib.Keys)
                    {
                        XmlNode xmlAttr = xmlDoc.CreateNode(XmlNodeType.Attribute, sKey, string.Empty);
                        xmlAttr.Value = slAtrib[sKey].ToString();
                        xmlNode.Attributes.SetNamedItem(xmlAttr);
                    }
                }        
            //  Devuelve el nodo resultante.
                return (xmlNode);
        }

        #endregion

        #endregion

        #region Consult

        public static List<XmlNode> Exist(XmlNode xmlNode, string sNodeName)
        {
            if (xmlNode.HasChildNodes)
            {
                List<XmlNode> lstNodes = new List<XmlNode>();
                foreach (XmlNode xmlChildNode in xmlNode.ChildNodes)
                {
                    if (xmlChildNode.Name.Equals(sNodeName)) lstNodes.Add(xmlChildNode);
                }
                return (lstNodes);
            }
            return (null);
        }

        public static XmlNode Exist(XmlDocument xmlDoc, string sNodeName)
        {
            XmlNodeList xmlNodeList = xmlDoc.GetElementsByTagName(sNodeName);
            if (xmlNodeList != null)
            {
                return (xmlNodeList[0]);
            }
            else return (null);
        }

        public static List<XmlNode> Exist(XmlDocument xmlDoc, string sNodeName, ExistResult eExistResult)
        {
            //  Busca dentro del documento el nodo indicado
                XmlNodeList xmlNodeList = xmlDoc.GetElementsByTagName(sNodeName);
                if (xmlNodeList != null)
                {
                    List<XmlNode> lstXmlNode = new List<XmlNode>();
                    if (eExistResult.Equals(ExistResult.All))
                    {
                        foreach (XmlNode xmlNode in xmlNodeList)
                        {
                            lstXmlNode.Add(xmlNode);
                        }
                    }
                    else if (eExistResult.Equals(ExistResult.First)) lstXmlNode.Add(xmlNodeList[0]);
                    return (lstXmlNode);
                }
                else return (null);
        }

        public static Dictionary<string, string> GetChildNodes(XmlDocument xmlDoc, XmlNode xmlNodeIn)
        {
            //  Si el nodo existe devolvemos el valor de sus nodos hijos.
                return (GetChildNodes(xmlDoc, xmlNodeIn.Name));
        }

        public static Dictionary<string, string> GetChildNodes(XmlDocument xmlDoc, string sNodeName)
        {
            //  Comprueba que no exista el nodo que se desea crear.
                XmlNodeList xmlChildNodes = xmlDoc.GetElementsByTagName(sNodeName);
                if (xmlChildNodes == null)
                {
                    throw new ArgumentException(MsgManager.ErrorMsg("MSG_XMLManager_000", sNodeName));
                }
            //  Crea el diccionario resultante y lo rellena con la información indicada.
                Dictionary<string, string> dcNodes = new Dictionary<string,string>(xmlChildNodes.Count);
                foreach (XmlNode xmlNode in xmlChildNodes)
                {
                    dcNodes.Add(xmlNode.Name, xmlNode.InnerText);
                }
                return (dcNodes);
        }

        public static string GetNodeValue(XmlDocument xmlDoc, string sNodeName)
        { 
            //  Comprueba que no exista el nodo que se desea usar.
                XmlNode xmlNode = Exist(xmlDoc, sNodeName);
                if (xmlNode == null)
                {
                    throw new ArgumentException(MsgManager.ErrorMsg("MSG_XMLManager_000", sNodeName));
                }
            //  Si el nodo existe devolvemos el valor de sus atributos.
                return (GetNodeValue(xmlNode));
        }

        public static string GetNodeValue(XmlNode xmlNode)
        { 
            //  Recupera el valor del atributo..
                return (xmlNode.InnerText);
        }

        public static string GetNodeAttributeValue(XmlDocument xmlDoc, string sNodeName, string sAttributeName)
        { 
            //  Comprueba que no exista el nodo que se desea usar.
                XmlNode xmlNode = Exist(xmlDoc, sNodeName);
                if (xmlNode == null)
                {
                    throw new ArgumentException(MsgManager.ErrorMsg("MSG_XMLManager_000", sNodeName));
                }
            //  Si el nodo existe devolvemos el valor de sus atributos.
                return (GetNodeAttributeValue(xmlNode, sAttributeName));
        }

        public static string GetNodeAttributeValue(XmlNode xmlNode, string sAttributeName)
        { 
            //  Recupera el valor del atributo..
                return (xmlNode.Attributes[sAttributeName].Value.ToString());
        }

        public static Dictionary<string, string> GetNodeAttributes(XmlDocument xmlDoc, string sNodeName)
        {
            //  Comprueba que no exista el nodo que se desea usar.
                XmlNode xmlNode = Exist(xmlDoc, sNodeName);
                if (xmlNode == null)
                {
                    throw new ArgumentException(MsgManager.ErrorMsg("MSG_XMLManager_000", sNodeName));
                }
            //  Si el nodo existe devolvemos el valor de sus atributos.
                return (GetNodeAttributes(xmlNode));
        }

        public static Dictionary<string, string> GetNodeAttributes(XmlNode xmlNode)
        {
            //  Recorre la lista de atributos rellenando el diccionario con los datos.
                Dictionary<string, string> dcAttributes = new Dictionary<string, string>(xmlNode.Attributes.Count);
                foreach (XmlAttribute xmlAttrribute in xmlNode.Attributes)
                {
                    dcAttributes.Add(xmlAttrribute.Name, xmlAttrribute.Value);
                }
                return (dcAttributes);
        }

        #endregion

        #region Information

        public static string ToString(XmlDocument xmlDoc)
        {
            MemoryStream mStream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(mStream, Encoding.Unicode);
            try
            {
                writer.Formatting = Formatting.Indented;
                xmlDoc.WriteContentTo(writer);
                writer.Flush();
                mStream.Flush();
                mStream.Position = 0;
                StreamReader sReader = new StreamReader(mStream);
                return (sReader.ReadToEnd());
            }
            catch (XmlException)
            {
                return (string.Empty);
            }
            finally
            {
                mStream.Close();
                writer.Close();
            }
        }

        #endregion
    }
}
