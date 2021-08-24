<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsRemittanceBatchDownLoadHistory.aspx.cs"
    Inherits="AccountsRemittanceBatchDownLoadHistory" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="YesNo" Src="~/UserControls/UserControlYesNo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlBankAccount" Src="~/UserControls/UserControlBankAccount.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
              <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvRemittence.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPurchaseForm" runat="server" autocomplete="off">
      <telerik:RadScriptManager runat="server" ID="RadScriptManager1"/>
    <telerik:RadAjaxPanel runat="server" ID="pnlRemittance">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
<%--                    <eluc:Title runat="server" ID="frmTitle" Text="Remittance"></eluc:Title>--%>
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                 <%--   <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>--%>
                    <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand">
                    </eluc:TabStrip>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvRemittence" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnItemCommand="gvRemittence_RowCommand" OnItemDataBound="gvRemittence_ItemDataBound"
                        OnDeleteCommand="gvRemittence_RowDeleting" GroupingEnabled="false" EnableHeaderContextMenu="true" OnNeedDataSource="gvRemittence_NeedDataSource"
                        AllowSorting="true"  EnableViewState="false" AllowPaging="true" AllowCustomPaging="true"
                        OnSortCommand="gvRemittence_Sorting" >
                       <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDBATCHID">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Batch Number" AllowSorting="true" SortExpression="FLDBATCHNUMBER">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                           <%--     <HeaderTemplate>
                                    <telerik:RadLabel ID="lblRemittenceNumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDBATCHNUMBER"
                                        ForeColor="White">Batch Number&nbsp;</telerik:RadLabel>
                                    <img id="FLDBATCHNUMBER" runat="server" visible="false" />
                                </HeaderTemplate>--%>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lnkBatchId" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItem%>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHNUMBER")  %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Revision Number" AllowSorting="true" SortExpression="FLDREVISIONNUMBER">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                             <%--   <HeaderTemplate>
                                    <telerik:RadLabel ID="lblRevisionNumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDBATCHNUMBER"
                                        ForeColor="White">Revision Number&nbsp;</telerik:RadLabel>
                                </HeaderTemplate>--%>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRevisionNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONNUMBER")  %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Generated date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               <ItemTemplate>
                                    <telerik:RadLabel ID="lblGeneratedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Genereated By">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                 <ItemTemplate>
                                    <telerik:RadLabel ID="lblGenereatedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDUSERNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Payment Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                 <ItemTemplate>
                                    <telerik:RadLabel ID="lblPaymentDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="No.of Instructions">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               <ItemTemplate>
                                    <telerik:RadLabel ID="lblAccountCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFINSTRUCTION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Download File">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                 <ItemTemplate>
                                    <telerik:RadLabel ID="lblFileName" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILENAME") %>'> </telerik:RadLabel>
                                    <asp:HyperLink ID="lnkfilename" Target="_blank" Text="View" runat="server" Width="14px"
                                        Height="14px" ToolTip="Download File">
                                        <telerik:RadLabel ID="lblFilePath" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILEPATH") %>'></telerik:RadLabel>
                                    </asp:HyperLink></ItemTemplate>
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
