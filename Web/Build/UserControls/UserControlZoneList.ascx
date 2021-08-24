<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlZoneList.ascx.cs"
    Inherits="UserControlZoneList" %>
<div runat="server" id="divZoneList" style="overflow-y: auto; overflow-x: hidden; height: 80px;width:90px" class="input">
    <asp:CheckBoxList ID="lstZone" DataTextField="FLDZONE" DataValueField="FLDZONEID" Width="90px"
        OnDataBound="lstZone_DataBound" runat="server"></asp:CheckBoxList>
</div>
