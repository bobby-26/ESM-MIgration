<%@ Page Language="C#" AutoEventWireup="True" CodeFile="CrewWorkingGearIssuePayByAccounts.aspx.cs"
    Inherits="CrewWorkingGearIssuePayByAccounts" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Working Gear Items</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
       <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmWorkingGearAdditionalItems" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlWorkingGearItem">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="Allocate Billing A/C for Issue"></eluc:Title>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblConfigureWorkingGearItem" width="80%">
                        <tr>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblIssueDate" runat="server" Text="Issue Date &nbsp;From"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtFromDate" runat="server" CssClass="input" />
                            </td>
                            <td>
                                <asp:Literal ID="lblTo" runat="server" Text="To"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtToDate" runat="server" CssClass="input" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuRegistersWorkingGearItem" runat="server" OnTabStripCommand="RegistersWorkingGearItem_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative;">
                    <asp:GridView ID="gvWorkGearIssue" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvWorkGearIssue_ItemDataBound"
                        OnRowEditing="gvWorkGearIssue_RowEditing" OnRowCancelingEdit="gvWorkGearIssue_RowCancelingEdit"
                        OnRowUpdating="gvWorkGearIssue_RowUpdating" ShowFooter="false"
                        Style="margin-bottom: 0px" EnableViewState="false" DataKeyNames="FLDTRANSACTIONID">
                        <FooterStyle ForeColor="#000066" BackColor="#dfdfdf"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle BackColor="#f9f9fa" />
                        <SelectedRowStyle BackColor="#bbddff" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField FooterText="New WorkingGearItem">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSeafarerNameHeader" runat="server">Seafarer Name</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTransactionId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDTRANSACTIONID"] %>'></asp:Label>
                                    <asp:Label ID="lnkSeafarerName" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDNAME"] %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblTransIdEdit" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDTRANSACTIONID"] %>'></asp:Label>
                                    <asp:Label ID="lblEmployeeIdEdit" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDEMPLOYEEID"] %>'></asp:Label>
                                    <asp:Label ID="lblTranTypeEdit" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDTRANSACTIONTYPE"] %>'></asp:Label>
                                    <asp:Label ID="lnkSeafarerNameEdit" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDNAME"] %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterText="New WorkingGearItem">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblReferenceNoHeader" runat="server">Reference No</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDREFERENCENO"]%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblRefNoEdit" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDREFERENCENO"]%>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblIssueDateHeader" runat="server">Issue Date</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDTRANSACTIONDATE"])%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblTranDateEdit" runat="server" Text='<%# string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDTRANSACTIONDATE"])%>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblRemarksHeader" runat="server">
                                   Vessel Issued for
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblIssueVesselId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDISSUEDVESSELID"] %>'></asp:Label>
                                    <asp:Label ID="lblIssueVessel" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDISSUEDVESSELNAME"] %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblIssueVesselIdEdit" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDISSUEDVESSELID"] %>'></asp:Label>
                                    <asp:Label ID="lblIssueVesselEdit" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDISSUEDVESSELNAME"] %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblRemarksHeader" runat="server">
                                   6Vessel Joined
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblJoinVesselId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDJOINEDVESSELID"] %>'></asp:Label>
                                    <asp:Label ID="lblJoinVessel" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDJOINEDVESSELNAME"] %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblJoinVesseIdEdit" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDJOINEDVESSELID"] %>'></asp:Label>
                                    <asp:Label ID="lblJoinVesseEdit" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDJOINEDVESSELNAME"] %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblOnAccountOfHeader" runat="server">On Account Of</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPayById" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDPAYABLEBY"] %>'></asp:Label>
                                    <asp:Label ID="lblPayBy" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDPAYABLEBYACCOUNT"] %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblPayByIdEdit" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDPAYABLEBY"] %>'></asp:Label>
                                    <asp:DropDownList ID="ddlPayBy" runat="server">
                                        <asp:ListItem Text="-- Select --" Value="DUMMY" />
                                        <asp:ListItem Text="Vessel Joined" Value="1" />
                                        <asp:ListItem Text="Previous Vessel" Value="0" />
                                        <asp:ListItem Text="Recd from Seafarer" Value="2" />
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Stock in Hand">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                   <asp:Label ID="lblVerifiedHeader" runat="server"> Verified</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVerified" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDVERIFIED"] %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkVerified" runat="server" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTypeHeader" runat="server">Type</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTypeDesc" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDYPEDESC"] %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                           
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdUpdate"
                                        ToolTip="Update"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <%--<div id="divPage" style="position: relative;">
                    <table width="100%" border="0" style="background-color: #88bbee">
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
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
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
                </div>--%>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="MenuRegistersWorkingGearItem" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
