using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.Xml;
using System.Net;
using System.Xml.XPath;

namespace CurrencyService
{
    public class CurrencyService : ICurrencyService
    {
        private static readonly string _directory = "C:\\TEMP\\";
        private static readonly string _lastUpdateFile = "lastUpdate.txt";
        private static readonly string _urlNBP = "http://www.nbp.pl/kursy/xml/LastA.xml";
        private static readonly string _currenciesFile = "waluty.xml";
        public string GetDate()
        {
            try
            {
                if (IsUpdateNeed())
                    Update();
                XmlDocument reader = new XmlDocument();
                reader.Load(Path.Combine(_directory, _currenciesFile));
                string date = reader.GetElementsByTagName("data_publikacji").Item(0).InnerText;                
                return date;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string[] GetCodes()
        {
            try
            {
                if (IsUpdateNeed())
                    Update();
                XmlDocument reader = new XmlDocument();
                reader.Load(Path.Combine(_directory,_currenciesFile));
                int count = reader.GetElementsByTagName("kod_waluty").Count;
                string[] myCodes = new string[count];
                for (int i = 0; i < count; i++)
                {
                    myCodes[i] = reader.GetElementsByTagName("kod_waluty").Item(i).InnerText;
                }
                return myCodes;                                
            }
            catch (Exception e)
            {
                string[] myCodes = {e.Message};
                return myCodes;
            }            
        }
        public string[] GetRate(string value)
        {
            try
            {
                if (IsUpdateNeed())
                    Update();
            
            string[] myRate = new string[2];
            XPathDocument readDocument = new XPathDocument(Path.Combine(_directory,_currenciesFile));
            XPathNavigator navigator = readDocument.CreateNavigator();
            myRate[0] = navigator.SelectSingleNode("/tabela_kursow/pozycja[kod_waluty=\""+value+"\"]/przelicznik").Value;
            myRate[1] = navigator.SelectSingleNode("/tabela_kursow/pozycja[kod_waluty=\"" + value + "\"]/kurs_sredni").Value;
            return myRate;
            }
            catch (Exception e)
            {
                string[] er = {e.Message};
                return er;
            }
        }

        private bool IsUpdateNeed()
        {
            if (Directory.Exists(_directory)&&File.Exists(Path.Combine(_directory,_lastUpdateFile))&&File.Exists(Path.Combine(_directory,_currenciesFile)))
            {
                using (var sr = new StreamReader(Path.Combine(_directory,_lastUpdateFile)))
                {
                    DateTimeOffset date = DateTimeOffset.Parse(sr.ReadLine());
                    if ((DateTimeOffset.Now - date).TotalMinutes >= 1440)
                        return true;
                }
            }
            else return true;
            return false;
        }

        private void Update()
        {
            WebClient readNBP = new WebClient();               
            if (!Directory.Exists(_directory))
                Directory.CreateDirectory(_directory);
            readNBP.DownloadFile(new Uri(_urlNBP), Path.Combine(_directory,_currenciesFile));
            using (var sw = new StreamWriter(Path.Combine(_directory,_lastUpdateFile), false))
            {
                sw.WriteLine(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 20, 00));
            }
        }
    }
}
