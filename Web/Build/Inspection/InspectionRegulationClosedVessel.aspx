<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRegulationClosedVessel.aspx.cs" Inherits="Inspection_InspectionRegulationClosedVessel" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Regulation Closed Vessel</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvRegulationCompletedVessel.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvNewRegulations">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvNewRegulations" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />

        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
            <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvRegulationCompletedVessel" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None"
                OnPreRender="gvRegulationCompletedVessel_PreRender"
                OnNeedDataSource="gvRegulationCompletedVessel_NeedDataSource"
                OnItemDataBound="gvRegulationCompletedVessel_ItemDataBound"
                OnItemCommand="gvRegulationCompletedVessel_ItemCommand" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <HeaderStyle Width="102px" />
                    <Columns>

                        <telerik:GridTemplateColumn HeaderText="Regulation">
                            <HeaderStyle Width="118px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRegulationTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Task">
                            <HeaderStyle Width="118px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActionPlan" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIONTOBETAKEN") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDACTIONTOBETAKEN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" UniqueName="Vessel">
                            <HeaderStyle Width="118px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Target Date">
                            <HeaderStyle Width="118px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTargetDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTARGETDATE","{0:dd-MM-yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Completed Date">
                            <HeaderStyle Width="118px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompletedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE","{0:dd-MM-yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Closed Date">
                            <HeaderStyle Width="118px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblClosedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLOSEDDATE","{0:dd-MM-yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <HeaderStyle Width="118px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLOSEDSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle Width="118px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="APPROVEEDIT" ID="cmdEdit"
                                    ToolTip="Verify Vessel">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment" ID="lnkAttachment" CommandName="ATTACHMENT"
                                    ToolTip="Regulation Attachment">
                                        <span class="icon"><i class="fa fa-paperclip"></i></span>
                                </asp:LinkButton>

                            </ItemTemplate>
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
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="460px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </div>

    </form>
</body>
</html>
