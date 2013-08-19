<!-- @source http://justin-hopkins.com/blog/2008/10/19/henrys-widgets-a-practice-in-xmlxslt -->
<!-- @seealso http://www.jenitennison.com/xslt/grouping/muenchian.html -->
<!-- @seealso http://saxon.sourceforge.net -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" omit-xml-declaration="yes"/>
  <xsl:key name="customerOrderBatch" match="order" use="@date"/>
  <xsl:param name="customer"/>
  <xsl:param name="last"/>
  <!-- @see http://stackoverflow.com/questions/586231/how-can-i-convert-a-string-to-upper-or-lower-case-with-xslt -->
  <xsl:variable name="lowercase" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="uppercase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

  <xsl:template match="nisan">
    <html>
      <head>
        <title>Customer Order List</title>
        <script type="text/javascript">
		/**
		 * Return month in word.
		 * @month interger Month in integer.
		 */
		function getMonth(month) {
			switch(month) {
				case 1: return "Jan";
				case 2: return "Feb";
				case 3: return "Mar";
				case 4: return "Apr";
				case 5: return "May";
				case 6: return "Jun";
				case 7: return "Jul";
				case 8: return "Aug";
				case 9: return "Sep";
				case 10: return "Oct";
				case 11: return "Nov";
				case 12: return "Dec";
			}
			
			return "Unknown";
		}
		
        /*
         * Returns the current date in 'dd MM yyyy' format as a string.
         *
         * @example 31-12-2007
         */
        function todayStr() {
			var today=new Date();
	        return today.getDate() + "-" + getMonth(today.getMonth()+1) + "-" + today.getFullYear();
        }
      	
      	$(function() {
      		$("#date").html(todayStr);
      		$('#calendar').load('calendar.html');
      	});
        </script>
      </head>
      <body>
        <div class="headerLeft">
          To:
          <br /><b><span id="f"><xsl:value-of select="$customer"/></span></b>
        </div>
        <div class="headerRight">
          Date: <span id="date"></span><br/>
          From:
          <br></br>
          <b>THEN YEANG SHING</b>
          <br />963 Jalan 6
          <br />Machang Bubok 14020
          <br />Bukit Mertajam, <u>Penang</u>
          <br />Hp: 012-4711134 Fax: 04-5521 696
          <br />Email: <a href="mailto:yancyn@hotmail.com">yancyn@hotmail.com</a>
          <br />
          <p/>
        </div>
        <div class="content">
          <h3>Customer Order List</h3>
          <hr></hr>
          <span class="orderHead">Order</span>
          <span class="deliver">Delivered</span>
          <span class="name">Description</span>
          <span class="item">Item</span>
          <span class="price">Amount</span>
          <hr></hr>

          <ul style="list-style-type:none;margin-left:0;padding-left:0;">
            <xsl:for-each select="order[count(. | key('customerOrderBatch', @date)[1])=1]">
              <xsl:sort select="@date"/>
              <xsl:sort select="@soldto"/>
              <xsl:if test="(substring(@date,1,4) >= substring($last,1,4)
                            and substring(@date,6,2) >= substring($last,6,2)
                            and substring(@date,9,2) >= substring($last,9,2))
                            or
                            (substring(@date,1,4) >= substring($last,1,4)
                            and substring(@date,6,2) > substring($last,6,2))
                            or
                            (substring(@date,1,4) > substring($last,1,4))
                            ">
                <li>
                  <xsl:variable name="header" select="@date"/>
                  <h4>
                  	<xsl:if test="count(../order[@soldto=$customer and @date=$header])=0">
						        <xsl:attribute name="style">
	                    	<xsl:text>display:none</xsl:text>
	                    </xsl:attribute>
                  	</xsl:if>
                    <xsl:value-of select="@date"/>
                  </h4>
                  <ol>
                    <xsl:for-each select="key('customerOrderBatch',@date)">
                      <xsl:if test="@soldto=$customer">
                        <li>
                          <span class="deliver">
                            <xsl:choose>
                              <xsl:when test="@delivered = ''">
                                -
                                <span class="tab"></span>
                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:value-of select="@delivered" />
                              </xsl:otherwise>
                            </xsl:choose>
                          </span>
                          <span class="name">
                            <xsl:if test="remarks != ''">*</xsl:if>
                            <xsl:value-of select="translate(name,$uppercase,$lowercase)"/>
                            <xsl:choose>
                              <xsl:when test="born != ''">
                                (
                                <xsl:value-of select="born"/>~
                                <xsl:value-of select="death"/>
                                <xsl:if test="deathm != ''">
                                  =
                                  <xsl:value-of select="deathm"/>
                                </xsl:if>)
                              </xsl:when>
                              <xsl:otherwise>
                                (
                                <xsl:value-of select="death"/>
                                <xsl:if test="deathm != ''">
                                  =
                                  <xsl:value-of select="deathm"/>
                                </xsl:if>)
                              </xsl:otherwise>
                            </xsl:choose>
                            <xsl:if test="age != ''">
                              <xsl:value-of select="age"/> thn
                            </xsl:if>
                            <xsl:if test="jawi != ''">
                              <xsl:value-of select="jawi"/>
                            </xsl:if>
                          </span>
                          <span class="item">
                            <xsl:value-of select="item" />
                          </span>
                          <span class="price">
                            <xsl:value-of select="@price" />
                          </span>
                        </li>
                      </xsl:if>
                    </xsl:for-each>
                  </ol>
                  <p/>
                </li>
              </xsl:if>
            </xsl:for-each>
          </ul>

          <hr />

          <div style="padding: 0 5px 0 0;float:right;font-weight:bold;">
            <xsl:value-of select="count(order/@price[../@soldto=$customer
                                                    and (
                                                    (substring(../@date,1,4) >= substring($last,1,4)
                                                    and substring(../@date,6,2) >= substring($last,6,2)
                                                    and substring(../@date,9,2) >= substring($last,9,2))

                                                    or (substring(../@date,1,4) >= substring($last,1,4)
                                                    and substring(../@date,6,2) > substring($last,6,2))

                                                    or (substring(../@date,1,4) > substring($last,1,4))
                                                    )
                                                    ])"/>pcs
            Total RM
            <xsl:value-of select="sum(order/@price[../@soldto=$customer
                                                    and (
                                                    (substring(../@date,1,4) >= substring($last,1,4)
                                                    and substring(../@date,6,2) >= substring($last,6,2)
                                                    and substring(../@date,9,2) >= substring($last,9,2))

                                                    or (substring(../@date,1,4) >= substring($last,1,4)
                                                    and substring(../@date,6,2) > substring($last,6,2))

                                                    or (substring(../@date,1,4) > substring($last,1,4))
                                                    )
                                                    ])"/>
            <br/>
            <xsl:if test="count(order[@soldto=$customer and @delivered='']) > 0">
              <xsl:value-of select="count(order[@soldto=$customer and @delivered=''])" />
              belum siap
            </xsl:if>
          </div>
          Cari bulan Melayu di
          <a href="http://www.hlgranite.com/nisan/calendar.aspx">http://www.hlgranite.com/nisan/calendar.aspx</a>
        </div>
        <br />
      	<div id="calendar" style="float:left"></div>
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
		<!-- TODO: Embed svg in xsl stylesheet -->
		<!-- embed src="G:\My Works\Hamid\AHLAWAHSAHLAYARAMADAN.svg" width="300" height="100" type="image/svg+xml" pluginspage="http://www.adobe.com/svg/viewer/install/" -->
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>