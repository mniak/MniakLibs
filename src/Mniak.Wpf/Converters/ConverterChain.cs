using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Mniak.WPF.Converters
{
    /// <summary>Represents a chain of <see cref="IValueConverter"/>s to be executed in succession.</summary>
    [ContentProperty("Converters")]
    [ContentWrapper(typeof(ValueConverterCollection))]
    public class ConverterChain : IValueConverter
    {
        ValueConverterCollection converters;
        /// <summary>Gets the converters to execute.</summary>
        public ValueConverterCollection Converters
        {
            get { return converters ?? (converters = new ValueConverterCollection()); }
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var valueConverter in Converters)
            {
                value = valueConverter.Convert(value, targetType, parameter, culture);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }

        #endregion
    }

    /// <summary>Represents a collection of <see cref="IValueConverter"/>s.</summary>
    public sealed class ValueConverterCollection : Collection<IValueConverter> { }
}
