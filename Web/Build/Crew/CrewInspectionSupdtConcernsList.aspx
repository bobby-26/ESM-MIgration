<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewInspectionSupdtConcernsList.aspx.cs"
    Inherits="Crew_CrewInspectionSupdtConcernsList" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSupdtConcerns" runat="server" autocomplete="off">

        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>

        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <eluc:TabStrip ID="MainMenuSupdtConcerns" runat="server" TabStrip="true" OnTabStripCommand="MainMenuSupdtConcerns_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />

            <table cellpadding="1" cellspacing="1" width="100%">
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
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="Employee Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <hr />

            <eluc:TabStrip ID="MenuSupdtConcerns" runat="server" OnTabStripCommand="MenuSupdtConcerns_TabStripCommand"></eluc:TabStrip>


            <telerik:RadGrid RenderMode="Lightweight" ID="gvSupdtConcerns" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvSupdtConcerns_NeedDataSource"
                OnItemCommand="gvSupdtConcerns_ItemCommand"
                OnItemDataBound="gvSupdtConcerns_ItemDataBound"
                GroupingEnabled="false" EnableHeaderContextMenu="true"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="30px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeFeedBackid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEFEEDBACKID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Event">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEvent" runat="server" ToolTip='<%#DataBinder.Eval(Container,"DataItem.FLDSOURCENAME") %>' Text='<%#DataBinder.Eval(Container,"DataItem.FLDSOURCENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Event Date">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEventDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSOURCEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ref No">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSOURCEREFERENCENUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Chapter">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblChapter" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHAPTER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Def Type">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDefType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEFTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Description">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Supt">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSupt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPERINTENDENT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarksDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDREMARKSDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Feedback Cat">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFeedBackCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFEEDBACKCATEGORY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Feedback Sub-Cat">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFeedBackSubCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFEEDBACKSUBCATEGORY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Supt Remarks">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                                <eluc:ToolTip runat="server" ID="ucRemarks" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKSTOOLTIP") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Key Anchor">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblKeyAnchor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDKEYANCHOR") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expected Closing Date">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExpectedClosingDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDEXPECTEDCLOSINGDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Actual CloseOut Date">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActualCloseOutDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDACTUALCLOSEOUTDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Closing Remarks">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblClosingRemarks" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDCLOSEOUTREMARKS") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLOSEOUTREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                    CommandName="CAttachment" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt"
                                    ToolTip="Attachment"></asp:ImageButton>

                                <asp:ImageButton runat="server" AlternateText="Close" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                    CommandName="CLOSE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdClose"
                                    ToolTip="Close"></asp:ImageButton>

                                <%--<asp:LinkButton runat="server" AlternateText="Close"
                                    CommandName="CLOSE" CommandArgument='<%# Container.DataSetIndex %>'
                                    ID="cmdClose" ToolTip="Close">
                                             <span class="icon"><i class="fas fa-times-circle-cancel"></i></span>
                                </asp:LinkButton>--%>

                            </ItemTemplate>
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

            <eluc:Status runat="server" ID="ucStatus" />
            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <img id="Img1" src="<%$ PhoenixTheme:images/red.png%>" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDocumentsExpired" runat="server" Text="* Documents Expired"></telerik:RadLabel>
                    </td>
                    <td>
                        <img id="Img2" src="<%$ PhoenixTheme:images/yellow.png%>" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDocumentsExpiringin30Days" runat="server" Text="* Documents Expiring in 30 Days"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
