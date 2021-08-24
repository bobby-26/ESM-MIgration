<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRegulationComplianceStatus.aspx.cs" Inherits="Inspection_InspectionRegulationComplianceStatus" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Regulation Compliance Status</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
         <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

         <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
        <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
        <telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">
            <eluc:TabStrip ID="MenuDivRegulation" runat="server" OnTabStripCommand="plan_TabStripCommand"></eluc:TabStrip>
        </telerik:RadCodeBlock>

           <%-- For Popup Relaod --%>
         <asp:Button ID="cmdHiddenSubmit" runat="server" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />  


        <telerik:RadGrid RenderMode="Lightweight" ID="gvRegulation" runat="server" Height="93%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None"
            OnPreRender="gvRegulation_PreRender"
            OnNeedDataSource="gvRegulation_NeedDataSource"
            OnItemCommand="gvRegulation_ItemCommand">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
                <NoRecordsTemplate>
                     <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                 </NoRecordsTemplate>

                <Columns>

                    <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="true" UniqueName="Vessel">
                        <HeaderStyle Width="125px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVessel" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
        
                    <telerik:GridTemplateColumn HeaderText="Task" AllowSorting="true">
                        <HeaderStyle Width="125px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblTask" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIONTOBETAKEN") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                     <telerik:GridTemplateColumn HeaderText="Target Date" Visible="true" AllowSorting="true">
                        <HeaderStyle Width="140px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblTargetDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTARGETDATE","{0:dd-MM-yyyy}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                     <telerik:GridTemplateColumn HeaderText="Completion Date" Visible="true" AllowSorting="true">
                        <HeaderStyle Width="140px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCopletionDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE","{0:dd-MM-yyyy}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Closed Date" Visible="true" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCLOSEDDATE">
                        <HeaderStyle Width="140px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblClosedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLOSEDDATE","{0:dd-MM-yyyy}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="true">
                        <HeaderStyle Width="125px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblStatus" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                  
                <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>