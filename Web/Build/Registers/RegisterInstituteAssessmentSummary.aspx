<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterInstituteAssessmentSummary.aspx.cs" Inherits="Registers_RegisterInstituteAssessmentSummary" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonAddress" Src="../UserControls/UserControlCommonAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="../UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MobileNumber" Src="../UserControls/UserControlMobileNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToBeDoneBy" Src="~/UserControls/UserControlOffshoreToBeDoneBy.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Occassion" Src="~/UserControls/UserControlOccassionForReport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="skin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="table1" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="FeedBackTabs" runat="server" OnTabStripCommand="FeedBackTabs_TabStripCommand"
            TabStrip="false"></eluc:TabStrip>
        <%--        <table>
            <tr>
                <td>--%>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvFeedBackQst" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
            CellSpacing="0" GridLines="None" OnNeedDataSource="gvFeedBackQst_NeedDataSource"
            OnItemDataBound="gvFeedBackQst_ItemDataBound"
            AutoGenerateColumns="false">
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed">
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
                    <telerik:GridTemplateColumn HeaderText="Questions">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="99%"></HeaderStyle>

                        <ItemTemplate>
                            <table cellspacing="10" id="table1" runat="server">

                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblQuestionId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDQUESTIONID")%>'></telerik:RadLabel>
                                        <%#Container.DataSetIndex+1 %> .
                                                    <telerik:RadLabel ID="lblQuestionName" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDQUESTION")%>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblRequirRemark" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDREQUIREREMARK")%>'></telerik:RadLabel>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButtonList ID="rblOptions" runat="server" DataValueField="FLDOPTIONID" CssClass="readonlytextbox" ReadOnly="true"
                                            DataTextField="FLDOPTIONNAME" DataSource='<%# PhoenixRegistersAddressAssessmentOptions.AssessmentOptionList(General.GetNullableInteger((DataBinder.Eval(Container,"DataItem.FLDQUESTIONID")).ToString()),null,1) %>'
                                            RepeatDirection="Horizontal">
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblcomment" Visible="true" runat="server" Text="Comments"></telerik:RadLabel>
                                        <br />
                                        <telerik:RadTextBox ID="txtComments" Visible="true" runat="server" CssClass="input" TextMode="MultiLine"
                                            onkeyDown="checkTextAreaMaxLength(this,event,'500');" Width="300px" Height="30px">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <%--                </td>
            </tr>
        </table>--%>
        <table>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblstatus" runat="server" Text="Status"></telerik:RadLabel>
                </td>
                <td>
                    <asp:RadioButtonList ID="rdbstatus" RepeatDirection="Horizontal" runat="server">
                        <asp:ListItem Text="Approved" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Rejected" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblinterviewby" runat="server" Text="Interviewer Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtinterviewername" runat="server"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lbldate" runat="server" Text="Interview Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtdate" runat="server"></eluc:Date>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblcomment" runat="server" Text="Remarks"></telerik:RadLabel>
                </td>
                <td>
                    <asp:TextBox ID="txtremark" runat="server" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
