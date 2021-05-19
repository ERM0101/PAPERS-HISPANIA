#region Librerias usadas por la clase

using MBCode.Framework.Managers.Messages;
using MBCode.Framework.Managers.XML;
using MBCode.FrameworkDemoWFP.InterfazUsuario;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

#endregion

namespace MBCode.FrameworkDemoWFP.LogicaNegocio
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha última modificación: 28/03/2012.
    /// Descripción: clase que se encarga de testear las operaciones con ficheros XML.
    /// </summary>
    public static class Manager_XML    
    {
        private const string ROOT_NODE = "root";

        internal static void OnExecuteCommand(XMLOperations eOperation, Dictionary<string, object> oParams = null)
        {
            switch (eOperation)
            {
                case XMLOperations.CreateXML:
                     OpCreateXML(oParams);
                     break;
                case XMLOperations.CreateXMLWithRootNode:
                     OpCreateXMLWithRootNode(oParams);
                     break;
                case XMLOperations.CreateNodeWithAttributes:
                     OpCreateNodeWithAttributes(oParams);
                     break;
                case XMLOperations.CreateNodeWithoutAttributes:
                     OpCreateNodeWithoutAttributes(oParams);
                     break;
                case XMLOperations.ChangeNodeValue:
                     OpChangeValue(oParams);
                     break;
                case XMLOperations.ChangeNodeAttibute:
                     OpChangeAttribute(oParams);
                     break;
                case XMLOperations.ApplyPattern:
                     OnApplyPattern(oParams);
                     break;
                case XMLOperations.Undefined:
                default:
                     Manager_IU.Window_XML.Execution_Failed(MsgManager.LiteralMsg("Error, operación no reconocida."));
                     break;
            }
        }

        private static void OpCreateXML(Dictionary<string, object> oParams = null)
        {
            try
            {
                bool bMustClosed = (bool)oParams[CreateXML.CloseDocument.ToString()];
                XmlDocument xmlDoc = XMLManager.CreateDocument(bMustClosed);
                Manager_IU.Window_XML.Execution_OK(xmlDoc);
            }
            catch (Exception ex)
            {
                Manager_IU.Window_XML.Execution_Failed(MsgManager.LiteralMsg("OpCreateXML",ex));
            }
        }

        private static void OpCreateXMLWithRootNode(Dictionary<string, object> oParams = null)
        {
            try
            {
                bool bMustClosed = (bool) oParams[CreateXMLWithRootNode.CloseDocument.ToString()];
                string sNodo = (string)oParams[CreateXMLWithRootNode.Nodo.ToString()];
                string sTexto = (string) oParams[CreateXMLWithRootNode.Texto.ToString()];
                XmlDocument xmlDoc = XMLManager.CreateDocument(sNodo, sTexto, bMustClosed);
                Manager_IU.Window_XML.Execution_OK(xmlDoc);
            }
            catch (Exception ex)
            {
                Manager_IU.Window_XML.Execution_Failed(MsgManager.LiteralMsg("OpCreateXMLWithRootNode",ex));
            }
        }

        private static void OpCreateNodeWithAttributes(Dictionary<string, object> oParams = null)
        {
            try
            {
                bool bMustClosed = (bool)oParams[CreateXML.CloseDocument.ToString()];
                XmlDocument xmlDoc = XMLManager.CreateDocument(ROOT_NODE, "CreateNodeWithAttributes");
                SortedList slAttrib = new SortedList();
                slAttrib.Add("Info", "Fecha ejecución");
                XMLManager.AddNode(ref xmlDoc, ROOT_NODE, "Fecha", DateTime.Now.ToString(), bMustClosed, slAttrib);
                Manager_IU.Window_XML.Execution_OK(xmlDoc);
            }
            catch (Exception ex)
            {
                Manager_IU.Window_XML.Execution_Failed(MsgManager.LiteralMsg("OpCreateNodeWithAttributes", ex));
            }
        }

        private static void OpCreateNodeWithoutAttributes(Dictionary<string, object> oParams = null)
        {
            try
            {
                bool bMustClosed = (bool)oParams[CreateXML.CloseDocument.ToString()];

                XmlDocument xmlDoc = XMLManager.CreateDocument(ROOT_NODE, "CreateNodeWithoutAttributes");
                XMLManager.AddNode(ref xmlDoc, ROOT_NODE, "Data", "Execution data", bMustClosed);
                Manager_IU.Window_XML.Execution_OK(xmlDoc);
            }
            catch (Exception ex)
            {
                Manager_IU.Window_XML.Execution_Failed(MsgManager.LiteralMsg("OpCreateNodeWithoutAttributes", ex));
            }
        }

        private static void OpChangeValue(Dictionary<string, object> oParams = null)
        {
            try
            {
                bool bMustClosed = (bool)oParams[CreateXML.CloseDocument.ToString()];

                XmlDocument xmlDoc = XMLManager.CreateDocument(ROOT_NODE, "ChangeValue");
                XMLManager.AddNode(ref xmlDoc, ROOT_NODE, "Data", "Execution data");
                XMLManager.ChangeValue(ref xmlDoc, "Data", "Value data", bMustClosed);
                Manager_IU.Window_XML.Execution_OK(xmlDoc);
            }
            catch (Exception ex)
            {
                Manager_IU.Window_XML.Execution_Failed(MsgManager.LiteralMsg("OpChangeValue", ex));
            }
        }

        private static void OpChangeAttribute(Dictionary<string, object> oParams = null)
        {
            try
            {
                bool bMustClosed = (bool)oParams[CreateXML.CloseDocument.ToString()];
                XmlDocument xmlDoc = XMLManager.CreateDocument(ROOT_NODE, "ChangeAttribute");
                SortedList slAttrib = new SortedList();
                slAttrib.Add("Info", "Fecha ejecución");
                XMLManager.AddNode(ref xmlDoc, ROOT_NODE, "Fecha", DateTime.MinValue.ToString(), false, slAttrib);
                XMLManager.ChangeAttribute(ref xmlDoc, "Fecha", "Info", "Fecha indeterminada", bMustClosed);
                Manager_IU.Window_XML.Execution_OK(xmlDoc);
            }
            catch (Exception ex)
            {
                Manager_IU.Window_XML.Execution_Failed(MsgManager.LiteralMsg("OpChangeAttribute", ex));
            }
        }

        private static void OnApplyPattern(Dictionary<string, object> oParams)
        {
            try
            {
                bool bMustClosed = (bool)oParams[ApplyPattern.CloseDocument.ToString()];
                PatternType ePatternType = (PatternType)oParams[ApplyPattern.PatternName.ToString()];

                // ToDo: Realizar operaciones para crear el patrón.
                //Manager_IU.Window_XML.Execution_OK(xmlDoc);
            }
            catch (Exception ex)
            {
                Manager_IU.Window_XML.Execution_Failed(MsgManager.LiteralMsg("OpCreateXML", ex));
            }
        }
    }
}
