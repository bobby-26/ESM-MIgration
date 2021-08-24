<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsMissingVesselSignOff.aspx.cs"
    Inherits="VesselAccountsMissingVesselSignOff" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignOffReason" Src="~/UserControls/UserControlSignOffReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel Sign-Off Missing List
    </title><%--(But It's there in Office SignOff List (tblemployeesignonoff))--%>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="frmRegistersRank" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>      <eluc:Status ID="Ucsstats" runat="server" />
            <asp:Button ID="Button1" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuCrewAdmin" runat="server" OnTabStripCommand="CrewAdmin_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSignOff" Height="95%" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" OnItemCommand="gvSignOff_ItemCommand1" EnableViewState="false" OnItemDataBound="gvSignOff_ItemDataBound"
                ShowFooter="false" ShowHeader="true" OnNeedDataSource="gvSignOff_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDSIGNONOFFID">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="File No.">
                            <HeaderStyle Width="6%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                 <telerik:RadLabel ID="lblsignonoffid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONOFFID") %>'></telerik:RadLabel>
                                <%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Employee">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblEmployeeid" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDEMPLOYEEID"]%>'></asp:Label>
                                <asp:Label ID="lblSignonVesselid" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNONVESSELID"]%>'></asp:Label>
                                <asp:Label ID="lblStatus" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSTATUS"]%>'></asp:Label>
                                <asp:LinkButton ID="lnkEployeeName" runat="server" 
                                    Text='<%# ((DataRowView)Container.DataItem)["FLDNAME"]%>' CommandName="SELECT"></asp:LinkButton>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign On Rank">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDSIGNONRANKNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Passport No">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate> <%# DataBinder.Eval(Container, "DataItem.FLDPASSPORTNO")%>
                                
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CDC No">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate><%# DataBinder.Eval(Container, "DataItem.FLDSEAMANBOOKNO")%>
                                
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign On">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# string.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container, "DataItem.FLDSIGNONDATE")) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Relief Due">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>  <%# string.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE")) %>
                                
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign Off">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblSignOffDate" runat="server" Text='<%# string.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container, "DataItem.FLDSIGNOFFDATE")) %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign Off">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblSeaPort" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNOFFSEAPORTID"]%>'></asp:Label>
                                <asp:Label ID="lblSignOffDate" runat="server" Text='<%# string.Format("{0:dd/MM/yyyy}", ((DataRowView)Container.DataItem)["FLDSIGNOFFDATE"]) %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign Off Reason">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblReason" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNOFFREASONID"]%>'></asp:Label>
                                <%# ((DataRowView)Container.DataItem)["FLDSIGNOFFREASON"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" >
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="ADD" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
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
