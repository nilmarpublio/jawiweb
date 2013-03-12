using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

using NUnit.Framework;

namespace JawiPatch
{
	[TestFixture]
	class Program
	{
		static void Main(string[] args)
		{
			//20121104
			SvgRepair repair = new SvgRepair();//@"G:\works\Output");
			repair.Patch();
			
			Console.Read();
		}
		
		private void WriteLine(string line)
		{
			System.Diagnostics.Debug.WriteLine(line);
			//Console.WriteLine(line);
		}
		
		private string Rename(string fileName,string extension)
		{
			//remove extension and get the file name only.
			fileName = fileName.Replace(extension,"");
			
			string newFileName = fileName.ToLower();
			
			Dictionary<string,string> dictionary = new Dictionary<string, string>();
			dictionary.Add("b.","bin");
			dictionary.Add("binti","bt");
			dictionary.Add("haji","hj");
			dictionary.Add("hajah","hjh");
			dictionary.Add("hajjah","hjh");
			foreach(KeyValuePair<string,string> pair in dictionary)
				newFileName = newFileName.Replace(pair.Key,pair.Value);
			
			newFileName = newFileName.Replace(".","");
			newFileName = TrimBracket(newFileName);
			
			newFileName += extension;
			return newFileName;
		}
		private string TrimBracket(string value)
		{
			int indexOfBracket = value.IndexOf('(');
			if (indexOfBracket > -1)
				return value.Substring(0, indexOfBracket);
			else
				return value;
		}
		[Test]
		public void SubstringTest()
		{
			string target = "fauziyah bt hj mohamad(5080a)";
			string expected = "fauziyah bt hj mohamad";
			string actual = TrimBracket(target);
			Assert.AreEqual(expected,actual);
		}
		/// <summary>
		/// Rename all files under a source folder to a new destination.
		/// </summary>
		/// <param name="source">Source folder.</param>
		/// <param name="destination">Destination folder.</param>
		private void RenameFile(string source, string destination, out int count)
		{
			count = 0;
			DirectoryInfo directoryInfo = new DirectoryInfo(source);
			FileInfo[] fileInfos = directoryInfo.GetFiles();
			foreach(FileInfo fileInfo in fileInfos)
			{
				++count;
				string newFileName = Rename(fileInfo.Name,fileInfo.Extension);
				fileInfo.CopyTo(destination+Path.DirectorySeparatorChar+newFileName,true);
				WriteLine("converting '"+fileInfo.Name+"' to '"+newFileName+"'");
			}
		}
		[Test]
		public void RenameTest()
		{
			//20121025
			int count = 0;
			DateTime start = DateTime.Now;
			WriteLine("Start: "+start);
			try
			{
				RenameFile(@"D:\JawiName", @"D:\JawiName2",out count);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
				Console.Read();
			}
			DateTime end = DateTime.Now;
			TimeSpan spent = end.Subtract(start);
			WriteLine("End: "+end);
			WriteLine("spent: "+spent+" for "+count+" files");
		}
		/// <summary>
		/// Print out word and count in console.
		/// </summary>
		/// <remarks>
		/// since 20100917.
		/// </remarks>
		[Test]
		public void CountWord()
		{
			WordScales wordScales = new WordScales();
			wordScales.Option = WordScalesOption.Word;

			string content = string.Empty;
			DirectoryInfo directoryInfo = new DirectoryInfo(@"E:\JawiName");
			FileInfo[] files = directoryInfo.GetFiles();
			foreach (FileInfo file in files)
				content += file.Name.ToLower() + " ";
			wordScales.Count(content);
			foreach (KeyValuePair<string, int> item in wordScales.Result)
				Console.WriteLine("{0}\t{1}", item.Key, item.Value);
		}
		/// <summary>
		/// Rename sticker item according to male or female item if has.
		/// </summary>
		/// <remarks>
		/// since 2010-09-07.
		/// </remarks>
		[Test]
		public void BreakStickerItem()
		{
			Console.WriteLine("Start rename sticker item according to male or female item.");
			
			DataSet dataSet = new DataSet();
			dataSet.ReadXml("nisan.xml");
			if (dataSet.Tables.Count == 0) return;

			DataTable table = dataSet.Tables[0];
			foreach (DataRow row in table.Rows)
			{
				if (row["item"].ToString() == "1½' Sticker")
				{
					if (row["name"].ToString().Contains(" bin "))
						row["item"] = "1½' Sticker(L)";
					if (row["name"].ToString().Contains(" bt "))
						row["item"] = "1½' Sticker(P)";
				}
				if (row["item"].ToString() == "2' Sticker")
				{
					if (row["name"].ToString().Contains(" bin "))
						row["item"] = "2' Sticker(L)";
					if (row["name"].ToString().Contains(" bt "))
						row["item"] = "2' Sticker(P)";
				}
				if (row["item"].ToString() == "2½' Sticker")
				{
					if (row["name"].ToString().Contains(" bin "))
						row["item"] = "2½' Sticker(L)";
					if (row["name"].ToString().Contains(" bt "))
						row["item"] = "2½' Sticker(P)";
				}
			}
			table.AcceptChanges();
			table.WriteXml("nisan2.xml");
			
			Console.WriteLine("Complete");
		}
		
		/// <summary>
		/// A task to fill up all jawi with rumi whatever we have up to date.
		/// </summary>
		/// <remarks>
		/// Since 2013-02-07
		/// </remarks>
		[Test]
		public void FillJawiForRumi()
		{
			DataSet dataSet = new DataSet();
			dataSet.ReadXml("e:\\works\\nisan.xml");
			DataTable table = dataSet.Tables[0].Copy();
			dataSet.Dispose();
			
			Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
			foreach(DataRow row in table.Rows)
			{
				//Console.WriteLine(row["name"].ToString());
				//Console.WriteLine(row["jawi"].ToString());
				
				char[] delimiters = new char[]{' ', '@', '/', '\\','.','.','،'};
				string[] rumis = row["name"].ToString().ToLower().Split(delimiters);
				string[] jawis = row["jawi"].ToString().Split(delimiters);
				if(rumis.Length == jawis.Length)
				{
					for(int i=0;i<rumis.Length;i++)
					{
						if(!dictionary.ContainsKey(rumis[i]))
						{
							List<string> jawi = new List<string>();
							jawi.Add(jawis[i]);
							dictionary.Add(rumis[i], jawi);
						}
						else
						{
							List<string> jawi = dictionary[rumis[i]];
							if(!jawi.Contains(jawis[i]))
								dictionary[rumis[i]].Add(jawis[i]);
						}
					}
				}
			}
			
			//write out result
			foreach(KeyValuePair<string, List<string>> dict in dictionary)
			{
				string output = string.Empty;
				output += dict.Key;
				foreach(string v in dict.Value)
					output += "\t" + v;
				Console.WriteLine(output);
			}
		}
	}
}