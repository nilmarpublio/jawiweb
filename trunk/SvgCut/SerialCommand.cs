using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SvgCut
{
    public partial class SerialCommand : IDataErrorInfo
    {
        #region Properties
        private string nameField;
        private ParameterType parameterTypeField;
        private Object minValueField;
        private Object maxValueField;
        private bool successField;
        private string unitField;
        private List<KeyValuePair<int, string>> parameterOptionsField;

        private string groupIdField;
        public string GroupId
        {
            get
            {
                return this.groupIdField;
            }
            set
            {
                if ((this.groupIdField != null))
                {
                    if ((groupIdField.Equals(value) != true))
                    {
                        this.groupIdField = value;
                        this.OnPropertyChanged("GroupId");
                    }
                }
                else
                {
                    this.groupIdField = value;
                    this.OnPropertyChanged("GroupId");
                }
            }
        }

        private string parameterIdField;
        public string ParameterId
        {
            get
            {
                return this.parameterIdField;
            }
            set
            {
                if ((this.parameterIdField != null))
                {
                    if ((parameterIdField.Equals(value) != true))
                    {
                        this.parameterIdField = value;
                        this.OnPropertyChanged("ParameterId");
                    }
                }
                else
                {
                    this.parameterIdField = value;
                    this.OnPropertyChanged("ParameterId");
                }
            }
        }

        private Object parameterValueField;
        public Object ParameterValue
        {
            get
            {
                return this.parameterValueField;
            }
            set
            {
                if ((this.parameterValueField != null))
                {
                    if ((parameterValueField.Equals(value) != true))
                    {
                        this.parameterValueField = value;
                        this.OnPropertyChanged("ParameterValue");
                    }
                }
                else
                {
                    this.parameterValueField = value;
                    this.OnPropertyChanged("ParameterValue");
                }
            }
        }

        private bool enquiringField;
        public bool Enquiring
        {
            get { return this.enquiringField; }
            set
            {
                if ((this.enquiringField != null))
                {
                    if ((enquiringField.Equals(value) != true))
                    {
                        this.enquiringField = value;
                        this.OnPropertyChanged("Enquiring");
                    }
                }
                else
                {
                    this.enquiringField = value;
                    this.OnPropertyChanged("Enquiring");
                }
            }
        }

        private bool hasChanged;
        /// <summary>
        /// Determine whether this parameter command has been modified since load from setting.
        /// </summary>
        [XmlIgnore]
        public bool HasChanged { get { return this.hasChanged; } }
        private string errorField;
        #endregion

        #region Constructors
        public SerialCommand()
        {
            Initialize();
        }
        public SerialCommand(string name, ParameterType type)
        {
            Initialize();

            this.nameField = name;
            this.parameterTypeField = type;
            InitialDefault();
        }
        public SerialCommand(string name, Object value, ParameterType type)
        {
            Initialize();

            this.nameField = name;
            this.parameterTypeField = type;
            InitialDefault();

            this.parameterValueField = value;
        }
        public SerialCommand(string name, Object value)
        {
            Initialize();

            this.nameField = name;
            this.parameterValueField = value;
        }
        public SerialCommand(string name, Object value, Object maxValue)
        {
            Initialize();

            this.nameField = name;
            this.parameterValueField = value;
            this.maxValueField = maxValue;
            InitialMin();
        }
        public SerialCommand(string name, Object value, string unit)
        {
            Initialize();

            this.nameField = name;
            this.parameterValueField = value;
            this.unitField = unit;
        }
        public SerialCommand(string name, Object value, Object maxValue, string unit)
        {
            Initialize();

            this.nameField = name;
            this.parameterValueField = value;
            this.unitField = unit;
            this.maxValueField = maxValue;
            InitialMin();
        }
        #endregion

        #region Methods
        private void Initialize()
        {
            this.parameterTypeField = ParameterType.Integer;
            this.maxValueField = new Object();
            this.minValueField = new Object();
            this.parameterOptionsField = new List<KeyValuePair<int, string>>();
            this.parameterValueField = new Object();
            this.hasChanged = false;
            this.enquiringField = false;
            this.successField = false;
        }
        private void InitialDefault()
        {
            switch (parameterTypeField)
            {
                case ParameterType.String:
                    this.minValueField = string.Empty;
                    this.maxValueField = string.Empty;
                    break;
                case ParameterType.Integer:
                    this.minValueField = 0;
                    this.maxValueField = 0;
                    break;
                case ParameterType.Hex:
                    this.minValueField = 0x0000;
                    this.maxValueField = 0xFFFF;
                    break;
            }
        }
        private void InitialMin()
        {
            if (maxValueField.GetType() == typeof(Int32))
                this.minValueField = 0;
            else if (maxValueField.GetType() == typeof(int))
                this.minValueField = 0;
            else if (maxValueField.GetType() == typeof(decimal))
                this.minValueField = 0.0d;
            else if (maxValueField.GetType() == typeof(string))
                this.minValueField = string.Empty;
            //todo: initial minValue for hex type
        }
        /// <summary>
        /// Return to original state when first load the value.
        /// </summary>
        public void ResetState()
        {
            this.hasChanged = false;
        }
        public void SetEnquiring(bool inquired)
        {
            this.enquiringField = inquired;
            OnPropertyChanged("Enquiring");

            this.successField = false;
            OnPropertyChanged("Success");
        }
        public void SetSuccess()
        {
            this.successField |= true;
            System.Diagnostics.Debug.WriteLine("Set " + this.nameField + " to " + this.successField);
            OnPropertyChanged("Success");

            //reset
            this.enquiringField = false;
            this.hasChanged = false;
        }
        /// <summary>
        /// TODO: fail to set error message on orange background
        /// </summary>
        /// <param name="message"></param>
        public void SetError(string message)
        {
            this.errorField = message;
            OnPropertyChanged("Error");
        }
        #endregion

        #region IDataErrorInfo members
        [XmlIgnore]
        public string Error
        {
            get { return this.errorField; }
        }
        [XmlIgnore]
        public string this[string columnName]
        {
            get
            {
                this.errorField = string.Empty;
                if (this.hasChanged)
                {
                    Int32 min = 0, max = 0, value = 0;
                    switch (columnName)
                    {
                        case "ParameterValue":
                            switch (this.parameterTypeField)
                            {
                                case ParameterType.String:
                                    break;
                                case ParameterType.Integer:
                                    min = (Int32)this.minValueField;
                                    max = (Int32)this.maxValueField;
                                    value = 0;
                                    Int32.TryParse(parameterValueField.ToString(), out value);
                                    if (value == 0 || value < min || value > max)
                                        this.errorField = string.Format("Value must between {0} and {1}.", min, max);
                                    break;
                                case ParameterType.Hex:
                                    min = Convert.ToInt32(this.minValueField);
                                    max = Convert.ToInt32(this.maxValueField);
                                    if (max == -1)
                                    {
                                        if (!IsHexString("0x" + parameterValueField.ToString().ToLower(), 8))
                                            this.errorField = "Value must between 0x00000000 and 0xFFFFFFFF";
                                    }
                                    else
                                    {
                                        value = 0;
                                        Int32.TryParse(parameterValueField.ToString(), out value);
                                        if (value < min || value > max)
                                            this.errorField = "Value must between 0x0000 and 0xFFFF";
                                    }
                                    break;
                            }
                            break;
                    }
                }

                return this.errorField;
            }
        }
        #endregion
        /// <summary>
        /// Return true if it is in the correct format of n digit hex string.
        /// </summary>
        /// <param name="sender">Input string to check.</param>
        /// <param name="digit">Normally 4 or 8.</param>
        /// <returns></returns>
        private bool IsHexString(string sender, int digit)
        {
            Match match = Regex.Match(sender, "[0][x][0-9a-fA-F]{" + digit + "}");
            if (match.Success)
            {
                if (match.Groups[0].Value.Equals(sender))
                    return true;
            }

            return false;
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler handler = this.PropertyChanged;
            if ((handler != null))
            {
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                if (propertyName.Equals("ParameterValue"))
                    this.hasChanged |= true;
            }
        }
    }
}