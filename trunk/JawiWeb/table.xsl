<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" omit-xml-declaration="yes"/>
  <xsl:template match="/">
    <html>
      <head>
        <title>Nisan Pending List</title>
        <style type="text/css">
          html {font-family:Tahoma,Verdana,Arial;}
          body {}
          .headerLeft {float:left;margin:0 0 10px 80px;font-size:10px;}
          .headerRight {float:right;margin: 0 0 10px 80px;font-size:10px;text-align:right;}
          .content { clear:both;font-size:9pt;}
          .content h3 { text-align:center;}
          .content ol li {text-transform: capitalize;}
          .headline { background-color: lightgrey; clear: both;font-size:12px;}

          .numbering {display:inline-table;width:30px;padding:0 6px 0 0;}
          .soldtoHead {display:inline-table;width:30px;padding:0 6px 0 0;text-align:center;}

          .soldto {display:inline-table;width:30px;padding:0 6px 0 0;text-align:right;}
          .order {display:inline-table;width:70px;padding:0 6px 0 0;text-align:right;}
          .deliver { display:inline-table;width:70px;padding:0 6px 0 0;text-align:right;}
          .name { display:inline-table;width:380px;padding:0 6px 0 0; }
          .item { display:inline-table;width:100px;padding:0 6px 0 0;}
          .jawi { }

          .price {display:inline-table;width:30px;text-align:right;}
          .sum {display:inline-table;width:580px;text-align:right;font-weight:bold}

          .footer { float:left;bottom:10px;font-size:10px;}
          .footer ul li { list-style:none;display:inline;}

          .left {float:left;}
          .tab {display:inline-table;width:20px; }
          .wait {color:red;}
        </style>
      </head>
      <body>
        <div class="headerLeft">
          From:<br></br>
          <b>THEN YEANG SHING</b><br/>
          963 Jalan 6 <br/>
          Machang Bubok 14020<br/>
          <u>Bukit Mertajam</u>, Penang<br/>
          Hp: 012-4711134    Fax: 04-5521 696<br/>
          Email: <a href="mailto:yancyn@hotmail.com">yancyn@hotmail.com</a><br/>
        </div>
        <div class="headerRight">
        </div>
        <div class="content">
          <h3>PENDING LIST</h3>
          <div class="headline">
            <span class="numbering">No</span>
            <span class="soldtoHead">Sold</span>
            <span class="order">Order</span>
            <span class="name">Name</span>
            <span class="item">Item</span>
          </div>
          <!-- @see http://stackoverflow.com/questions/586231/how-can-i-convert-a-string-to-upper-or-lower-case-with-xslt -->
          <xsl:variable name="lowercase" select="'abcdefghijklmnopqrstuvwxyz'"/>
          <xsl:variable name="uppercase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
          <xsl:variable name="last">2010-02-10</xsl:variable>
          <table>
            <xsl:for-each select="nisan/order" >
              <xsl:sort select="@soldto" data-type="text" order="ascending" />
              <xsl:sort select="@date" data-type="text" order="ascending" />
              <xsl:if test="@soldto='ADI'
                      and (@delivered=''
                      or
                      (substring(@date,1,4) >= substring($last,1,4)
                      and substring(@date,6,2) >= substring($last,6,2)
                      and substring(@date,9,2) >= substring($last,9,2))
                      )">
                <tr>
                  <xsl:if test="contains(tags,'w')='true'">
                    <xsl:attribute name="style">
                      <xsl:text>color:red</xsl:text>
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test="contains(item,'½')='true'">
                    <xsl:attribute name="style">
                      <xsl:text>font-style:italic</xsl:text>
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test="contains(item,'Hijau') or contains(item,'G')='true'">
                    <xsl:attribute name="style">
                      <xsl:text>color:green</xsl:text>
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test="contains(item,'Batik')='true'">
                    <xsl:attribute name="style">
                      <xsl:text>color:blue</xsl:text>
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test="contains(item,'Putih')='true'">
                    <xsl:attribute name="style">
                      <xsl:text>color:grey</xsl:text>
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test="contains(item,'emas')='true'">
                    <xsl:attribute name="style">
                      <xsl:text>color:goldenrod</xsl:text>
                    </xsl:attribute>
                  </xsl:if>
                  
                  <td><xsl:value-of select="@soldto"/></td>
                  <td><xsl:value-of select="@date"/></td>
                  <td><xsl:value-of select="@delivered"/></td>
                  <td><xsl:value-of select="translate(name,$uppercase,$lowercase)"/></td>
                  <td><xsl:value-of select="item"/></td>
                  <td><xsl:value-of select="@price"/></td>
                </tr>
              </xsl:if>
            </xsl:for-each>
          </table>
          <hr />
        </div>
        <div class="footer">
          <ul>
            <li>ADI - Addin | </li>
            <li>HAM - Hamid | </li>
            <li>JEF - Sabak Bernam Jeffri | </li>
            <li>KEN - Tmn Kenari | </li>
            <!--li>KIL - Kilang Lama | </li-->
            <li>SEL - Selama | </li>
            <li>SEM - Semanggol</li>
            <!--li>PAS - Pasir Puteh</li -->
          </ul>
          <span class="left">
            Bulan Melayu:
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
            <br/>
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