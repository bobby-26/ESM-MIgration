<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMOCQuestionList.aspx.cs"
    Inherits="InspectionMOCQuestionList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmApprovalRemarks" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblGeneric" runat="server"
        DecoratedControls="All" EnableRoundedCorners="true" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
        Height="100%" EnableAJAX="false">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuMOC" runat="server" OnTabStripCommand="MOC_TabStripCommand">
        </eluc:TabStrip>
        <telerik:RadFormDecorator ID="rfdinstruction" RenderMode="LightWeight" runat="server"
            DecoratedControls="All" EnableRoundedCorners="true" DecorationZoneID="divinstruction">
        </telerik:RadFormDecorator>
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" Visible="false" />
        <telerik:RadLabel ID="lblmocquestion" runat="server" />
        <telerik:RadGrid ID="gvMOCQuestion" runat="server" AutoGenerateColumns="False" OnNeedDataSource="gvMOCQuestion_NeedDataSource"
            Font-Size="11px" Width="100%" CellPadding="3" AllowPaging="false" AllowCustomPaging="false"
            ShowFooter="false" EnableHeaderContextMenu="true" GroupingEnabled="false" ShowHeader="true"
            EnableViewState="true" AllowSorting="true" Height="40%">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                AllowNaturalSort="false" AutoGenerateColumns="false">
                <NoRecordsTemplate>
                    <table id="Table1" runat="server" width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                                    Font-Bold="true">
                                </telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Question">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblQuestionid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCHARDCODE") %>'>
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblQuestionName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCHARDNAME") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Select">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadCheckBox ID="chkRequiredYNEdit" runat="server" AutoPostBack="false" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
