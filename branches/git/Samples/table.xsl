<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="html" omit-xml-declaration="yes"/>
    <!-- @see http://stackoverflow.com/questions/586231/how-can-i-convert-a-string-to-upper-or-lower-case-with-xslt -->  
    <xsl:variable name="lowercase" select="'abcdefghijklmnopqrstuvwxyz'"/>
    <xsl:variable name="uppercase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
    <xsl:param name="customer"/>
    <xsl:param name="last"/>
  
    <xsl:template name="laterThan">
        <xsl:param name="orderDate"/>
        <xsl:param name="last"/>
        <xsl:value-of select="(substring($orderDate,1,4) >= substring($last,1,4)
                  and substring($orderDate,6,2) >= substring($last,6,2)
                  and substring($orderDate,9,2) >= substring($last,9,2))
                  or
                  (substring($orderDate,1,4) >= substring($last,1,4)
                  and substring($orderDate,6,2) > substring($last,6,2))
                  or
                  (substring($orderDate,1,4) > substring($last,1,4))"/>
    </xsl:template>
  
    <xsl:template match="/">
        <html>
            <head>
                <title>For Export to Spreadsheet Use Only</title>
            </head>
            <body>
                <div class="content">
                    <h3>For Export to Spreadsheet Use Only</h3>
                    <table>
                        <tr>
                            <th>Order</th>
                            <th>Delivered</th>
                            <th>Name</th>
                            <th>Item</th>
                            <th>Price</th>
                        </tr>
                        <xsl:for-each select="nisan/order[
                          @soldto=$customer
                          and (@delivered=''
                          or
                           (substring(@date,1,4) >= substring($last,1,4)
                            and substring(@date,6,2) >= substring($last,6,2)
                            and substring(@date,9,2) >= substring($last,9,2))
                            or
                            (substring(@date,1,4) >= substring($last,1,4)
                            and substring(@date,6,2) > substring($last,6,2))
                            or
                            (substring(@date,1,4) > substring($last,1,4))
                          )
                          ]">
                            <!--<xsl:sort select="@soldto" data-type="text" order="ascending" />-->
                            <xsl:sort select="@date" data-type="text" order="ascending" />
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

                                <!--<td><xsl:number/></td>-->
                                <td>
                                    <xsl:value-of select="@date"/>
                                </td>
                                <td>
                                    <xsl:value-of select="@delivered"/>
                                </td>
                                <td>
                                    <xsl:value-of select="translate(name,$uppercase,$lowercase)"/>
                                </td>
                                <td>
                                    <xsl:value-of select="item"/>
                                </td>
                                <td class="right">
                                    <xsl:value-of select="@price"/>
                                </td>
                            </tr>
                        </xsl:for-each>
                    </table>
                    <hr />
                </div>
            </body>
        </html>
    </xsl:template>
</xsl:stylesheet>