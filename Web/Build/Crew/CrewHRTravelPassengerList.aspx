<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewHRTravelPassengerList.aspx.cs" Inherits="CrewHRTravelPassengerList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Family Travel Request</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewHrPassenger" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
           <eluc:TabStrip ID="MenuTravelPassengerMain" runat="server" OnTabStripCommand="TravelPassengerMain_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
             <eluc:TabStrip ID="MenuTitle" runat="server" Title=""></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="85%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />           
            <telerik:RadLabel ID="lblTitle" runat="server" Visible="false"></telerik:RadLabel>         
            <telerik:RadLabel ID="lblTravelBreakUp" runat="server" Text="Breakup Details" Font-Bold="true"></telerik:RadLabel>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvTravelRequestBreakup" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" OnItemCommand="gvTravelRequestBreakup_ItemCommand" OnNeedDataSource="gvTravelRequestBreakup_NeedDataSource" 
                OnItemDataBound="gvTravelRequestBreakup_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true"
                ShowFooter="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"
                    DataKeyNames="FLDTRAVELBREAKUPID">
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
                        <telerik:GridTemplateColumn HeaderText="S.No." HeaderStyle-Width="45px">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblRequestNoHeader" runat="server">S.No.</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSerialNo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDBREAKUPROWNUMBER") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTravelRequestId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTRAVELREQUESTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblTravelBreakupId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTRAVELBREAKUPID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblPersonalInfosn" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDPERSONALINFOSN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblOfficeStaffId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDOFFICESTAFFID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Departure" HeaderStyle-Width="110px">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblOriginHeader" runat="server">Origin</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDepatureCityName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDEPATURECITY") %>'
                                    CommandName="SELECT" ></asp:LinkButton>
                                <telerik:RadLabel ID="lblDepatureCityName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPATURECITY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDepatureCityId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPATURECITYID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListDeparturebreakupEdit">
                                    <telerik:RadTextBox ID="txtDepatureBreakupEdit" runat="server" Width="80%" Enabled="False"
                                        CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPATURECITY") %>'>
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowDepaturebreakup" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="Top" Text=".." CommandName="DEPATUREBREAKUP" 
                                        OnClientClick="return showPickList('spnPickListDeparturebreakupEdit', 'codehelp1', '', 'Common/CommonPickListCity.aspx', true); " />
                                    <telerik:RadTextBox ID="txtDepatureIdBreakupEdit" runat="server" Width="0px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPATURECITYID") %>'>
                                    </telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListDeparturebreakupAdd">
                                    <telerik:RadTextBox ID="txtDepatureBreakupAdd" runat="server" Width="80%" Enabled="False"
                                        CssClass="input_mandatory">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowDepaturebreakupAdd" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="Top" Text=".." CommandName="DEPATUREBREAKUPADD" 
                                        OnClientClick="return showPickList('spnPickListDeparturebreakupAdd', 'codehelp1', '', 'Common/CommonPickListCity.aspx', true); " />
                                    <telerik:RadTextBox ID="txtDepatureIdBreakupAdd" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Destination" HeaderStyle-Width="110px">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDestinationHeader" runat="server">Destination</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDestinationCityName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDESTINATIONCITY") %>'
                                    CommandName="SELECT"></asp:LinkButton>
                                <telerik:RadLabel ID="lblDestinationCityName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONCITY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDestinationCityId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONCITYID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListDestinationBreakupEdit">
                                    <telerik:RadTextBox ID="txtDestinationBreakupEdit" runat="server" Width="80%" Enabled="False"
                                        CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONCITY") %>'>
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowDestinationbreakupEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="Top" Text=".." CommandName="DESTINATIONBREAKUPADD" 
                                        OnClientClick="return showPickList('spnPickListDestinationBreakupEdit', 'codehelp1', '', 'Common/CommonPickListCity.aspx', true); " />
                                    <telerik:RadTextBox ID="txtDestinationIdBreakupEdit" runat="server" Width="0px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONCITYID") %>'>
                                    </telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListDestinationBreakupAdd">
                                    <telerik:RadTextBox ID="txtDestinationBreakupAdd" runat="server" Width="80%" Enabled="False"
                                        CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONCITY") %>'>
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowDestinationbreakup" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="Top" Text=".." CommandName="DESTINATIONBREAKUP"
                                        OnClientClick="return showPickList('spnPickListDestinationBreakupAdd', 'codehelp1', '', 'Common/CommonPickListCity.aspx', true); " />
                                    <telerik:RadTextBox ID="txtDestinationIdBreakupAdd" runat="server" Width="0px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONCITYID") %>'>
                                    </telerik:RadTextBox>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="DepartureDate" HeaderStyle-Width="200px">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDepartureDateHeader" runat="server">Departure</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepartureDate" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDEPATUREDATE")) %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDepartureTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPATURETIMEAMPM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtDepartureDateEdit" CssClass="input_mandatory" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDEPATUREDATE")) %>'></eluc:Date>
                                <telerik:RadComboBox ID="ddlDepartureTimeEdit" runat="server" CssClass="dropdown_mandatory" Width="55px"
                                    Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True" EmptyMessage="Type to select">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--" Value="DUMMY" />
                                        <telerik:RadComboBoxItem Text="AM" Value="1" />
                                        <telerik:RadComboBoxItem Text="PM" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtDepartureDateAdd" CssClass="input_mandatory"></eluc:Date>
                                <telerik:RadComboBox ID="ddlDepartureTimeAdd" runat="server" CssClass="dropdown_mandatory" Width="55px"
                                    Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True" EmptyMessage="Type to select">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--" Value="DUMMY" />
                                        <telerik:RadComboBoxItem Text="AM" Value="1" />
                                        <telerik:RadComboBoxItem Text="PM" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ArrivalDate" HeaderStyle-Width="200px">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblArrivalDateHeader" runat="server">Arrival</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblArrivalDate" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE")) %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblArrivalTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALTIMEAMPM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtArrivalDateEdit" CssClass="input" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDARRIVALDATE")) %>'></eluc:Date>
                                <telerik:RadComboBox ID="ddlArrivalTimeEdit" runat="server" CssClass="dropdown_mandatory" Width="55px"
                                    Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True" EmptyMessage="Type to select">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--" Value="DUMMY" />
                                        <telerik:RadComboBoxItem Text="AM" Value="1" />
                                        <telerik:RadComboBoxItem Text="PM" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtArrivalDateAdd" CssClass="input" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDARRIVALDATE")) %>'></eluc:Date>
                                <telerik:RadComboBox ID="ddlArrivalTimeAdd" runat="server" CssClass="input" Width="55px"
                                    Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True" EmptyMessage="Type to select">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--" Value="DUMMY" />
                                        <telerik:RadComboBoxItem Text="AM" Value="1" />
                                        <telerik:RadComboBoxItem Text="PM" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Class" HeaderStyle-Width="130px">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblClassHeader" runat="server">Class</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTravelClass" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTRAVELCLASSNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Hard ID="ucTravelClassEdit" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    HardTypeCode="227" SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELCLASSID") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Hard ID="ucTravelClassAdd" runat="server" AppendDataBoundItems="true" CssClass="input"
                                    HardList="<%# PhoenixRegistersHard.ListHard(1,227) %>" HardTypeCode="227" SelectedHard="1300" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="70px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Center" Width="100px" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdRowSave"
                                    ToolTip="Save" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>                               
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add"
                                    CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                    ToolTip="Add New" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" 
                        ScrollHeight=""/>
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
          
            <telerik:RadLabel ID="lblPassengerDetails" runat="server" Text="Passenger Details" Font-Bold="true"></telerik:RadLabel>
            <eluc:TabStrip ID="MenuTravelPassenger" runat="server" OnTabStripCommand="TravelPassenger_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvTravelPassenger" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvTravelPassenger_ItemCommand" OnNeedDataSource="gvTravelPassenger_NeedDataSource" 
                OnItemDataBound="gvTravelPassenger_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"
                    DataKeyNames="FLDTRAVELPASSENGERID">
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
                        <telerik:GridTemplateColumn HeaderText="S.No." HeaderStyle-Width="45px">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblRequestNoHeader" runat="server" Text="S.No."></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSerialNo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTravelRequestId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTRAVELREQUESTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblTravelPassengerId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTRAVELPASSENGERID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         
                        <telerik:GridTemplateColumn HeaderText="Salutation" HeaderStyle-Width="70px">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblSalutationHeader" runat="server" Text="Salutation"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSalutation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSALUTATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblSalutation1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSALUTATION") %>'></telerik:RadLabel>
                                <%--  <eluc:Quick ID="ddlsalutation" runat="server" QuickTypeCode="158" AppendDataBoundItems="true" SelectedText='<%# DataBinder.Eval(Container,"DataItem.FLDSALUTATION") %>' CssClass="input_mandatory" />--%>
                                <telerik:RadTextBox ID="txtSalutationEdit" runat="server"  CssClass="input" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSALUTATION") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="130px">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="35%"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblNameHeader" runat="server" Text="Name"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                    CommandName="SELECT" ></asp:LinkButton>
                                <telerik:RadLabel ID="lblpersonalinfosn" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDPERSONALINFOSN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblFamilymembersn" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFAMILYMEMBERSN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblOfficeStaffId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDOFFICESTAFFID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="DOB" HeaderStyle-Width="130px">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" ></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDOBHeader" runat="server" Text="DOB"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDOB" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEOFBIRTH")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtDOBEdit" runat="server" CssClass="input" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEOFBIRTH")) %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Age group" HeaderStyle-Width="60px">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblAgeHeader" runat="server" Text="Age Group"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAge" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGEBYCATEGORY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Gender" HeaderStyle-Width="150px">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblGenderHeader" runat="server" Text="Gender"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGender" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGENDER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtGenderEdit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGENDER") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Passport No" HeaderStyle-Width="150px">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPassportNoHeader" runat="server" Text="Passport No"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPassportNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSPORTNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtPassportNoEdit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSPORTNO") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Passport Expiry Date" HeaderStyle-Width="130px">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPassportDOEHeader" runat="server" Text="Passport Expiry Date"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPassportDOE" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPASSPORTEXPIRYDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="lblPassportDOEEdit" runat="server" CssClass="input" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPASSPORTEXPIRYDATE")) %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="70px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdUpdate"
                                    ToolTip="Save" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" 
                        ScrollHeight=""/>
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
