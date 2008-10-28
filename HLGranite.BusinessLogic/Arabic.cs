using System;
using System.Collections.Generic;
using System.Text;

namespace HLGranite.BusinessLogic
{
    /// <summary>
    /// Maintain Arabic encoding (178 CharSet).
    /// </summary>
    public class Arabic
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Arabic()
        {
        }
        private static int charset;
        /// <summary>
        /// Indicate character set: 0,2,178 only.
        /// </summary>
        public static int CharSet
        {
            get { return charset; }
            set { charset = value; }
        }

        #region methods
        public static string Get0()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Zero;
                    break;
            }

            return output;
        }
        public static string Get1()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.One;
                    break;
            }

            return output;
        }
        public static string Get2()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Two;
                    break;
            }

            return output;
        }
        public static string Get3()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Three;
                    break;
            }

            return output;
        }
        public static string Get4()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Four;
                    break;
            }

            return output;
        }
        public static string Get5()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Five;
                    break;
            }

            return output;
        }
        public static string Get6()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Six;
                    break;
            }

            return output;
        }
        public static string Get7()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Seven;
                    break;
            }

            return output;
        }
        public static string Get8()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Eight;
                    break;
            }

            return output;
        }
        public static string Get9()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Nine;
                    break;
            }

            return output;
        }


        public static string GetA()
        {
            string output = "";
            if (Arabic.CharSet == 0)
            {
            }
            else if (Arabic.CharSet == 2)
            {
            }
            else if (Arabic.CharSet == 178)
                output = Arabic.Alef;

            return output;
        }
        public static string GetB()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Beh;
                    break;
            }

            return output;
        }
        public static string GetC()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Tcheh;
                    break;
            }

            return output;
        }
        public static string GetD()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Dal;
                    break;
            }

            return output;
        }
        public static string GetE()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    //output = Arabic.
                    break;
            }

            return output;
        }

        public static string GetF()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Tcheh;
                    break;
            }

            return output;
        }
        public static string GetG()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Gaf;
                    break;
            }

            return output;
        }
        public static string GetH()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Heh;
                    break;
            }

            return output;
        }
        public static string GetI()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Yeh;
                    break;
            }

            return output;
        }
        public static string GetJ()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Jeem;
                    break;
            }

            return output;
        }
        public static string GetK()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Kaf;
                    break;
            }

            return output;
        }
        public static string GetL()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Lam;
                    break;
            }

            return output;
        }
        public static string GetM()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Meem;
                    break;
            }

            return output;
        }
        public static string GetN()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Noon;
                    break;
            }

            return output;
        }

        public static string GetO()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Waw;
                    break;
            }

            return output;
        }
        public static string GetP()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Peh;
                    break;
            }

            return output;
        }
        public static string GetQ()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Qaf;
                    break;
            }

            return output;
        }
        public static string GetR()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Reh;
                    break;
            }

            return output;
        }
        public static string GetS()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Seen;
                    break;
            }

            return output;
        }
        public static string GetT()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Teh;
                    break;
            }

            return output;
        }
        public static string GetU()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Waw;
                    break;
            }

            return output;
        }
        public static string GetV()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Veh;
                    break;
            }

            return output;
        }
        public static string GetW()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Waw;
                    break;
            }

            return output;
        }
        public static string GetX()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    break;
            }

            return output;
        }
        public static string GetY()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Yeh;
                    break;
            }

            return output;
        }
        public static string GetZ()
        {
            string output = "";
            switch (charset)
            {
                case 0:
                    break;
                case 2:
                    break;
                case 178:
                    output = Arabic.Thal;
                    break;
            }

            return output;
        }

        #endregion

        #region basic character

        /// <summary>
        /// ا
        /// </summary>
        public const string Alef = "\u0627";
        /// <summary>
        /// و
        /// </summary>
        public const string Waw = "\u0648";
        /// <summary>
        /// ى
        /// </summary>
        public const string AlefMaksura = "\u0649";
        /// <summary>
        /// ي
        /// </summary>
        public const string Yeh = "\u064A";




        /// <summary>
        /// ب
        /// </summary>
        public const string Beh = "\u0628";
        /// <summary>
        /// ة
        /// </summary>
        public const string TehMarbuta = "\u0629";
        /// <summary>
        /// ت
        /// </summary>
        public const string Teh = "\u062A";
        /// <summary>
        /// ث
        /// </summary>
        public const string Theh = "\u062B";
        /// <summary>
        /// ج
        /// </summary>
        public const string Jeem = "\u062C";
        /// <summary>
        /// ح
        /// </summary>
        public const string Hah = "\u062D";
        /// <summary>
        /// خ
        /// </summary>
        public const string Khah = "\u062E";
        /// <summary>
        /// د
        /// </summary>
        public const string Dal = "\u062F";
        /// <summary>
        /// ذ
        /// </summary>
        public const string Thal = "\u0630";
        /// <summary>
        /// ر
        /// </summary>
        public const string Reh = "\u0631";
        /// <summary>
        /// ز
        /// </summary>
        public const string Zain = "\u0632";
        /// <summary>
        /// س
        /// </summary>
        public const string Seen = "\u0633";
        /// <summary>
        /// ش
        /// </summary>
        public const string Sheen = "\u0634";
        /// <summary>
        /// ص
        /// </summary>
        public const string Sad = "\u0635";
        /// <summary>
        /// ض
        /// </summary>
        public const string Dad = "\u0636";
        /// <summary>
        /// ط
        /// </summary>
        public const string Tah = "\u0637";
        /// <summary>
        /// ظ
        /// </summary>
        public const string Zah = "\u0638";
        /// <summary>
        /// ع
        /// </summary>
        public const string Ain = "\u0639";
        /// <summary>
        /// غ
        /// </summary>
        public const string Ghain = "\u063A";
        /// <summary>
        /// ف
        /// </summary>
        public const string Feh = "\u0641";
        /// <summary>
        /// ق
        /// </summary>
        public const string Qaf = "\u0642";
        /// <summary>
        /// ك
        /// </summary>
        public const string Kaf = "\u0643";
        /// <summary>
        /// ل
        /// </summary>
        public const string Lam = "\u0644";
        /// <summary>
        /// م
        /// </summary>
        public const string Meem = "\u0645";
        /// <summary>
        /// ن
        /// </summary>
        public const string Noon = "\u0646";
        /// <summary>
        /// ه
        /// </summary>
        public const string Heh = "\u0647";
        /// <summary>
        /// پ
        /// </summary>
        public const string Peh = "\u067E";
        /// <summary>
        /// چ
        /// </summary>
        public const string Tcheh = "\u0686";
        /// <summary>
        /// ژ
        /// </summary>
        public const string Jeh = "\u0690";
        /// <summary>
        /// ڤ
        /// </summary>
        public const string Veh = "\u06A4";
        /// <summary>
        /// ک
        /// </summary>
        public const string Kekeh = "\u06A9";
        /// <summary>
        /// گ
        /// </summary>
        public const string Gaf = "\u06AF";
        /// <summary>
        /// ە
        /// </summary>
        public const string Ae = "\u06D5";
        #endregion

        #region Punctuation
        /// <summary>
        /// ء
        /// </summary>
        public const string Hamza = "\u0621";
        /// <summary>
        /// ٴ
        /// </summary>
        public const string HamzaHigh = "\u0674";
        /// <summary>
        /// ً
        /// </summary>
        public const string Fathatan = "\u064B";
        /// <summary>
        /// ٌ
        /// </summary>
        public const string Dammatan = "\u064C";
        /// <summary>
        /// ٍ
        /// </summary>
        public const string Kasratan = "\u064D";
        /// <summary>
        /// َ
        /// </summary>
        public const string Fatha = "\u064E";
        /// <summary>
        /// ُ
        /// </summary>
        public const string Damma = "\u064F";
        /// <summary>
        /// ِ
        /// </summary>
        public const string Kasra = "\u0650";
        /// <summary>
        /// ّ
        /// </summary>
        public const string Shadda = "\u0651";
        /// <summary>
        /// ْ
        /// </summary>
        public const string Sukun = "\u0652";
        /// <summary>
        /// ۤ
        /// </summary>
        public const string MaddaHigh = "\u06E4";

        /// <summary>
        /// ﷲ
        /// </summary>
        public const string Allah = "\uFDF2";


        #endregion

        #region Numeric
        /// <summary>
        /// ٠
        /// </summary>
        public const string Zero = "\u0660";
        /// <summary>
        /// ١
        /// </summary>
        public const string One = "\u0661";
        /// <summary>
        /// ٢
        /// </summary>
        public const string Two = "\u0662";
        /// <summary>
        /// ٣
        /// </summary>
        public const string Three = "\u0663";
        /// <summary>
        /// ٤
        /// </summary>
        public const string Four = "\u0664";
        /// <summary>
        /// ۴
        /// </summary>
        public const string FourIndic = "\u06F4";
        /// <summary>
        /// ٥
        /// </summary>
        public const string Five = "\u0665";
        /// <summary>
        /// ۵
        /// </summary>
        public const string FiveIndic = "\u06F5";
        /// <summary>
        /// ٦
        /// </summary>
        public const string Six = "\u0666";
        /// <summary>
        /// ۶
        /// </summary>
        public const string SizIndic = "\u06F6";
        /// <summary>
        /// ٧
        /// </summary>
        public const string Seven = "\u0667";
        /// <summary>
        /// ٨
        /// </summary>
        public const string Eight = "\u0668";
        /// <summary>
        /// ٩
        /// </summary>
        public const string Nine = "\u0669";
        #endregion

        /// <summary>
        /// Arabic thesaurus (for those advance character behavior).
        /// </summary>
        public class Thesaurus
        {
            private Thesaurus thesaurus = null;
            /// <summary>
            /// Default constructor.
            /// </summary>
            public Thesaurus()
            {
                thesaurus = this;
            }
            /// <summary>
            /// Formal constructor.
            /// </summary>
            /// <param name="word">Rumi word.</param>
            /// <param name="translate">Jawi or Arabic value.</param>
            public Thesaurus(string word, string translate)
            {
                rumi = word;
                jawi = translate;
                thesaurus = this;
            }
            /// <summary>
            /// Make it indexing.
            /// </summary>
            public virtual object this[object key]
            {
                get
                {
                    string value = null;
                    if (thesaurus != null)
                        value = thesaurus[key] as string;
                    return value;
                }
                set { }
            }

            private string rumi;
            /// <summary>
            /// Rumi in character spelling.
            /// </summary>
            public string Rumi
            {
                get { return rumi; }
                set { rumi = value; }
            }

            private string jawi;
            /// <summary>
            /// Jawi or arabic value.
            /// </summary>
            public string Jawi
            {
                get { return jawi; }
                set { jawi = value; }
            }


            public static Thesaurus AL = new Thesaurus("al", Arabic.Ain + Arabic.Lam);
            public static Thesaurus LLAH = new Thesaurus("llah", Arabic.Allah);


            /// <summary>
            /// Convert and recorrect to more well known spelling.
            /// </summary>
            /// <param name="rumi"></param>
            /// <param name="jawi"></param>
            /// <returns></returns>
            public static string Synchronize(string rumi, string jawi)
            {
                string output = jawi;
                rumi = rumi.ToLower();


                jawi = Replace(rumi, jawi, Thesaurus.AL);
                jawi = Replace(rumi, jawi, Thesaurus.LLAH);

                //i = rumi.IndexOf(


                output = jawi;
                return output;
            }
            private static string Replace(string roman, string original, Thesaurus thes)
            {
                string old = "";
                string source = thes.Rumi;
                foreach (char c in source)
                    old += JawiTranslator.MapArabicCharacter(Arabic.CharSet, c.ToString());
                int i = roman.IndexOf(thes.Rumi, 0);
                if (i > -1) original = original.Replace(old, thes.Jawi);

                return original;
            }


        }
        //end class Thesaurus

    }//end class Arabic
}//end namesapce