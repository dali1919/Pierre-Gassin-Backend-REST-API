using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebsiteRESTAPI.Helper
{
    public static class StringExtensions
    {
        /// <summary>  
        /// Removes all accents from the input string.  
        /// </summary>  
        /// <param name="text">The input string.</param>  
        /// <returns></returns>  
        public static string RemoveAccents(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            char[] chars = text
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c)
                != UnicodeCategory.NonSpacingMark).ToArray();

            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        /// <summary>  
        /// Turn a string into a slug by removing all accents,   
        /// special characters, additional spaces, substituting   
        /// spaces with hyphens & making it lower-case.  
        /// </summary>  
        /// <param name="phrase">The string to turn into a slug.</param>  
        /// <returns></returns>  
        public static string Slugify(this string phrase)
        {
            // Remove all accents and make the string lower case.  
            string output = phrase.RemoveAccents().ToLower();

            // Remove all special characters from the string.  
            output = Regex.Replace(output, @"[^A-Za-z0-9\s-]", "");

            // Remove all additional spaces in favour of just one.  
            output = Regex.Replace(output, @"\s+", " ").Trim();

            // Replace all spaces with the hyphen.  
            output = Regex.Replace(output, @"\s", "-");

            // Return the slug.  
            return output;
        }
        public static string AppendSeizesList(List<string > seizes)
        {
            StringBuilder builder = new StringBuilder();
            foreach(var item in seizes)
            {
                builder.Append($"{item}\n\n");
            }
            return (builder.ToString());
        }
        public static string CreateEmailBody(string to, string details,string seizes )
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"Dear {to} \n\n\t");
            builder.Append("Thanks for choosing our photos, following are your billing details:\n\n");
            builder.Append($"{details}\n\n");
            builder.Append("you can find your photos under the following links:\n\n");
            builder.Append($"{seizes}\n\n\t");
            builder.Append("Best regards");
            return builder.ToString();
 
        }
    }
}
