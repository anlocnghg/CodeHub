using LocLib.Extensions;

using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

using System;

using TSS.Lib.Common.Models;

namespace TSS
{
    public sealed class DateIsWeekendOrHolidayToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int max = parameter == null ? 100 : int.Parse(parameter.ToString());

            DateTime date = (DateTime)value;
            if (date.IsNewYearsDay())
            {
                return "New Year’s Day";
            }
            else if (date.IsMartinLutherKingDay())
            {
                if (parameter == null)
                {
                    return "Birthday of Martin Luther King, Jr."; // Birthday of Martin Luther King, Jr.
                }
                else
                {
                    return "Birthday of Martin Luther King, Jr.".FirstNCharacters(max) + "...";
                }
            }
            else if (date.IsPresidentDay())
            {
                if (parameter == null)
                {
                    return "Washington’s Birthday";
                }
                else
                {
                    return "Washington’s Birthday".FirstNCharacters(max) + "...";
                }
            }
            else if (date.IsMemorialDay())
            {
                return "Memorial Day";
            }
            else if (date.IsJuneteenthDay())
            {
                if (parameter == null)
                {
                    return "Juneteenth National Independence Day"; // Juneteenth National Independence Day
                }
                else
                {
                    return "Juneteenth National Independence Day".FirstNCharacters(max) + "...";
                }
            }
            else if (date.IsIndependenceDay())
            {
                if (parameter == null)
                {
                    return "Independence Day";
                }
                else
                {
                    return "Independence Day".FirstNCharacters(max) + "...";
                }
            }
            else if (date.IsLaborDay())
            {
                return "Labor Day";
            }
            else if (date.IsColumbusDay())
            {
                return "Columbus Day";
            }
            else if (date.IsVeteransDay())
            {
                return "Veterans Day";
            }
            else if (date.IsThanksgivingDay())
            {
                return "Thanksgiving Day";
            }
            else if (date.IsChristmasDay())
            {
                return "Christmas Day";
            }
            else
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class IsWeekendOrHolidayToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value is bool && (bool)value) ? new SolidColorBrush(Colors.OrangeRed) : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class DateToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var date = (DateTime)value;
            return date.ToString("ddd, dd MMM yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class WhatShiftToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            WhatShift shift = (WhatShift)value;
            switch (shift)
            {
                case WhatShift.FirstShift:
                    return "0600 - 1530 PT";

                case WhatShift.SecondShift:
                    return "1600 - 0230 PT";

                case WhatShift.ThirdShift:
                    return "2000 - 0600 PT";

                default:
                    return "N/A";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
