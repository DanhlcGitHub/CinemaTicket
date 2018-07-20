using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CrawlData.Utility
{
    public class FormatDataUtility
    {
        public string formatScript(string input)
        {
            string output = "";
            // remove all script
            Regex rRemScript = new Regex(@"<script[^>]*>[\s\S]*?</script>");
            output = rRemScript.Replace(input, "");///<!--.*?-->/
            // remove all special character
            output = Regex.Replace(output, "[^0-9a-zA-Z |<|>|=|\"]+", "");
            return output;
        }

        public string removeAllComment(string input)
        {
            var blockComments = @"/\*(.*?)\*/";
            var lineComments = @"//(.*?)\r?\n";
            var strings = @"""((\\[^\n]|[^""\n])*)""";
            var verbatimStrings = @"@(""[^""]*"")+";
            var htmlComments = @"/<!--.*?-->/";

            string noComments = Regex.Replace(input,
                    blockComments + "|" + lineComments + "|" + strings + "|" + verbatimStrings + "|" + htmlComments,
                    me =>
                    {
                        if (me.Value.StartsWith("/*") || me.Value.StartsWith("//"))
                            return me.Value.StartsWith("//") ? Environment.NewLine : "";
                        // Keep the literal strings
                        return me.Value;
                    },
                    RegexOptions.Singleline);
            return noComments;
        }


    }
}