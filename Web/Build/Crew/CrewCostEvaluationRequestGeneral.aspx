<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCostEvaluationRequestGeneral.aspx.cs"
    Inherits="CrewCostEvaluationRequestGeneral" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlCommonVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TravelReason" Src="~/UserControls/UserControlTravelReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MUCCity" Src="~/UserControls/UserControlMultiColumnCity.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Request Details</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function resize() {
                var obj = document.getElementById("ifMoreInfo");
                obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 190 + "px";
                // onresize="resize()"
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmCrewTravelVisaLineItem" DecoratedControls="All" />
    <form id="frmCrewTravelVisaLineItem" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenPick_Click" />
            <eluc:TabStrip ID="MenuCrewCostGeneral" runat="server" OnTabStripCommand="MenuCrewCostGeneral_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuCrewCostGeneralSub" runat="server" OnTabStripCommand="MenuCrewCostGeneralSub_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <table width="100%" cellpadding="2" cellspacing="2">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" AssignedVessels="true"
                            EntityType="VSL" ActiveVessels="true" Width="180px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRequestNo" runat="server" Text="Request No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReqNo" runat="server" CssClass="readonlytextbox" Width="180px" ReadOnly="true" Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblNoofOnsigners" runat="server" Text="No. of On Signers"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtNoofJoiner" runat="server" CssClass="input_mandatory" DefaultZero="true"
                            IsInteger="true" IsPositive="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblNoofOffSigners" runat="server" Text="No. of Off Signers"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtNoofOffSigner" runat="server" CssClass="input_mandatory" DefaultZero="true"
                            IsInteger="true" IsPositive="true" />
                    </td>
                </tr>
            </table>
            <div id="divAssesment" runat="server" visible="false">
                <asp:Panel ID="Panel1" GroupingText="Crew Change Assesment" Direction="LeftToRight"
                    Width="100%" runat="server">
                    <telerik:RadLabel ID="lblcrewchange" runat="server" Font-Bold="true" Text="Crew Change for the Month :"></telerik:RadLabel>
                    <telerik:RadTextBox ID="txtCrewChangeCount" Width="30" runat="server" CssClass="readonlytextbox"
                        ReadOnly="true">
                    </telerik:RadTextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp;<telerik:RadLabel ID="RadLabel1" runat="server" Font-Bold="true" Text="Crew Change Reason :"></telerik:RadLabel>
                    <eluc:TravelReason ID="ucCrewChangeReason" runat="server" AppendDataBoundItems="true"
                        CssClass="dropdown_mandatory" ReasonFor="1" Width="180px" />
                </asp:Panel>
            </div>
            <br />
            <div id="divReqDetail" runat="server">
                <eluc:TabStrip ID="MenuArraivalPort" runat="server" OnTabStripCommand="MenuArraivalPort_TabStripCommand"></eluc:TabStrip>

                <telerik:RadGrid RenderMode="Lightweight" ID="gvEvaluationPort" runat="server" EnableViewState="false"
                    AllowCustomPaging="true" AllowSorting="true" AllowPaging="false" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                    OnNeedDataSource="gvEvaluationPort_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvEvaluationPort_ItemDataBound"
                    OnItemCommand="gvEvaluationPort_ItemCommand" OnUpdateCommand="gvEvaluationPort_UpdateCommand" ShowFooter="false" OnDeleteCommand="gvEvaluationPort_DeleteCommand"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" DataKeyNames="FLDEVALUATIONPORTID" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed">
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
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Port" AllowSorting="false" ShowSortIcon="true">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkPortName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'
                                        CommandName="SELECT" CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                                    <telerik:RadLabel ID="lblPortName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblEvaluationPortId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEVALUATIONPORTID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblPortCallId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTCALLID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblApprovedYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVEDYN") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="City" AllowSorting="false" ShowSortIcon="true">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCityId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblCityName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYNAME") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblCountry" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYID") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblEvaluationPortIdEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEVALUATIONPORTID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblCityIdEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblPortCallIDEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTCALLID") %>'></telerik:RadLabel>
                                    <eluc:MUCCity ID="txtCityIdEdit" runat="server" CssClass="input_mandatory" Width="100%" />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="ETA" AllowSorting="false" ShowSortIcon="true">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblETA" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDETA") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="ETD" AllowSorting="false" ShowSortIcon="true">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblETD" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDETD") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Visa" ID="cmdVisa" ToolTip="Visa" CommandName="VISA">
                                      <span class="icon"><i class="fab fa-cc-visa"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="EDIT">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="DELETE" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Crew Change Request" ID="cmdIniTravel" Visible="false" CommandName="CREWCHANGEREQUEST"
                                        ToolTip="Crew Change Request">
                                    <span class="icon"><i class="fas fa-plane-departure"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="Update" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="Cancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                    </asp:LinkButton>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>

                <iframe runat="server" id="ifMoreInfo" style="min-height: 400px; width: 100%; border: 0;"
                    scrolling="auto"></iframe>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
