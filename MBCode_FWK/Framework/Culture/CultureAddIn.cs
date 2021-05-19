#region Librerias usadas por la clase

using MBCode.Framework.Managers.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

#endregion

namespace MBCode.Framework.Managers.Culture
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha: 16/02/2012.
    /// Descripción: elemento que se incrusta en cada control o formulario en el que se desea que se aplique el cambio de
    ///              cultura.
    /// </summary>
    [ToolboxItem(true)]
    public class CultureAddIn : Component
    {
        #region Atributos

        /// <summary>
        /// Control en el que se desea aplicar la localización
        /// </summary>
        private Control _managedControl;

        /// <summary>
        /// Almacena la lista de propiedades que serán excluídas de la localización.
        /// </summary>
        private List<string> _excludeProperties = new List<string>();

        /// <summary>
        /// Almacena un valor que indica si se debe llevar a cabo o no la sincronización del elemento en el que se ha
        /// colocado el AddIn.
        /// </summary>
        private bool _synchronizeCulture = true;

        /// <summary>
        /// Si es 'true' conserva el tamaño al cambiar la cultura.
        /// </summary>
        private bool _preserveFormSize = true;

        /// <summary>
        /// Si es 'true' conserva la posición al cambiar la cultura.
        /// </summary>
        private bool _preserveFormLocation = true;

        /// <summary>
        /// Define la alineación horizontal del control.
        /// </summary>
        private const AnchorStyles anchorLeftRight = AnchorStyles.Left | AnchorStyles.Right;

        /// <summary>
        /// Define la alineación vertical del control.
        /// </summary>
        private const AnchorStyles anchorTopBottom = AnchorStyles.Top | AnchorStyles.Bottom;

        /// <summary>
        /// Define la alineación combinada (horizontal y vertical) del control.
        /// </summary>
        private const AnchorStyles anchorAll = anchorLeftRight | anchorTopBottom;

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene/Establece un valor que indica si se debe llevar a cabo o no la sincronización del elemento en el 
        /// que se ha colocado el AddIn.
        /// </summary>
        [DefaultValue(true)]
        [Description("Should the UICulture of this form be changed when the ApplicationUICulture")]
        public bool SynchronizeCulture
        {
            get 
            { 
                return (_synchronizeCulture); 
            }
            set 
            { 
                _synchronizeCulture = value; 
            }
        }

        /// <summary>
        /// Lista de propiedades que serán excluídas de la localización.
        /// </summary>
        [Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", 
                typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Lista de propiedades que serán excluídas de la localización.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<string> ExcludeProperties
        {
            get 
            { 
                return (_excludeProperties); 
            }
            set
            {
                if (value == null) throw new ArgumentNullException(MsgManager.ErrorMsg("MSG_CultureAddIn_00", "null"));
                _excludeProperties = value;
            }
        }

        /// <summary>
        /// Almacena el control que implementará los procedimientos de localización. Es importante que esté aquí aunque  no
        /// se visualize ni sea usada por el usuario, ya que de esta manera el diseñador crea la línea de código que asigna
        /// el formulario al que desea aplicar la localización a este atributo. 
        /// </summary>
        [Browsable(false)]
        [Description("Control que implementará los procedimientos de localización.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Control ManagedControl
        {
            get
            {
                if (_managedControl == null)
                {
                    if (Site != null)
                    {
                        IDesignerHost host = Site.GetService(typeof(IDesignerHost)) as IDesignerHost;
                        if (host != null && host.Container != null && host.Container.Components.Count > 0)
                        {
                            _managedControl = host.Container.Components[0] as Control;
                        }
                    }
                }
                return (_managedControl);
            }
            set
            {
                if (value == null) throw new ArgumentNullException(MsgManager.ErrorMsg("MSG_CultureAddIn_001", "null"));
                _managedControl = value;
            }
        }

        /// <summary>
        /// Obtiene/Establece un valor que indica que si es 'true' conserve el tamaño al cambiar la cultura.
        /// </summary>
        [DefaultValue(true)]
        [Description("Indica que si se conserva o no el tamaño de los controles al cambiar la cultura")]
        public bool PreserveFormSize
        {
            get 
            { 
                return (_preserveFormSize); 
            }
            set 
            { 
                _preserveFormSize = value; 
            }
        }

        /// <summary>
        /// Obtiene/Establece un valor que indica que si es 'true' conserve la posición al cambiar la cultura.
        /// </summary>
        [DefaultValue(true)]
        [Description("Indica que si se conserva o no la posición de los controles al cambiar la cultura")]
        public bool PreserveFormLocation
        {
            get 
            { 
                return (_preserveFormLocation); 
            }
            set 
            { 
                _preserveFormLocation = value; 
            }
        }

        #endregion

        #region Constructores y Destructores

        /// <summary>
        /// Constructor por defecto de la clase.
        /// </summary>
        public CultureAddIn()
        {
        }

        /// <summary>
        /// Create a new instance of the component.
        /// </summary>
        public CultureAddIn(IContainer container)
            : this()
        {
            container.Add(this);
            CultureManager.evChangeCulture += new CultureManager.dlgChangeCulture(OnChangeCulture);
        }

        /// <summary>
        /// Destructor por defecto de la clase.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CultureManager.evChangeCulture -= new CultureManager.dlgChangeCulture(OnChangeCulture);
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Métodos

        #region Cambio de Cultura de los elementos del formulario.

        /// <summary>
        /// Método que se ejecuta al cambiar la cultura.
        /// </summary>
        private void OnChangeCulture()
        {
            if ((ManagedControl != null) && (SynchronizeCulture))
            {
                _managedControl.SuspendLayout();
                foreach (Control childControl in _managedControl.Controls)
                {
                    childControl.SuspendLayout();
                }
                try
                {
                    //  Procede a aplicar el cambio de cultura.
                        ApplyResources(_managedControl.GetType(), _managedControl, CultureManager.ActualCulture);
                }
                finally
                {
                    foreach (Control childControl in _managedControl.Controls)
                    {
                        childControl.ResumeLayout();
                    }
                    _managedControl.ResumeLayout();
                }
            }
        }

        /// <summary>
        /// Recursively apply localized resources to a component and its constituent components
        /// </summary>
        /// <param name="componentType">The type we are applying resources for</param>
        /// <param name="instance">The component instance to apply resources to</param>
        protected virtual void ApplyResources(Type componentType, IComponent instance, CultureInfo culture)
        {
            if (componentType.Assembly.GetManifestResourceStream(componentType.FullName + ".resources") == null) return;
            if (componentType.BaseType != null) ApplyResources(componentType.BaseType, instance, culture);
            SortedList<string, object> resources = new SortedList<string, object>();
            LoadResources(new ComponentResourceManager(componentType), resources, culture);
            Dictionary<string, IComponent> components = new Dictionary<string, IComponent>();
            Dictionary<Type, IExtenderProvider> extenderProviders = new Dictionary<Type, IExtenderProvider>();
            components["$this"] = instance;
            FieldInfo[] fields = componentType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (FieldInfo field in fields)
            {
                //  In VB the field names are prepended with an "underscore" so we need to remove this
                    string fieldName = field.Name;
                    if (IsVBAssembly(componentType.Assembly)) fieldName = fieldName.Substring(1, fieldName.Length - 1);
                //  Check whether this field is a localized component of the parent
                    if (resources.ContainsKey(">>" + fieldName + ".Name"))  // ResourceName
                    {
                        IComponent childComponent = field.GetValue(instance) as IComponent;
                        if (childComponent != null)
                        {
                            //  Apply resources localized in the child component type
                                components[fieldName] = childComponent;
                                ApplyResources(childComponent.GetType(), childComponent, culture);
                            //  If this component is an extender provider then keep track of it
                                if (childComponent is IExtenderProvider)
                                {
                                    extenderProviders[childComponent.GetType()] = childComponent as IExtenderProvider;
                                }
                        }
                    }
            }
            AssignValues(resources, components, extenderProviders);
        }

        #region Auxiliares

        /// <summary>
        /// Carga los recursos asociados al elemento ordenados por nombre de recurso.
        /// </summary>
        /// <param name="rm">ResourceManager para cargar los recursos del formulario.</param>
        /// <param name="resources">Lista de recursos cargados.</param>
        /// <param name="culture">Cultura para la que se cargan los recursos.</param>
        private void LoadResources(ComponentResourceManager rm, SortedList<string, object> resources, CultureInfo culture)
        {
            if (!culture.Equals(CultureInfo.InvariantCulture))
            {
                LoadResources(rm, resources, culture.Parent);
            }
            ResourceSet resourceSet = rm.GetResourceSet(culture, true, true);
            if (resourceSet != null)
            {
                foreach (DictionaryEntry entry in resourceSet)
                {
                    resources[(string)entry.Key] = entry.Value;
                }
            }
        }

        private void AssignValues(SortedList<string, object> resources, Dictionary<string, IComponent> components,
                                  Dictionary<Type, IExtenderProvider> extenderProviders)
        {
            foreach (KeyValuePair<string, object> pair in resources)  // Pair :- Resource <name, value>.
            {
                string propertyName = pair.Key.Split('.')[1];
                if (pair.Key.Split('.')[0].StartsWith(">>")) continue; // ComponentName
                if (_excludeProperties.Contains(propertyName)) continue;
                IComponent component = null;
                if (!components.TryGetValue(pair.Key.Split('.')[0], out component)) continue;
                Control control = component as Control;
                if (control != null)
                {
                    switch (propertyName)
                    {
                        case "Size":
                            SetControlSize(control, (Size)pair.Value);
                            continue;
                        case "Location":
                            SetControlLocation(control, (Point)pair.Value);
                            continue;
                        case "ClientSize":
                            if (control is Form && PreserveFormSize) continue;
                            break;
                    }
                }
                PropertyDescriptor pd = TypeDescriptor.GetProperties(component).Find(propertyName, false);
                if (((pd != null) && (!pd.IsReadOnly)) &&
                    ((pair.Value == null) || pd.PropertyType.IsInstanceOfType(pair.Value)))
                {
                    pd.SetValue(component, pair.Value);
                }
                else
                {
                    if (control != null) ApplyExtenderResource(extenderProviders, control, propertyName, pair.Value);
                }
            }
        }

        /// <summary>
        /// Set the size of a control handling docked/anchored controls appropriately
        /// </summary>
        /// <param name="control">The control to set the size of</param>
        /// <param name="size">The new size of the control</param>
        protected virtual void SetControlSize(Control control, Size size)
        {
            //  if the control is a form and we are preserving form size then exit
                if (control is Form && PreserveFormSize) return;
            //  if dock fill or anchor all is set then don't change the size
                if (control.Dock == DockStyle.Fill || control.Anchor == anchorAll) return;
            //  if docked top/bottom or anchored left/right don't change the width
                if (control.Dock == DockStyle.Top || control.Dock == DockStyle.Bottom ||
                   (control.Anchor & anchorLeftRight) == anchorLeftRight)
                {
                    size.Width = control.Width;
                }
            //  if docked left/right or anchored top/bottom don't change the height
                if (control.Dock == DockStyle.Left || control.Dock == DockStyle.Right ||
                   (control.Anchor & anchorTopBottom) == anchorTopBottom)
                {
                    size.Height = control.Height;
                }
                control.Size = size;
        }

        /// <summary>
        /// Set the location of a control handling docked/anchored controls appropriately
        /// </summary>
        /// <param name="control">The control to set the location of</param>
        /// <param name="location">The new location of the control</param>
        protected virtual void SetControlLocation(Control control, Point location)
        {
            //  if the control is a form and we are preserving form location then exit
                if (control is Form && PreserveFormLocation) return;
            //  if dock is set then don't change the location
                if (control.Dock != DockStyle.None) return;
            //  if anchored to the right (but not left) then don't change x coord
                if ((control.Anchor & anchorLeftRight) == AnchorStyles.Right)
                {
                    location.X = control.Left;
                }
            //  if anchored to the bottom (but not top) then don't change y coord
                if ((control.Anchor & anchorTopBottom) == AnchorStyles.Bottom)
                {
                    location.Y = control.Top;
                }
                control.Location = location;
        }

        /// <summary>
        /// Apply a resource for an extender provider to the given control
        /// </summary>
        /// <param name="extenderProviders">Extender providers for the parent control indexed by type</param>
        /// <param name="control">The control that the extended resource is associated with</param>
        /// <param name="propertyName">The extender provider property name</param>
        /// <param name="value">The value to apply</param>
        /// <remarks>
        /// This can be overridden to add support for other ExtenderProviders.  The default implementation
        /// handles <see cref="ToolTip">ToolTips</see>, <see cref="HelpProvider">HelpProviders</see>,
        /// and <see cref="ErrorProvider">ErrorProviders</see>
        /// </remarks>
        protected virtual void ApplyExtenderResource(Dictionary<Type, IExtenderProvider> extenderProviders,
                                                     Control control, string propertyName, object value)
        {
            IExtenderProvider extender = null;

            if (propertyName == "ToolTip")
            {
                if (extenderProviders.TryGetValue(typeof(ToolTip), out extender))
                {
                    (extender as ToolTip).SetToolTip(control, value as string);
                }
            }
            else if (propertyName == "HelpKeyword")
            {
                if (extenderProviders.TryGetValue(typeof(HelpProvider), out extender))
                {
                    (extender as HelpProvider).SetHelpKeyword(control, value as string);
                }
            }
            else if (propertyName == "HelpString")
            {
                if (extenderProviders.TryGetValue(typeof(HelpProvider), out extender))
                {
                    (extender as HelpProvider).SetHelpString(control, value as string);
                }
            }
            else if (propertyName == "ShowHelp")
            {
                if (extenderProviders.TryGetValue(typeof(HelpProvider), out extender))
                {
                    (extender as HelpProvider).SetShowHelp(control, (bool)value);
                }
            }
            else if (propertyName == "Error")
            {
                if (extenderProviders.TryGetValue(typeof(ErrorProvider), out extender))
                {
                    (extender as ErrorProvider).SetError(control, value as string);
                }
            }
            else if (propertyName == "IconAlignment")
            {
                if (extenderProviders.TryGetValue(typeof(ErrorProvider), out extender))
                {
                    (extender as ErrorProvider).SetIconAlignment(control, (ErrorIconAlignment)value);
                }
            }
            else if (propertyName == "IconPadding")
            {
                if (extenderProviders.TryGetValue(typeof(ErrorProvider), out extender))
                {
                    (extender as ErrorProvider).SetIconPadding(control, (int)value);
                }
            }
        }

        /// <summary>
        /// Return true if the given assembly was compiled using VB.NET
        /// </summary>
        /// <param name="assembly">The assembly to check</param>
        /// <returns>True if the assembly was compiled using VB</returns>
        protected static bool IsVBAssembly(Assembly assembly)
        {
            AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
            foreach (AssemblyName refAssembly in referencedAssemblies)
            {
                if (refAssembly.Name == "Microsoft.VisualBasic")
                    return true;
            }
            return false;
        }

        #endregion

        #endregion

        #endregion
    }
}
