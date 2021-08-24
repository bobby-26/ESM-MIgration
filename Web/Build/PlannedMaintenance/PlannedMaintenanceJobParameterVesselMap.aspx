<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceJobParameterVesselMap.aspx.cs"
    Inherits="PlannedMaintenanceJobParameterVesselMap" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Component" Src="~/UserControls/UserControlMultiColumnComponents.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Job" Src="~/UserControls/UserControlMultiColumnJobs.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvJobParameter.ClientID %>"));
                }, 200);
            }
            window.onload = window.onresize = Resize;
            function CloseUrlModelWindow() {

                var wnd = $find('<%=RadWindow_NavigateUrl.ClientID %>');
                wnd.close();
                __doPostBack("<%=cmdHiddenSubmit.UniqueID %>", "");
            }
            function pageLoad() {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmJobParameter" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="radWindowManager1" runat="server"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%" EnableAJAX="true">
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadNotification ID="ucStatus" RenderMode="Lightweight" runat="server" AutoCloseDelay="3500" ShowCloseButton="false" Title="Status" TitleIcon="none" ContentIcon="none"
                EnableRoundedCorners="true" Height="80px" Width="300px" OffsetY="30" Position="TopCenter" Animation="Fade" ShowTitleMenu="false">
            </telerik:RadNotification>
            <eluc:TabStrip ID="menuRevise" runat="server" OnTabStripCommand="menuRevise_TabStripCommand" />
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblJob" runat="server" Text="Job"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Job ID="ucJob" runat="server" Width="300px" OnTextChangedEvent="ucJob_TextChangedEvent" AutoPostBack="true"></eluc:Job>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblvsl" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="lblVesselName" runat="server" Text="Vessel" Enabled="false" CssClass="input"
                            Width="200px">
                        </telerik:RadTextBox>
                        <eluc:Vessel ID="ucVessel" runat="server" AssignedVessels="true" Entitytype="VSL" VesselsOnly="true"
                            AutoPostBack="true" OnTextChangedEvent="ucVessel_TextChangedEvent" Width="200px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblComponent" runat="server" Text="Component"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Component ID="ucComponent" runat="server" Width="300px" OnTextChangedEvent="ucJob_TextChangedEvent"
                            AutoPostBack="true"></eluc:Component>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRevision" runat="server" Text="Revision No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRevisionNo" runat="server" Width="40px" Enabled="false"></telerik:RadTextBox>
                        <asp:LinkButton ID="cmdRevision" runat="server" AlternateText="Revision History"
                            ToolTip="Revision History" OnClick="cmdRevision_Click">
                            <span class="icon"><i class="fas fa-history"></i></span>
                        </asp:LinkButton>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRevDate" runat="server" Text="Rev. Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDatePicker ID="dtRevisedDate" runat="server" Enabled="false"></telerik:RadDatePicker>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblreason" runat="server" Text="Reason"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReason" runat="server" TextMode="MultiLine" Width="300px" ReadOnly="true"></telerik:RadTextBox>
                    </td>

                </tr>
            </table>
            <eluc:TabStrip ID="MenuParameter" runat="server" OnTabStripCommand="MenuParameter_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvJobParameter" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvJobParameter_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                OnItemCommand="gvJobParameter_ItemCommand" OnItemDataBound="gvJobParameter_ItemDataBound"
                EnableViewState="true" Height="60%" EnableHeaderContextMenu="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Job" HeaderStyle-Width="35%" ItemStyle-Width="100%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblJob" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOB") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component" HeaderStyle-Width="25%" ItemStyle-Width="100%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblComponent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Parameter" HeaderStyle-Width="15%" ItemStyle-Width="100%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblParameterId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARAMETERID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblParameter" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARAMETERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Minimum" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="10%" ItemStyle-Width="100%" HeaderStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDMINVALUE") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtMinValueEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMINVALUE") %>'
                                    CssClass="input" MaxLength="9" Width="100%" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Maximum" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="10%" ItemStyle-Width="100%" HeaderStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDMAXVALUE") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtMaxValueEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMAXVALUE") %>'
                                    CssClass="input" MaxLength="9" Width="100%" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
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
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>

        <telerik:RadWindow runat="server" ID="RadWindow_NavigateUrl" Width="550px" Height="450px"
            Modal="true" OffsetElementID="main" VisibleStatusbar="false" KeepInScreenBounds="true" ReloadOnShow="true" ShowContentDuringLoad="true">
        </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server">
            <script type="text/javascript">                

                $modalWindow.modalWindowID = "<%=RadWindow_NavigateUrl.ClientID %>";
            
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
