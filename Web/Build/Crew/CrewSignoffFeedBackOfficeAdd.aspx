<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewSignoffFeedBackOfficeAdd.aspx.cs"
    Inherits="Crew_CrewSignoffFeedBackOfficeAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewOperation" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sign off Feedback</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript">
            function checkTextAreaMaxLength(textBox, e, length) {

                var mLen = textBox["MaxLength"];
                if (null == mLen)
                    mLen = length;

                var maxLength = parseInt(mLen);
                if (!checkSpecialKeys(e)) {
                    if (textBox.value.length > maxLength - 1) {
                        if (window.event)//IE
                            e.returnValue = false;
                        else//Firefox
                            e.preventDefault();
                    }
                }
            }

            function checkSpecialKeys(e) {
                if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
                    return false;
                else
                    return true;
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSignoffFBQuestion" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <div class="navigation" id="Div1" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false" />

                <eluc:TabStrip ID="FeedBackTabs" runat="server" OnTabStripCommand="FeedBackTabs_TabStripCommand"
                    TabStrip="false"></eluc:TabStrip>

                <%--<div class="subHeader" style="position: relative;">
                    <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                        <eluc:TabStrip ID="CrewFeedBack" runat="server" OnTabStripCommand="CrewFeedBack_TabStripCommand">
                        </eluc:TabStrip>
                    </span>
                </div>--%>
                <div id="divPrimarySection" runat="server">
                    <h3>
                        <telerik:RadLabel ID="lblPrimaryDetails" runat="server" Text="Primary Details"></telerik:RadLabel>
                    </h3>
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtFirstName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtMiddleName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtLastName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
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
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Vessel ID="ddlVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true" Entitytype="VSL" ActiveVesselsOnly="true"
                                    CssClass="input_mandatory" AppendItemPreSea="true" VesselList="<%#PhoenixRegistersVessel.ListVessel() %>" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblSignOnDate" runat="server" Text="SignOn Date"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="txtSignOnDate" runat="server" CssClass="input_mandatory" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="dvFeedBackQst" runat="server">
                    <hr />
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    </div>
                    <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                        <%-- <asp:GridView ID="gvFeedBackQst" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnRowDataBound="gvFeedBackQst_RowDataBound">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />--%>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvFeedBackQst" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                            CellSpacing="0" GridLines="None" OnNeedDataSource="gvFeedBackQst_NeedDataSource"
                            OnItemDataBound="gvFeedBackQst_ItemDataBound"
                            GroupingEnabled="false" EnableHeaderContextMenu="true"
                       
                            AutoGenerateColumns="false">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Fixed">
                                <HeaderStyle Width="102px" />
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="S.No">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10px"></HeaderStyle>
                                       
                                        <ItemTemplate>
                                            <%#Container.DataSetIndex+1 %>
                                        </ItemTemplate>
                                        
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText=" Feed Back Questions">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                      
                                        <ItemTemplate>
                                            <table cellspacing="10">
                                                <tr>
                                                    <td style="font-weight: bold;">
                                                        <telerik:RadLabel ID="lblQuestionId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDQUESTIONID")%>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblCommentsyn" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDISCOMMENTSYN")%>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblQuestionName" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDQUESTIONNAME")%>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblOrder" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDORDERNO")%>'
                                                            Visible="false"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButtonList ID="rblOptions" runat="server" DataValueField="FLDOPTIONID"
                                                            DataTextField="FLDOPTIONNAME" DataSource='<%# PhoenixCrewSignOffFeedBack.GetOptionsforQuestion(General.GetNullableInteger((DataBinder.Eval(Container,"DataItem.FLDQUESTIONID")).ToString())) %>'
                                                            RepeatDirection="Horizontal">
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr runat="server" id="trcomments">
                                                    <td>Comments (If Any)<br />
                                                        <telerik:RadTextBox ID="txtComments" runat="server" CssClass="input" TextMode="MultiLine"
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
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true"  ScrollHeight="" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </div>
                </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
