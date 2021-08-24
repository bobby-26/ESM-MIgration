<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewNewApplicantCompanyExperience.aspx.cs"
    Inherits="CrewNewApplicantCompanyExperience" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Company Experience</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
        function Resize() {
        setTimeout(function () {
        TelerikGridResize($find("<%= gvCrewCompanyExperience.ClientID %>"));
        }, 200);
        }
        window.onresize = window.onload = Resize;
        
        function pageLoad(sender, eventArgs) {
        Resize();
        fade('statusmessage');
        }
</script>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewCompanyExperience" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel runat="server" ID="pnlCrewCompanyExperienceEntry" >
            <eluc:TabStrip ID="TabStrip1" runat="server" Title="Company Experience"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"/>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAppliedRank" runat="server" Text="Applied Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRank" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>

                <eluc:TabStrip ID="MenuCrewCompanyExperience" runat="server" OnTabStripCommand="CrewCompanyExperience_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid ID="gvCrewCompanyExperience" runat="server" AutoGenerateColumns="False" Width="100%"  ShowHeader="true" EnableViewState="false"
                CellSpacing="0" GridLines="None" GroupingEnabled="false" AllowCustomPaging="true" AllowPaging="true" AllowSorting="true" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvCrewCompanyExperience_NeedDataSource" OnItemDataBound="gvCrewCompanyExperience_ItemDataBound" OnItemCommand="gvCrewCompanyExperience_ItemCommand">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" >
                    
                    <HeaderStyle Width="120px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vessel">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVessel" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Type">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCrewCompanyExperienceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYEXPERIENCEID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkRank" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="DWT">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDwtNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELDWT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="GRT">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGrtNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELGT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="TEU">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTEU" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTEU") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Engine Type / Model">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEngine" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENGINETYPEMODEL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="From Date" UniqueName="Fromdate">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFromDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMDATE", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To Date" UniqueName="todate">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbToDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTODATE", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Duration">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDuration" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDURATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Gap">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGap" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDGAP") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Employer">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblManningCompanyName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMANNINGCOMPANY").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDMANNINGCOMPANY").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDMANNINGCOMPANY").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucManningCompanyTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANNINGCOMPANY") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last drawn salary/day(USD)">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLastSalary" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDAILYRATEUSD")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Owner">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOwner" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPRINCIPALNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign off Reason">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignOffReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNOFFREASONNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ice Experience">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIceClassed" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDICEEXPYN") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Move to other experience" CommandName="MOVETOOTHEREXPERIENCE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdMove" ToolTip="Move to other experience">
                                    <span class="icon"><i class="fas fa-file-export"></i></span>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" Position="Bottom" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" SaveScrollPosition="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" ResizeGridOnColumnResize="false" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
