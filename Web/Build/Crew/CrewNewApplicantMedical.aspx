<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewNewApplicantMedical.aspx.cs"
    Inherits="CrewNewApplicantMedical" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Medical</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewMedical" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%; position: absolute;">

                <img src="<%$ PhoenixTheme:images/attachment.png %>" id="imgClip" runat="server" style="float: left; top: 5px; left: 10%; position: absolute" />

                <eluc:TabStrip ID="CrewMedical1" runat="server" Title="Crew Medical" OnTabStripCommand="CrewMedical1_TabStripCommand"></eluc:TabStrip>


                <table width="90%" cellpadding="1" cellspacing="1">
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
                            <telerik:RadLabel ID="lblAppliedRank" runat="server" Text="Applied Rank"></telerik:RadLabel>

                        </td>
                        <td colspan="5">
                            <telerik:RadTextBox ID="txtRank" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
                <hr />

                <br />
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td colspan="4">
                            <b>Medical History</b>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblDiseaseSuffered" runat="server" Text="Did you suffer or Are you Presently suffering from any Disease likely to render<br/>
                            you unfit for Service at Sea or likely to endanger the health of others on board.">
                            </telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <asp:RadioButtonList ID="rblDiseaseSuffered" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                <asp:ListItem Value="0">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblDrugAddict" runat="server" Text="Are you addicted to alcohol or drugs of any kind."></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <asp:RadioButtonList ID="rblDrugAddict" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                <asp:ListItem Value="0">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblSuffered" runat="server" Text="Have you suffered from following"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <asp:CheckBoxList ID="cblSuffered" runat="server" RepeatColumns="4" RepeatDirection="Horizontal">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblPsychiatric" runat="server" Text="Did you ever undergo psychiatric treatment"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <asp:RadioButtonList ID="rblPsychiatric" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                <asp:ListItem Value="0">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblSingedOffReason" runat="server" Text="Have you ever signed off from a ship due to Medical reasons"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <asp:RadioButtonList ID="rblSignedOffReason" runat="server" RepeatDirection="Horizontal"
                                AutoPostBack="true" OnSelectedIndexChanged="rblSignedOffReason_SelectedIndexChanged">
                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                <asp:ListItem Value="0">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>

                <eluc:TabStrip ID="CrewMedicalHistory" runat="server" OnTabStripCommand="CrewMedicalHistory_TabStripCommand"></eluc:TabStrip>

                <%--  <asp:GridView ID="gvMedical" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" ShowFooter="true" ShowHeader="true" EnableViewState="false"
                OnRowCommand="gvMedical_RowCommand" OnRowCancelingEdit="gvMedical_RowCancelingEdit" OnRowEditing="gvMedical_RowEditing"
                OnRowUpdating="gvMedical_RowUpdating" OnRowDeleting="gvMedical_RowDeleting" OnRowDataBound="gvMedical_RowDataBound">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                <RowStyle Height="10px" />--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvMedical" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvMedical_NeedDataSource"
                    OnItemCommand="gvMedical_ItemCommand"
                    OnItemDataBound="gvMedical_ItemDataBound"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top" ShowFooter="true">
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
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                        <Columns>

                            <telerik:GridTemplateColumn HeaderText="Vessel Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDVESSELNAME") %>
                                    <telerik:RadLabel ID="lblMedicalHistoryId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMEDICALHISTORYID") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblMedicalHistoryIdEdit" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMEDICALHISTORYID") %>'></telerik:RadLabel>
                                    <telerik:RadTextBox ID="txtVesselNameEdit" runat="server" CssClass="gridinput_mandatory" MaxLength="100"
                                        Text='<%#DataBinder.Eval(Container, "DataItem.FLDVESSELNAME") %>'>
                                    </telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtVesselNameAdd" runat="server" CssClass="gridinput_mandatory" ToolTip="Vessel Name"></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Date of Occurrence">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDDATEOFOCCURRENCE", "{0:dd/MMM/yyyy}") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="txtOccurrenceDateEdit" runat="server" CssClass="input_mandatory" Text=' <%#DataBinder.Eval(Container, "DataItem.FLDDATEOFOCCURRENCE", "{0:dd/MMM/yyyy}") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Date ID="txtOccurrenceDateAdd" runat="server" CssClass="input_mandatory" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Description">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtDescriptionEdit" runat="server" CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION")%>'
                                        MaxLength="300" ToolTip="Brief Description of Illness / Injury/ Accident">
                                    </telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtDescriptionAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="300"
                                        ToolTip="Brief Description of Illness / Injury/ Accident">
                                    </telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDTYPEOFMEDICALCASENAME")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblTypeofcaseEdit" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDTYPEOFMEDICALCASE") %>'></telerik:RadLabel>

                                    <eluc:Hard ID="ucTypeofcaseEdit" runat="server" AppendDataBoundItems="true"
                                        CssClass="dropdown_mandatory" HardList="<%# PhoenixRegistersHard.ListHard(1,174) %>"
                                        HardTypeCode="174" SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFMEDICALCASE") %>' />
                                </EditItemTemplate>

                                <FooterTemplate>

                                    <eluc:Hard ID="ucTypeofcaseAdd" runat="server" AppendDataBoundItems="true"
                                        CssClass="dropdown_mandatory" HardList="<%# PhoenixRegistersHard.ListHard(1,174) %>"
                                        HardTypeCode="174" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblActionHeader" runat="server">
                                        Action
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                        ToolTip="Edit">
                                     <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>

                                    <asp:LinkButton runat="server" AlternateText="Delete"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                        ToolTip="Delete">
                                    <span class="icon"><i class="fa fa-trash"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Save"
                                        CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                        ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                    </asp:LinkButton>

                                    <asp:LinkButton runat="server" AlternateText="Cancel"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                    </asp:LinkButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Save"
                                        CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                        ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                    </asp:LinkButton>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>

                <eluc:Status ID="ucStatus" runat="server" />
            </div>
        </telerik:RadAjaxPanel>
    </form>

</body>
</html>
