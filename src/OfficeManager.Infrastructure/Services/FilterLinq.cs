using AutoMapper.Execution;
using OfficeManager.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OfficeManager.Infrastructure.Services
{
    public class FilterLinq : IFilterLinq
    {
        public Expression GetPropertyExpression(Expression pe, string chain)
        {
            var properties = chain.Split('.');
            foreach (var property in properties)
            {
                pe = Expression.Property(pe, property);
            }

            return pe;
        }

        public Expression<Func<T, bool>> GetWherePredicate<T>(Dictionary<string, string> SearchFieldList)
        {
            ParameterExpression pe = Expression.Parameter(typeof(T), typeof(T).Name);

            //combine them with and 1=1 Like no expression
            Expression combined = null;

            if (SearchFieldList != null)
            {
                foreach (var fieldItem in SearchFieldList)
                {
                    //Expression for accessing Fields name property
                    Expression columnNameProperty = GetPropertyExpression(pe, fieldItem.Key);

                    //the name constant to match 
                    Expression columnValue = Expression.Constant(fieldItem.Value);

                    object info = GetValueWithType<T>(fieldItem.Key, fieldItem.Value);

                    Expression e1 = null;
                    if (info.GetType().ToString().Contains("System.String"))
                    {
                        MethodInfo contains = info.GetType().GetMethods().Where(a => a.Name == "Contains").FirstOrDefault();
                        e1 = Expression.Call(columnNameProperty, contains, Expression.Constant(info, typeof(string)));
                    }
                    else
                        e1 = Expression.Equal(columnNameProperty, Expression.Constant(info));

                    if (combined == null)
                    {
                        combined = e1;
                    }
                    else
                    {
                        combined = Expression.And(combined, e1);
                    }
                }
            }

            //create and return the predicate
            return Expression.Lambda<Func<T, bool>>(combined, new ParameterExpression[] { pe });
        }

        private object GetValueWithType<T>(string proertyName,string value)
        {
            PropertyInfo pInfo = typeof(T).GetProperty(proertyName);
            if (pInfo == null) return typeof(String);
            if(pInfo.PropertyType.FullName.Equals("System.String"))
                return Convert.ToString(value);
            else if(pInfo.PropertyType.FullName.Contains("System.Int16"))
                return Convert.ToInt16(value);
            else if (pInfo.PropertyType.FullName.Contains("System.Int32"))
                return Convert.ToInt32(value);
            else if (pInfo.PropertyType.FullName.Contains("System.Int64"))
                return Convert.ToInt64(value);
            else if (pInfo.PropertyType.FullName.Contains("System.DateTime"))
                return Convert.ToDateTime(value);
            else if (pInfo.PropertyType.FullName.Contains("System.TimeSpan"))
                return Convert.ToDateTime(value);
            else if (pInfo.PropertyType.FullName.Contains("System.Decimal"))
                return Convert.ToDecimal(value);
            else if (pInfo.PropertyType.FullName.Contains("System.Byte"))
                return Convert.ToByte(value);
            else if (pInfo.PropertyType.FullName.Contains("System.Double"))
                return Convert.ToDouble(value);
            else if(pInfo.PropertyType.FullName.Equals("System.Boolean"))
                return Convert.ToBoolean(value);
            
            return Convert.ToString(value);
        }
    }
}
