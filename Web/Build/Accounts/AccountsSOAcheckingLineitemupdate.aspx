<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSOAcheckingLineitemupdate.aspx.cs"
    Inherits="AccountsSOAcheckingLineitemupdate" EnableEventValidation="false" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiOwnerBudgetCode" Src="~/UserControls/UserControlMultiColumnOwnerBudgetCodeT.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SOAUpdate</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>


    </telerik:RadCodeBlock>
</head>
<body>
    <form id="SOAUpdate" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">

            <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
            <div>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status ID="ucStatus" runat="server" Text="" />
               
                <asp:Button runat="server" ID="cmdHiddenPick" OnClick="cmdHiddenPick_Click" />

                <eluc:TabStrip ID="MenuGeneral" runat="server" OnTabStripCommand="MenuGeneral_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>


                <eluc:TabStrip ID="MenuGenralSub" runat="server" OnTabStripCommand="MenuGenralSub_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>


                <eluc:TabStrip ID="MenuCompanyList" runat="server" OnTabStripCommand="CompanyList_TabStripCommand"></eluc:TabStrip>

                <br />
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text="Voucher Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtVoucherNumber" runat="server" MaxLength="500" Width="150px" CssClass="readonlytextbox"
                                ReadOnly="true">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVoucherDate" runat="server" Text="Voucher Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtVoucherDate" runat="server" MaxLength="500" Width="70px" CssClass="readonlytextbox"
                                ReadOnly="true">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblAccountCodeDescription" runat="server" Text="Account Code/Description"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtAccountCode" runat="server" MaxLength="500" Width="50px" CssClass="readonlytextbox"
                                ReadOnly="true">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtAccountDescription" runat="server" MaxLength="500" Width="250px"
                                CssClass="readonlytextbox" ReadOnly="true">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblBudgetCode" runat="server" Text="Budget Code"></telerik:RadLabel>
                        </td>
                        <td>
                            <span id="spnPickListBudgetCode">
                                <telerik:RadTextBox ID="txtBudgetCode" runat="server" CssClass="input_mandatory" MaxLength="20"
                                  Enabled="false"  Width="50px">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtBudgetCodeDescription" runat="server" CssClass="input_mandatory"
                                   Enabled="false" MaxLength="50" Width="250px">
                                </telerik:RadTextBox>
                                <img runat="server" id="imgShowBudgetCode" style="cursor: pointer; vertical-align: top"
                                    src="<%$ PhoenixTheme:images/picklist.png %>" />
                                <telerik:RadTextBox ID="txtBudgetCodeId" runat="server" CssClass="input_mandatory" MaxLength="20"
                                    Width="0px">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="input_mandatory"></telerik:RadTextBox>
                            </span>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOwnerBudgetcode" runat="server" Text="Owner Budget code"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:MultiOwnerBudgetCode ID="ucOwnerBudgetCode" runat="server" CssClass="input_mandatory"
                                Width="300px" Enabled="true" />
                            <%--<telerik:RadTextBox runat="server" ID="txtOwnerbudgetcode" MaxLength="20" Width="180px" CssClass="input_mandatory"></telerik:RadTextBox>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <telerik:RadLabel ID="lblNotIncludeYNHeader" runat="server">Included in Owner SOA</telerik:RadLabel>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIncludeYNEdit" runat="server"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblShowInSummaryBalance" runat="server" Text="Show separately </br> in Vessel Summary Balance"></telerik:RadLabel>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkShowInSummaryBalance" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblDescription" runat="server" Text="Description"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtdescription" TextMode="MultiLine" Rows="5" Width="340px"
                                CssClass="input">
                            </telerik:RadTextBox>
                        </td>
                    </tr>

                </table>
            </div>
            <br />


        
                    <%-- <asp:GridView ID="gvOwnersAccount" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" AllowSorting="true" ShowHeader="true" EnableViewState="false"
                        OnRowDataBound="gvOwnersAccount_RowDataBound" OnRowCommand="gvOwnersAccount_RowCommand"
                        OnRowCreated="gvOwnersAccount_RowCreated" OnRowEditing="gvOwnersAccount_RowEditing"
                        OnRowCancelingEdit="gvOwnersAccount_RowCancelingEdit" OnRowUpdating="gvOwnersAccount_RowUpdating"
                        DataKeyNames="FLDVOUCHERDETAILID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                        <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvOwnersAccount" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvOwnersAccount_NeedDataSource"
                        OnItemCommand="gvOwnersAccount_ItemCommand"
                        OnItemDataBound="gvOwnersAccount_ItemDataBound"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"

                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDVOUCHERDETAILID">
                            <NoRecordsTemplate>
                                <table width="100%" border="0">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <HeaderStyle Width="102px" />
                            <Columns>
                                <%--    <telerik:GridTemplateColumn>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkChecked" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>--%>
                                <telerik:GridTemplateColumn HeaderText="Row No" HeaderStyle-Width="60px">
                                    
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblRow" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERROW") %>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Budget Code" HeaderStyle-Width="150px">
                               
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblEsmBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDESMBUDGETCODE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblDtKey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                        Visible="false"></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Owner Budget Code" HeaderStyle-Width="100px">
                                   
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblOwnerBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODE") %>'></telerik:RadLabel>
                                </itemtemplate>
                                    <%--<EditItemTemplate>
                                    <span id="spnPickListOwnerBudgetEdit">
                                        <telerik:RadTextBox ID="txtOwnerBudgetCodeEdit" runat="server" MaxLength="20" CssClass="input"
                                            Width="60px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODE") %>'></telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtOwnerBudgetNameEdit" runat="server" Width="0px" CssClass="input"
                                            Enabled="False"></telerik:RadTextBox>
                                        <asp:ImageButton ID="btnShowOwnerBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataItemIndex %>" />
                                        <telerik:RadTextBox ID="txtOwnerBudgetIdEdit" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtOwnerBudgetgroupIdEdit" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                    </span>
                                </EditItemTemplate>--%>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Description" HeaderStyle-Width="200px">
                                    
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblVoucherId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDETAILID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblDescription" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLONGDESCRIPTION") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkDesription" runat="server" Width="250px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLONGDESCRIPTION") %>'
                                        CommandName="Select" CommandArgument="<%# Container.DataSetIndex %>"></asp:LinkButton>
                                </itemtemplate>
                                    <%--<EditItemTemplate>
                                    <telerik:RadTextBox ID="txtDescriptionEdit" runat="server" TextMode="MultiLine" Width="300px"
                                        CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLONGDESCRIPTION") %>'></telerik:RadTextBox>
                                </EditItemTemplate>--%>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Prime Currency" HeaderStyle-Width="80px">
                                    
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblPrimeCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Prime Amount" HeaderStyle-Width="100px">
                                    <itemstyle wrap="False" horizontalalign="Right"></itemstyle>
                                  
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Included in Owner SOA" HeaderStyle-Width="100px">
                                    <itemstyle wrap="False" horizontalalign="Right"></itemstyle>
                                  
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblIncludedinOwnerSOA" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOTINCULDEINOWNERREPORT") %>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Show separately in Vessel Summary Balance" HeaderStyle-Width="150px">
                                    <itemstyle wrap="False" horizontalalign="Right"></itemstyle>
                                   
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblShowsummarybal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHOWINSUMMARYBALANCE") %>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="150px">
                                  
                                    <itemtemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                        ToolTip="Edit" Visible="false"></asp:ImageButton>
                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Merge" ImageUrl="<%$ PhoenixTheme:images/select.png %>"
                                        CommandName="MERGE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdMerge"
                                        ToolTip="Merge"></asp:ImageButton>
                                    <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="UnMerge" ImageUrl="<%$ PhoenixTheme:images/54.png %>"
                                        CommandName="UNMERGE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdUnMerge"
                                        ToolTip="UnMerge"></asp:ImageButton>
                                    <img id="Img3" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                        CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAttachment"
                                        ToolTip="Attachment" Visible="false"></asp:ImageButton>
                                    <img id="Img4" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="No Attachment" ImageUrl="<%$ PhoenixTheme:images/no-attachment.png %>"
                                        CommandName="NOATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdNoAttachment"
                                        Visible="false" ToolTip="No Attachment"></asp:ImageButton>
                                </itemtemplate>
                                    <%--<EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img5" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>--%>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="415px" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
            
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
