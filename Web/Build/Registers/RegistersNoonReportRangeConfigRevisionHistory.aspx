<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersNoonReportRangeConfigRevisionHistory.aspx.cs"
    Inherits="RegistersNoonReportRangeConfigRevisionHistory" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Noon Report Range Config</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {

                   TelerikGridResize($find("<%= gvNRConfig.ClientID %>"));
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmNoonReportRangeConfig" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%" >
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Status runat="server" ID="ucStatus" />
            <%--<eluc:TabStrip ID="MenuNewSaveTabStrip" TabStrip="true" runat="server" OnTabStripCommand="NewSaveTap_TabStripCommand"></eluc:TabStrip>--%>
            <table id="tblSearch" width="100%" style="display:none;">
                <tr>
                    <td width="10%">
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                    </td>
                    <td width="20%">
                        <eluc:Vessel ID="UcVessel" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true" SyncActiveVesselsOnly="True" AssignedVessels="true"
                            VesselsOnly="true" AppendDataBoundItems="true" Width="98%" OnTextChangedEvent="UcVessel_TextChangedEvent" />
                    </td>
                <td width="10%">
                        <telerik:RadLabel ID="lblpublishedyn" runat="server" Text="Published YN"></telerik:RadLabel>
                    </td>
                    <td width="5%">
                        <telerik:RadCheckBox ID="ChkPublishedYN" runat="server" Enabled="false"></telerik:RadCheckBox>
                    </td>
                    <td width="10%">
                        <telerik:RadLabel ID="lblrevno" runat="server" Text="Rev No"></telerik:RadLabel>
                    </td>
                    <td width="20%">
                        <eluc:Number ID="txtrevno" Width="50px" runat="server" MaxLength="2" IsInteger="true" IsPositive="true" DecimalPlace="0" Enabled="false" />
                        <asp:LinkButton ID="cmdRevision" runat="server" AlternateText="Revision History"
                            ToolTip="Revision History">
                            <span class="icon"><i class="fas fa-history"></i></span>
                        </asp:LinkButton>
                    </td>
                    <td width="10%">
                        <telerik:RadLabel ID="lbldate" runat="server" Text="Published Date"></telerik:RadLabel>
                    </td>
                    <td width="10%">
                        <eluc:Date ID="ucdate" runat="server" Enabled="false" />
                    </td>
                     </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTargetVessel" Visible="false" runat="server" Text="Copy to Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ucTargetVessel" runat="server" CssClass="input" Visible="false" AutoPostBack="true"
                            VesselsOnly="true" AppendDataBoundItems="true" />
                    </td>
               </tr>
            </table>
            <eluc:TabStrip ID="MenuNRRangeConfig" runat="server" OnTabStripCommand="MenuNRRangeConfig_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvNRConfig" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvNRConfig_ItemCommand" OnItemDataBound="gvNRConfig_ItemDataBound"
                OnSortCommand="gvNRConfig_SortCommand" OnNeedDataSource="gvNRConfig_NeedDataSource" OnUpdateCommand="gvNRConfig_UpdateCommand"
                ShowFooter="false" ShowHeader="true" EnableViewState="true" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDCONFIGID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Field Name" AllowSorting="true" SortExpression="FLDDISPLAYTEXT">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblConfigId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONFIGID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkFieldName" runat="server" Visible="false" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISPLAYTEXT") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lbldisplaytext" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISPLAYTEXT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblfield" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOLUMNNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblConfigIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONFIGID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblconfigrevisionid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONFIGREVISIONID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                
                                <telerik:RadDropDownList runat="server" ID="ddlFieldNameEdit" AppendDataBoundItems="true" Width="98%"
                                    CssClass="dropdown_mandatory">
                                    <Items>
                                        <telerik:DropDownListItem Text="--Select--" Value="" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadDropDownList runat="server" ID="ddlFieldNameAdd" AppendDataBoundItems="true" Width="98%"
                                    CssClass="dropdown_mandatory" EnableDirectionDetection="true">
                                    <Items>
                                        <telerik:DropDownListItem Text="--Select--" Value="" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="" Visible="false">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Vessel ID="ucVesselEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                    VesselList="<%#PhoenixRegistersVessel.ListVessel() %>" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Vessel ID="ucVesselAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                    VesselList="<%#PhoenixRegistersVessel.ListVessel() %>" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Min Value">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMinValue" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMINVALUE") %>'></telerik:RadLabel>
                                <eluc:Number ID="txtMinValueEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMINVALUE") %>'
                                    CssClass="input" MaxLength="5" Width="120px" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtMinValueAdd" runat="server" CssClass="input" MaxLength="5" Width="98%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Max Value">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMaxValue" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXVALUE") %>'></telerik:RadLabel>
                                <eluc:Number ID="txtMaxValueEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMAXVALUE") %>'
                                    CssClass="input" MaxLength="5" Width="120px" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtMaxValueAdd" runat="server" CssClass="input" MaxLength="5" Width="98%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Min Alert Level">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAlertLevel" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALERTLEVEL") %>'></telerik:RadLabel>
                                <eluc:Number ID="txtAlertLevelEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDALERTLEVEL") %>'
                                    CssClass="input" MaxLength="5" Width="120px" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtAlertLevelAdd" runat="server" CssClass="input" MaxLength="5" Width="98%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Max Alert Level">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMaxAlertLevel" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXALERTLEVEL") %>'></telerik:RadLabel>
                                <eluc:Number ID="txtMaxAlertLevelEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMAXALERTLEVEL") %>'
                                    CssClass="input" MaxLength="5" Width="120px" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtMaxAlertLevelAdd" runat="server" CssClass="input" MaxLength="5" Width="98%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active Y/N" Visible="false">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="170px"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRequiredYN" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUIREDYESNO") %>'></telerik:RadLabel>
                                <telerik:RadCheckBox runat="server" ID="chkRequiredEdit" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDREQUIREDYN").ToString().Equals("1") ? true : false %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox runat="server" ID="chkRequiredAdd" Checked="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
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
