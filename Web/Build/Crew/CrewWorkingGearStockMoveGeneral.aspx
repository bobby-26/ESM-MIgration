<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewWorkingGearStockMoveGeneral.aspx.cs"
    Inherits="CrewWorkingGearStockMoveGeneral" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Move Received stock to Zone</title>
       <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript">
            function CalculateTotal(rowindex)
            {
              rowindex = parseInt(rowindex)+1;  
              var grid = document.getElementById('<%=gvWorkGearPendingItem.ClientID %>');
              var row;
              var rowtoatal = 0;
              var nodelength;
              
              row = grid.rows[rowindex];
              nodelength = row.childNodes.length;
              for (j = 0; j < nodelength; j++)
              {
                  if(row.childNodes[j].childNodes.length > 0 && row.childNodes[j].childNodes[0].type != null)
                  {
                     if (row.childNodes[j].childNodes[0].type == "text" && row.childNodes[j].childNodes[0].id.search("txtTotal")== -1)
                     {
     	                if(!isNaN(row.childNodes[j].childNodes[0].value) &&  row.childNodes[j].childNodes[0].value != "")
                         {
                            rowtoatal += parseInt(row.childNodes[j].childNodes[0].value)
                         }
                     }
                     if(row.childNodes[j].childNodes[0].id.search("txtTotal") > 0)
                     {
                       var lbltot = row.childNodes[j].childNodes[0];
                       lbltot.value = rowtoatal.toString();
                     }
                  }
              }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlWorkGearStockMove">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="Move Received stock to Zone" ShowMenu="<%# Title1.ShowMenu %>">
                    </eluc:Title>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuWorkGearGeneral" runat="server" OnTabStripCommand="MenuWorkGearGeneral_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div style="color: Blue;">
                    1. First select Stock moving zones and save it
                    <br />
                    2. Then only you can move stock to those zones. But you can't change the zones once
                    it saved.
                </div>
                <br style="clear: both" />
                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <asp:Literal ID="lblOrderNo" runat="server" Text="Order No"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRefNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblSupplierName" Runat="server" Text="Supplier Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSupplierName" runat="server" Width="180px" CssClass="readonlytextbox"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblReceivedDate" runat="server" Text="Received Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtReceivedDate" runat="server" CssClass="readonlytextbox" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblStockMovingZones" runat="Server" Text="Stock moving Zones"></asp:Literal>
                        </td>
                        <td>
                            <div id="divVesselList" class="input" style="overflow: auto; width: 80%; height: 85px">
                                <asp:CheckBoxList runat="server" ID="cblZone" Height="100%" RepeatColumns="2" RepeatDirection="Horizontal"
                                    DataTextField="FLDZONE" DataValueField="FLDZONEID" RepeatLayout="Flow">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
                <br style="clear: both" />
                <h4>
                    <asp:Literal ID="lblReceivedItems" runat="server" Text="Received Items"></asp:Literal></h4>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuStockMoveItem" runat="server" OnTabStripCommand="MenuStockMoveItem_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" runat="server" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvWorkGearPendingItem" runat="server" AutoGenerateColumns="true"
                        Font-Size="11px" Width="100%" CellPadding="3" OnRowDataBound="gvWorkGearPendingItem_RowDataBound"
                        ShowHeader="true" EnableViewState="false" DataKeyNames="FLDORDERLINEID" OnRowCreated="gvWorkGearPendingItem_RowCreated">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                    </asp:GridView>
                    <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap="nowrap" align="center">
                                <asp:Label ID="lblPagenumber" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPages" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecords" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap="nowrap" align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev"><< Prev </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap="nowrap" align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap="nowrap" align="center">
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
                <br style="clear: both" />
                <eluc:Status runat="server" ID="ucStatus" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
