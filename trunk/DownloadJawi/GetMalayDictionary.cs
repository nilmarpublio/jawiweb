/*
 * Created by SharpDevelop.
 * User: yeang-shing.then
 * Date: 5/29/2013
 * Time: 4:11 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

using NUnit.Framework;

namespace DownloadJawi
{
	/// <summary>
	/// Description of Main.
	/// </summary>
	[TestFixture]
	public class GetMalayDictionary
	{
		private DataTable table;
		/// <summary>
		/// A dictionary sorted by key.
		/// </summary>
		private SortedDictionary<string, string> dictionary;
		
		/// <summary>
		/// Initialize dataset structure.
		/// </summary>
		private void Initialize()
		{
			dictionary = new SortedDictionary<string, string>();
			
			DataColumn column1 = new DataColumn("rumi", typeof(string));
			DataColumn column2 = new DataColumn("jawi", typeof(string));
			
			table = new DataTable("entry");
			table.Columns.Add(column1);
			table.Columns.Add(column2);
		}
		
		/// <summary>
		/// Access the website and extract Malay word.
		/// </summary>
		/// <param name="url"></param>
		private bool Extract(string url)
		{
			try
			{
				WebRequest req = WebRequest.Create(url);
				using(WebResponse res = req.GetResponse())
				{
					using(StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding("utf-8")))
					{
						string line = string.Empty;
						while(reader.Read() > 0)
						{
							line = reader.ReadLine();
							Match match = Regex.Match(line, "<h1><a .*</a></h1>");
							if(match.Success)
							{
								//System.Diagnostics.Debug.WriteLine(match.Groups[0].Value);
								Match match2 = Regex.Match(match.Groups[0].Value,"\">.*</a>");
								if(match2.Success)
								{
									string chop = match2.Groups[0].Value;
									//System.Diagnostics.Debug.WriteLine(chop);
									string word = chop.Substring(2, chop.Length-6);
									System.Diagnostics.Debug.WriteLine(word);
									if(!dictionary.ContainsKey(word))
										dictionary.Add(word, string.Empty);
								}
							}
						}
						//html = reader.ReadToEnd();
					}
				}
				
				return true;
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex);
				return false;
			}
		}
		
		/// <summary>
		/// Write output as xml.
		/// </summary>
		private void WriteXml()
		{
			foreach(KeyValuePair<string, string> entry in dictionary)
			{
				// only care about single word
				if(!entry.Key.Contains(" "))
				{
					DataRow row = table.NewRow();
					row["rumi"] = entry.Key.ToLower();
					table.Rows.Add(row);
				}
			}
			
			DataSet dataSet = new DataSet("entries");
			dataSet.Tables.Add(table);	
			dataSet.AcceptChanges();
			dataSet.WriteXml("kamus.xml");
		}
		
		[Test]
		public void Execute()
		{
			System.Diagnostics.Debug.WriteLine(System.DateTime.Now);
			Initialize();
			
			// start extracting
			bool success = true;
			string html = string.Empty;
			for(int i=1;i<759;i++)
			{
				string url = @"http://mykamus.com/free/category/mykamus-malay-english/page/{0}/";
				success &= Extract(string.Format(url,i));
				if(!success) break;
			}
			
			WriteXml();
			
			Assert.AreNotEqual(0, dictionary.Count);
			System.Diagnostics.Debug.WriteLine(System.DateTime.Now);
		}
		
		/// <summary>
		/// Test and equal operator.
		/// </summary>
		[Test]
		public void AndFalseTest()
		{
			bool actual = true;
			actual &= true;
			actual &= false;
			actual &= true;
			Assert.IsFalse(actual);
		}
	}
}
