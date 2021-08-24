<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListComponentJob.aspx.cs"
    Inherits="CommonPickListComponentJob" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlConfirmMessage.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Component Job</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%= btnConfirm.UniqueID %>", "");
                }

            }
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvComponentJob.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvComponentJob">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvComponentJob" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="ucConfirm" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ucConfirm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ucConfirm" />
                        <telerik:AjaxUpdatedControl ControlID="gvComponentJob" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <eluc:TabStrip ID="MenuComponentType" runat="server" OnTabStripCommand="MenuComponentType_TabStripCommand"></eluc:TabStrip>

        <br clear="all" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

        <table>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblComponentNumber" runat="server" Text="Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtNumber" CssClass="input" runat="server"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblComponentName" runat="server" Text="Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtName" CssClass="input" Width="200px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lbljobcode" runat="server" Text="Job Code"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtjobcode" runat="server" CssClass="input"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lbljobtitle" runat="server" Text="Job Title"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtjobtitle" runat="server" CssClass="input" Width="200px"></telerik:RadTextBox>
                </td>
            </tr>
        </table>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvComponentJob" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadGrid RenderMode="Lightweight" ID="gvComponentJob" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnNeedDataSource="gvComponentJob_NeedDataSource"
            OnItemDataBound="gvComponentJob_ItemDataBound" OnItemCommand="gvComponentJob_ItemCommand" OnSortCommand="gvComponentJob_SortCommand">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDCOMPONENTJOBID">
                <Columns>
                    <telerik:GridTemplateColumn SortExpression="FLDCOMPONENTNUMBER">
                        <HeaderStyle Width="30px" />
                        <ItemTemplate>
                            <telerik:RadCheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Job Code" AllowSorting="true" SortExpression="FLDJOBCODE">
                        <HeaderStyle Width="70px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblJobCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBCODE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Job Title" AllowSorting="true" SortExpression="FLDJOBTITLE">
                        <HeaderStyle Width="200px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblComponentJobId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTJOBID") %>'></telerik:RadLabel>
                            <asp:LinkButton ID="lnkJobName" runat="server" CommandName="Select" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBTITLE") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Component No." AllowSorting="true" SortExpression="FLDCOMPONENTNUMBER">
                        <HeaderStyle Width="70px" />
                        <ItemTemplate>
                            <%#  DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Componet Name" AllowSorting="true" SortExpression="FLDCOMPONENTNAME">
                        <HeaderStyle Width="100px" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Frequency" AllowSorting="true" SortExpression="FLDFREQUENCYNAME">
                        <HeaderStyle Width="70px" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDFREQUENCYNAME") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Last Done Date" AllowSorting="true" SortExpression="FLDJOBLASTDONEDATE">
                        <HeaderStyle Width="70px" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDJOBLASTDONEDATE","{0:dd/MMM/yyyy}") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Priority" AllowSorting="true" SortExpression="FLDPRIORITY">
                        <HeaderStyle Width="50px" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDPRIORITY") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Resp Discipline" AllowSorting="true" SortExpression="FLDDISCIPLINENAME">
                        <HeaderStyle Width="100px" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDDISCIPLINENAME") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Next Due Date" AllowSorting="true" SortExpression="FLDJOBNEXTDUEDATE">
                        <HeaderStyle Width="70px" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDJOBNEXTDUEDATE","{0:dd/MMM/yyyy}") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" CssClass="RadGrid_Default rgPagerTextBox" PagerTextFormat="{4}<strong>{5}</strong> Records Found" AlwaysVisible="true"
                    PageSizeLabelText="Records per page:" />
            </MasterTableView>
            <ClientSettings EnablePostBackOnRowClick="true" EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <asp:Button ID="btnConfirm" runat="server" Text="confirm" OnClick="btnConfirm_Click" />
        <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" OnConfirmMesage="ucConfirm_OnClick" Visible="false" />
        <telerik:RadCodeBlock runat="server">
            <script type="text/javascript">
                Sys.Application.add_load(function () {
                    setTimeout(function () {
                        TelerikGridResize($find("<%= gvComponentJob.ClientID %>"));
                    }, 200);
                });
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
