<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreDMRMidNightReportPassengerList.aspx.cs"
    Inherits="CrewOffshoreDMRMidNightReportPassengerList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>DMR Location</title>
    <telerik:RadCodeBlock ID="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmLocation" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />


        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

        <eluc:Status runat="server" ID="Status1" />


        <eluc:TabStrip ID="MenuReportTap" TabStrip="true" runat="server" OnTabStripCommand="ReportTapp_TabStripCommand"></eluc:TabStrip>

        <eluc:TabStrip ID="MenuNewSaveTabStrip" runat="server" OnTabStripCommand="NewSaveTap_TabStripCommand"></eluc:TabStrip>


        <div style="top: 40px; position: relative;">
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td width="10%">
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td width="20%">
                        <eluc:Vessel ID="ucVessel" runat="server" CssClass="input_mandatory" VesselsOnly="true"
                            AppendDataBoundItems="true" Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCrew" runat="server" Text="Crew"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtCrew" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            IsInteger="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLifeBoatCapacity" runat="server" Text="Life Boat Capacity"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="ucLifeBoatCapacity" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            IsInteger="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPassengerCount" runat="server" Text="No.Of Passenger"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="ucPassenger" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            IsInteger="true" />
                    </td>
                </tr>
            </table>
            <u><b>
                <telerik:RadLabel ID="lblPassenger" runat="server" Text="Passenger"></telerik:RadLabel>
            </b></u>
            <br />
            <table width="40%">
                <tr class="DataGrid-HeaderStyle">
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblMeal" runat="server" Text="Meal"></telerik:RadLabel>
                        </b>
                    </td>
                    <td align="right">
                        <b>
                            <telerik:RadLabel ID="lblClientMeal" runat="server" Text="Client Personnel"></telerik:RadLabel>
                        </b>
                    </td>
                    <td align="right">
                        <b>
                            <telerik:RadLabel ID="lblSupernumeraryMeal" runat="server" Text="Supernumerary"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr class="datagrid_alternatingstyle">
                    <td>
                        <telerik:RadLabel ID="lblBreakfast" runat="server" Text="Breakfast"></telerik:RadLabel>
                    </td>
                    <td align="right">
                        <eluc:Number ID="ucBreakfast" runat="server" CssClass="input" DecimalPlace="0" />
                    </td>
                    <td align="right">
                        <eluc:Number ID="ucSupBreakFast" runat="server" CssClass="input" DecimalPlace="0" />
                    </td>
                </tr>
                <tr class="datagrid_alternatingstyle">
                    <td>
                        <telerik:RadLabel ID="lblTea1" runat="server" Text="Tea"></telerik:RadLabel>
                    </td>
                    <td align="right">
                        <eluc:Number ID="ucTea1" runat="server" CssClass="input" DecimalPlace="0" />
                    </td>
                    <td align="right">
                        <eluc:Number ID="ucSupTea1" runat="server" CssClass="input" DecimalPlace="0" />
                    </td>
                </tr>
                <tr class="datagrid_alternatingstyle">
                    <td>
                        <telerik:RadLabel ID="lblLunch" runat="server" Text="Lunch"></telerik:RadLabel>
                    </td>
                    <td align="right">
                        <eluc:Number ID="ucLunch" runat="server" CssClass="input" DecimalPlace="0" />
                    </td>
                    <td align="right">
                        <eluc:Number ID="ucSupLunch" runat="server" CssClass="input" DecimalPlace="0" />
                    </td>
                </tr>
                <tr class="datagrid_alternatingstyle">
                    <td>
                        <telerik:RadLabel ID="lblTea2" runat="server" Text="Tea"></telerik:RadLabel>
                    </td>
                    <td align="right">
                        <eluc:Number ID="ucTea2" runat="server" CssClass="input" DecimalPlace="0" />
                    </td>
                    <td align="right">
                        <eluc:Number ID="ucSupTea2" runat="server" CssClass="input" DecimalPlace="0" />
                    </td>
                </tr>
                <tr class="datagrid_alternatingstyle">
                    <td>
                        <telerik:RadLabel ID="lblDinner" runat="server" Text="Dinner"></telerik:RadLabel>
                    </td>
                    <td align="right">
                        <eluc:Number ID="ucDinner" runat="server" CssClass="input" DecimalPlace="0" />
                    </td>
                    <td align="right">
                        <eluc:Number ID="ucSupDinner" runat="server" CssClass="input" DecimalPlace="0" />
                    </td>
                </tr>
                <tr class="datagrid_alternatingstyle">
                    <td>
                        <telerik:RadLabel ID="lblSupper" runat="server" Text="Supper"></telerik:RadLabel>
                    </td>
                    <td align="right">
                        <eluc:Number ID="ucSupper" runat="server" CssClass="input" DecimalPlace="0" />
                    </td>
                    <td align="right">
                        <eluc:Number ID="ucSupSupper" runat="server" CssClass="input" DecimalPlace="0" />
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuLocation" runat="server" OnTabStripCommand="MenuLocation_TabStripCommand"></eluc:TabStrip>

            <div id="divGrid" style="position: relative; z-index: 1">
                <%--<asp:GridView ID="gvPassengerList" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvPassengerList_RowCommand" OnRowDataBound="gvPassengerList_ItemDataBound"
                        OnRowCreated="gvPassengerList_RowCreated" OnRowCancelingEdit="gvPassengerList_RowCancelingEdit"
                        OnRowDeleting="gvPassengerList_RowDeleting" AllowSorting="true" OnRowEditing="gvPassengerList_RowEditing"
                        OnRowUpdating="gvPassengerList_RowUpdating" OnSorting="gvPassengerList_Sorting"
                        ShowFooter="true" ShowHeader="true" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvPassengerList" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" ShowFooter="true"
                    OnItemCommand="gvPassengerList_ItemCommand"
                    OnItemDataBound="gvPassengerList_ItemDataBound"
                    OnNeedDataSource="gvPassengerList_NeedDataSource"
                    OnUpdateCommand="gvPassengerList_UpdateCommand">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDPASSENGERID" CommandItemDisplay="Top" Height="10px">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                        <Columns>

                            <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <telerik:GridTemplateColumn FooterText="Sort Order">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblSlnoHeader" runat="server" Text="Sl.No"></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSlno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblSortOrderEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                                </EditItemTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Passenger Name" FooterText="New Dmr">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPassengerId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSENGERID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblLocationName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSENGERNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblPassengerIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSENGERID") %>'></telerik:RadLabel>
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtPassengerEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSENGERNAME") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200" Width="90%">
                                    </telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtPassengerAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"
                                        Width="90%">
                                    </telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadDropDownList ID="ddlTypeEdit" runat="server" CssClass="gridinput_mandatory">
                                        <Items>
                                            <telerik:DropDownListItem Text="--Select--" Value=""></telerik:DropDownListItem>
                                            <telerik:DropDownListItem Text="Client" Value="CLIENT"></telerik:DropDownListItem>
                                            <telerik:DropDownListItem Text="Service/Supernumary" Value="SUPERNUMERARY"></telerik:DropDownListItem>
                                        </Items>
                                    </telerik:RadDropDownList>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadDropDownList ID="ddlTypeAdd" runat="server" CssClass="gridinput_mandatory">
                                        <Items>
                                            <telerik:DropDownListItem Text="--Select--" Value=""></telerik:DropDownListItem>
                                            <telerik:DropDownListItem Text="Client" Value="CLIENT"></telerik:DropDownListItem>
                                            <telerik:DropDownListItem Text="Service/Supernumary" Value="SUPERNUMERARY"></telerik:DropDownListItem>
                                        </Items>
                                    </telerik:RadDropDownList>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Embarked">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblEmbarkedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMBARKEDDATE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="txtEmbarkedDateEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMBARKEDDATE") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Date ID="txtEmbarkedDateAdd" runat="server" CssClass="input_mandatory" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Disembarked">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDisembarkedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISEMBARKEDDATE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="txtDisembarkedDateEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISEMBARKEDDATE") %>'
                                        CssClass="gridinput" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Date ID="txtDisembarkeddateAdd" runat="server" CssClass="gridinput" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Remark">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRemark" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtRemarksEdit" runat="server" CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtRemarksAdd" runat="server" CssClass="gridinput"></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblActionHeader" runat="server">
                                        Action
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                        ToolTip="Edit">
                                         <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Delete"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                        ToolTip="Delete">
                                         <span class="icon"><i class="fa fa-trash"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                        CommandName="Update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                        ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                    </asp:LinkButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Cancel"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel">
                                         <span class="icon"><i class="fa fa-trash"></i></span>
                                    </asp:LinkButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                        CommandName="Add" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                        ToolTip="Add New">
                                         <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                    </asp:LinkButton>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>



            </div>

            <eluc:Status runat="server" ID="ucStatus" />
        </div>

    </form>
</body>
</html>
