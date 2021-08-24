<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPoolList.ascx.cs"
    Inherits="UserControlPoolList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div runat="server" id="divPoolList" style="overflow-y: auto; overflow-x: hidden; height: 80px">
    <telerik:RadListBox RenderMode="Lightweight" ID="lstPool" DataTextField="FLDPOOLNAME" DataValueField="FLDPOOLID" 
        CheckBoxes="true" ShowCheckAll="true" runat="server" 
       ></telerik:RadListBox>  
</div>
