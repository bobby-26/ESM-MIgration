<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsOpeningRobExcelUpload.aspx.cs"
    Inherits="VesselAccountsOpeningRobExcelUpload" %>

<%@ Import Namespace="System.Data" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel Accounting Opening Rob Excel Upload</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmForwarderExcelUpload" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <div class="subHeader">
                <div id="divHeading" style="vertical-align: top">
                    <eluc:Title runat="server" ID="Attachment" Text="Rob Initialize" ShowMenu="false"></eluc:Title>
                </div>
                <eluc:Status runat="server" ID="ucStatus" />
            </div>
            <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                <eluc:TabStrip ID="MenuOpeningRob" runat="server" OnTabStripCommand="MenuOpeningRob_OnTabStripCommand"></eluc:TabStrip>
            </div>
            <br />
            <table>
                <tr>
                    <td></td>
                    <td></td>
                    <td>
                        <%--<asp:Literal ID="lblSendMailtoVessel" runat="server" Text="Send Mail to Vessel"></asp:Literal> <br />
                    <asp:Literal ID="lbltoTrashVesselAccounting" runat="server" Text="to Trash Vessel Accounting"></asp:Literal>  <br />
                    <asp:Literal ID="lblinVesselSide" runat="server" Text="in Vessel Side"></asp:Literal>--%>
                    </td>
                    <td>
                        <%--<asp:Button ID="btnSendMailForTrash" runat="server" CssClass="input" Text="Send Mail" />--%>
                        <%--<asp:Button ID="btnSendMailForTrash" runat="server" CssClass="input" OnClick="btnSendMailForTrash_Click" Text="Send Mail" />--%>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td colspan="4">
                        <font color="blue"><b>
                            <asp:Literal ID="lblNote" runat="server" Text="Note:"></asp:Literal>
                        </b>
                            <asp:Literal ID="lblCheckWeathertheVesselAccountingStartDateisCorrectornot" runat="server"
                                Text="Check Weather the Vessel Accounting Start Date is Correct or not"></asp:Literal>
                            <br />
                            <asp:Literal ID="lblIfitsCorrectClickthesendmailbuttonPopuppagewillOpeninthatpopuppagepleasereadthemailcontent"
                                runat="server" Text="If it's Correct, Click the send mail button.. Popup page will Open in that popup page please read the mail content.."></asp:Literal>
                            <br />
                            <asp:Literal ID="lblMailContentShouldbefollowedbyus" runat="server" Text="Mail Content Should be followed by us.."></asp:Literal>
                            <br />
                            <asp:Literal ID="lblAfterimportingvesseldata" runat="server" Text="After importing vessel data"></asp:Literal>
                            <br />
                            <asp:Literal ID="lblUploadtheROBexcelsheet" runat="server" Text="Upload the ROB excel sheet"></asp:Literal>
                            <br />
                            <asp:Literal ID="lblFinallyClicktheInitializebutton" runat="server" Text="Finally Click the Initialize button.."></asp:Literal>
                        </font>
                    </td>
                    <td width="50%">
                        <table width="100%">
                            <tr>
                                <td width="15%">
                                    <asp:Literal ID="lblVesselAccountingStartDate" runat="server" Text="Vessel Accounting Start Date"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Label ID="lblHardCode" runat="server" Visible="false"></asp:Label>
                                    <eluc:Date ID="ucPBStart" runat="server" CssClass="input_mandatory" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblChooseafile" runat="server" Text="Choose a file"></asp:Literal>
                                </td>
                                <td>
                                    <asp:FileUpload ID="FileUpload" runat="server" CssClass="input" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>
                        <%--<asp:Button ID="btnConfirm" runat="server" Text="Initialize" CssClass="input" 
                       onclick="btnConfirm_Click" />&nbsp;&nbsp;--%>
                    </td>
                </tr>
            </table>
            <hr />
            <br />
            <asp:GridView ID="gvAttachment" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                GridLines="None" Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
            </asp:GridView>
            <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                <asp:GridView ID="gvOpeningSearch" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    GridLines="None" Width="100%" CellPadding="3" OnRowDataBound="gvOpeningSearch_RowDataBound"
                    OnRowEditing="gvOpeningSearch_RowEditing" OnRowCancelingEdit="gvOpeningSearch_RowCancelingEdit"
                    OnRowUpdating="gvOpeningSearch_RowUpdating" OnRowDeleting="gvOpeningSearch_RowDeleting"
                    ShowHeader="true" ShowFooter="true" EnableViewState="false" DataKeyNames="FLDOPENINGROBID">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField HeaderText="Number">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDNUMBER"]%>
                                <asp:TextBox ID="txtNumber" runat="server" CssClass="input_mandatory" Width="90px"
                                    Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDNUMBER"] %>'>
                                </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDNAME"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtName" runat="server" CssClass="input_mandatory" Text='<%#((DataRowView)Container.DataItem)["FLDNAME"] %>'>
                                </asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quantity">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDQTY"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtQuantityEdit" runat="server" CssClass="input_mandatory" Width="90px"
                                    Text='<%#((DataRowView)Container.DataItem)["FLDQTY"] %>' IsPositive="true" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Price">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDTOTAL"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtTotalEdit" runat="server" CssClass="input_mandatory" Width="90px"
                                    Text='<%#((DataRowView)Container.DataItem)["FLDTOTAL"] %>' IsPositive="true" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:Literal ID="lblProvisionTotalAmountUSD" runat="server" Text="Provision Total Amount (USD)"></asp:Literal>
                                <br />
                                <asp:Literal ID="lblProvisionTotalQuantity" runat="server" Text="Provision Total Quantity"></asp:Literal>
                                <br />
                                <asp:Literal ID="lblBondTotalAmountUSD" runat="server" Text="Bond Total Amount (USD)"></asp:Literal><br />
                                <asp:Literal ID="lblBondTotalQuantity" runat="server" Text="Bond Total Quantity"></asp:Literal>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Store Id">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDSTORECLASS"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtStoreClassEdit" runat="server" CssClass="input_mandatory" Width="90px"
                                    Text='<%#((DataRowView)Container.DataItem)["FLDSTORECLASS"] %>' IsInteger="true"
                                    IsPositive="true" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Store Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblStoreType" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblAction" runat="server" Text="Action"></asp:Literal>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
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
                        <td width="20px">&nbsp;
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
        </div>
    </form>
</body>
</html>
