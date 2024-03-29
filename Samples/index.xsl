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
          html {font-family:Amiri,Tahoma,Verdana,Arial;}
          body > .header {position:fixed;}
          .rumi {padding-right:10px;}

          .header {
            background:#DDDDDD none repeat scroll 0 0;
            left:0;
            overflow:auto;
            position:absolute;
            top:0;
            width:100%;
            padding: 10px;
          }
          .content {margin-top:150px;}

          li,
          #legend {
            border:solid 1px;
            padding-left: 4px;
            padding-right:20px;
            margin: 4px;
            border: solid 1px;
            border-radius: 10px;
          }

          li {list-style:none;display:inline-block;}
          h3 a {text-transform:uppercase;}
          h4 {float:left;width:30px;padding:0;margin:0;text-transform:uppercase;}
          .count {float:left;font-size:smaller;margin-left:10px;}
          .block {background:gainsboro;margin:0 0 20px 0; padding: 10px;}
          .male {background:blue; color:white;}
          .female {background: red;}
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
                  #<xsl:value-of select="english"/>
                </xsl:attribute>
                <xsl:value-of select="english"/>
              </a> |
            </xsl:for-each>
          </h3>
          Total <xsl:value-of select="count(//rumi)"/>
          names -
          <span id="legend">share</span>
          <span class="male" id="legend">male</span>
          <span class="female" id="legend">female</span>
        </div>

        <div class="content">
          <xsl:for-each select="jawiname/character">
            <xsl:variable name="character" select="english"/>
            <div class="block">
              <xsl:variable name="count" select="count(//rumi[starts-with(.,$character)])"></xsl:variable>
              <h4>
                <xsl:attribute name="id">
                  <xsl:value-of select="$character"/>
                </xsl:attribute>
                <span class="rumi">
                  <xsl:value-of select="$character"/>
                </span>
                <xsl:value-of select="arabic"/>
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
                        <xsl:attribute name="class">
                          <xsl:text>male</xsl:text>
                        </xsl:attribute>
                      </xsl:if>
                      <xsl:if test="sex='2'">
                        <xsl:attribute name="class">
                          <xsl:text>female</xsl:text>
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