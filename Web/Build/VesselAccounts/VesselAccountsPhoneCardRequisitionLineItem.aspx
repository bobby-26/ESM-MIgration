<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsPhoneCardRequisitionLineItem.aspx.cs"
    Inherits="VesselAccountsPhoneCardRequisitionLineItem" %>


<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <title>Component</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmBondReq" runat="server" autocomplete="off">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlPhonReqLine">
            <ContentTemplate>
                <div style="top: 100px; margin-left: auto; margin-right: auto; width: 100%;">
                    <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
                        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                        <div class="subHeader" style="position: relative; right: 0px">
                            <eluc:Title runat="server" ID="Title1" Text="Line Item" ShowMenu="false"></eluc:Title>
                            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                        </div>
                    </div>
                    <div class="navSelect" style="position: relative; clear: both; width: 15px">
                        <eluc:TabStrip ID="MenuPhoneReq" runat="server" OnTabStripCommand="MenuPhoneReq_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                        <asp:GridView ID="gvPhoneReqLine" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            GridLines="None" Width="100%" CellPadding="3" OnRowDataBound="gvPhoneReqLine_RowDataBound"
                            ShowHeader="true" EnableViewState="false" AllowSorting="true" ShowFooter="true"
                            OnRowEditing="gvPhoneReqLine_RowEditing" OnSorting="gvPhoneReqLine_Sorting" OnRowCancelingEdit="gvPhoneReqLine_RowCancelingEdit"
                            OnRowDeleting="gvPhoneReqLine_RowDeleting" DataKeyNames="FLDREQUESTLINEID" OnRowCommand="gvPhoneReqLine_RowCommand">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblEmpName" runat="server" Text="Employee"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDREQUESTID"] %>'></asp:Label>
                                        <asp:Label ID="lblStatus" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDREQUESTSTATUS"] %>'></asp:Label>
                                        <asp:Label ID="lblOrderStatus" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDORDERSTATUS"] %>'></asp:Label>
                                        <asp:LinkButton ID="lnkEmpName" runat="server" CommandName="select"><%# ((DataRowView)Container.DataItem)["FLDEMPNAME"]%></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblRequestLineIdEdit" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDREQUESTLINEID"] %>'></asp:Label>
                                        <eluc:VesselCrew ID="ddlEmployeeEdit" runat="server" CssClass="input" AppendDataBoundItems="true"
                                            SelectedEmployee='<%# ((DataRowView)Container.DataItem)["FLDEMPLOYEEID"] %>' />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:VesselCrew ID="ddlEmployee" runat="server" CssClass="input" AppendDataBoundItems="true" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblStoreName" runat="server" Text="Card Type"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDNAME"]%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <span id="spnPickListCard">
                                            <asp:TextBox ID="txtStoreClass" runat="server" Width="90px" CssClass="input_mandatory"></asp:TextBox>
                                            <asp:TextBox ID="txtStoreClassName" runat="server" Width="240px" CssClass="input_mandatory"></asp:TextBox>
                                            <asp:ImageButton runat="server" ID="btnStoreClassPickList" OnClientClick="return showPickList('spnPickListCard', 'codehelp1', '', '../Common/CommonPickListStoreItem.aspx?storeclass=633', true);"
                                                Text=".." ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" ImageAlign="AbsMiddle" />
                                            <asp:TextBox ID="txtStoreId" runat="server" Width="0px" CssClass="input" Text=''></asp:TextBox>
                                        </span>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQty" runat="server" Text="Quantity"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuantity" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDQUANTITY"]%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Number ID="txtQuantityEdit" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDQUANTITY"]%>'
                                            CssClass="input_mandatory" Width="90px" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Number ID="txtQuantity" runat="server" CssClass="input_mandatory" DefaultZero="false"
                                            Width="90px" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblHdCardNumber" runat="server" Text="Card Number-PIN Number"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCardNumber" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDISSUECARDNO"]%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtCardNumberEdit" runat="server" CssClass="input" ToolTip="CardNo1-PinNo1,CardNo2-PinNo2,..."
                                            Text='<%# ((DataRowView)Container.DataItem)["FLDISSUECARDNO"]%>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblHdrIssueDate" runat="server" Text="Issue Date"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblIssueDate" runat="server" Text='<%# string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDISSUEDDATE"]) %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Date ID="txtIssueDateEdit" runat="server" CssClass="input" Text='<%# ((DataRowView)Container.DataItem)["FLDISSUEDDATE"] %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblActionHeader" runat="server">
                                            Action
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>
                                        <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Approve" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                            CommandName="APPROVE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdApprove"
                                            ToolTip="Confirm Issue"></asp:ImageButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="Save" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                            ToolTip="Save"></asp:ImageButton>
                                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                            ToolTip="Cancel"></asp:ImageButton>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                            CommandName="Add" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                            ToolTip="Add New"></asp:ImageButton>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="divPage" style="position: relative;">
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
                                <td width="20px">&nbsp;
                                </td>
                                <td nowrap align="right" width="50px">
                                    <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                                </td>
                                <td nowrap align="center">
                                    <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                    </asp:TextBox>
                                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                        Width="40px"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
