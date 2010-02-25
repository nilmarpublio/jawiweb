<!--
@todo: write today date
@todo: sum all price with the if condition
-->
<html xsl:version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" lang="en">
  <head>
    <title>Hamid Sticker List</title>
    <style>
      html {font-family:Tahoma,Verdana,Arial;}
      body {}
      .headerLeft {float:left;margin:0 0 10px 80px;font-size:10px;}
      .headerRight {float:right;margin: 0 0 10px 80px;font-size:10px;text-align:right;}
      .content { clear:both;font-size:9pt;}
      .content h3 { text-align:center;}
      .headline { background-color: lightgrey; clear: both;font-size:12px;}

      .numbering {display:inline-table;width:36px;padding:0 6px 0 0;}
      .orderHead {display:inline-table;width:110px;padding:0 6px 0 0;text-align:center;}
      .soldtoHead {display:inline-table;width:30px;padding:0 6px 0 0;text-align:center;}

      .soldto {display:inline-table;width:30px;padding:0 6px 0 0;text-align:right;}
      .order {display:inline-table;width:70px;padding:0 6px 0 0;text-align:right;}
      .deliver { display:inline-table;width:70px;padding:0 6px 0 0;text-align:right;}
      .name { display:inline-table;padding:0 6px 0 0; }
      .item { display:inline-table;width:120px;padding:0 6px 0 0;}
      .jawi { }

      .price {display:inline-table;width:30px;text-align:right;}
      .sum {display:inline-table;width:580px;text-align:right;font-weight:bold}

      .footer { float:left;bottom:10px;font-size:10px;}
      .footer ul li { list-style:none;display:inline;}

      .left {float:left;}
      .tab {display:inline-table;width:20px; }
      .wait {color:red;}
    </style>
    <script language="JavaScript">
	 /*
      * Returns the current date in 'dd MM yyyy' format as a string.
      * @example 31-12-2007
      */
      function todayStr() {
        var today=new Date();
		return today.getDate()+"-"+(today.getMonth()+1)+"-"+today.getYear();
      }
    </script>
  </head>

  <body>
    <div class="headerLeft">
      From:<br></br>
      <b>THEN YEANG SHING</b><br/>
      963 Jalan 6<br/>
      Machang Bubok 14020<br/>
      Bukit Mertajam, <u>Penang</u><br/>
      Hp: 012-4711134  Fax: 04-5521 696<br/>
      Email: <a href="mailto:yancyn@hotmail.com">yancyn@hotmail.com</a><br/>
      <p></p>
      To:<br/>
      <b>En. Hamid</b><br/>
      Kg Dodol 10150<br/>
      Jelutong, <u>Penang</u><br />
      Hp: 012-4513655  012-4690249<br/>
      Fax: 04-283 7729<br/>
    </div>
    <div class="headerRight">
      Date: <!--script>document.write(todayStr())</script-->
    </div>
    <div class="content">
      <h3>STICKER LIST</h3>
      <hr></hr>
      <span class="orderHead">Order</span>
      <!--span class="deliver">Delivered</span-->
      <span class="name">Description</span>
      <hr></hr>
      <xsl:variable name="last">2010-02-21</xsl:variable>
	  <xsl:variable name="sum">0</xsl:variable>
      <ol>
        <xsl:for-each select="nisan/order">
          <xsl:sort select="@date" data-type="text" order="ascending" />
          <xsl:if test="@soldto='HAM'
            and (@delivered=$last or @delivered=''
            or
            (substring(@date,1,4) &gt;= substring($last,1,4)
            and substring(@date,6,2) &gt;= substring($last,6,2)
            and substring(@date,9,2) &gt;= substring($last,9,2))
            or
            (substring(@date,1,4) &gt;= substring($last,1,4)
            and substring(@date,6,2) &gt; substring($last,6,2))
            )">
            <div>
              <li>
                <xsl:if test="contains(item,'Hijau')='true'">
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
                    <xsl:text>color:gray</xsl:text>
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test="contains(item,'½')='true'">
                  <xsl:attribute name="style">
                    <xsl:text>font-style:italic</xsl:text>
                  </xsl:attribute>
                </xsl:if>

                <span class="order">
                  <xsl:value-of select="@date" />
                </span>
                <!--span class="deliver">
                  <xsl:choose>
                    <xsl:when test="@delivered = ''">
                      -<span class="tab"></span>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="@delivered" />
                    </xsl:otherwise>
                  </xsl:choose>
                </span-->
                <span class="name">
                  <xsl:if test="remarks != ''">
                    *
                  </xsl:if>
                  <xsl:value-of select="name" />
                  (
                  <xsl:if test="born != ''">
                    <xsl:value-of select="born"/>~
                  </xsl:if>
                  <xsl:value-of select="death" />
                  <xsl:if test="deathm != ''">
                    =<xsl:value-of select="deathm" />
                  </xsl:if>
                  )

                  <xsl:if test="old != ''">
                    <xsl:value-of select="old" /> thn
                  </xsl:if>
                  <xsl:value-of select="jawi"/>
                </span>
              </li>
            </div>
          </xsl:if>
        </xsl:for-each>
      </ol>
      <hr/>
      <div style="float:right;font-weight:bold;">Total RM 
	  <xsl:value-of select="$sum"/></div>
      Cari bulan Melayu di http://www.hlgranite.com/nisan/calendar.aspx
    </div>
    <br/><br/>
    <div class="footer">
      Bulan Melayu:
      <span class="left">
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