#region Librerias usada por la clase

using MBCode.Framework.Managers.Messages;
using MBCode.Framework.Managers.XML;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;
using System.Xml;

#endregion

namespace MBCode.FrameworkDemoWFP.InterfazUsuario
{
    #region Enumerados

    public enum XMLOperations
    {
        Undefined                   = -1,
        CreateXML                   =  0,
        CreateXMLWithRootNode       =  1,
        CreateNodeWithoutAttributes =  2,
        CreateNodeWithAttributes    =  3,
        ChangeNodeValue             =  4,
        ChangeNodeAttibute          =  5,
        ApplyPattern                =  6,
    }

    public enum PatternType
    {
        [InfoValue("Patrón basico", null)]
        Basic,
    }

    public enum CreateXML
    {
        CloseDocument,
    }

    public enum ApplyPattern
    {
        PatternName,
        CloseDocument,
    }

    public enum CreateXMLWithRootNode
    {
        Nodo,
        Texto,
        CloseDocument,
    }

    #endregion

    /// <summary>
    /// Lógica de interacción para WindowDemoCulture.xaml
    /// </summary>
    public partial class WindowDemoXML : Window
    {
        #region Eventos

        public delegate void dlgExecuteCommand(XMLOperations eOperation, Dictionary<string, object> oParams = null);

        public event dlgExecuteCommand evExecuteCommand;

        #endregion

        #region Atributos

        XMLOperations eOperation;

        Dictionary<string, object> oParams;

        #endregion

        #region Propiedades

        private XMLOperations Operation
        {
            set 
            {
                eOperation = value;
                ActualizarEstado();
            }
        }

        #endregion

        #region Constructores

        public WindowDemoXML()
        {
            InitializeComponent();            
            this.Loaded += new RoutedEventHandler(OnWindowLoaded);
            this.Closing += new CancelEventHandler(OnWindowClosing);
        }

        #endregion

        #region Gestores de los eventos de la Ventana

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            rbCreateXML.Checked += new RoutedEventHandler(OnCheckedOperation);
            rbCreateXMLWithRootNode.Checked += new RoutedEventHandler(OnCheckedOperation);
            rbNodeWithAttributes.Checked += new RoutedEventHandler(OnCheckedOperation);
            rbNodeWithoutAttributes.Checked += new RoutedEventHandler(OnCheckedOperation);
            rbChangeNodeValue.Checked += new RoutedEventHandler(OnCheckedOperation);
            rbChangeNodeAttribute.Checked += new RoutedEventHandler(OnCheckedOperation);
            rbApplyPatterm.Checked += new RoutedEventHandler(OnCheckedOperation);
            btnDoAction.Click += new RoutedEventHandler(OnDoAction);
            cbPattern.ItemsSource = GetDictionaryFromEnum(typeof(PatternType));
            cbPattern.SelectedValuePath = "Value";
            cbPattern.DisplayMemberPath = "Key";
            InsertCommandInfo("Initialitation");
            rbCreateXML.IsChecked = true;
            oParams = null;
        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            rbCreateXML.Checked -= new RoutedEventHandler(OnCheckedOperation);
            rbCreateXMLWithRootNode.Checked -= new RoutedEventHandler(OnCheckedOperation);
            rbNodeWithAttributes.Checked -= new RoutedEventHandler(OnCheckedOperation);
            rbNodeWithoutAttributes.Checked -= new RoutedEventHandler(OnCheckedOperation);
            rbChangeNodeValue.Checked -= new RoutedEventHandler(OnCheckedOperation);
            rbChangeNodeAttribute.Checked -= new RoutedEventHandler(OnCheckedOperation);
            rbApplyPatterm.Checked -= new RoutedEventHandler(OnCheckedOperation);
            btnDoAction.Click -= new RoutedEventHandler(OnDoAction);
        }

        #endregion

        #region Gestores de los eventos de los Controles de la Ventana
        
        private void OnDoAction(object sender, System.EventArgs e)
        {
            //string sXmlDoc = "<?xml version=\"1.0\"?> " +
            //                 "  <Root Contains=\"Root description\">" +
            //                 "     <AnteTest Type=\"System.String\">Ante Texto</AnteTest>" +
            //                 "     <Test Type=\"System.String\">Texto</Test>" +
            //                 "  </Root>";
            //XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.LoadXml(sXmlDoc);
            //System.Collections.SortedList slAtrib = new System.Collections.SortedList();
            //slAtrib.Add("Type", typeof(string));
            //XMLManager.AddNode(ref xmlDoc, XMLManager.ROOT_NODE, "MidTest", "Mid Texto", false, slAtrib, FormatXML.Normal, LocationXML.Specific, 10);
            if (ValidateData())
            {
                if (evExecuteCommand != null) evExecuteCommand(eOperation, oParams);
            }
        }

        private bool ValidateData()
        {
            if (oParams == null) oParams = new Dictionary<string, object>();
            else oParams.Clear();
            switch (eOperation)
            {
                case XMLOperations.CreateXML:
                     oParams.Add(CreateXML.CloseDocument.ToString(), true);
                     return (true);
                case XMLOperations.CreateXMLWithRootNode:
                     if ((tbNodo.Text.Equals(string.Empty) || tbTexto.Text.Equals(string.Empty)))
                     {
                         MsgManager.ShowMessage(MsgManager.LiteralMsg("Error, ninguno de los parámetros puede ser nulo."));
                         return (false);
                     }
                     else
                     {
                         oParams.Add(CreateXMLWithRootNode.CloseDocument.ToString(), true);
                         oParams.Add(CreateXMLWithRootNode.Nodo.ToString(), tbNodo.Text);
                         oParams.Add(CreateXMLWithRootNode.Texto.ToString(), tbTexto.Text);
                         return (true);
                     }
                case XMLOperations.CreateNodeWithAttributes:
                case XMLOperations.CreateNodeWithoutAttributes:
                case XMLOperations.ChangeNodeValue:
                case XMLOperations.ChangeNodeAttibute:
                     oParams.Add(CreateXML.CloseDocument.ToString(), true);
                     return (true);
                case XMLOperations.ApplyPattern:
                     oParams.Add(ApplyPattern.PatternName.ToString(),  (PatternType) cbPattern.SelectedValue);
                     oParams.Add(ApplyPattern.CloseDocument.ToString(), true);
                     return (true);
                case XMLOperations.Undefined:
                default:
                     return (true);
            }        
        }

        private void OnCheckedOperation(object sender, RoutedEventArgs e)
        {
            if (rbCreateXML.IsChecked.Equals(true)) Operation = XMLOperations.CreateXML;
            else if (rbCreateXMLWithRootNode.IsChecked.Equals(true)) Operation = XMLOperations.CreateXMLWithRootNode;
            else if (rbNodeWithAttributes.IsChecked.Equals(true)) Operation = XMLOperations.CreateNodeWithAttributes;
            else if (rbNodeWithoutAttributes.IsChecked.Equals(true)) Operation = XMLOperations.CreateNodeWithoutAttributes;
            else if (rbChangeNodeValue.IsChecked.Equals(true)) Operation = XMLOperations.ChangeNodeValue;
            else if (rbChangeNodeAttribute.IsChecked.Equals(true)) Operation = XMLOperations.ChangeNodeAttibute;
            else if (rbApplyPatterm.IsChecked.Equals(true)) Operation = XMLOperations.ApplyPattern;
            else eOperation = XMLOperations.Undefined;
        }

        #endregion

        #region Métodos de uso público

        public void Execution_OK(XmlDocument xmlDoc)
        {
            InsertCommandAction(eOperation.ToString(), XMLManager.ToString(xmlDoc));
        }

        public void Execution_Failed(string sMessage)
        {
            MsgManager.ShowMessage(sMessage);
        }

        #endregion

        #region Internos

        private void ActualizarEstado()
        {
            Visibility ViewData = ((eOperation.Equals(XMLOperations.CreateXMLWithRootNode)) ? Visibility.Visible : Visibility.Hidden);
            tbNodo.Visibility = tbTexto.Visibility = lblNodo.Visibility = lblTexto.Visibility = ViewData;
            Visibility ViewPatternData = ((eOperation.Equals(XMLOperations.ApplyPattern)) ? Visibility.Visible : Visibility.Hidden);
            lblPattern.Visibility = cbPattern.Visibility = ViewPatternData;
            switch (eOperation)
            {
                case XMLOperations.CreateXML:
                case XMLOperations.CreateNodeWithAttributes:
                case XMLOperations.CreateNodeWithoutAttributes:
                case XMLOperations.ChangeNodeValue:
                case XMLOperations.ChangeNodeAttibute:
                     break;
                case XMLOperations.CreateXMLWithRootNode:
                     tbNodo.Text = "root";
                     tbTexto.Text = "Node root";
                     break;
                case XMLOperations.ApplyPattern:
                     cbPattern.Text = GetTextValue(PatternType.Basic);
                     break;
                case XMLOperations.Undefined:
                default:
                     break;
            }
        }

        #endregion

        #region Auxiliares

        private void InsertCommandInfo(string sCommandInfo)
        {
            if (!tbResult.Text.Equals(string.Empty))
            {
                tbResult.Inlines.InsertBefore(tbResult.Inlines.FirstInline, new LineBreak());
                tbResult.Inlines.InsertBefore(tbResult.Inlines.FirstInline,
                                              new Bold(new Run { Text = DateTime.Now.ToString() + " : " + sCommandInfo }));
            }
            else tbResult.Inlines.Add(new Bold(new Run { Text = DateTime.Now.ToString() + " : " + sCommandInfo }));
        }

        private void InsertCommandAction(string sCommandInfo, string sCommandResult)
        {
            tbResult.Inlines.InsertBefore(tbResult.Inlines.FirstInline, new LineBreak());
            tbResult.Inlines.InsertBefore(tbResult.Inlines.FirstInline, new Italic(new Run { Text = "\n" + sCommandResult + "\n" }));
            InsertCommandInfo(sCommandInfo);
        }

        private string GetTextValue(Enum value)
        {
            Type type = value.GetType();
            return ((type.GetField(value.ToString()).GetCustomAttributes(typeof(InfoValue), false)[0] as InfoValue).Value);
        }

        private Type GetTypeValue(Enum value)
        {
            Type type = value.GetType();
            return ((type.GetField(value.ToString()).GetCustomAttributes(typeof(InfoValue), false)[0] as InfoValue).TypeOfEnum);
        }

        private Dictionary<string, Enum> GetDictionaryFromEnum(Type tEnumType)
        {
            Dictionary<string, Enum> dcGroups = new Dictionary<string, Enum>();
            foreach (Enum e in Enum.GetValues(tEnumType))
            {
                dcGroups.Add(GetTextValue(e), e);
            }
            return (dcGroups);
        }

        private List<string> GetTextFromEnumValues(Type tEnumType)
        {
            List<string> lstQueries = new List<string>();
            foreach (Enum e in Enum.GetValues(tEnumType))
            {
                lstQueries.Add(GetTextValue(e));
            }
            return (lstQueries);
        }
        #endregion
    }
}
