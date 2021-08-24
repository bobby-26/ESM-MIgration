 <%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementDocumentListByVessel.aspx.cs" Inherits="DocumentManagementDocumentListByVessel" %>

<%@ Import Namespace="SouthNests.Phoenix.DocumentManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Document List By Vessel</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/Fonts/fontawesome/css/all.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script language="javascript" type="text/javascript">
            function resizeDiv(obj) {
                // var obj = document.getElementById("divGrid");
                obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 120 + "px";
            }
        </script>
        <script type="text/javascript">
            function fnConfirmTelerik(sender, msg) {
                var callBackFn = function (shouldSubmit) {
                    if (shouldSubmit) {
                        //sender.click();
                        //if (Telerik.Web.Browser.ff) {
                        //    sender.get_element().click();
                        //}
                        eval(sender.target.parentElement.parentElement.href);
                    }
                    else {
                        if (e.which)
                            e.stopPropagation();
                        else
                            window.event.cancelBubble = true;
                        return false;
                    }
                }
                var confirm;

                if (msg == null)
                    confirm = radconfirm("Are you sure you want to delete this record?", callBackFn);
                else
                    confirm = radconfirm(msg, callBackFn);

                return false;
            }
        </script>

        <script type="text/javascript" language="javascript">

            // Maintain scroll position on list box.                 
            function setScrollPosition(divname, hdnname) {
                var div = $get(divname);
                var hdn = $get(hdnname);
                hdn.value = div.scrollTop;
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            }

            function EndRequestHandler(sender, args) {
                var listBox = $get('<%= divDocument.ClientID %>');
                var hdn = $get('<%= hdnDocumentScroll.ClientID %>');
                listBox.scrollTop = hdn.value;

                listBox = $get('<%= divRiskAssessment.ClientID %>');
                hdn = $get('<%= hdnRiskAssessmentScroll.ClientID %>');
                listBox.scrollTop = hdn.value;
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body onresize="resizeDiv(divDocument);resizeDiv(divRiskAssessment);" onload="resizeDiv(divDocument);resizeDiv(divRiskAssessment);">
    <form id="frmDocumentByVessel" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="RadAjaxPanel1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuDocument" runat="server" OnTabStripCommand="MenuDocument_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <table id="tblDocument" width="80%" align="center" runat="server">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselname" runat="server" Text="Vessel" Font-Bold="true"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                            CssClass="input" OnTextChangedEvent="ucVessel_changed" Width="180px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Total Documents distributed to " Font-Bold="true"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="lblGrandTotal" runat="server" Text="0" ReadOnly="true" Font-Bold="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbljhara" runat="server" Text="JHA / RA" Font-Bold="true"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblRA" runat="server" Columns="2" Direction="Vertical" Font-Bold="true" OnSelectedIndexChanged="rblRA_SelectedIndexChanged" AutoPostBack="true" >
                            <Items>
                                <telerik:ButtonListItem Text="Old" Value="0" Selected="True" />
                                <telerik:ButtonListItem Text="New" Value="1" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
            </table>
            <table width="100%" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="40%" colspan="2">
                        <eluc:TabStrip ID="MenuDocumentList" runat="server" TabStrip="false" OnTabStripCommand="MenuDocumentList_TabStripCommand"></eluc:TabStrip>
                    </td>
                    <td width="60%" colspan="2">
                        <eluc:TabStrip ID="MenuRiskAssessment" runat="server" TabStrip="false" OnTabStripCommand="MenuRiskAssessment_TabStripCommand" />
                    </td>
                </tr>
                <tr>
                    <td width="40%" colspan="2">
                        <asp:HiddenField ID="hdnDocumentScroll" runat="server" />
                        <div runat="server" id="divDocument" style="position: relative; z-index: 0; overflow: auto; height: 450px;"
                            onscroll="javascript:setScrollPosition('divDocument','hdnDocumentScroll');">
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvDocumentByVessel" runat="server" AutoGenerateColumns="False"
                                Font-Size="11px" Width="100%" CellPadding="3" GridLines="None" OnNeedDataSource="gvDocumentByVessel_NeedDataSource"
                                OnItemCommand="gvDocumentByVessel_ItemCommand"
                                OnItemDataBound="gvDocumentByVessel_ItemDataBound" ShowFooter="true" ShowHeader="true"
                                EnableViewState="false" AllowSorting="true">
                                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                                    AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="None" DataKeyNames="FLDCATEGORYID">
                                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Category" HeaderStyle-Width="70%">
                                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="70%"></ItemStyle>
                                            <HeaderTemplate>
                                                Category
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblCategoryId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>' Visible="false"></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                Total:
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Count" HeaderStyle-Width="30%">
                                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="30%"></ItemStyle>
                                            <HeaderTemplate>
                                                Count
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDocumentCount" runat="server" CommandName="Sort" CommandArgument="FLDDOCUMENTCOUNT"
                                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTCOUNT") %>'></asp:LinkButton>
                                                <telerik:RadLabel ID="lblCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTCOUNT") %>' Visible="false"></telerik:RadLabel>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadLabel ID="lblTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGRANDTOTAL") %>'></telerik:RadLabel>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" AllowDragToGroup="false">
                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" SaveScrollPosition="true" FrozenColumnsCount="7" EnableNextPrevFrozenColumns="true" />
                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </div>
                    </td>
                    <td width="60%" colspan="2">
                        <asp:HiddenField ID="hdnRiskAssessmentScroll" runat="server" />
                        <div runat="server" id="divRiskAssessment" style="position: relative; z-index: +2; overflow: auto; height: 450px;"
                            onscroll="javascript:setScrollPosition('divRiskAssessment','hdnRiskAssessmentScroll');">
                            <telerik:RadGrid ID="gvRiskAssessment" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" GridLines="None"
                                OnNeedDataSource="gvRiskAssessment_NeedDataSource" OnItemCommand="gvRiskAssessment_ItemCommand"
                                OnRowDataBound="gvRiskAssessment_RowDataBound" OnItemDataBound="gvRiskAssessment_ItemDataBound"
                                ShowFooter="true" Style="margin-bottom: 0px" EnableViewState="false">
                                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                                    AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="None" DataKeyNames="FLDACTIVITYID">
                                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                                    <Columns>                                       
                                        <telerik:GridTemplateColumn HeaderText="Category" HeaderStyle-Width="23%" FooterStyle-HorizontalAlign="Center">
                                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="23%"></ItemStyle>
                                            <HeaderTemplate>
                                                Activity
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblAcitivityID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITYID") %>'
                                                    Visible="false">
                                                </telerik:RadLabel>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                Total:
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="JHA" FooterStyle-HorizontalAlign="Center">
                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            <HeaderTemplate>
                                                JHA
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkJHACount" runat="server" CommandName="Sort" CommandArgument="FLDDOCUMENTCOUNT"
                                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDJHACOUNT") %>'></asp:LinkButton>
                                                <telerik:RadLabel ID="lblJHA" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJHACOUNT") %>' Visible="false"></telerik:RadLabel>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadLabel ID="lblJHATotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJHATOTAL") %>'></telerik:RadLabel>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Process" FooterStyle-HorizontalAlign="Center">
                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            <HeaderTemplate>
                                                Process
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkProcessCount" runat="server" CommandName="Sort" CommandArgument="FLDPROCESSCOUNT"
                                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCESSCOUNT") %>'></asp:LinkButton>
                                                <telerik:RadLabel ID="lblProcess" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCESSCOUNT") %>' Visible="false"></telerik:RadLabel>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadLabel ID="lblProcessTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCESSTOTAL") %>'></telerik:RadLabel>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Generic" FooterStyle-HorizontalAlign="Center">
                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            <HeaderTemplate>
                                                Generic
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkGenericCount" runat="server" CommandName="Sort" CommandArgument="FLDGENERICCOUNT"
                                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDGENERICCOUNT") %>'></asp:LinkButton>
                                                <telerik:RadLabel ID="lblGeneric" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGENERICCOUNT") %>' Visible="false"></telerik:RadLabel>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadLabel ID="lblGenericTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGENERICTOTAL") %>'></telerik:RadLabel>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Machinery" FooterStyle-HorizontalAlign="Center">
                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            <HeaderTemplate>
                                                Machinery
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkMachineryCount" runat="server" CommandName="Sort" CommandArgument="FLDMACHINERYCOUNT"
                                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDMACHINERYCOUNT") %>'></asp:LinkButton>
                                                <telerik:RadLabel ID="lblMachinery" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMACHINERYCOUNT") %>' Visible="false"></telerik:RadLabel>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadLabel ID="lblMachineryTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMACHINERYTOTAL") %>'></telerik:RadLabel>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Navigation" FooterStyle-HorizontalAlign="Center">
                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            <HeaderTemplate>
                                                Navigation
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkNavigationCount" runat="server" CommandName="Sort" CommandArgument="FLDNAVIGATIONCOUNT"
                                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAVIGATIONCOUNT") %>'></asp:LinkButton>
                                                <telerik:RadLabel ID="lblNavigation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAVIGATIONCOUNT") %>' Visible="false"></telerik:RadLabel>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadLabel ID="lblNavigationTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAVIGATIONTOTAL") %>'></telerik:RadLabel>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Cargo" FooterStyle-HorizontalAlign="Center">
                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            <HeaderTemplate>
                                                Cargo
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkCargoCount" runat="server" CommandName="Sort" CommandArgument="FLDCARGOCOUNT"
                                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOCOUNT") %>'></asp:LinkButton>
                                                <telerik:RadLabel ID="lblCargo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOCOUNT") %>' Visible="false"></telerik:RadLabel>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadLabel ID="lblCargoTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOTOTAL") %>'></telerik:RadLabel>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Count" FooterStyle-HorizontalAlign="Center">
                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            <HeaderTemplate>
                                                Count
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblTotalCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTAL") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadLabel ID="lblTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGRANDTOTAL") %>'></telerik:RadLabel>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" AllowDragToGroup="false">
                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" SaveScrollPosition="true" FrozenColumnsCount="7" EnableNextPrevFrozenColumns="true" />
                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </div>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
