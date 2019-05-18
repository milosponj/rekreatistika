using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Rekreatistika.Helpers
{
    public static class MyHTMLHelpers
    {
        public static String ToSerbianDate(this IHtmlHelper htmlHelper, DateTime date)
        {
            return "x" + date.Year;
        }

		//so it can be called from view
		public static bool IsSuperAdmin(this IHtmlHelper htmlHelper, ClaimsPrincipal user)
		{
			return Utilities.IsSuperAdmin(user);
		}
	}
}
