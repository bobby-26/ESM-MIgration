<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersEUMRVVesselVerifierMaping.aspx.cs" Inherits="RegistersEUMRVVesselVerifierMaping" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Event Type</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {

                   TelerikGridResize($find("<%= gvVesselVerifierMap.ClientID %>"));
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
    <style>
         .RadGrid .rgHeader, .RadGrid th.rgResizeCol {
            padding-left: 2px !important;
            padding-right: 2px !important;
        }
        .RadGrid .rgRow td,.RadGrid .rgAltRow td
        {
            padding-left: 2px !important;
            padding-right: 2px !important;    
        }
        .RadGrid .rgFooter td, .rgEditRow td
        {
            padding-left: 2px !important;
            padding-right: 2px !important; 
        }
        .RadGrid .rgEditRow td
        {
            padding-left: 2px !important;
            padding-right: 2px !important; 
        }
        

        .RadInput .riTextBox, .RadInputMgr {
            padding: 4px 2px !important;
        }
    </style>
</head>
<body>
    <form id="frmVPRSEventType" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="Status1" />
            <eluc:TabStrip ID="MenuVesselVerifierMap" runat="server" OnTabStripCommand="VesselVerifierMap_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvVesselVerifierMap" Height="93%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvVesselVerifierMap_ItemCommand" OnItemDataBound="gvVesselVerifierMap_ItemDataBound"
                ShowFooter="true" ShowHeader="true" EnableViewState="true" EnableHeaderContextMenu="true" GroupingEnabled="false"
                OnUpdateCommand="gvVesselVerifierMap_UpdateCommand" OnNeedDataSource="gvVesselVerifierMap_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDVESSELID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="EU MRV" Name="EU MRV" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="IMO DCS" Name="IMO DCS" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="true" SortExpression="FLDVESSELNAME">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselVerifierMapid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAPEUMRVVERIFIERTOVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <eluc:Vessel ID="UcVessel" runat="server" CssClass="gridinput_mandatory" AppendDataBoundItems="true" SyncActiveVesselsOnly="True" VesselsOnly="true"
                                    VesselList="<%#PhoenixRegistersVessel.ListVessel() %>" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Verifier">
                            <HeaderStyle Width="7%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVerifierItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFIERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDropDownList ID="ddlVerifierEdit" runat="server" CssClass="gridinput padding" DataSource='<%# PhoenixRegistersEUMRVVesselVerifierMaping.EUMRVVerifierList() %>' DataValueField="FLDEUMRVVERIFIERID" DataTextField="FLDVERIFIERNAME" Width="100%"></telerik:RadDropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadDropDownList ID="ddlVerifierAdd" runat="server" CssClass="gridinput padding" DataSource='<%# PhoenixRegistersEUMRVVesselVerifierMaping.EUMRVVerifierList() %>' DataValueField="FLDEUMRVVERIFIERID" DataTextField="FLDVERIFIERNAME" Width="100%"
                                    EnableDirectionDetection="true">
                                </telerik:RadDropDownList>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Data Format">
                            <HeaderStyle Width="5%" HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDataFormat" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATAFORMAT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDropDownList ID="ddlDataFormatEdit" runat="server" CssClass="gridinput" Width="65px" EnableDirectionDetection="true">
                                    <Items>
                                        <telerik:DropDownListItem Text="--Select--" Value="" />
                                        <telerik:DropDownListItem Text="PDF" Value="PDF" />
                                        <telerik:DropDownListItem Text="EXCEL" Value="EXCEL" />
                                        <telerik:DropDownListItem Text="XML" Value="XML" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadDropDownList ID="ddlDataFormatAdd" runat="server" CssClass="gridinput" EnableDirectionDetection="true" Width="60px">
                                    <Items>
                                        <telerik:DropDownListItem Text="--Select--" Value="" />
                                        <telerik:DropDownListItem Text="PDF" Value="PDF" />
                                        <telerik:DropDownListItem Text="EXCEL" Value="EXCEL" />
                                        <telerik:DropDownListItem Text="XML" Value="XML" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="To Mail" ColumnGroupName="EU MRV">
                            <HeaderStyle Width="16%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblToMail" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTOMAIL").ToString().Length > 30 ? DataBinder.Eval(Container, "DataItem.FLDTOMAIL").ToString().Substring(0, 30) + "..." : DataBinder.Eval(Container, "DataItem.FLDTOMAIL").ToString()%>'></telerik:RadLabel>
                                <eluc:Tooltip ID="ucToMail" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOMAIL") %>' TargetControlId="lblToMail" Position="TopCenter" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtToMailEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOMAIL") %>'
                                    CssClass="gridinput" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtToMailAdd" runat="server" CssClass="gridinput" Width="100%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="CC Mail" ColumnGroupName="EU MRV">
                            <HeaderStyle Width="16%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCCMail" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCCMAIL").ToString().Length > 30 ? DataBinder.Eval(Container, "DataItem.FLDCCMAIL").ToString().Substring(0, 30) + "..." : DataBinder.Eval(Container, "DataItem.FLDCCMAIL").ToString()%>'></telerik:RadLabel>
                                <eluc:Tooltip ID="ucCCMail" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCCMAIL") %>' TargetControlId="lblCCMail" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtCCMailEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCCMAIL") %>'
                                    CssClass="gridinput" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtCCMailAdd" runat="server" CssClass="gridinput" Width="100%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Recognized Organisation">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblROfficerItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRONAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDropDownList ID="ddlROfficerEdit" runat="server" CssClass="gridinput" DataSource='<%# PhoenixRegistersVPRSReportingOfficer.ListROfficer() %>' DataValueField="FLDROID" DataTextField="FLDRONAME"
                                    Width="100%">
                                </telerik:RadDropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadDropDownList ID="ddlROfficerAdd" runat="server" CssClass="gridinput" DataSource='<%# PhoenixRegistersVPRSReportingOfficer.ListROfficer() %>' DataValueField="FLDROID" DataTextField="FLDRONAME"
                                    Width="100%" EnableDirectionDetection="true">
                                </telerik:RadDropDownList>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="To Mail" ColumnGroupName="IMO DCS">
                            <HeaderStyle Width="16%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMPAToMail" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMPATOMAIL").ToString().Length > 30 ? DataBinder.Eval(Container, "DataItem.FLDMPATOMAIL").ToString().Substring(0, 30) + "..." : DataBinder.Eval(Container, "DataItem.FLDMPATOMAIL").ToString()%>'></telerik:RadLabel>
                                <eluc:Tooltip ID="ucMPAToMail" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMPATOMAIL") %>' TargetControlId="lblMPAToMail" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtMPAToMailEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMPATOMAIL") %>'
                                    CssClass="gridinput" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtMPAToMailAdd" runat="server" CssClass="gridinput" Width="100%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="CC Mail" ColumnGroupName="IMO DCS">
                            <HeaderStyle Width="16%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMPACCMail" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMPACCMAIL").ToString().Length > 30 ? DataBinder.Eval(Container, "DataItem.FLDMPACCMAIL").ToString().Substring(0, 30) + "..." : DataBinder.Eval(Container, "DataItem.FLDMPACCMAIL").ToString()%>'></telerik:RadLabel>
                                <eluc:Tooltip ID="ucMMPACCMail" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMPACCMAIL") %>' TargetControlId="lblMPACCMail" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtMPACCMailEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMPACCMAIL") %>'
                                    CssClass="gridinput" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtMPACCMailAdd" runat="server" CssClass="gridinput" Width="100%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave"
                                    ToolTip="Save">
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
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
