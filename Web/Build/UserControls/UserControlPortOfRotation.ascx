<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPortOfRotation.ascx.cs" Inherits="UserControlPortOfRotation" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ucPortOfRotation" runat="server" DataTextField="FLDPORTOFROTATION" DataValueField="FLDPORTOFROTATIONID" EnableLoadOnDemand="True"
    OnDataBound="ucPortOfRotation_DataBound" OnSelectedIndexChanged="ucPortOfRotation_TextChanged" EmptyMessage="Type to select Port of Rotation" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
