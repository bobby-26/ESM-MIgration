<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreTrainingMatrixList.aspx.cs"
    Inherits="CrewOffshoreTrainingMatrixList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Common" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TrainingMatrix" Src="~/UserControls/UserControlTrainingMatrixStandard.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Query Activity</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
            }
        }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
         <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            
                <eluc:Status runat="server" ID="ucStatus" />
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <asp:Button ID="confirm" runat="server" OnClick="btnConfirm_Click" />
                <div id="divTable" runat="server">
                    <table id="tblTraining" rules="server" cellpadding="2" cellspacing="2" width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblVesselTypeTitle" runat="server" Text="Vessel Type"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:VesselType ID="ucVesselType" Width="300px"  runat="server"
                                    AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblRankTitle" runat="server" Text="Rank"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Rank ID="ucRank" runat="server"  Width="200px" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblCharterer" runat="server" Text="Charterer"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:TrainingMatrix ID="ucCharterer"  Width="200px" AppendDataBoundItems="true"
                                    runat="server" AddressType='<%# ((int)PhoenixAddressType.CHARTERER).ToString() %>' />
                                <%--<eluc:AddressType ID="ucCharterer" AutoPostBack="true" Width="200px" AppendDataBoundItems="true"
                                    runat="server"  AddressType='<%# ((int)PhoenixAddressType.CHARTERER).ToString() %>' />--%>
                            </td>
                        </tr>
                    </table>
                </div>

                <eluc:TabStrip ID="CrewTrainingMenu" runat="server" OnTabStripCommand="CrewTrainingMenu_TabStripCommand"></eluc:TabStrip>


                <%-- <asp:GridView ID="gvTrainingMatrix" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvTrainingMatrix_RowCommand" OnRowDataBound="gvTrainingMatrix_RowDataBound"
                        OnRowDeleting="gvTrainingMatrix_RowDeleting" ShowHeader="true" ShowFooter="false"
                        EnableViewState="false" AllowSorting="true" OnSorting="gvTrainingMatrix_Sorting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvTrainingMatrix" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvTrainingMatrix_NeedDataSource"
                    OnItemCommand="gvTrainingMatrix_ItemCommand"
                    OnItemDataBound="gvTrainingMatrix_ItemDataBound"
                    OnSortCommand="gvTrainingMatrix_SortCommand"
                  
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed">
                        <NoRecordsTemplate>
                            <table width="100%" border="0">
                                <tr>
                                    <td align="center">
                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </NoRecordsTemplate>
                        <HeaderStyle Width="102px" />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="S. No.">
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="30px"></ItemStyle>
                                <HeaderStyle Width="50px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblNo" runat="server" Width="30px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Width="400px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblMatrixId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMATRIXID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblMatrixName" Width="400px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMATRIXNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <%--  <FooterTemplate>
                                    <telerik:RadTextBox ID="txtMatrixNameAdd" Width="400px" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                                </FooterTemplate>--%>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="150px"></HeaderStyle>

                                <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Details"
                                        CommandName="DETAILS" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDetails"
                                        ToolTip="Details">
                                        <span class="icon"><i class="fas fa-table-72"></i></span>
                                    </asp:LinkButton>
                                  
                                    <asp:LinkButton runat="server" AlternateText="Edit"
                                        CommandName="NAVIGATEEDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                        ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>

                                    <asp:LinkButton runat="server" AlternateText="Delete"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                        ToolTip="Delete">
                                         <span class="icon"><i class="fa fa-trash"></i></span>
                                    </asp:LinkButton>
                                   
                                    <asp:LinkButton runat="server" AlternateText="Copy"
                                        CommandName="COPY" CommandArgument="<%# Container.DataSetIndex %>" ID="imgCopy"
                                        ToolTip="Copy">
                                        <span class="icon"><i class="fas fa-copy"></i></span>
                                    </asp:LinkButton>

                                    <asp:LinkButton runat="server" AlternateText="History"
                                        CommandName="HISTORY" CommandArgument="<%# Container.DataSetIndex %>" ID="imgHistory"
                                        ToolTip="History">
                                        <span class="icon"><i class="fas fa-check-square-showlist"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true"  ScrollHeight="415px" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>

              
                <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Localization-OK="Yes"
                    Localization-Cancel="No" Width="100%" />
       
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
