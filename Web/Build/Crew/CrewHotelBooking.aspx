<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewHotelBooking.aspx.cs"
    Inherits="CrewHotelBooking" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MoreLink" Src="~/UserControls/UserControlMoreLinks.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Hotel Booking</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript">
            function resizeDiv() {
                var obj = document.getElementById("divScroll");
                var iframe = document.getElementById("ifMoreInfo");
                var rect = iframe.getBoundingClientRect();
                var x = rect.left;
                var y = rect.top;
                var w = rect.right - rect.left;
                var h = rect.bottom - rect.top;                
                obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - (h + 70) + "px";
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body onload="resizeDiv()" onresize="resizeDiv()">
    <form id="frmPurchaseForm" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="frmTitle" Text="Hotel Booking"></eluc:Title>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <asp:TextBox ID="lblBookingId" runat="server"></asp:TextBox>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuHotelBookingMain" runat="server" OnTabStripCommand="MenuHotelBookingMain_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
        </div>
        <iframe runat="server" id="ifMoreInfo" scrolling="no" style="min-height: 320px; width: 100%">
        </iframe>
        <asp:HiddenField ID="hdnScroll" runat="server" />
        <div class="navSelect" style="position: relative; width: 15px">
            <eluc:TabStrip ID="MenuHotelBooking" runat="server" OnTabStripCommand="MenuHotelBooking_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <div id="divScroll" style="position: relative; z-index: 1; width: 100%; height: 250px;
            overflow: auto;" onscroll="javascript:setScroll('divScroll', 'hdnScroll');">
            <div id="divGrid" style="position: relative; z-index: 1; width: 100%;">
                <asp:GridView ID="gvBookingDetails" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowCommand="gvBookingDetails_RowCommand" OnRowDataBound="gvBookingDetails_ItemDataBound"
                    OnRowCancelingEdit="gvBookingDetails_RowCancelingEdit" OnRowDeleting="gvBookingDetails_RowDeleting"
                    AllowSorting="true" OnRowEditing="gvBookingDetails_RowEditing" ShowHeader="true"
                    EnableViewState="false" OnSorting="gvBookingDetails_Sorting" OnSelectedIndexChanging="gvBookingDetails_SelectedIndexChanging"
                    DataKeyNames="FLDBOOKINGID">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDoubleClick" runat="server" Visible="false" CommandName="DoubleClick"
                                    CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkDoubleClickEdit" runat="server" Visible="false" CommandName="Edit"
                                    CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="number">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblAbbreviationHeader" runat="server" CommandName="Sort" CommandArgument="FLDREFERENCENO"
                                    ForeColor="White">Number</asp:LinkButton>
                                <img id="FLDREFERENCENO" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBookingId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBOOKINGID") %>'></asp:Label>
                                <asp:Label ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                <asp:Label ID="lblFormStatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></asp:Label>
                                <asp:LinkButton ID="lnkReferenceNumberName" runat="server" CommandName="Select" CommandArgument='<%# Container.DataItemIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENO") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Hotel">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblTitelHeader" runat="server" CommandName="Sort" CommandArgument="FLDHOTELID"
                                    ForeColor="White">Hotel</asp:LinkButton>
                                <img id="FLDHOTEL" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblHotelName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDHOTELNAME").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDTITLE").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDHOTELNAME").ToString() %>'></asp:Label>
                                <eluc:ToolTip ID="uclblHotelNameTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOTELNAME") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vessel">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblVesselID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                                <asp:Label ID="lblVessel" runat="server"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="City">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblCity" runat="server" Text="City"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCityId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblRequestedDate" runat="server" CommandName="Sort" CommandArgument="FLDREQUESTEDDATE"
                                    ForeColor="White">Requested Date</asp:LinkButton>
                                <img id="FLDREQUESTEDDATE" runat="server" visible="false" alt="Sort" src="" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblRequestedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTEDDATE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCurrentStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div id="divPage" style="position: relative; z-index: 0">
                <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
                    <tr>
                        <td nowrap align="center">
                            <asp:Label ID="lblPagenumber" runat="server">
                            </asp:Label>
                            <asp:Label ID="lblPages" runat="server">
                            </asp:Label>
                            <asp:Label ID="lblRecords" runat="server">
                            </asp:Label>&nbsp;&nbsp;
                        </td>
                        <td nowrap align="left" width="50px">
                            <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                        </td>
                        <td width="20px">
                            &nbsp;
                        </td>
                        <td nowrap align="right" width="50px">
                            <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                        </td>
                        <td nowrap align="center">
                            <asp:TextBox ID="txtnopage" MaxLength="5" Width="20px" runat="server" CssClass="input">
                            </asp:TextBox>
                            <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                Width="40px"></asp:Button>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
</html> 