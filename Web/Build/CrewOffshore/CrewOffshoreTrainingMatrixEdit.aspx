<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreTrainingMatrixEdit.aspx.cs"
    Inherits="CrewOffshoreTrainingMatrixEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TrainingMatrix" Src="~/UserControls/UserControlTrainingMatrixStandard.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Training Matrix</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status ID="ucStatus" runat="server" />

                <eluc:TabStrip ID="CrewTrainingMenu" Title="Edit Training Matrix" runat="server" OnTabStripCommand="CrewTrainingMenu_TabStripCommand"></eluc:TabStrip>

                <div id="table" runat="server">
                    <table id="tblTraining" rules="server" cellpadding="2" cellspacing="2" width="100%">
                        <tr>
                            <td style="width: 20%">
                                <telerik:RadLabel ID="lblMatrixName" runat="server" Text="Matrix Name"></telerik:RadLabel>
                            </td>
                            <td colspan="4" style="width: 30%">
                                <telerik:RadTextBox ID="txtMatrixName" runat="server" CssClass="readonlytextbox" Enabled="false"
                                    Width="300px">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%">
                                <telerik:RadLabel ID="lblVesselTypeTitle" runat="server" Text="Vessel Type"></telerik:RadLabel>
                            </td>
                            <td colspan="4" style="width: 30%">
                                <eluc:VesselType ID="ucVesselType" Width="300px" runat="server" AutoPostBack="true"
                                    OnTextChangedEvent="setFilter" AppendDataBoundItems="true" CssClass="input_mandatory" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%">
                                <telerik:RadLabel ID="lblRankTitle" runat="server" Text="Rank"></telerik:RadLabel>
                            </td>
                            <td colspan="4" style="width: 30%">
                                <eluc:Rank ID="ucRank" runat="server" Width="200px" AutoPostBack="true" AppendDataBoundItems="true"
                                    OnTextChangedEvent="setFilter" CssClass="input_mandatory" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%">
                                <telerik:RadLabel ID="lblCharterer" runat="server" Text="Charterer"></telerik:RadLabel>
                            </td>
                            <td colspan="4" style="width: 30%">
                                <%--<eluc:AddressType ID="ucCharterer" Width="200px" AutoPostBack="true" AppendDataBoundItems="true"
                                    runat="server" OnTextChangedEvent="setFilter" CssClass="input_mandatory" AddressType='<%# ((int)PhoenixAddressType.CHARTERER).ToString() %>' />--%>
                                <eluc:TrainingMatrix ID="ucCharterer" AutoPostBack="true" Width="200px" AppendDataBoundItems="true"
                                    runat="server" CssClass="input_mandatory" AddressType='<%# ((int)PhoenixAddressType.CHARTERER).ToString() %>' />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%">
                                <telerik:RadLabel ID="lblManager" runat="server" Text="Management Company"></telerik:RadLabel>
                            </td>
                            <td colspan="4" style="width: 30%">
                                <eluc:AddressType ID="ucManager" runat="server" Width="200px" AppendDataBoundItems="true"
                                    CssClass="input_mandatory" AddressType='<%# ((int)PhoenixAddressType.MANAGER).ToString() %>' />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <telerik:RadLabel ID="lblReqDocs" runat="server" Text="Required Documents" Font-Bold="true"></telerik:RadLabel>
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblFilterby" runat="server" Text="Show all documents Y/N"></telerik:RadLabel>
                            </td>
                            <td colspan="4">
                                <%--<telerik:RadLabel id="lblFRank" runat="server" Text="Rank: "></telerik:RadLabel> <asp:CheckBox ID="chkRank" runat="server" AutoPostBack="true" />
                                <telerik:RadLabel id="lblFVesselType" runat="server" Text="Vessel Type: "></telerik:RadLabel> <asp:CheckBox ID="chkVesselType" runat="server" AutoPostBack="true" />
                                <telerik:RadLabel id="lblFCharterer" runat="server" Text="Charterer: "></telerik:RadLabel> <asp:CheckBox ID="chkCharterer" runat="server" AutoPostBack="true" />--%>
                                <asp:CheckBox ID="chkshowall" runat="server" AutoPostBack="true" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <%--  <asp:GridView ID="gvDocumentCategory" runat="server" AutoGenerateColumns="False"
                                    Font-Size="11px" Width="80%" CellPadding="2" AllowSorting="true" ShowHeader="true"
                                    ShowFooter="false" EnableViewState="false" OnRowDataBound="gvDocumentCategory_ItemDataBound">
                                    <FooterStyle CssClass="datagrid_footerstyle" />
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                                <telerik:RadGrid RenderMode="Lightweight" ID="gvDocumentCategory" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                    CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvDocumentCategory_NeedDataSource"
                                    OnItemDataBound="gvDocumentCategory_ItemDataBound1"
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
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblDocumentCategoryNameHeader" runat="server" Text="Document Category"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <b>
                                                        <telerik:RadLabel ID="lblDocumentCategoryName" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCATEGORYNAME"]%>'></telerik:RadLabel>
                                                    </b>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblDocumentNameHeader" runat="server" Text="Document"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div id="divDocs" runat="server" style="overflow: auto; height: 150px;">
                                                        <div>
                                                            <asp:CheckBoxList ID="chkListcoc" RepeatDirection="Vertical" runat="server">
                                                            </asp:CheckBoxList>
                                                        </div>
                                                        <div>
                                                            <asp:CheckBoxList ID="chkListstcw" RepeatDirection="Vertical" runat="server">
                                                            </asp:CheckBoxList>
                                                        </div>
                                                        <div>
                                                            <asp:CheckBoxList ID="chkListMedical" RepeatDirection="Vertical" runat="server">
                                                            </asp:CheckBoxList>
                                                        </div>
                                                        <div>
                                                            <asp:CheckBoxList ID="chkListotherdoc" RepeatDirection="Vertical" runat="server">
                                                            </asp:CheckBoxList>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                            PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
                                    </MasterTableView>
                                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                    </ClientSettings>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblDocument" runat="server" Text="Document" Font-Bold="true"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblAvailable" runat="server" Text="Available" Font-Bold="true"></telerik:RadLabel>
                            </td>
                            <td></td>
                            <td>
                                <telerik:RadLabel ID="lblSelected" runat="server" Text="Selected" Font-Bold="true"></telerik:RadLabel>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td rowspan="2" style="width: 20%">
                                <telerik:RadLabel ID="lblExpinRank" runat="server" Text="Experience in Rank"></telerik:RadLabel>
                            </td>
                            <td style="width: 20%">
                                <asp:ListBox ID="lstExpinRank" runat="server" Width="200px"  SelectionMode="Multiple"></asp:ListBox>
                            </td>
                            <td style="width: 5%">
                                <asp:Button ID="btnExpinRankSelect" runat="server" Text=">>"  OnClick="btnExpinRankSelect_Click" />
                                <br />
                                <br />
                                <asp:Button ID="btnExpinRankDeselect" runat="server" Text="<<"  OnClick="btnExpinRankDeselect_Click" />
                            </td>
                            <td style="width: 20%">
                                <asp:ListBox ID="lstSelectedExpinRank" runat="server" Width="200px" 
                                    SelectionMode="Multiple"></asp:ListBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkWaivedRankExp" runat="server" Enabled="false" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <eluc:Number ID="txtRankExp" runat="server"  Width="30px" />
                                <telerik:RadLabel ID="lblRankexp" runat="server" Text="months experience in ANY of these ranks"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2" style="width: 20%">
                                <telerik:RadLabel ID="lblExpinVesseltype" runat="server" Text="Experience on Vessel Type"></telerik:RadLabel>
                            </td>
                            <td style="width: 20%">
                                <asp:ListBox ID="lstExpinVesseltype" runat="server" Width="200px" 
                                    SelectionMode="Multiple"></asp:ListBox>
                            </td>
                            <td style="width: 5%">
                                <asp:Button ID="btnExpinVTSelect" runat="server" Text=">>"  OnClick="btnExpinVTSelect_Click" />
                                <br />
                                <br />
                                <asp:Button ID="btnExpinVTDeselect" runat="server" Text="<<"  OnClick="btnExpinVTDeselect_Click" />
                            </td>
                            <td style="width: 20%">
                                <asp:ListBox ID="lstSelectedExpinVesseltype" runat="server" Width="200px" 
                                    SelectionMode="Multiple"></asp:ListBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkWaivedVesselTypeExp" runat="server" Enabled="false" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <eluc:Number ID="txtVesseltypeExp" runat="server"  Width="30px" />
                                <telerik:RadLabel ID="lblVesseltypeExp" runat="server" Text="months experience in ANY of these vessel types"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
