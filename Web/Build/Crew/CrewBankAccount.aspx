<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewBankAccount.aspx.cs"
    Inherits="CrewBankAccount" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Bank Account</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewBankAccount" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="tblid" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:TabStrip ID="CrewMainPersonal" runat="server" Title="Bank Acct."></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <table cellpadding="2" cellspacing="2" width="100%" id="tblid">
                <tr>
                    <td style="width: 13%;">

                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <telerik:RadTextBox runat="server" ID="txtFirstName" Enabled="False" CssClass="input" ReadOnly="true" Width="85%"></telerik:RadTextBox>
                    </td>
                    <td style="width: 13%;">
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <telerik:RadTextBox runat="server" ID="txtMiddleName" Enabled="False" CssClass="input" ReadOnly="true" Width="85%"></telerik:RadTextBox>
                    </td>
                    <td style="width: 13%;">
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <telerik:RadTextBox runat="server" ID="txtLastName" Enabled="False" CssClass="input" ReadOnly="true" Width="85%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" Enabled="False" CssClass="input" ReadOnly="true" Width="85%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtRank" Enabled="False" CssClass="input" ReadOnly="true" Width="85%"></telerik:RadTextBox>
                    </td>
                    <td colspan="2">
                        <telerik:RadCheckBox ID="chkShowOnlyActive" runat="server" AutoPostBack="true" Text="Show Active Only"></telerik:RadCheckBox>
                        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <eluc:TabStrip ID="MenuCrewBankAccount" runat="server" OnTabStripCommand="CrewBankAccount_TabStripCommand"></eluc:TabStrip>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewBankAccount" Height="72%" runat="server"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvCrewBankAccount_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvCrewBankAccount_ItemDataBound"
                OnItemCommand="gvCrewBankAccount_ItemCommand" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Default">
                            <HeaderStyle Width="6%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:ImageButton ID="imgFlag" runat="server" Visible='<%# DataBinder.Eval(Container,"DataItem.FLDISDEFAULT").ToString() == "1" ? true : false%>'
                                    ImageUrl="<%$ PhoenixTheme:images/14.png %>" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <HeaderStyle Width="7%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCrewBankAccountId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBANKACCOUNTID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkAccountType" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTTYPENAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Beneficiary">
                            <HeaderStyle Width="18%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccountName" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Account No.">
                            <HeaderStyle Width="8%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccountNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTNUMBER" ) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Seafarer Bank">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbSeafarerBank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBANKNAME" ) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="IFSC Code">
                            <HeaderStyle Width="9%" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDBANKIFSCCODE"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Intermediate Bank">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIntermediateBank" runat="server" CommandName="EDIT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTERMEDIATEBANK") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Modified By">
                            <HeaderStyle Width="15%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDLASTEDITEDBY").ToString().Equals("") ? " " : DataBinder.Eval(Container, "DataItem.FLDLASTEDITEDBY") + " on " + SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDMODIFIEDDATE"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Approve YN">
                            <HeaderStyle Width="6%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblApproved" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISLOCKYN") %>'></telerik:RadLabel>
                                <%# DataBinder.Eval(Container, "DataItem.FLDISLOCKYN").ToString().Equals("1") ? "Yes" : "No"%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <HeaderStyle Width="6%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEINACTIVENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" UniqueName="Action">
                            <HeaderStyle Width="5%" HorizontalAlign="Center" />
                            <ItemStyle Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="View Contract"
                                    CommandName="update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDefault"
                                    ToolTip="Set Default Bank Account">
                                <span class="icon"><i class="fas fa-file-notepad"></i></span>
                                </asp:LinkButton>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">

                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>


        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
