<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectronicLogEngineParameterConfig.aspx.cs"
    Inherits="ElectronicLogEngineParameterConfig" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Engine Parameter</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {

                   TelerikGridResize($find("<%= gvNRConfig.ClientID %>"));
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
               Resize();
               fade('statusmessage');
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
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="Status1" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuNewSaveTabStrip" runat="server" OnTabStripCommand="NewSaveTap_TabStripCommand"></eluc:TabStrip>
    
            <table style="width:50%">
                <tr>
                    <td><telerik:RadLabel runat="server" ID="lbltype" Text="Type"></telerik:RadLabel></td>
                    <td><telerik:RadComboBox runat="server" ID="ddltype" AutoPostBack="true"  OnSelectedIndexChanged="ddltype_SelectedIndexChanged"></telerik:RadComboBox></td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuNRRangeConfig" runat="server" OnTabStripCommand="MenuNRRangeConfig_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvNRConfig" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvNRConfig_ItemCommand" OnItemDataBound="gvNRConfig_ItemDataBound"
                OnUpdateCommand="gvNRConfig_UpdateCommand" OnNeedDataSource="gvNRConfig_NeedDataSource" OnSortCommand="gvNRConfig_SortCommand"
                ShowHeader="true" EnableViewState="true" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDENGINELOGPARAMETERID">
                    <HeaderStyle Width="102px" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Variable" AllowSorting="true" SortExpression="FLDDESCRIPTION">
                            <HeaderStyle Width="20%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblparameterId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENGINELOGPARAMETERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblparameterconfigId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENGINELOGPARAMETERCONFIGID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblparameter" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENGINELOGPARAMETER") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lnkFieldName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" >
                            <HeaderStyle Width="15%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbltype" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATATYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Format"  >
                            <HeaderStyle Width="15%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblformat" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMAT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Unit"  >
                            <HeaderStyle Width="15%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblunit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNIT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Min Value">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMinValue" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMINVAL") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblparameterIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENGINELOGPARAMETERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblparameterconfigIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENGINELOGPARAMETERCONFIGID") %>'></telerik:RadLabel>
                                <eluc:Number ID="txtMinValueEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMINVAL") %>'
                                    CssClass="input"  Width="100%" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Max Value">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMaxValue" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXVAL") %>'></telerik:RadLabel>
                                <eluc:Number ID="txtMaxValueEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMAXVAL") %>'
                                    CssClass="input"  Width="100%" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Min Alert Level">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAlertLevel" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMINALERT") %>'></telerik:RadLabel>
                                <eluc:Number ID="txtAlertLevelEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMINALERT") %>'
                                    CssClass="input"  Width="100%" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Max Alert Level">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMaxAlertLevel" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXALERT") %>'></telerik:RadLabel>
                                 <eluc:Number ID="txtMaxAlertLevelEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMAXALERT") %>'
                                    CssClass="input"  Width="100%" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Allow NA">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblallowyn" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOWNAYN") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadCheckBox runat="server" ID="chkallowna" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDALLOWNA").ToString().Equals("1") ? true : false %>'></telerik:RadCheckBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Is Active" Visible="false">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="false" HorizontalAlign="left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblisactive" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISACTIVEYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox runat="server" ID="chkisactive" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDISACTIVE").ToString().Equals("1") ? true : false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
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
