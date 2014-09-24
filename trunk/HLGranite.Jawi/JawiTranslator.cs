using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace HLGranite.Jawi
{
    /// <summary>
    /// Jawi Translator class.
    /// </summary>
    public class JawiTranslator
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <remarks>
        /// TODO: Lookup Google translate.
        /// </remarks>
        public JawiTranslator()
        {
        }
        /// <summary>
        /// Translate rumi to jawi.
        /// </summary>
        /// <param name="rumi"></param>
        /// <returns></returns>
        public string Translate(string rumi)
        {
            string jawi = string.Empty;
            string html = string.Empty;
            string queryString = "http://www.ejawi.net/v3/getTranslationRumiJawi.php?rumi={0}&jenis=RJ&teknik=DK";
            WebRequest req = WebRequest.Create(string.Format(queryString, rumi));
            //req.Proxy = GetProxy();
            if (req.Proxy != null) req.Credentials = req.Proxy.Credentials;
            using (WebResponse res = req.GetResponse())
            {
                using (StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding("utf-8")))
                    html = sr.ReadToEnd();
            }

            //WebClient web = new WebClient();
            //web.Proxy = GetProxy();
            //if (web.Proxy != null) web.Credentials = web.Proxy.Credentials;
            //return web.DownloadString(string.Format(queryString, rumi));

            //Sample html response
            /*
             * <div id="content" class="translation">
             * <span style="cursor: help" id="pagi" trans="ڤاݢي<"ڤاݢي</span>
             * </div>
             */
            Match match = Regex.Match(html, "trans=\".+\"");
            if (match.Success)
            {
                //trans="ڤاݢي"
                string hold = match.Groups[0].Value;
                string[] holds = hold.Split(new char[] { '=' });
                jawi = holds[holds.Length - 1].Trim(new char[] { '"' });
            }

            return jawi;
        }
        /// <summary>
        /// Get proxy from configuration.
        /// </summary>
        /// <returns></returns>
        private WebProxy GetProxy()
        {
            //TODO: move to app.setting
            string proxyName = "co-proxy-003";
            string userName = "yeang-shing.then";
            string password = "Q1w2e3r4a";

            WebProxy proxy = new WebProxy(proxyName, 80);
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                if (!userName.Contains(@"\"))
                    proxy.Credentials = new NetworkCredential(userName, password);
                else
                {
                    string[] userInfo = userName.Split('\\');
                    proxy.Credentials = new NetworkCredential(userInfo[1], password, userInfo[0]);
                }
            }

            return proxy;
        }
    }
}