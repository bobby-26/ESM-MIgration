<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionVesselCheckItemList.aspx.cs" Inherits="InspectionVesselCheckItemList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByOwner" Src="~/UserControls/UserControlVesselByOwner.ascx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inspection Check Items</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvVesselCheckItems.ClientID %>"));
                }, 200);
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
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="80%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblItem" runat="server" Text="Item" Width="120px"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtItem" runat="server" Text="" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblvessel" runat="server" Text="Vessel" Width="120px"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:VesselByOwner runat="server" ID="ucVessel" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                VesselsOnly="true" Width="270px" OnTextChangedEvent="ucVessel_TextChangedEvent" AutoPostBack="true" />
                    </td>

                      <td>
                        <telerik:RadLabel ID="lblParent" runat="server" Text="Parent Item">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlParentitem" runat="server" AutoPostBack="true" Filter="Contains" Width="300px" OnSelectedIndexChanged="ddlParentitem_SelectedIndexChanged" EmptyMessage="Type to select "></telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <div runat="server" style="width:200%;">
            <eluc:TabStrip  RenderMode="Lightweight" ID="MenuCheckItem" runat="server" OnTabStripCommand="MenuCheckItem_TabStripCommand" />
                </div>
            <telerik:RadGrid ID="gvVesselCheckItems" runat="server" AllowPaging="true" AllowCustomPaging="true" AllowSorting="true"
                OnNeedDataSource="gvVesselCheckItems_NeedDataSource" OnItemCommand="gvVesselCheckItems_ItemCommand"
                ShowHeader="true" ShowFooter="false" Width="200%" OnItemDataBound="gvVesselCheckItems_ItemDataBound" GroupingEnabled="false" EnableViewState="false"  >
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table width="99.9%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Reference Number" HeaderStyle-Width="8%">
                            <ItemStyle Wrap="False" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReference" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="8%">
                            <ItemStyle Wrap="true" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblvesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblvesselcheckitemid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONVESSELCHECKITEMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Parent Item" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="False" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblParentitem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARENTITEMNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Item" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="true" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCheckitemid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGLOBALCHECKITEMID") %> ' Visible="false"></telerik:RadLabel>
                                <asp:LinkButton ID="lnkItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITEM") %>' CommandName="EDIT"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Audit / Inspection / Chapter Code" HeaderStyle-Width="20%">
                            <ItemStyle Wrap="true" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblChapterCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CHAPTERCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Deficiency Category" HeaderStyle-Width="20%">
                            <ItemStyle Wrap="true" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDeficiencyCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEFICIENCYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Description" HeaderStyle-Width="20%">
                            <ItemStyle Wrap="true" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Guidence Note" HeaderStyle-Width="20%">
                            <ItemStyle Wrap="true" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGuidenceNote" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGUIDENCENOTE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Components Code" HeaderStyle-Width="30%">
                            <ItemStyle Wrap="true" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Location" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="true" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLocation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Procedure Forms and Checklists" HeaderStyle-Width="25%">
                            <ItemStyle Wrap="true" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFormschecklist" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMSCHECKLISTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Filing System" HeaderStyle-Width="25%">
                            <ItemStyle Wrap="true" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFilingsystem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILINGSYSTEMNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Mapping" HeaderStyle-Width="25%">
                            <ItemStyle Wrap="true" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobs" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Responsibility" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="true" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblResponsibility" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPRANKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Checklist Mapping" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="true" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblChecklist" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHECKINSPECTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="5%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ToolTip="Delete" Width="20PX" Height="20PX"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
