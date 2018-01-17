<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html"/>
  <xsl:template match="ReportResponseViewModel">
    <html>
      <head>
        <title>Report</title>
      </head>
      <body>
        <h2>Report on complain</h2>
        <br/>
        <h3>
          <xsl:value-of select="ComplainId"/>
        </h3>
        <h3>
          <xsl:value-of select="ComplainText"/>
        </h3>
        <h3>
          <xsl:value-of select="DealOwnerUsername"/>
        </h3>
        <h3>
          <xsl:value-of select="DealCreditorUsername"/>
        </h3>
        <h3>
          <xsl:value-of select="DateOfCreating"/>
        </h3>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
