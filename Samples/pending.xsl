<!--
TODO: move into different template so that easy to maintain since 2010-09-21
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" omit-xml-declaration="yes"/>

  <!-- @see http://stackoverflow.com/questions/586231/how-can-i-convert-a-string-to-upper-or-lower-case-with-xslt -->
  <xsl:variable name="lowercase" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="uppercase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

  <xsl:variable name="batikItem_L15">1½' Batu Batik(L)</xsl:variable>
  <xsl:variable name="batikItem_P15">1½' Batu Batik(P)</xsl:variable>
  <xsl:variable name="putihItem_L15">1½' Batu Putih(L)</xsl:variable>
  <xsl:variable name="putihItem_P15">1½' Batu Putih(P)</xsl:variable>
  <xsl:variable name="hitamItem_L15">1½' Batu Hitam(L)</xsl:variable>
  <xsl:variable name="hitamItem_P15">1½' Batu Hitam(P)</xsl:variable>
  <xsl:variable name="hijauItem_L15">1½' Batu Hijau(L)</xsl:variable>
  <xsl:variable name="hijauItem_P15">1½' Batu Hijau(P)</xsl:variable>
  <xsl:variable name="batikItem_L20">2' Batu Batik(L)</xsl:variable>
  <xsl:variable name="batikItem_P20">2' Batu Batik(P)</xsl:variable>
  <xsl:variable name="putihItem_L20">2' Batu Putih(L)</xsl:variable>
  <xsl:variable name="putihItem_P20">2' Batu Putih(P)</xsl:variable>
  <xsl:variable name="hitamItem_L20">2' Batu Hitam(L)</xsl:variable>
  <xsl:variable name="hitamItem_P20">2' Batu Hitam(P)</xsl:variable>
  <xsl:variable name="hijauItem_L20">2' Batu Hijau(L)</xsl:variable>
  <xsl:variable name="hijauItem_P20">2' Batu Hijau(P)</xsl:variable>
  <xsl:variable name="batikItem_L25">2½' Batu Batik(L)</xsl:variable>
  <xsl:variable name="batikItem_P25">2½' Batu Batik(P)</xsl:variable>
  <xsl:variable name="putihItem_L25">2½' Batu Putih(L)</xsl:variable>
  <xsl:variable name="putihItem_P25">2½' Batu Putih(P)</xsl:variable>
  <xsl:variable name="hitamItem_L25">2½' Batu Hitam(L)</xsl:variable>
  <xsl:variable name="hitamItem_P25">2½' Batu Hitam(P)</xsl:variable>
  <xsl:variable name="hijauItem_L25">2½' Batu Hijau(L)</xsl:variable>
  <xsl:variable name="hijauItem_P25">2½' Batu Hijau(P)</xsl:variable>

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
          .item { display:inline-table;width:110px;padding:0 6px 0 0;}
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
          <b>Stock Available YTD</b>
          <table cellspacing="0" cellpadding="0" border="0">
            <tr>
              <td>
                <table cellspacing="0" cellpadding="0" border="0">
                  <tr>
                    <td></td>
                    <td>普</td>
                    <td>浮</td>
                  </tr>
                  <tr>
                    <td>天官</td>
                    <td><xsl:value-of select="sum(nisan/purchase/quantity[../item='天官'])-count(nisan/order[item='天官'])"/></td>
                    <td><xsl:value-of select="sum(nisan/purchase/quantity[../item='天官(浮)'])-count(nisan/order[item='天官(浮)'])"/></td>
                  </tr>
                  <tr>
                    <td>天官龙</td>
                    <td><xsl:value-of select="sum(nisan/purchase/quantity[../item='天官龙'])-count(nisan/order[item='天官'])"/></td>
                    <td><xsl:value-of select="sum(nisan/purchase/quantity[../item='天官龙(浮)'])-count(nisan/order[item='天官(浮)'])"/></td>
                  </tr>
                  <tr>
                    <td>地主</td>
                    <td><xsl:value-of select="sum(nisan/purchase/quantity[../item='地主'])-count(nisan/order[item='地主'])"/></td>
                    <td><xsl:value-of select="sum(nisan/purchase/quantity[../item='地主(浮)'])-count(nisan/order[item='地主(浮)'])"/></td>
                  </tr>
                  <tr>
                    <td>拿督</td>
                    <td><xsl:value-of select="sum(nisan/purchase/quantity[../item='拿督'])-count(nisan/order[item='拿督'])"/></td>
                    <td><xsl:value-of select="sum(nisan/purchase/quantity[../item='拿督(浮)'])-count(nisan/order[item='拿督(浮)'])"/></td>
                  </tr>
                  <tr>
                    <td>灶君</td>
                    <td><xsl:value-of select="sum(nisan/purchase/quantity[../item='灶君'])-count(nisan/order[item='灶君'])"/></td>
                    <td><xsl:value-of select="sum(nisan/purchase/quantity[../item='灶君(浮)'])-count(nisan/order[item='灶君(浮)'])"/></td>
                  </tr>
                </table>
              </td>
              <td style="width:20px;"> </td>
              <td>
                <table cellspacing="0" cellpadding="0" border="0">
                  <tr>
                    <td></td>
                    <td style="color:blue;">花</td>
                    <td style="color:grey;">白</td>
                    <td>黑</td>
                    <td style="color:green;">青</td>
                  </tr>
                  <tr>
                    <td>1½'♂</td>
                    <td style="color:blue;">
                      <xsl:choose>
                        <xsl:when test="sum(nisan/purchase/quantity[../item=$batikItem_L15])-count(nisan/order[item=$batikItem_L15])=0">
                          <xsl:text>-</xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="sum(nisan/purchase/quantity[../item=$batikItem_L15])-count(nisan/order[item=$batikItem_L15])"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                    <td style="color:grey;">
                      <xsl:choose>
                        <xsl:when test="sum(nisan/purchase/quantity[../item=$putihItem_L15])-count(nisan/order[item=$putihItem_L15])=0">
                          <xsl:text>-</xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="sum(nisan/purchase/quantity[../item=$putihItem_L15])-count(nisan/order[item=$putihItem_L15])"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                    <td>
                      <xsl:choose>
                        <xsl:when test="sum(nisan/purchase/quantity[../item=$hitamItem_L15])-count(nisan/order[item=$hitamItem_L15])=0">
                          <xsl:text>-</xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="sum(nisan/purchase/quantity[../item=$hitamItem_L15])-count(nisan/order[item=$hitamItem_L15])"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                    <td style="color:green;">
                      <xsl:choose>
                        <xsl:when test="sum(nisan/purchase/quantity[../item=$hijauItem_L15])-count(nisan/order[item=$hijauItem_L15])=0">
                          <xsl:text>-</xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="sum(nisan/purchase/quantity[../item=$hijauItem_L15])-count(nisan/order[item=$hijauItem_L15])"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                  </tr>
                  <tr>
                    <td>1½'♀</td>
                    <td style="color:blue;">
                      <xsl:choose>
                        <xsl:when test="sum(nisan/purchase/quantity[../item=$batikItem_P15])-count(nisan/order[item=$batikItem_P15])=0">
                          <xsl:text>-</xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="sum(nisan/purchase/quantity[../item=$batikItem_P15])-count(nisan/order[item=$batikItem_P15])"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                    <td style="color:grey;">
                      <xsl:choose>
                        <xsl:when test="sum(nisan/purchase/quantity[../item=$putihItem_P15])-count(nisan/order[item=$putihItem_P15])=0">
                          <xsl:text>-</xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="sum(nisan/purchase/quantity[../item=$putihItem_P15])-count(nisan/order[item=$putihItem_P15])"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                    <td>
                      <xsl:choose>
                        <xsl:when test="sum(nisan/purchase/quantity[../item=$hitamItem_P15])-count(nisan/order[item=$hitamItem_P15])=0">
                          <xsl:text>-</xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="sum(nisan/purchase/quantity[../item=$hitamItem_P15])-count(nisan/order[item=$hitamItem_P15])"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                    <td style="color:green;">
                      <xsl:choose>
                        <xsl:when test="sum(nisan/purchase/quantity[../item=$hijauItem_P15])-count(nisan/order[item=$hijauItem_P15])=0">
                          <xsl:text>-</xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="sum(nisan/purchase/quantity[../item=$hijauItem_P15])-count(nisan/order[item=$hijauItem_P15])"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                  </tr>
                  <tr>
                    <td>2'♂</td>
                    <td style="color:blue;">
                      <xsl:choose>
                        <xsl:when test="sum(nisan/purchase/quantity[../item=$batikItem_L20])-count(nisan/order[item=$batikItem_L20 and @soldto != 'ADI'])=0">
                          <xsl:text>-</xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="sum(nisan/purchase/quantity[../item=$batikItem_L20])-count(nisan/order[item=$batikItem_L20 and @soldto != 'ADI'])"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                    <td style="color:grey;">
                      <xsl:choose>
                        <xsl:when test="sum(nisan/purchase/quantity[../item=$putihItem_L20])-count(nisan/order[item=$putihItem_L20 and @soldto != 'ADI'])=0">
                          <xsl:text>-</xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="sum(nisan/purchase/quantity[../item=$putihItem_L20])-count(nisan/order[item=$putihItem_L20 and @soldto != 'ADI'])"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                    <td>
                      <xsl:choose>
                        <xsl:when test="sum(nisan/purchase/quantity[../item=$hitamItem_L20])-count(nisan/order[item=$hitamItem_L20 and @soldto != 'ADI'])=0">
                          <xsl:text>-</xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="sum(nisan/purchase/quantity[../item=$hitamItem_L20])-count(nisan/order[item=$hitamItem_L20 and @soldto != 'ADI'])"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                    <td style="color:green;">
                      <xsl:choose>
                        <xsl:when test="sum(nisan/purchase/quantity[../item=$hijauItem_L20])-count(nisan/order[item=$hijauItem_L20 and @soldto != 'ADI'])=0">
                          <xsl:text>-</xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="sum(nisan/purchase/quantity[../item=$hijauItem_L20])-count(nisan/order[item=$hijauItem_L20 and @soldto != 'ADI'])"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                  </tr>
                  <tr>
                    <td>2'♀</td>
                    <td style="color:blue;">
                      <xsl:choose>
                        <xsl:when test="sum(nisan/purchase/quantity[../item=$batikItem_P20])-count(nisan/order[item=$batikItem_P20 and @soldto != 'ADI'])=0">
                          <xsl:text>-</xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="sum(nisan/purchase/quantity[../item=$batikItem_P20])-count(nisan/order[item=$batikItem_P20 and @soldto != 'ADI'])"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                    <td style="color:grey;">
                      <xsl:choose>
                        <xsl:when test="sum(nisan/purchase/quantity[../item=$putihItem_P20])-count(nisan/order[item=$putihItem_P20 and @soldto != 'ADI'])=0">
                          <xsl:text>-</xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="sum(nisan/purchase/quantity[../item=$putihItem_P20])-count(nisan/order[item=$putihItem_P20 and @soldto != 'ADI'])"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                    <td>
                      <xsl:choose>
                        <xsl:when test="sum(nisan/purchase/quantity[../item=$hitamItem_P20])-count(nisan/order[item=$hitamItem_P20 and @soldto != 'ADI'])=0">
                          <xsl:text>-</xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="sum(nisan/purchase/quantity[../item=$hitamItem_P20])-count(nisan/order[item=$hitamItem_P20 and @soldto != 'ADI'])"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                    <td style="color:green;">
                      <xsl:choose>
                        <xsl:when test="sum(nisan/purchase/quantity[../item=$hijauItem_P20])-count(nisan/order[item=$hijauItem_P20 and @soldto != 'ADI'])=0">
                          <xsl:text>-</xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="sum(nisan/purchase/quantity[../item=$hijauItem_P20])-count(nisan/order[item=$hijauItem_P20 and @soldto != 'ADI'])"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                  </tr>
                  <tr>
                    <td>2½'♂</td>
                    <td style="color:blue;">
                      <xsl:choose>
                        <xsl:when test="sum(nisan/purchase/quantity[../item=$batikItem_L25])-count(nisan/order[item=$batikItem_L25])=0">
                          <xsl:text>-</xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="sum(nisan/purchase/quantity[../item=$batikItem_L25])-count(nisan/order[item=$batikItem_L25])"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                    <td style="color:grey;">
                      <xsl:choose>
                        <xsl:when test="sum(nisan/purchase/quantity[../item=$putihItem_L25])-count(nisan/order[item=$putihItem_L25])=0">
                          <xsl:text>-</xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="sum(nisan/purchase/quantity[../item=$putihItem_L25])-count(nisan/order[item=$putihItem_L25])"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                    <td>
                      <xsl:choose>
                        <xsl:when test="sum(nisan/purchase/quantity[../item=$hitamItem_L25])-count(nisan/order[item=$hitamItem_L25])=0">
                          <xsl:text>-</xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="sum(nisan/purchase/quantity[../item=$hitamItem_L25])-count(nisan/order[item=$hitamItem_L25])"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                    <td style="color:green;">
                      <xsl:choose>
                        <xsl:when test="sum(nisan/purchase/quantity[../item=$hijauItem_L25])-count(nisan/order[item=$hijauItem_L25])=0">
                          <xsl:text>-</xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="sum(nisan/purchase/quantity[../item=$hijauItem_L25])-count(nisan/order[item=$hijauItem_L25])"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                  </tr>
                  <tr>
                    <td>2½'♀</td>
                    <td style="color:blue;">
                      <xsl:choose>
                        <xsl:when test="sum(nisan/purchase/quantity[../item=$batikItem_P25])-count(nisan/order[item=$batikItem_P25])=0">
                          <xsl:text>-</xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="sum(nisan/purchase/quantity[../item=$batikItem_P25])-count(nisan/order[item=$batikItem_P25])"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                    <td style="color:grey;">
                      <xsl:choose>
                        <xsl:when test="sum(nisan/purchase/quantity[../item=$putihItem_P25])-count(nisan/order[item=$putihItem_P25])=0">
                          <xsl:text>-</xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="sum(nisan/purchase/quantity[../item=$putihItem_P25])-count(nisan/order[item=$putihItem_P25])"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                    <td>
                      <xsl:choose>
                        <xsl:when test="sum(nisan/purchase/quantity[../item=$hitamItem_P25])-count(nisan/order[item=$hitamItem_P25])=0">
                          <xsl:text>-</xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="sum(nisan/purchase/quantity[../item=$hitamItem_P25])-count(nisan/order[item=$hitamItem_P25])"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                    <td style="color:green;">
                      <xsl:choose>
                        <xsl:when test="sum(nisan/purchase/quantity[../item=$hijauItem_P25])-count(nisan/order[item=$hijauItem_P25])=0">
                          <xsl:text>-</xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="sum(nisan/purchase/quantity[../item=$hijauItem_P25])-count(nisan/order[item=$hijauItem_P25])"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
          </table>
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
          <ol>
            <xsl:for-each select="nisan/order[@delivered='' and name != '']" >
              <xsl:sort select="@soldto" data-type="text" order="ascending" />
              <xsl:sort select="item" data-type="text" order="ascending" />
              <li>
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

                <span class="soldto">
                  <xsl:value-of select="@soldto" />
                </span>
                <span class="order">
                  <xsl:value-of select="@date" />
                </span>
                <span class="name">
                  <xsl:if test="remarks != ''">*</xsl:if>
                  <xsl:value-of select="translate(name,$uppercase,$lowercase)"/>
                  <xsl:choose>
                    <xsl:when test="born != ''">
                      (<xsl:value-of select="born"/>~<xsl:value-of select="death"/>
                      <xsl:if test="deathm != ''">
                        =<xsl:value-of select="deathm"/>
                      </xsl:if>)
                    </xsl:when>
                    <xsl:otherwise>
                      (<xsl:value-of select="death"/>
                      <xsl:if test="deathm != ''">
                        =<xsl:value-of select="deathm"/>
                      </xsl:if>)
                    </xsl:otherwise>
                  </xsl:choose>
                  <xsl:if test="age != ''">
                    <xsl:value-of select="age"/> thn
                  </xsl:if>
                </span>
                <span class="item">
                  <xsl:value-of select="item" />
                </span>
              </li>
            </xsl:for-each>
          </ol>
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