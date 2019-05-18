using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rekreatistika
{
    public class Utilities
    {
        public static string GetCurrentUserId(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public static string GetLogoPath(string folder, int id, string logoExtension)
        {
            var localPath = @"C:\temp"; //read from web config TODO
            return String.Format("{0}/{1}/{2}{3}", localPath, folder, id, logoExtension);
        }

        public static IList<string> ExtractEmails(string text)
        {
            //instantiate with this pattern 
            Regex emailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
                RegexOptions.IgnoreCase);
            
            IList<string> matches = new List<string>();
            if (!string.IsNullOrEmpty(text))
            {
                //find items that matches with our pattern
                MatchCollection emailMatches = emailRegex.Matches(text);
                foreach (System.Text.RegularExpressions.Match emailMatch in emailMatches)
                {
                    matches.Add(emailMatch.Value);
                }
            }

            return matches;

        }

		internal static bool IsSuperAdmin(ClaimsPrincipal user)
		{
			return user.Claims.Select(x=>x.Value).Contains("SuperAdmin");
		}
	}
}
