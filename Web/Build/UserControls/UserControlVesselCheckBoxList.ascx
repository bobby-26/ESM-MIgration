<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlVesselCheckBoxList.ascx.cs"
    Inherits="UserControlVesselCheckBoxList" %>

<%--<div runat="server" id="divVesselList" style="overflow-y: auto; overflow-x: hidden; height: 80px">
    <asp:CheckBoxList ID="lstVessel" runat="server" DataTextField="FLDVESSELNAME"
        DataValueField="FLDVESSELID" RepeatDirection="Vertical" RepeatColumns="1" OnDataBound="lstVessel_DataBound" OnTextChanged="lstVessel_TextChanged">
    </asp:CheckBoxList>
</div>--%>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div runat="server" id="divVesselList" style="overflow-y: auto; overflow-x: hidden; height: 80px">
    <telerik:RadListBox RenderMode="Lightweight" ID="lstVessel" DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID" 
        CheckBoxes="true" ShowCheckAll="true" runat="server" OnDataBound="lstVessel_DataBound" Localization-CheckAll="--Check All--"
        OnTextChanged="lstVessel_TextChanged"></telerik:RadListBox>
</div>

