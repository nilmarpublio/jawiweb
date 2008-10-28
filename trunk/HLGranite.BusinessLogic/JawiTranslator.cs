using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;

namespace HLGranite.BusinessLogic
{
	/// <summary>
	/// Jawi tool.
	/// </summary>
	public class JawiTranslator
	{
		/// <summary>
		/// Maximum list out name so that not take much resource to get name list.
		/// </summary>
		private const int MaximumList = 8;

		/// <summary>
		/// Jawi name table (contains all related data).
		/// </summary>
		/// <remarks>
		/// Consist columns [JawiName],[JawiValue],[JawiSex],[JawiKey],and [JawiCode].
		/// </remarks>
		public DataTable JawiTable = new DataTable("Jawi");
		/// <summary>
		/// Dictionary path.
		/// </summary>
		private string sourcePath;
	
		/// <summary>
		/// Default constructor.
		/// </summary>
		public JawiTranslator(string source)
		{
			if(File.Exists(source))
			{
				sourcePath = source;

				DataSet lds_dataset = new DataSet();
				lds_dataset.ReadXml(source,XmlReadMode.Auto);
				if(lds_dataset.Tables.Count>0)
					JawiTable = lds_dataset.Tables[0].Copy();
			}

		}	


		/// <summary>
		/// Convert the ASCII character to arabic character set.
		/// </summary>
		/// <param name="font">Arabic font.</param>
		/// <param name="source">Original text to translate.</param>
		/// <returns></returns>
		public string ConvertToArabic(Font font,string source)
		{
			if(source == string.Empty) return string.Empty;

			string output = "";			
			string[] ascii = new string[]{};//a valid recognize jawi pronounce

			#region remark
			/* int lastIndex = 0;//indicate last substring position
			ArrayList list = new ArrayList();
			for(int j=0;j<source.Length;j++)
			{
				string ls_Check = source.Substring(j,1);
				if(IsConsonant(Convert.ToChar(ls_Check)))
				{
					if(j-lastIndex>0)
					{
						string s = source.Substring(lastIndex,j-lastIndex);
						list.Add(s);
						lastIndex = lastIndex + s.Length;
					}
					
					list.Add(ls_Check);
					lastIndex++;
				}

				//last string
				if(j==source.Length-1)
				{
					if(j-lastIndex > -1)
					{
						string s = source.Substring(lastIndex, Math.Max(source.Length-lastIndex,1));
						list.Add(s);
						lastIndex = lastIndex + s.Length;
					}
				}
			}//end loops
			
			ascii = new string[list.Count];
			for(int j=0;j<ascii.Length;j++)
				ascii[j] = list[j].ToString(); */

			#endregion

			char[] pieces = new char[source.Length];
			int i = -1;//couter
			foreach(char c in source)
			{
				i++;
				pieces[i] = c;
			}//end loops

			ArrayList al = new ArrayList();
			for(int j=1;j<pieces.Length;j++)
			{
				string ls_Mix = new string(new char[]{pieces[j-1],pieces[j]});
				if( IsJawi(ls_Mix) )
					al.Add(ls_Mix);
				else
				{
					if(j==1) al.Add(pieces[0].ToString());
					al.Add(pieces[j].ToString());
				}
			}//end loops
			//al.Add(pieces[pieces.Length-1].ToString());//add last index


			ascii = new string[al.Count];
			for(int j=0;j<ascii.Length;j++)
				ascii[j] = al[j].ToString();

			//start basic mapping
			for(int j=0;j<ascii.Length;j++)
				output += MapArabicCharacter(font.GdiCharSet,ascii[j]);

			//correct with thesaurus
			output = Arabic.Thesaurus.Synchronize(source,output);

			return output;
		}
		/// <summary>
		/// Check whether the input character is consonant word.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public bool IsConsonant(char source)
		{
			if(source == 'a' || source == 'A'
				|| source == 'e' || source == 'E'
				|| source == 'i' || source == 'I'
				|| source == 'o' || source == 'O'
				|| source == 'u' || source == 'U'
				|| source == ' ')
				return true;
			else
				return false;
		}		
		/// <summary>
		/// Check whether is a recognized jawi character (eg. ng,ch).
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public bool IsJawi(string source)
		{
			if(source.ToLower() == "ch"
				|| source.ToLower() == "ng"
				|| source.ToLower() == "ny"
				|| source.ToLower() == "sh"
				|| source.ToLower() == "sy" 
				|| source.ToLower() == "th" )
				return true;
			else
				return false;
		}
		/// <summary>
		/// Mapping with selected arabic font.
		/// </summary>
		/// <param name="font">Arabic font.</param>
		/// <param name="source">Source character.</param>
		/// <returns></returns>
		public static string MapArabicCharacter(int charset,string source)
		{
			string output = "";

			Arabic.CharSet = charset;
			if(source.Length == 1)
			{
				#region one length
				switch(Convert.ToChar(source))
				{
						#region number (??????????)
					case '0':
						output = Arabic.Get0();//Arabic.Zero;
						break;
					case '1':
						output = Arabic.Get1();//Arabic.One;
						break;
					case '2': 
						output = Arabic.Get2();//Arabic.Two;
						break;
					case '3':
						output = Arabic.Get3();//Arabic.Three;
						break;
					case '4':
						output = Arabic.Get4();//Arabic.Four;
						break;
					case '5':
						output = Arabic.Get5();//Arabic.Five;
						break;
					case '6':
						output = Arabic.Get6();//Arabic.Six;
						break;
					case '7':
						output = Arabic.Get7();//Arabic.Seven;
						break;
					case '8':
						output = Arabic.Get8();//Arabic.Eight;
						break;
					case '9':
						output = Arabic.Get9();//Arabic.Nine;
						break;
						#endregion


						//alef, ?
					case 'a':
					case 'A':
						output = Arabic.GetA();//Arabic.Alef;//Arabic.Ain;//Arabic.Alef;
						break;

						//beh, ?
					case 'b':
					case 'B':
						output = Arabic.GetB();//Arabic.Beh;
						break;

					case 'c':
					case 'C':
						output = Arabic.GetC();//Arabic.Tcheh;
						break;

						//dah, ?
					case 'd':
					case 'D':
						output = Arabic.GetD();//Arabic.Dal;
						break;
					
						//case 'e': same like 'i'
						//case 'E':
						//break;

					case 'f':
					case 'F':
						output = Arabic.GetF();
						break;

						//Gah, ?
					case 'g':
					case 'G':
						output = Arabic.GetG();//Arabic.Gaf;
						break;

						//heh, ?
					case 'h':
					case 'H':
						output = Arabic.GetH();//Arabic.Heh;
						break;

						//Yeh, ?
					case 'e':
					case 'E':
					case 'i':
					case 'I':
					case 'v':
					case 'V':
					case 'y':
					case 'Y':
						output = Arabic.GetI();//Arabic.Yeh;
						break;

					case 'j':
					case 'J':
						output = Arabic.GetJ();//Arabic.Jeem;
						break;

						//Kekeh, ?
					case 'k':
					case 'K':
						output = Arabic.GetK();//Arabic.Kaf;//Arabic.Kekeh;
						break;

						//lam, ?
					case 'l':
					case 'L':
						output = Arabic.GetL();//Arabic.Lam;
						break;

						//Meem, ?
					case 'm':
					case 'M':
						output = Arabic.GetM();//Arabic.Meem;
						break;

						//noon, ?
					case 'n':
					case 'N':
						output = Arabic.GetN();//Arabic.Noon;
						break;

						//Waw, ?
					case 'o':
					case 'O':
					case 'u':
					case 'U':
					case 'w':
					case 'W':
						output = Arabic.GetO();//Arabic.Waw;
						break;

						//Peh, ?
					case 'p':
					case 'P':
						output = Arabic.GetP();//Arabic.Peh;
						break;
						
						//Qaf, ?
					case 'q':
					case 'Q':
						output = Arabic.GetQ();//Arabic.Qaf;
						break;

						//Reh, ?
					case 'r':
					case 'R':
						output = Arabic.GetR();//Arabic.Reh;
						break;

						//Seen, ?
					case 's':
					case 'S':
						output = Arabic.GetS();//Arabic.Seen;
						break;

						//Teh Marbuta, ?
					case 't':
					case 'T':
						output = Arabic.GetT();//Arabic.Teh;//Arabic.Tah
						break;

						//Waw, ?
						//case 'u': same like 'o'
						//case 'U':
						//	output = "\u0648";
						//	break;

						//case 'v': same like 'i'
						//case 'V':
						//	break;
						//case 'w':
						//case 'W':
						//	break;

						//case 'x': //leave
						//case 'X':
						//	break;

						//case 'y': same 'i'
						//case 'Y':
						//	break;

						//tal, ?
					case 'z':
					case 'Z':
						output = Arabic.GetZ();//Arabic.Thal;
						break;

					case ' ':
						output = " ";
						break;

					default:
						output = source;
						break;

				}//end switch
				#endregion
			}
			else if(source.Length == 2)
			{
				#region two length
				switch(source.ToLower())
				{
						//cheh, ?
					case "ch":
						output = Arabic.Tcheh;
						break;

						//case "ng":
						//	break;

						//Sheen, ? 
					case "sh":
						output = Arabic.Sheen;
						break;

						//Theh, ?
					case "th":
						output = Arabic.Tah;
						break;
				}
				#endregion
			}

			return output;
		}

		/// <summary>
		/// Check whether is 'bin' or 'binti'.
		/// </summary>
		/// <param name="sex">'bin', or 'binti'.</param>
		/// <returns>1=male,2=female.</returns>
		private int IsSex(string sex)
		{
			string filtered = sex.Trim().TrimEnd('.');
			if(filtered.ToLower() == "bin" || filtered.ToLower() == "b")
				return 1;
			else if(filtered.ToLower() == "binti" || filtered.ToLower() == "bt" || filtered.ToLower() == "b.t")
				return 2;

			return 0;
		}

		/// <summary>
		/// Return sex by rumi name given.
		/// </summary>
		/// <param name="name">Rumi name.</param>
		/// <returns>1=male,2=female.</returns>
		private int GetSex(string name)
		{
			int sex = 0;
			DataRow[] ldr_selected = JawiTable.Select("JawiName = '"+name.Trim().TrimEnd('.')+"'");
			if(ldr_selected.Length>0)
				sex = ConvertToInteger(ldr_selected[0]["JawiSex"]);

			return sex;
		}
		/// <summary>
		/// Check whether name already contains bin or binti.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool HasSex(string name)
		{
			try
			{
				name = name.Trim().TrimEnd('.');
				string[] names = name.Split(' ');
				name = names[names.Length-1];

				if(name.IndexOf("bin",0,3)>-1 || name.IndexOf("b",0,1)>-1)
					return true;
				else if(name.IndexOf("binti",0,5)>-1 || name.IndexOf("bt",0,2)>-1 || name.IndexOf("b.t",0,3)>-1)
					return true;
				else
					return false;
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex);
				return false;
			}
		}
		/// <summary>
		/// Auto add up bin or binti.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public string AddSex(string name)
		{
			int sex = GetSex(name);
			if(sex == 1)
				name += " bin ";
			else if(sex == 2)
				name += " binti ";

			return name;
		}
		/// <summary>
		/// Check whether is a person title (eg. hj,hajjah).
		/// </summary>
		/// <param name="val">Text to check.</param>
		/// <returns></returns>
		private bool IsTitle(string val)
		{
			if(val == null) return false;
			val = val.ToLower();
			val = val.Trim().TrimEnd('.');
			if(IsSex(val)==1 || IsSex(val)==2)
				return true;
			if(val == "hj" || val == "haji")
				return true;
			else if(val == "hajjah" || val == "hjh" || val == "hjjh")
				return true;
			else
				return false;
		}

		/// <summary>
		/// Save jawi name into database(xml).
		/// </summary>
		/// <param name="name">jawi name in alphabet.</param>
		/// <param name="val">jawi name in jawi text.</param>
		/// <returns></returns>
		public bool Save(string name,string val)
		{
			try
			{
				name = name.ToLower();
				name = name.Trim().TrimEnd('.');
				string[] names = name.Split(' ');

				val = val.ToLower();
				val = val.Trim().TrimEnd('.');
				string[] jawis = val.Split(' ');

				int sex = 0;
				int marker = -1;
				//determine male or female
				for(int i=0;i<names.Length;i++)
				{
					if(IsSex(names[i])>0)
					{
						sex = IsSex(names[i]);
						marker = i;
					}
				}

				if(sex != 1 && sex != 2) return false;//just a valid sex (but not gay or lesbian)
				for(int i=0;i<names.Length;i++)
				{
					if(!IsTitle(names[i]))
					{
						DataRow[] ldr_selected = JawiTable.Select("JawiName = '"+names[i].ToLower()+"'");
						if(ldr_selected.Length == 1)
						{
							//update frequency
							if(i<marker)
								ldr_selected[0]["Frequency"] = ConvertToInteger(ldr_selected[0]["Frequency"])+1;
						}
						else if(ldr_selected.Length==0)
						{
							//add new
							DataRow row			= JawiTable.NewRow();
							row["JawiName"]		= names[i];
							row["JawiSex"]		= i<marker ? sex:1;//after bin/binti is a father name
							row["JawiValue"]	= jawis[i];
							row["JawiKey"]		= "";
							row["JawiCode"]		= "";
							row["Frequency"]	= 1;
							JawiTable.Rows.Add(row);
						}
					}
				}//end loops
				

				JawiTable.AcceptChanges();
				return true;
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex);
				return false;
			}
		}
		/// <summary>
		/// Sort database and saveed.
		/// </summary>
		/// <param name="col">Column to be sort.</param>
		public void SaveWithSorted(string col)
		{
			DataSet lds_dataset = new DataSet();

			try
			{				
				DataView view = JawiTable.DefaultView;
				view.Sort = col;

				DataTable hold = JawiTable.Clone();
				for(int i=0;i<view.Count;i++)
				{
					DataRow newRow = hold.NewRow();
					for(int j=0;j<JawiTable.Columns.Count;j++)
						newRow[j] = view[i][j];
					hold.Rows.Add(newRow);
				}//end loops

				hold.AcceptChanges();				
				lds_dataset.Tables.Add(hold);
				lds_dataset.WriteXml(sourcePath);
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex);
				return;
			}
			finally{lds_dataset.Dispose();}
		}

		/// <summary>
		/// Convert object (datarow here) to integer. If null return 0.
		/// </summary>
		/// <param name="sender">DataRow object (eg. row["Frequency"]).</param>
		/// <returns>integer.</returns>
		private int ConvertToInteger(object sender)
		{
			if(sender == null)
				return 0;
			else
			{
				System.Text.RegularExpressions.Regex objNotIntPattern = new System.Text.RegularExpressions.Regex("[^0-9-]");
				System.Text.RegularExpressions.Regex objIntPattern = new System.Text.RegularExpressions.Regex("^-[0-9]+$|^[0-9]+$");
				if(!objNotIntPattern.IsMatch(sender.ToString()) &&  objIntPattern.IsMatch(sender.ToString()))
					return Math.Max(0,Convert.ToInt32(sender));//make sure no negative number

				return 0;
			}

        }


        #region Quaranteen
        /// <summary>
        /// Get a collection of name list by given started text.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="target">Target object to set name list into.</param>
        /// <param name="key"></param>
        /* public void GetNearestNameList(object sender,object target,string key)
        {
            if(key.Trim().Length == 0) return;

            try
            {
                if(target.GetType() == typeof(System.Windows.Forms.ContextMenu))
                {
                    #region ContextMenu
                    (target as ContextMenu).MenuItems.Clear();
                    DataRow[] ldr_selected = JawiTable.Select("JawiName LIKE '"+key+"%'");
                    if(ldr_selected.Length > 0)
                    {
                        (target as ContextMenu).Show( (sender as Control), new Point(0, (sender as Control).Height) );
                        for(int i=0;i<Math.Min(ldr_selected.Length,MaximumList);i++)
                        {
                            (target as ContextMenu).MenuItems.Add( ldr_selected[i]["JawiName"].ToString());
                            (target as ContextMenu).MenuItems[i].Click +=new EventHandler(MenuItem_Click);
                        }
                    }
                    #endregion
                }
                else if(target.GetType() == typeof(System.Windows.Forms.ListBox))
                {
                    #region ListBox
                    (target as ListBox).Visible	= false;
                    (target as ListBox).Items.Clear();

                    string[] str = key.Split(' ');
                    if(str[str.Length-1] == "") return;
                    if(str[str.Length-1].IndexOf("'") > -1) return;//data table fail to filter 'quote' sign
                    DataRow[] ldr_selected = JawiTable.Select("JawiName LIKE '"+str[str.Length-1]+"%'","JawiName");
                    if(ldr_selected.Length > 0)
                    {
                        (target as ListBox).Height = 17 * Math.Min(MaximumList,ldr_selected.Length);//.ItemHeight
                        (target as ListBox).Visible	= true;
                        System.Drawing.Graphics g = (target as ListBox).CreateGraphics();
                        float mostWiden = 0f;

                        char[] lc_Trim = new char[str[str.Length-1].Length];
                        for(int i=str[str.Length-1].Length-1;i>=0;i--)
                            lc_Trim[i] = Convert.ToChar(str[str.Length-1].Substring(i,1));
                        (target as ListBox).Location = new Point((sender as Control).Location.X+Maysoft.Art.GraphicHelper.GetLeftIndent(target,key.TrimEnd(lc_Trim)), (sender as Control).Location.Y+(sender as Control).Height);
						
                        for(int i=0;i<ldr_selected.Length;i++)
                        {
                            if(mostWiden < g.MeasureString(ldr_selected[i]["JawiName"].ToString(), (target as ListBox).Font).Width)
                                mostWiden = g.MeasureString(ldr_selected[i]["JawiName"].ToString(), (target as ListBox).Font).Width;

                            (target as ListBox).Items.Add( ldr_selected[i]["JawiName"].ToString());
                            //(target as ContextMenu).MenuItems[i].Click +=new EventHandler(MenuItem_Click);
                        }

                        (target as ListBox).Width = Convert.ToInt32(mostWiden)+24;//plus scrollbar
                    }
					
                    #endregion
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
        } */


        /// <summary>
        /// Export graphic on control to bitmap.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="fileName"></param>
        /* public bool Export(Control sender,string fileName)
        {
            //if(File.Exists(fileName))
            //	File.Delete(fileName);
            //Graphics g = sender.CreateGraphics();
            //Rectangle rect = sender.Bounds;
            //g.DrawImage(g,rect);
            //GraphicHelper.ExportBitmap(sender).Save(fileName,ImageFormat.Tiff);

            try
            {
                if(sender.GetType() == typeof(Label))
                {
                    #region a label control
                    Label lbl = (sender as Label);				
			
                    //declare a line art bitmap
                    decimal ratio = 300/96;//cuz Bitmap default Resolution = 96 px
                    Bitmap pic = new Bitmap(Convert.ToInt32(lbl.Width*ratio), Convert.ToInt32(lbl.Height*ratio));// Brushes.Black);
                    pic.SetResolution(300f,300f);//default first
                    //Bitmap pic = new Bitmap(lbl.Width, lbl.Height, PixelFormat.Format1bppIndexed);
                    Graphics g = Graphics.FromImage(pic);
                    g.DrawString(lbl.Text, lbl.Font, Brushes.Black, new PointF(0f,0f));

                    if(File.Exists(fileName))
                        File.Delete(fileName);
                    pic.Save(fileName, ImageFormat.Tiff);

                    g.Flush();
                    pic = null;
                    #endregion
                }

                return true;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
        } */
        #endregion

    }
    //end class JawiTranslator

}//end namespace