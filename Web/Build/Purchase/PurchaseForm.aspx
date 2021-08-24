<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseForm.aspx.cs" Inherits="PurchaseForm" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MoreLink" Src="~/UserControls/UserControlMoreLinks.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchase Form</title>

    <style type="text/css">
        .datagrid_selectedstyle1{
        color:Black;height:10px;

    }
        .odd{background-color: white;} 
        .even{background-color: gray;} 
    </style>

    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
         <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
        function copyrequisition(args) {
            if (args) {
                __doPostBack("<%=copy.UniqueID %>", "");
            }
        }
        function PaneResized(sender, args) {
            var splitter = $find('RadSplitter1');
            var browserHeight = $telerik.$(window).height();
            splitter.set_height(browserHeight - 40);
            splitter.set_width("100%");
            var grid = $find("rgvForm");           
            var contentPane = splitter.getPaneById("listPane");
            grid._gridDataDiv.style.height = (contentPane._contentElement.offsetHeight - 96) + "px";
            //console.log(grid._gridDataDiv.style.height, contentPane._contentElement.offsetHeight);
        }
        function pageLoad() {
            PaneResized();
        }
    </script>
    </telerik:RadCodeBlock>
    
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmPurchaseForm" runat="server" autocomplete="off">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No"></telerik:RadWindowManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ifMoreInfo">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ifMoreInfo" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rgvForm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgvForm" />
                        <telerik:AjaxUpdatedControl ControlID="ifMoreInfo" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuOrderFormMain">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuOrderFormMain" />
                        <telerik:AjaxUpdatedControl ControlID="rgvForm" />
                        <telerik:AjaxUpdatedControl ControlID="ifMoreInfo" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ucConfirm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgvForm" />
                        <telerik:AjaxUpdatedControl ControlID="ucConfirm" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls> 
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuOrderForm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuOrderForm"/>
                    </UpdatedControls> 
                </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
         <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>
            
            <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    <asp:Button ID="copy" runat="server" Text="copy" OnClick="copy_Click" />
                    <asp:TextBox ID="lblorderId" runat ="server" ></asp:TextBox>

            <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
                    <telerik:RadPane ID="generalPane" runat="server" Scrolling="None">
                        <iframe runat="server" id="ifMoreInfo" scrolling="no" style="min-height: 600px; width: 100%" frameborder="0"></iframe>
                    </telerik:RadPane>
                    <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both"></telerik:RadSplitBar>
                    <telerik:RadPane ID="listPane" runat="server" Scrolling="None" OnClientResized="PaneResized">
                        <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand"></eluc:TabStrip>

                        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="rgvForm" DecoratedControls="All" EnableRoundedCorners="true" />
                        <telerik:RadGrid RenderMode="Lightweight" ID="rgvForm" runat="server" AllowCustomPaging="true" AllowSorting="true"  AllowPaging="true" EnableHeaderContextMenu="true"
                    CellSpacing="0" GridLines="None" OnDeleteCommand="rgvForm_DeleteCommand" OnSortCommand="rgvForm_SortCommand" GroupingEnabled="false" 
                    OnNeedDataSource="rgvForm_NeedDataSource" OnPreRender="rgvForm_PreRender" OnInsertCommand="rgvForm_InsertCommand"
                    OnItemDataBound="rgvForm_ItemDataBound" OnItemCommand="rgvForm_ItemCommand" OnUpdateCommand="rgvForm_UpdateCommand">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDORDERID,FLDVESSELID,FLDSTOCKTYPE,FLDFORMNO">
                <%--<HeaderStyle Width="102px" />--%>
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Number" UniqueName="FORMNO" SortExpression="FLDFORMNO" AllowSorting="true">
                        <HeaderStyle Width="120px" />
                        <ItemStyle Width="120px" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkFormNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>' CommandName="SELECT"></asp:LinkButton>
                            <asp:Image ID="imgPriority" runat="server" ImageUrl="~/css/Theme1/images/red-symbol.png"></asp:Image>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Title" UniqueName="TITLE" AllowSorting="true" SortExpression="FLDTITLE">
                        <HeaderStyle Width="180px" />
                        <ItemStyle Width="180px" />
                        <ItemTemplate>                           
                            <telerik:RadLabel ID="lblTitle" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Vendor" AllowSorting="true" SortExpression="FLDVENDORNAME">
                        <HeaderStyle Width="180px" />
                        <ItemStyle Width="180px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVendor" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDVENDORNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Type" AllowSorting="true" SortExpression="FLDFORMTYPENAME">
                        <%--<HeaderStyle Width="100px" />
                        <ItemStyle Width="100px" />--%>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFormType" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFORMTYPENAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="true" SortExpression="FLDFORMSTATUSNAME">
                        <%--<HeaderStyle Width="100px" />
                        <ItemStyle Width="100px" />--%>
                                        <ItemTemplate>
                                             <telerik:RadLabel ID="lblStatusId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMSTATUS") %>' Visible="false"></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblStatus" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFORMSTATUSNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Budget" UniqueName="Budget" AllowSorting="true" SortExpression="FLDSUBACCOUNT">
                                        <%--<HeaderStyle Width="60px" />
                                        <ItemStyle Width="60px" />--%>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblBudget" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'></telerik:RadLabel>
                                            <asp:Label ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                            <asp:Label ID="lblStockId" runat="server" Visible ="false"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKCLASSID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Approved" AllowSorting="true" SortExpression="FLDPURCHASEAPPROVEDATE">
                        <%--<HeaderStyle Width="80px" />
                        <ItemStyle Width="80px" />--%>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblApproved" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDPURCHASEAPPROVEDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Ordered" AllowSorting="true" SortExpression="FLDORDEREDDATE">
                                        <%--<HeaderStyle Width="80px" />
                                        <ItemStyle Width="80px" />--%>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblOrdered" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDORDEREDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Received" AllowSorting="true" SortExpression="FLDVENDORDELIVERYDATE">
                                        <%--<HeaderStyle Width="80px" />
                                        <ItemStyle Width="80px" />--%>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblReceived" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDVENDORDELIVERYDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Committed $">
                        <HeaderStyle Width="100px" />
                        <ItemStyle HorizontalAlign="Right" Width="100px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCommitted" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOMMITTEDUSD","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Type" UniqueName="stock_type">
                        <%--<HeaderStyle Width="60px" />
                        <ItemStyle Width="60px" />--%>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblType" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSTOCKTYPE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Req Staus" UniqueName="Requisition_Status">
                        <%--<HeaderStyle Width="120px" />
                        <ItemStyle Width="120px" />--%>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblReqStatus" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <%--<telerik:GridTemplateColumn HeaderText="PO Staus" UniqueName="PO_Status">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblPOStatus" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDPOSTATUS") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Reason" UniqueName="Reason">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblReason" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDREASONFORREQUISITIONNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>--%>

                    <%--<telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn"  ConfirmText="Delete this City?" ConfirmDialogType="RadWindow">
                                        <HeaderStyle Width="20px" />
                                    </telerik:GridButtonColumn>--%>
                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                        <HeaderStyle Width="70px" />
                        <ItemStyle Width="70px" />
                        <ItemTemplate >
                                     <asp:LinkButton runat="server" AlternateText="Vessel Receipt Report" CommandName="VESSELRECEIPT" 
                                        ID="cmdVesselReceipt" ToolTip="Vessel Receipt Report">
                                         <span class="icon"><i class="fas fa-receipt"></i></span>
                                     </asp:LinkButton>
                                     <asp:LinkButton runat="server" AlternateText="Select" 
                                        CommandName="COPY" ID="cmdCopy" ToolTip="Copy requisition to another vessel">
                                         <span class="icon"><i class="fas fa-copy"></i></span>
                                     </asp:LinkButton>
                             <asp:LinkButton runat="server" AlternateText="Return" CommandName="RETURN" ID="cmdReturn" ToolTip="Return">
                                     <span class="icon"><i class="fa fa-share-square-24"></i></span>
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
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Requisitions matching your search criteria"
                    PageSizeLabelText="Requisitions per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" EnablePostBackOnRowClick="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="4" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                <%--<ClientEvents OnGridCreated="GridCreated" />--%>
            </ClientSettings>
        </telerik:RadGrid>
                    </telerik:RadPane>
                </telerik:RadSplitter>
                <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="CopyForm_Click" OKText="Yes"
                    CancelText="No" />
            </div>
    </form>
</body>
</html>
