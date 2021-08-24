<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceDailyWorkPlanMeeting.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceDailyWorkPlanMeeting" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function CloseModelWindow() {
                var radwindow = $find('<%=modalPopup.ClientID %>');
                radwindow.close();

                var wnd = getRadWindow('dsd');
                var button = wnd.GetContentFrame().contentWindow.document.getElementById("cmdHiddenSubmit");
                if (button != null)
                    button.click();
                wnd = getRadWindow('dp');
                setRadWindowZIndex(wnd);
                wnd.setActive(true);
                wnd = getRadWindow('codehelp');
                setRadWindowZIndex(wnd);
                wnd.setActive(true);

                var masterTable = $find('<%=gvToolBoxList.ClientID %>').get_masterTableView();
                masterTable.rebind();                
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuMain" runat="server" OnTabStripCommand="MenuMain_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid ID="gvToolBoxList" runat="server" OnNeedDataSource="gvToolBoxList_NeedDataSource"
            Width="100%" EnableHeaderContextMenu="true">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDDAILYWORKPLANMEETINGID">
                <HeaderStyle Width="102px" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Date & Time" HeaderStyle-Width="76px" AllowSorting="true" ShowFilterIcon="false"
                        ShowSortIcon="true" SortExpression="FLDDATEANDTIME" DataField="FLDDATEANDTIME" FilterDelay="200">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%#  General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATETIME"), DateDisplayOption.DateTime) %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="PIC">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"Dataitem.FLDPERSONINCHARGENAME") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Others">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDOTHERMEMBERSNAME").ToString().Trim(',') %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Notes">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblNotes" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOTES") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>                    
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
        <telerik:RadWindow runat="server" ID="modalPopup" Width="600px" Height="365px"
            Modal="true" OffsetElementID="main" VisibleStatusbar="false" KeepInScreenBounds="true">
        </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server" ID="rdbScripts">
            <script type="text/javascript">
                $modalWindow.modalWindowID = "<%=modalPopup.ClientID %>";                
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
