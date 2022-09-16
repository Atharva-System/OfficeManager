using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OfficeManager.Application.Common.Interfaces
{
    public interface IFilterLinq
    {
        public Expression GetPropertyExpression(Expression pe, string chain);

        public Expression<Func<T, Boolean>> GetWherePredicate<T>(Dictionary<string, string> SearchFieldList);
    }
}
