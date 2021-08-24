<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersTravelReasonOwnerBudgetCodeMap.aspx.cs"
    Inherits="RegistersTravelReasonOwnerBudgetCodeMap" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Splitter" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Travel Reason</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="VoucherUpload" runat="server" submitdisabledcontrols="true">

        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />

    <telerik:RadAjaxPanel runat="server" ID="pnlCourseRegister" Height="100%">
      
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
      
                    
                    <eluc:Status runat="server" ID="ucStatus" />
                <table id="tbl1" width="50%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblPrincipal" runat="server" Text="Owner"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Address runat="server" ID="ucPrincipal" CssClass="dropdown_mandatory" AddressType="128" AppendDataBoundItems="true" Width="45%"
                                AutoPostBack="true" />
                        </td>
                    </tr>
                </table>
                
                    <eluc:TabStrip ID="MenuTravelReason" runat="server" OnTabStripCommand="MenuTravelReason_TabStripCommand" TabStrip="true">
                    </eluc:TabStrip>
               
                <telerik:RadGrid RenderMode="Lightweight" ID="gvTravelReason" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None" Height="90%"
                    Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnItemDataBound="gvTravelReason_ItemDataBound" OnSortCommand="gvTravelReason_SortCommand"
                   OnDeleteCommand="gvTravelReason_OnDeleteCommand" OnNeedDataSource="gvTravelReason_OnNeedDataSource" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    OnItemCommand="gvTravelReason_ItemCommand"> 
                      <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
          
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="170px">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblTravelReasonHdr" runat="server" Text="Travel Reason"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="lblTravelReasonId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTRAVELREASONID") %>'></asp:Literal>
                                <asp:Literal ID="lblTravelReasonOBCId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTRAVELREASONOBCID") %>'></asp:Literal>
                                <asp:Literal ID="lblTravelReason" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDREASON")%>'></asp:Literal>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="95px">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblOnBudgetCodeHdr" runat="server" Text="Budget Code"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="lblOnBudgetCodeId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDONSIGNERBUDGETID")%>'></asp:Literal>
                                <asp:Literal ID="lblOnBudgetCode" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDONSIGNERSUBACCOUNT")%>'></asp:Literal>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="130px">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Literal ID="lblOnOwnerBudgetcodeHdr" runat="server" Text="Owner Budget Code"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="lblOnOwnerBudgetcode" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDONSIGNEROWNERBUDGETCODE")%>'></asp:Literal>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListOnOwnerBudgetEdit">
                                    <telerik:RadTextBox ID="txtOnOwnerBudgetCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNEROWNERBUDGETCODE") %>'
                                        MaxLength="20" CssClass="input" Width="60px"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOnOwnerBudgetNameEdit" runat="server" Width="0px" CssClass="input"
                                        Enabled="False"></telerik:RadTextBox>
                                     <asp:LinkButton ID="btnShowOnOwnerBudgetEdit" runat="server" CommandArgument="<%# Container.DataSetIndex %>"  >
                                       <span class="icon"><i class="fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtOnOwnerBudgetIdEdit" runat="server" Width="0px" CssClass="input"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNEROWNERBUDGETCODEID") %>'></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOnOwnerBudgetgroupIdEdit" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                </span>

                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="100px">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblOffBudgetCodeHdr" runat="server" Text="Budget Code"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="lblOffBudgetCodeId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDBUDGETID")%>'></asp:Literal>
                                <asp:Literal ID="lblOffBudgetCode" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERSUBACCOUNT")%>'></asp:Literal>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="130px">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Literal ID="lblOffOwnerBudgetcodeHdr" runat="server" Text="Owner Budget Code"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="lblOffOwnerBudgetcode" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDOFFSIGNEROWNERBUDGETCODE")%>'></asp:Literal>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListOffOwnerBudgetEdit">
                                    <telerik:RadTextBox ID="txtOffOwnerBudgetCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNEROWNERBUDGETCODE") %>'
                                        MaxLength="20" CssClass="input" Width="60px"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOffOwnerBudgetNameEdit" runat="server" Width="0px" CssClass="input"
                                        Enabled="False"></telerik:RadTextBox>
                                    <asp:LinkButton ID="btnShowOffOwnerBudgetEdit" runat="server" CommandArgument="<%# Container.DataSetIndex %>"  >
                                       <span class="icon"><i class="fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtOffOwnerBudgetIdEdit" runat="server" Width="0px" CssClass="input"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNEROWNERBUDGETCODEID") %>'></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOffOwnerBudgetgroupIdEdit" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                       <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                            <HeaderStyle Width="80px" />
                            <ItemStyle Width="20px" Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                              </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                           <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
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
