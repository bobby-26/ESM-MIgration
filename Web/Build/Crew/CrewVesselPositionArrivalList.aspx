<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewVesselPositionArrivalList.aspx.cs" Inherits="CrewVesselPositionArrivalList" EnableEventValidation="false" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DTime" Src="~/UserControls/UserControlMaskedTextBox.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel Position Arrival</title>

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVesselPosition" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server" Height="90%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuVesselPosition" runat="server" OnTabStripCommand="MenuVesselPosition_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <br />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ucVessel" runat="server" CssClass="input_mandatory" VesselsOnly="true" AutoPostBack="true"
                            AppendDataBoundItems="true" OnTextChangedEvent="ucVessel_TextChangedEvent" AssignedVessels="true"
                            Entitytype="VSL" ActiveVesselsOnly="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVoyageMonth" runat="server" Text="Voyage Month"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlMonth" runat="server" CssClass="input_mandatory" HardTypeCode="55" AppendDataBoundItems="true" SortByShortName="true" AutoPostBack="true" OnTextChangedEvent="BindData" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVoyageYear" runat="server" Text="Voyage Year"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ddlYear" runat="server" CssClass="input_mandatory" QuickTypeCode="55" AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="BindData" />
                    </td>
                    <td><span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                        <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="300px" ShowEvent="onmouseover"
                            RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true"
                            HideEvent="ManualClose" Position="MiddleRight" EnableRoundedCorners="true"
                            Text="Click on the port name to navigate to Noon-Report">
                        </telerik:RadToolTip>
                    </td>
                </tr>
            </table>
            <br />
            <eluc:TabStrip ID="MenuVesselPositionArrival" runat="server"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvVesselPositionArrival" runat="server" Height="80%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvVesselPositionArrival_NeedDataSource"
                OnItemCommand="gvVesselPositionArrival_ItemCommand" OnItemDataBound="gvVesselPositionArrival_ItemDataBound"
                OnSortCommand="gvVesselPositionArrival_SortCommand" GroupingEnabled="false" EnableHeaderContextMenu="true" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridButtonColumn Text="SingleClick" CommandName="Edit" Visible="false" />
                        <telerik:GridTemplateColumn HeaderText="Vessel">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sequence" HeaderStyle-Width="7%">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDSEQUENCE")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Port">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPositionId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDPOSITIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkPortName" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                    Text='<%#DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>' CommandName="SELECT"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblPositionIdEdit" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDPOSITIONID") %>'></telerik:RadLabel>
                                <eluc:Port ID="ddlPortEdit" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                    SeaportList='<%#PhoenixRegistersSeaport.ListSeaport() %>' SelectedSeaport='<%#DataBinder.Eval(Container,"DataItem.FLDSEAPORTID") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ETA">

                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDETA", "{0:dd/MMM/yyyy}") %>
                                <%#DataBinder.Eval(Container, "DataItem.FLDETA", "{0:hh:mm tt}").ToString() == "12:00 AM" ? string.Empty : DataBinder.Eval(Container, "DataItem.FLDETA", "{0:hh:mm tt}").ToString()%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtETAEdit" runat="server" CssClass="input" DateTimeFormat="dd/MM/YYYY" Text='<%#DataBinder.Eval(Container, "DataItem.FLDETA", "{0:dd/MMM/yyyy}") %>' />
                                <eluc:DTime ID="txtETATimeEdit" runat="server" CssClass="input" Width="50px" MaskText="##:##" Text=' <%#DataBinder.Eval(Container, "DataItem.FLDETA", "{0:hh:mm tt}").ToString() == "12:00 AM" ? string.Empty : DataBinder.Eval(Container, "DataItem.FLDETA", "{0:hh:mm tt}").ToString()%>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ETD">

                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDETD", "{0:dd/MMM/yyyy}") %>
                                <%#DataBinder.Eval(Container, "DataItem.FLDETD", "{0:hh:mm tt}").ToString() == "12:00 AM" ? string.Empty : DataBinder.Eval(Container, "DataItem.FLDETD", "{0:hh:mm tt}").ToString()%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtETDEdit" runat="server" CssClass="input" DateTimeFormat="dd/MM/YYYY" Text='<%#DataBinder.Eval(Container, "DataItem.FLDETD", "{0:dd/MMM/yyyy}") %>' />
                                <eluc:DTime ID="txtETDTimeEdit" runat="server" CssClass="input" Width="50px" MaskText="##:##" Text='<%#DataBinder.Eval(Container, "DataItem.FLDETD", "{0:hh:mm tt}").ToString() == "12:00 AM" ? string.Empty : DataBinder.Eval(Container, "DataItem.FLDETD", "{0:hh:mm tt}").ToString()%>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Voyage Number">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVoyageNumber" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVOYAGENUMBER")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Port Call ID">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPortCallId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPORTCALLID")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="7%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                    ToolTip="Edit"><span class="icon"><i class="fas fa-edit"></i></span></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete"><span class="icon"><i class="fa fa-trash"></i></span></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="UPDATE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                    ToolTip="Save"><span class="icon"><i class="fas fa-save"></i></span></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                    ToolTip="Cancel"><span class="icon"><i class="fa fa-trash"></i></span></asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
