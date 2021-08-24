<%@ Page Language="C#" AutoEventWireup="True" CodeFile="RegistersSeaport.aspx.cs"
    Inherits="RegistersSeaport" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Airport" Src="~/UserControls/UserControlAirport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="~/UserControls/UserControlCity.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Seaport</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript">
            function DeleteRecord(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmDelete.UniqueID %>", "");
                }
            }
        </script>
        <script type="text/javascript">
            function pageLoad() {
                PaneResized();
                fade('statusmessage');
            }
            function PaneResized(sender, args) {
                var browserHeight = $telerik.$(window).height();
                var grid = $find("gvSeaport");
                grid._gridDataDiv.style.height = (browserHeight - 220) + "px";
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmRegistersSeaport" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuregistersSeaPortMain" runat="server" OnTabStripCommand="MenuregistersSeaPortMain_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            </div>
            <table id="tblConfigureSeaport" width="100%">
                <tr>
                    <td width="4%">
                        <telerik:RadLabel ID="lblCountry" runat="server" Text="Country"></telerik:RadLabel>
                    </td>
                    <td width="29.3%">
                        <eluc:Country ID="ddlCountrySearch" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true" 
                               AppendDataBoundItems="true" Width="45%" OnTextChangedEvent="ddlCountrySearch_TextChangedEvent"  />
                    </td>
                    <td width="4%">
                        <telerik:RadLabel ID="lblCode" runat="server" Text="Code"></telerik:RadLabel>
                    </td>
                    <td width="29.3%">
                        <telerik:RadTextBox ID="txtSeaportCode" runat="server" MaxLength="6"></telerik:RadTextBox>
                    </td>
                    <td width="4%">
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td width="29.3%">
                        <telerik:RadTextBox ID="txtSearch" runat="server" MaxLength="100"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersSeaport" runat="server" OnTabStripCommand="RegistersSeaport_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSeaport" Height="81%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvSeaport_ItemCommand" OnItemDataBound="gvSeaport_ItemDataBound"
                OnSortCommand="gvSeaport_SortCommand" ShowFooter="True" EnableViewState="true" OnNeedDataSource="gvSeaport_NeedDataSource" 
                EnableHeaderContextMenu="true" GroupingEnabled="false">
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDSEAPORTID">
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
                        <telerik:GridTemplateColumn HeaderText="Code" AllowSorting="true" SortExpression="FLDSEAPORTCODE">
                            <HeaderStyle Width="9%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSeaportcode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtSeaportcodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTCODE") %>'
                                    CssClass="gridinput_mandatory" MaxLength="6" Width="100%"></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtSeaportcodeAdd" runat="server" Width="100%" CssClass="gridinput_mandatory" MaxLength="6"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="true" SortExpression="FLDSEAPORTNAME">
                            <HeaderStyle Width="12%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSeaportid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkSeaportName" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>' Visible="false"></asp:LinkButton>
                                <telerik:RadLabel ID="lblSeaportName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblSeaportidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtSeaportNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'
                                    CssClass="gridinput_mandatory" MaxLength="100" Width="100%"></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtSeaportNameAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="100" ToolTip="Enter Seaport Name" Width="100%" ></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Airport" AllowSorting="true" SortExpression="">
                            <HeaderStyle Width="14%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblairportid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRPORTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lnkairportName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRPORTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblAirportId" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRPORTID") %>'></telerik:RadLabel>
                                <eluc:Airport ID="Airport1" runat="server" AirportList='<%# PhoenixRegistersAirport.ListAirport(null)%>'
                                    CssClass="dropdown_mandatory" AppendDataBoundItems="true" SelectedAirport='<%# DataBinder.Eval(Container,"DataItem.FLDAIRPORTID") %>' Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Airport ID="Airport2" runat="server" CssClass="dropdown_mandatory" AirportList='<%# PhoenixRegistersAirport.ListAirport(null)%>'
                                    AppendDataBoundItems="true" Width="100%"  />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="City" AllowSorting="true" SortExpression="">
                            <HeaderStyle Width="14%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCityid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCityName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListddlCityEdit">
                                    <telerik:RadTextBox ID="ddlCityNameEdit" runat="server" Width="85%" Enabled="False"
                                        CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYNAME") %>'></telerik:RadTextBox>
                                    <asp:LinkButton ID="btnShowddlCityEdit" runat="server" CommandName="BUDGETCODE">
                                        <span class="icon"><i class="fas fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="ddlCityEdit" runat="server" Width="0px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYID") %>'></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListddlCityAdd">
                                    <telerik:RadTextBox ID="ddlCityAddNameAdd" runat="server" Width="85%" Enabled="False"
                                        CssClass="input_mandatory"></telerik:RadTextBox>
                                    <asp:LinkButton ID="btnShowddlCityAdd" runat="server" CommandName="BUDGETCODE">
                                        <span class="icon"><i class="fas fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="ddlCityAdd" runat="server" Width="0px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYID") %>'></telerik:RadTextBox>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="EU Regulation Y/N" AllowSorting="true" SortExpression="">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEURegulation" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDEUSEAPORTYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkEURegulationEdit" runat="server" AutoPostBack="false" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDEUSEAPORTYN").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkEURegulationAdd" runat="server" AutoPostBack="false"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ECA Y/N" AllowSorting="true" SortExpression="">
                            <HeaderStyle Width="7%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblECA" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDECAYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkECAEdit" runat="server" AutoPostBack="false" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDECAYN").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkECAAdd" runat="server" AutoPostBack="false"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="OPN scrubber Y/N" AllowSorting="true" SortExpression="">
                            <HeaderStyle Width="9%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblopnscrubyn" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDOPNSCRBRALWDYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkopnscrubynEdit" runat="server" AutoPostBack="false" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDOPNSCRBRALWDYN").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkopnscrubynAdd" runat="server" AutoPostBack="false"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active Y/N" AllowSorting="true" SortExpression="">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActiveYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkActiveYNEdit" runat="server" AutoPostBack="false" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkActiveYNAdd" runat="server" AutoPostBack="false"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="12%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" />
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="PORTCOMMENTS" ID="cmdPortComments" ToolTip="Port Comments">
                                    <span class="icon"><i class="fas fa-file-signature"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Save" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
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
            <asp:Button ID="ucConfirmDelete" runat="server" OnClick="ucConfirmDelete_Click" CssClass="hidden" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
