<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewWarnList.aspx.cs" Inherits="CrewWarnList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/UserControls/UserControlTabsTelerik.ascx" TagPrefix="eluc" TagName="TabStrip" %>
<%@ Register Src="~/UserControls/UserControlErrorMessage.ascx" TagPrefix="eluc" TagName="Error" %>
<%@ Register Src="~/UserControls/UserControlNationality.ascx" TagName="UserControlNationality" TagPrefix="eluc" %>
<%@ Register Src="~/UserControls/UserControlRank.ascx" TagName="UserControlRank" TagPrefix="eluc" %>
<%@ Register Src="~/UserControls/UserControlDate.ascx" TagName="UserControlDate" TagPrefix="eluc" %>
<%@ Register Src="~/UserControls/UserControlStatus.ascx" TagName="UserControlStatus" TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Warn List</title>

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmCrewPersonal" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>

        <telerik:RadAjaxPanel ID="panel1" runat="server" Height="90%">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <eluc:TabStrip ID="CrewWarnListTab" runat="server" OnTabStripCommand="CrewWarnListTab_TabStripCommand"></eluc:TabStrip>

            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFirstname" runat="server"  MaxLength="200"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtMiddleName" runat="server"  MaxLength="200"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLastName" runat="server"  MaxLength="200"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPassportNumber" runat="server" Text="Passport Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPassport" runat="server" CssClass="input_mandatory upperCase"
                            MaxLength="50">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSeamansBook" runat="server" Text="CDC No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSeamenBookNumber" runat="server" CssClass="input_mandatory upperCase" MaxLength="50"></telerik:RadTextBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblNationality" runat="server" Text="Nationality"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlNationality ID="ddlNationality" runat="server" AppendDataBoundItems="true"
                            CssClass="dropdown_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbldateofBirth" runat="server" Text="D.O.B"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtDateofBirth" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastRank" runat="server" Text="Last Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlRank ID="ddlRank" runat="server" AppendDataBoundItems="true"  />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastShip" runat="server" Text="Last Ship"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLastShip" runat="server"  MaxLength="100"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCompany" runat="server"  MaxLength="200"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReason" runat="server" Text="Reason"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtReason" runat="server" TextMode="MultiLine"  MaxLength="500"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSelectPrincipal" runat="server" Text="Select Principal/Manager"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblPrincipalManager" runat="server" RepeatDirection="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="PrincipalManagerClick">
                            <asp:ListItem Value="1" Selected="true">Manager</asp:ListItem>
                            <asp:ListItem Value="2">Principal</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPrincipalManager" runat="server" Text="Principal/Manager"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:AddressType ID="ddlManager" runat="server" AddressType="126" AppendDataBoundItems="true"
                            CssClass="dropdown_mandatory" />
                        <div runat="server" visible="false" id="dvAddressType" class="input_mandatory" style="overflow: auto; width: 50%; height: 100px">
                            <telerik:RadCheckBoxList runat="server" ID="cblAddressType" Height="100%" Columns="1"
                                RepeatDirection="Horizontal" RepeatLayout="Flow" data>
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>
            <br />


            <eluc:TabStrip ID="MenuWarnList" runat="server" OnTabStripCommand="MenuWarnList_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvWarnList" runat="server" Height="50%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvWarnList_NeedDataSource"
                OnItemCommand="gvWarnList_ItemCommand"
                OnItemDataBound="gvWarnList_ItemDataBound"
                OnSortCommand="gvWarnList_SortCommand"
                GroupingEnabled="false" EnableHeaderContextMenu="true"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
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
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridButtonColumn Text="SingleClick" CommandName="Edit" Visible="false" />
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWarnListId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDWARNLISTID") %>'></telerik:RadLabel>
                                <%#DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Passport Number">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPassportNo" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                    Text='<%#DataBinder.Eval(Container,"DataItem.FLDPASSPORTNO") %>' CommandName="EDIT"></asp:LinkButton>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CDC No">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDSEAMANBOOKNO")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="First Name">

                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDFIRSTNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Middle Name">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDMIDDLENAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Name">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDLASTNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Rank">

                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nationality">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDNATIONALITYNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                    ToolTip="Edit"><span class="icon"><i class="fas fa-edit"></i></span></asp:LinkButton>
                                <img id="Img1" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" runat="server" />
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete"><span class="icon"><i class="fa fa-trash"></i></span></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <eluc:UserControlStatus ID="ucStatus" runat="server" />

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
