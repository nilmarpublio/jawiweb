<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" omit-xml-declaration="yes"/>
  <xsl:variable name="customer">ADI</xsl:variable>
  <xsl:variable name="month">01</xsl:variable>
  <xsl:variable name="year">2011</xsl:variable>
  <xsl:variable name="bf">10005</xsl:variable>
  <xsl:template name="monthTemplate">
      <xsl:if test="$month=1">
          <xsl:text>January</xsl:text>
      </xsl:if>
      <xsl:if test="$month=2">
          <xsl:text>February</xsl:text>
      </xsl:if>
      <xsl:if test="$month=3">
          <xsl:text>March</xsl:text>
      </xsl:if>
      <xsl:if test="$month=4">
          <xsl:text>April</xsl:text>
      </xsl:if>
      <xsl:if test="$month=5">
          <xsl:text>May</xsl:text>
      </xsl:if>
      <xsl:if test="$month=6">
          <xsl:text>June</xsl:text>
      </xsl:if>
      <xsl:if test="$month=7">
          <xsl:text>July</xsl:text>
      </xsl:if>
      <xsl:if test="$month=8">
          <xsl:text>August</xsl:text>
      </xsl:if>
      <xsl:if test="$month=9">
          <xsl:text>September</xsl:text>
      </xsl:if>
      <xsl:if test="$month=10">
          <xsl:text>October</xsl:text>
      </xsl:if>
      <xsl:if test="$month=11">
          <xsl:text>November</xsl:text>
      </xsl:if>
      <xsl:if test="$month=12">
          <xsl:text>December</xsl:text>
      </xsl:if>
  </xsl:template>
  <xsl:template match="/">
    <html>
      <head>
        <title>CUSTOMER STATEMENT</title>
        <style>
          html {font-family:Tahoma,Verdana,Arial;}
          body {}
          .headerLeft {float:left;margin:0 0 10px 80px;font-size:10px;}
          .headerRight {float:right;margin: 0 0 10px 80px;font-size:10px;text-align:right;}
          .content { clear:both;font-size:9pt;}
          .content h3 { font-variant:small-caps;text-align:center;}
          .content ol li {text-transform: capitalize;}
          .headline { background-color: lightgrey; clear: both;font-size:12px;}

          .numbering {display:inline-table;width:36px;padding:0 6px 0 0;}
          .orderHead {display:inline-table;width:110px;padding:0 6px 0 0;text-align:center;}
          .soldtoHead {display:inline-table;width:30px;padding:0 6px 0 0;text-align:center;}

          .soldto {display:inline-table;width:30px;padding:0 6px 0 0;text-align:right;}
          .order {display:inline-table;width:70px;padding:0 6px 0 0;text-align:right;}
          .deliver { display:inline-table;width:70px;padding:0 6px 0 0;text-align:right;}
          .name { display:inline-table;width:390px;padding:0 6px 0 0; }
          .item { display:inline-table;width:90px;padding:0 6px 0 0;}
          .jawi { }

          .price {display:inline-table;width:40px;text-align:right;}
          .sum {display:inline-table;width:580px;text-align:right;font-weight:bold}

          .footer {float:left;bottom:10px;font-size:10px;}
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
          963 Jalan 6<br/>
          Machang Bubok 14020<br/>
          Bukit Mertajam, <u>Penang</u><br/>
          Hp: 012-4711134  Fax: 04-5521 696<br/>
          Email: <a href="mailto:yancyn@hotmail.com">yancyn@hotmail.com</a><br/>
          <p></p>
          To:<br/>
          <b>ADDIN MARBLE</b><br/>
          F78 Parit 18 Kg Baru<br/>
          Hutan Melintang<br/>
          36400 Perak<br/>          
          Hp: 012-6900 280  013-3624 363<br/>
          Email: <a href="mailto:nuur_iema343@yahoo.com">nuur_iema343@yahoo.com</a><br/>
        </div>
        <div class="headerRight">
            Date: <xsl:call-template name="monthTemplate">
                    <!--xsl:with-param name="month">11</xsl:with-param-->
                </xsl:call-template> 2010
            <br/><br/><br/>
            <br/><br/><br/>
            <br/><br/><br/>
            BULAN LEPAS <xsl:value-of select="$bf"/>
            <br/>
            BULAN INI <xsl:value-of select="sum(nisan/order/@price
              [../@soldto=$customer
              and substring(../@date,1,4) = $year
              and substring(../@date,6,2) = $month])"/>
            <br/>
            BAYAR (<xsl:value-of select="sum(nisan/invoice/amount
              [../from=$customer
              and substring(../date,1,4) = $year
              and substring(../date,6,2) = $month])"/>)
            <br/>
            <b>
            BAKI <xsl:value-of select="$bf
            + sum(nisan/order/@price
              [../@soldto=$customer
              and substring(../@date,1,4) = $year
              and substring(../@date,6,2) = $month])
            - sum(nisan/invoice/amount
              [../from=$customer
              and substring(../date,1,4) = $year
              and substring(../date,6,2) = $month])
            "/>
            </b>
        </div>
        <div class="content">
          <h3>Customer Statement</h3>
          <hr></hr>
          <span class="orderHead">Order</span>
          <span class="deliver">Delivered</span>
          <span class="name">Description</span>
          <span class="item">Item</span>
          <span class="price">Price</span>
          <hr></hr>
          <ol>
            <xsl:for-each select="nisan/order">
              <xsl:sort select="@date" data-type="text" order="ascending" />
              <xsl:if test="@soldto=$customer
                      and substring(@date,1,4) = $year
                      and substring(@date,6,2) = $month">
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
                    <span class="deliver">
                    <xsl:choose>
                      <xsl:when test="@delivered = ''">
                        -<span class="tab"></span>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="@delivered" />
                      </xsl:otherwise>
                    </xsl:choose>
                    </span>
                    <span class="name">
                      <xsl:if test="contains(item,'½')='true'">*</xsl:if>
                      <xsl:value-of select="name" />
                      <xsl:choose>
                        <xsl:when test="born != ''">
                          (<xsl:value-of select="born"/>~<xsl:value-of select="death" />
                          <xsl:if test="deathm != ''">
                            =<xsl:value-of select="deathm" />
                          </xsl:if>)
                        </xsl:when>
                        <xsl:otherwise>
                          (<xsl:value-of select="death" />
                          <xsl:if test="deathm != ''">
                            =<xsl:value-of select="deathm" />
                          </xsl:if>)
                        </xsl:otherwise>
                      </xsl:choose>
                      <xsl:if test="age != ''">
                        <xsl:value-of select="age" /> thn
                      </xsl:if>
                    </span>
                    <span class="item">
                      <xsl:value-of select="item" />
                    </span>
                    <span class="price">
                      <xsl:value-of select="@price" />
                    </span>
                  </li>
                </div>
              </xsl:if>
            </xsl:for-each>
          </ol>
          
          <ol>
            <xsl:for-each select="nisan/invoice">
              <xsl:sort select="@date" data-type="text" order="ascending" />
              <xsl:if test="from=$customer
                      and substring(date,1,4) = $year
                      and substring(date,6,2) = $month">
                  <li style="list-style:none;">
                      <span class="order"> </span>
                      <span class="deliver">
                          <xsl:value-of select="date"/>
                      </span>
                      <span class="name">
                          <xsl:value-of select="remarks"/>
                      </span>
                      <span class="item"> </span>
                      <span class="price"> </span>
                      <span class="price">
                        <xsl:value-of select="amount"/>
                      </span>
                  </li>
              </xsl:if>
              </xsl:for-each>
          </ol>
          <hr/>
          <div style="float:right;font-weight:bold;padding:0 120px 0 0;">
            Total RM
            <xsl:value-of
              select="sum(nisan/order/@price
              [../@soldto=$customer
              and substring(../@date,1,4) = $year
              and substring(../@date,6,2) = $month])"/>
          </div>
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>