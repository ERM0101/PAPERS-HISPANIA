#region Librerias usadas por la clase

using System;

#endregion

namespace MBCode.FrameworkDemoWFP.InterfazUsuario
{
    public class InfoValue : Attribute
    {
        private string _value;

        private Type _tEnumValue;

        public InfoValue(string value, Type tEnumValue)
        {
            _value = value;
            _tEnumValue = tEnumValue;
        }

        public string Value
        {
            get { return _value; }
        }

        public Type TypeOfEnum
        {
            get { return _tEnumValue;  }
        }
    }

}