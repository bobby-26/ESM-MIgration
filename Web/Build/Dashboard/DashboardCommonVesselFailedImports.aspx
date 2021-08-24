<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardCommonVesselFailedImports.aspx.cs"
    Inherits="DashboardCommonVesselFailedImports" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DashboardMenu" Src="~/UserControls/UserControlDashboardMenu.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Failed Imports</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">     
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
          <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">   
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <telerik:RadGrid ID="gvFolderList" runat="server" AutoGenerateColumns="false" EnableViewState="false" CellSpacing="0" GridLines="None"
                        Height="88%" OnNeedDataSource="gvFolderList_NeedDataSource" OnItemDataBound="gvFolderList_ItemDataBound" AllowSorting="true"
                         EnableHeaderContextMenu="true"  >

                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed">
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
                                <telerik:GridTemplateColumn HeaderText="Vessel Name" SortExpression="FLDVESSELNAME" AllowSorting="true" HeaderStyle-HorizontalAlign="Left">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblScheduledJobID" runat="server" Visible="false"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-HorizontalAlign="Left" AllowSorting="true" SortExpression="FLDTYPE">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>                                      
                                      <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Seq. No" HeaderStyle-HorizontalAlign="Left" AllowSorting="true" SortExpression="FLDENTITYSERIAL">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSeqNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENTITYSERIAL") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-HorizontalAlign="Left" AllowSorting="true" SortExpression="FLDLASTRUNOK">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lblLastRunOk" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTRUNOK") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>           
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
