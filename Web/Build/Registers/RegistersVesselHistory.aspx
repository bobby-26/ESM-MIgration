<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVesselHistory.aspx.cs" Inherits="RegistersVesselHistory" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>History</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript">
            
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvVesselHistory.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;
            function pageLoad() {
                Resize();
            }
         </script>
    </telerik:RadCodeBlock>
</head>
<body>

    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersVesselList" DecoratedControls="All" />
    <form id="frmRegistersVesselList" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuVesselList" runat="server" OnTabStripCommand="MenuVesselList_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table id="tblConfigureVesselList" width="100%">
                <tr>
                    <td>Vessel Name
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVesselName" runat="server" MaxLength="100" CssClass="readonlytextbox" ReadOnly="true" Width="360px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuRegistersVesselList" runat="server" OnTabStripCommand="RegistersVesselList_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvVesselHistory" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" OnItemCommand="gvVesselHistory_ItemCommand" OnItemDataBound="gvVesselHistory_ItemDataBound" EnableViewState="false"
                ShowFooter="false" ShowHeader="true" OnNeedDataSource="gvVesselHistory_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" CommandItemDisplay="Top">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="W.E.F.">

                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWEF" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWEF") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Valid Until">

                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblValidUntil" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALIDUNTIL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks">

                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString() %>'>
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucRemarksTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amended By">

                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmendedBy" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDCREATEDBYNAME").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDCREATEDBYNAME").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDCREATEDBYNAME").ToString() %>'>
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucAmendedByTT" runat="server" Text='<%# String.Concat( String.Concat(Eval("FLDCREATEDBYNAME")," - "),Eval("FLDMODIFIEDDATE")) %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name">

                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Flag">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFlag" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Management">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblManagement" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANAGEMENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Owner">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOwner" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDOWNERNAME").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDOWNERNAME").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDOWNERNAME").ToString() %>'>
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucOwnerTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Principal">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPrincipal" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDPRINCIPALNAME").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDPRINCIPALNAME").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDPRINCIPALNAME").ToString() %>'>
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucPrincipalTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRINCIPALNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Manager">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblManager" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDMANAGERNAME").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDMANAGERNAME").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDMANAGERNAME").ToString() %>'>
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucManagerTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANAGERNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Officer" ColumnGroupName="WageScale">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWageScale" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDWAGESCALENAME").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDWAGESCALENAME").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDWAGESCALENAME").ToString() %>'>
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucWageScaleTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAGESCALENAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Ratings" ColumnGroupName="WageScale">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRatingsWageScale" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDWAGESCALERATINGNAME").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDWAGESCALERATINGNAME").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDWAGESCALERATINGNAME").ToString() %>'>
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucRatingsWageScaleTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAGESCALERATINGNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Seniority" ColumnGroupName="WageScale">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSeniorityWageScale" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDSENIORITYSCALENAME").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDSENIORITYSCALENAME").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDSENIORITYSCALENAME").ToString() %>'>
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucSeniorityWageScaleTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSENIORITYSCALENAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Std Component">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblESMStdComp" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDSTANDARDWAGECODENAME").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDSTANDARDWAGECODENAME").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDSTANDARDWAGECODENAME").ToString() %>'>
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucESMStdCompTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTANDARDWAGECODENAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active/In-Active">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActive" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVE") %>'></telerik:RadLabel>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="5" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" ResizeGridOnColumnResize="false" />
                </ClientSettings>
            </telerik:RadGrid>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
