<!-- @source http://justin-hopkins.com/blog/2008/10/19/henrys-widgets-a-practice-in-xmlxslt -->
<!-- @seealso http://www.jenitennison.com/xslt/grouping/muenchian.html -->
<!-- @seealso http://saxon.sourceforge.net -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" omit-xml-declaration="yes"/>
  <xsl:key name="customerOrderBatch" match="order" use="@soldto"/>
  <xsl:variable name="year">2010</xsl:variable>
  <xsl:variable name="month">12</xsl:variable>
  <!-- @see http://stackoverflow.com/questions/586231/how-can-i-convert-a-string-to-upper-or-lower-case-with-xslt -->
  <xsl:variable name="lowercase" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="uppercase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

  <xsl:template match="nisan">
    <html>
      <head>
        <title>Monthly Sales</title>
        <style>
          html {font-family:Tahoma,Verdana,Arial;}
          body {}
          .headerLeft {float:left;margin:0 0 10px 80px;font-size:10px;}
          .headerRight {float:right;margin: 0 0 10px 80px;font-size:10px;text-align:right;}
          .content { clear:both;font-size:9pt;}
          .content h3 {font-variant:small-caps;text-align:center;}
          .content h4 {font-family:Cambria;font-variant:small-caps;}
          .content ol li {text-transform: capitalize;}
          .headline { background-color: lightgrey; clear:both;font-size:12px;}
          .orderHead {display:inline-table;padding:0 10px 0 0;text-align:right;}

          .soldto {display:inline-table;width:30px;padding:0 6px 0 0;text-align:right;}
          .order {display:inline-table;width:70px;padding:0 6px 0 0;text-align:right;}
          .deliver {display:inline-table;width:70px;padding:0 6px 0 0;text-align:right;}
          .name { display:inline-table;width:420px;padding:0 6px 0 0; }
          .item {display:inline-table;width:100px;padding:0 6px 0 0;}
          .jawi { }

          .price {display:inline-table;width:40px;text-align:right;}
          .sum {display:inline-table;width:580px;text-align:right;font-weight:bold}

          .footer {float:left;bottom:10px;font-size:10px;}
          .footer ul li { list-style:none;display:inline;}

          .left {float:left;}
          .tab {display:inline-table;width:20px; }
          .wait {color:red;}
        </style>
        <script language="JavaScript">
          /*
          * Returns the current date in 'dd MM yyyy' format as a string.
          *
          @example 31-12-2007
          */
          function todayStr() {
          var today=new Date();
          return
          today.getDate()+"-"+(today.getMonth()+1)+"-"+today.getFullYear();
          }
        </script>
      </head>
      <body>
        <div class="headerLeft">
          From:
          <br></br>
          <b>THEN YEANG SHING</b>
          <br />963 Jalan 6
          <br />Machang Bubok 14020
          <br />Bukit Mertajam, <u>Penang</u>
          <br />Hp: 012-4711134 Fax: 04-5521 696
          <br />Email: <a href="mailto:yancyn@hotmail.com">yancyn@hotmail.com</a>
        </div>
        <div class="headerRight">
          <!--Date: <script>document.write(todayStr());</script> -->
        </div>
        <div class="content">
          <h3>Sales List</h3>
          <hr></hr>
          <span class="orderHead">Order</span>
          <span class="deliver">Delivered</span>
          <span class="name">Description</span>
          <span class="item">Item</span>
          <span class="price">Amount</span>
          <hr></hr>
          <ul style="list-style-type:none;margin-left:0;padding-left:0;">
            <xsl:for-each select="order[count(. | key('customerOrderBatch', @soldto)[1])=1]">
              <xsl:sort select="@soldto"/>
              <xsl:sort select="@date"/>
              <li>
                  <h4>
                    <xsl:value-of select="@soldto"/>
                  </h4>
                  <ol>
                    <xsl:for-each select="key('customerOrderBatch',@soldto)">
                        <xsl:if test="substring(@date,1,4) = $year and substring(@date,6,2) = $month">
                            <li>
                                <xsl:value-of select="@date"/>
                                 <xsl:value-of select="name"/>
                                 <xsl:value-of select="@price"/>
                            </li>
                        </xsl:if>
                    </xsl:for-each>
                  </ol>
              </li>
            </xsl:for-each>
          </ul>

          <hr />

          <div style="padding: 0 5px 0 0;float:right;font-weight:bold;">
            Total RM
            <xsl:value-of select="sum(order/@price[substring(../@date,1,4) = $year
                                                   and substring(../@date,6,2) = $month
                                                    ])"/>
            <br/>
          </div>
          Cari bulan Melayu di
          <a href="http://www.hlgranite.com/nisan/calendar.aspx">http://www.hlgranite.com/nisan/calendar.aspx</a>
          <br />
          *2½' sticker
        </div>
        <br />
        <br />
        <div class="footer">
          <span class="left">
            <span class="left">
              Bulan Melayu:
            </span>
            <ol>
              <li>Muharram</li>
              <li>Safar</li>
              <li>Rabiulawal</li>
              <li>Rabiulakhir</li>
              <li>Jamadilawal</li>
              <li>Jamadilakhir</li>
              <li>Rejab</li>
              <li>Syaaban</li>
              <li>Ramadhan</li>
              <li>Syawal</li>
              <li>Zulkaedah</li>
              <li>Zulhijjah</li>
            </ol>
          </span>
          <span class="left">
            <ol>
              <li>محرّم</li>
              <li>صفر</li>
              <li>ربيع الاول</li>
              <li>ربيع الاخير</li>
              <li>جمادالاول</li>
              <li>جمادالاخير</li>
              <li>رجب</li>
              <li>شعبان</li>
              <li>رمضان</li>
              <li>شوال</li>
              <li>ذوالقعده</li>
              <li>ذوالحجه</li>
            </ol>
          </span>
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>