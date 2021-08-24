<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentDrugAndAlcoholTest.aspx.cs"
    Inherits="InspectionIncidentDrugAndAlcoholTest" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Result" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Drug And Alcohol Test</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionDrugAlcoholTest" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuIncidentDrugTest" runat="server" OnTabStripCommand="MenuIncidentDrugTest_TabStripCommand"
                Title="Drug And Alcohol Test" ></eluc:TabStrip>
            <table id="DrugTestDate" runat="server">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date "></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtDateOfTest" CssClass="input_mandatory" Enabled="true"
                            DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTime" runat="server" Text="Time "></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTimePicker ID="txtTimeOfTest" runat="server" Width="80px" CssClass="input_mandatory" Enabled="true"></telerik:RadTimePicker>
                        &nbsp hrs
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuDrugAlcoholTest" runat="server" OnTabStripCommand="MenuDrugAlcoholTest_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvDrugAlcoholTest" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowCustomPaging="true" AllowPaging="true"
                Width="100%" CellPadding="3" OnItemDataBound="gvDrugAlcoholTest_ItemDataBound" OnUpdateCommand="gvDrugAlcoholTest_UpdateCommand"
                OnItemCommand="gvDrugAlcoholTest_ItemCommand" OnNeedDataSource="gvDrugAlcoholTest_NeedDataSource" Height="80%"
                Style="margin-bottom: 0px" EnableViewState="false" AllowSorting="true" ShowFooter="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDDRUGALCOHOLTESTID">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Crew Name" AllowSorting="true" SortExpression="FLDNAME">
                            <HeaderStyle Width="30%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCrewName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblempid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDrugTestid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDRUGALCOHOLTESTID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <span id="spnCrewInCharge">
                                    <telerik:RadTextBox ID="txtCrewName" runat="server" CssClass="input_mandatory" Enabled="false"
                                        MaxLength="50" Width="70%"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtCrewRank" runat="server" CssClass="input_mandatory" Enabled="false"
                                        MaxLength="50" Width="25%"></telerik:RadTextBox>
                                    <asp:LinkButton runat="server" ID="imgShowCrewInCharge">
                                        <span class="icon"><i class="fas fas fa-tasks"></i></span> 
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtCrewId" runat="server" CssClass="input" MaxLength="20" Width="10px"></telerik:RadTextBox>
                                </span><span id="spnPersonInChargeOffice">
                                    <telerik:RadTextBox ID="txtOfficePersonName" runat="server" CssClass="input_mandatory" Enabled="false"
                                        MaxLength="50" Width="50%"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOfficePersonDesignation" runat="server" CssClass="input_mandatory"
                                        Enabled="false" MaxLength="50" Width="20%"></telerik:RadTextBox>
                                    <asp:LinkButton runat="server" ID="imgPersonOffice">
                                        <span class="icon"><i class="fas fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtPersonOfficeId" runat="server" CssClass="input" MaxLength="20"
                                        Width="10px"></telerik:RadTextBox>
                                    <telerik:RadTextBox runat="server" ID="txtPersonOfficeEmail" CssClass="input" Width="0px"
                                        MaxLength="20"></telerik:RadTextBox>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="true" SortExpression="FLDRANKNAME">
                            <HeaderStyle Width="30%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRankName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRankId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Result">
                            <HeaderStyle Width="30%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblResult" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESULTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Result ID="ucResult" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                    HardTypeCode="178" HardList='<%# PhoenixRegistersHard.ListHard(1, 178) %>' SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDRESULT") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Result ID="ucResultAdd" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                    HardTypeCode="178" HardList='<%# PhoenixRegistersHard.ListHard(1, 178) %>' />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave"
                                    ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel"
                                    ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="ADD" ID="cmdAdd"
                                    ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
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
