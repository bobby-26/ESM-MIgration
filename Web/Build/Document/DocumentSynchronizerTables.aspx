<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentSynchronizerTables.aspx.cs" Inherits="Document_DocumentSynchronizerTables" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MODULE VESSEL LIST</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
     <form id="form1" runat="server" enableviewstate="true">
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
   
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
         
            <table width="100%" cellspacing="5">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblModules" runat="server" Text="Modules"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlModules" runat="server" CssClass="input" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlModules_SelectedIndexChanged" Width="250px">
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTables" runat="server" Text="Table"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlTables" runat="server" CssClass="input" Width="250px"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlTables_SelectedIndexChanged">
                        </telerik:RadDropDownList>

                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvVesselList" Height="92%" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" OnItemCommand="gvVesselList_ItemCommand" OnNeedDataSource="gvVesselList_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="true" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
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
                        <telerik:GridTemplateColumn>
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                               <%-- <telerik:RadCheckBox ID="ChkSelectVessel" runat="server"
                                    AutoPostBack="true" OnCheckedChanged="ChkSelectVessel_ChekedChange" Checked='<%# bool.Parse(Eval("FLDSTATUS").ToString() == "1" ? "True":"False") %>' />--%>

                                 
                             <Telerik:RadcheckBox runat="server" ID="ChkSelectVessel" CommandName="UPDATE" Checked='<%# bool.Parse(Eval("FLDSTATUS").ToString() == "1" ? "True":"False") %>'
                                        AutoPostBack="true" EnableViewState="true"/>
                            
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Vessel Name">
                            <HeaderStyle Width="80%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>' Visible="false"></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                    </Columns>
                
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

