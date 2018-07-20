
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace CrawlData.Utility
{
    public class XMLHandler
    {
        public void LoadTransactionFile(String path)
        {
            List<Film> filmList = new List<Film>();
            XmlTextReader reader = new XmlTextReader(path);

            StringBuilder xml = new StringBuilder();
            while (reader.Read())
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            String Element = "<" + reader.Name + ">";
                            xml.Append(Element);
                            break;
                        case XmlNodeType.Text: //Display the text in each element.
                            String Type = reader.Value;
                            xml.Append(Type);
                            break;
                        case XmlNodeType.EndElement: //Display the end of the element.
                            String EndElement = "</" + reader.Name + ">";
                            xml.Append(EndElement);
                            break;
                    }
                }
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.ToString());
            XmlNodeList idNodes = doc.SelectNodes("Transactions/Transaction");
            foreach (XmlNode node in idNodes)
            {
                Film aFilm = new Film();
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    XmlNode childNode = node.ChildNodes[i];
                    String element = childNode.Name;
                    String value = childNode.InnerText;
                   /* if (element == "Account")
                    {
                        transaction.Account = value;
                    }
                    else if (element == "Type")
                    {
                        transaction.Type = value;
                    }
                    else if (element == "Amount")
                    {
                        transaction.Amount = Convert.ToDouble(value);
                    }*/
                }
                filmList.Add(aFilm);
            }
            //return transactions;
        }
    }
}