<!--
  @todo: view problem under IE
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" omit-xml-declaration="yes"/>
  <xsl:template match="/">
    <html>
      <head>
        <title>Jawi Name Index</title>
        <style type="text/css">
          html {font-family:Tahoma,Verdana,Arial;}
          body > .header {position:fixed;}
          .rumi {padding-right:10px;}
          .header {
          background:#DDDDDD none repeat scroll 0 0;
          height:8em;
          left:0;
          overflow:auto;
          position:absolute;
          top:0;
          width:100%;
          }
          .content {margin-top:150px;}
          /*.tab {width:10px;}*/
          /*.jawi{text-align:right;float:right;}*/
          li {list-style:none;display:inline-block;padding-right:20px;}
          h3 a {text-transform:uppercase;}
          h4 {float:left;width:30px;padding:0;margin:0;text-transform:uppercase;}
          .count{float:left;font-size:smaller;padding:0;margin:0;}
          .block {background:gainsboro;margin:0 0 20px 0;}
        </style>
      </head>
      <body>
        <div class="header">
          <h3>JAWI NAME INDEX</h3>
          <h3>
            back to
            <xsl:for-each select="jawiname/character">
              <a>
                <xsl:attribute name="href">
                  #<xsl:value-of select="."/>
                </xsl:attribute>
                <xsl:value-of select="."/>
              </a> |
            </xsl:for-each>
          </h3>
          <div class="legend">
            name - share
            <span style="color:blue"> male</span>
            <span style="color:orangered"> female</span>
          </div>
        </div>

        <div class="content">
          <xsl:for-each select="jawiname/character">
            <xsl:variable name="character" select="."/>
            <div class="block">
              <xsl:variable name="count" select="count(//rumi[starts-with(.,$character)])"></xsl:variable>
              <h4>
                <xsl:attribute name="id">
                  <xsl:value-of select="$character"/>
                </xsl:attribute>
                <xsl:value-of select="$character"/>
              </h4>
              <span class="count">
                <xsl:value-of select="$count"/> count
              </span>
              <br/>
              <ol>
                <xsl:for-each select="/jawiname/rootname" >
                  <xsl:sort select="rumi" data-type="text" order="ascending" />
                  <xsl:if test="starts-with(rumi,$character)">
                    <li>
                      <xsl:if test="sex='1'">
                        <xsl:attribute name="style">
                          <xsl:text>color:blue;</xsl:text>
                        </xsl:attribute>
                      </xsl:if>
                      <xsl:if test="sex='2'">
                        <xsl:attribute name="style">
                          <xsl:text>color:orangered;</xsl:text>
                        </xsl:attribute>
                      </xsl:if>
                      <span class="rumi">
                        <xsl:value-of select="rumi"/>
                      </span>
                      <span class="tab"> </span>
                      <span class="jawi">
                        <xsl:value-of select="jawi"/>
                      </span>
                    </li>
                  </xsl:if>
                </xsl:for-each>
              </ol>
            </div>
          </xsl:for-each>
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>