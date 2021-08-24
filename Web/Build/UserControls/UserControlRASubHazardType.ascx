<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlRASubHazardType.ascx.cs" 
Inherits="UserControlRASubHazardType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadListBox ID="cblSubHazardType" runat="server" DataTextField="FLDNAME"
    DataValueField="FLDSUBHAZARDID" OnTextChanged="cblSubHazardType_TextChanged" OnDataBound="cblSubHazardType_DataBound">
</telerik:RadListBox>
