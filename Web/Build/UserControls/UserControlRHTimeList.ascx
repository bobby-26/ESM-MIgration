<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlRHTimeList.ascx.cs" Inherits="UserControlRHTimeList" %>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<div style="position: relative">
    <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
        <asp:DataList ID="dlstTimeList" runat="server" RepeatDirection="Horizontal"
            OnItemCommand="dlstTimeList_ItemCommand"
            OnItemDataBound="dlstTimeList_ItemDataBound" Height="33px"
             BorderColor="ControlLight" GridLines="Both" RepeatLayout="Table" BorderWidth="1px">
            <ItemTemplate>           
                <itemstyle wrap="False" horizontalalign="Left" Width="100%"></itemstyle>
                <asp:Button ID="btnTimeEntry" runat="server" Height ="32px"  style="font-family: Tahoma; font-size: 9px; border: 1px ControlLight"
                  Width="32px" ForeColor="Blue"  BorderStyle="Inset"/>
                <asp:TextBox ID="txtWorkHours" runat="server" Width="0px" Visible="false"></asp:TextBox>
                <asp:TextBox ID="txtid" runat="server" Width="0px" Visible="false"></asp:TextBox>  
                <center>             
                <asp:Label ID="lblsno" runat="server" style="font-family: Tahoma; font-size: 9px; height:inherit"></asp:Label>
                </center>                  
            </ItemTemplate>                        
         </asp:DataList>
    </div>
</div>
