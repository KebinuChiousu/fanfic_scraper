<?xml version="1.0"?> 
<xslt:stylesheet xmlns:xslt="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xslt:output method="text" />
  <xslt:preserve-space elements="comment" />
  <xslt:template match="/">
    <xslt:apply-templates select="sections/comment" />
    <xslt:apply-templates select="sections/section"/>
  </xslt:template>
  <xslt:template match="section">
    <xslt:value-of select="@name"/>
    <xslt:apply-templates select="comment" />
    <xslt:apply-templates select="item"/>
  </xslt:template>
  <xslt:template match="item">
    <xslt:apply-templates select="@key" />
    <xslt:text>=</xslt:text>
    <xslt:apply-templates select="@value" />
    <xslt:text></xslt:text>
  </xslt:template>
  <xslt:template match="@key">
    <xslt:value-of select="."/>
  </xslt:template>
  <xslt:template match="@value">
    <xslt:value-of select="."/>
  </xslt:template>
  <xslt:template match="comment">
      <xslt:text>;</xslt:text>
      <xslt:value-of select="text()"/>
      <xslt:text></xslt:text>
  </xslt:template>
</xslt:stylesheet>
