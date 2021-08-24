<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewMedical.aspx.cs" Inherits="CrewMedical"
    EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

        <img src="<%$ PhoenixTheme:images/attachment.png %>" id="imgClip" runat="server"
            style="float: left; top: 5px; left: 10%; position: absolute" />


        <eluc:TabStrip ID="CrewMedical1" Title="Crew Medical" runat="server" OnTabStripCommand="CrewMedical1_TabStripCommand"></eluc:TabStrip>

        <div>
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
        </div>
        <br />
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td colspan="2">
                    <b>
                        <telerik:RadLabel ID="lblMedicalHistory" runat="server" Text="Medical History"></telerik:RadLabel>
                    </b>
                </td>
            </tr>

            <tr>
                <td>
                    <telerik:RadLabel ID="lblDidyousufferorAreyouPresentlysufferingfromanyDiseaselikelytorenderyouunfitforServiceatSeaorlikelytoendangerthehealthofothersonboard"
                        runat="server" Text="Did you suffer or Are you Presently suffering from any Disease likely to render<br />you unfit for Service at Sea or likely to endanger the health of others on board.">
                    </telerik:RadLabel>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblDiseaseSuffered" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">Yes</asp:ListItem>
                        <asp:ListItem Value="0">No</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblAreyouaddictedtoalcoholordrugsofanykind" runat="server" Text="Are you addicted to alcohol or drugs of any kind."></telerik:RadLabel>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblDrugAddict" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">Yes</asp:ListItem>
                        <asp:ListItem Value="0">No</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblHaveyousufferedfromfollowing" runat="server" Text="Have you suffered from following"></telerik:RadLabel>
                </td>
                <td>
                    <asp:CheckBoxList ID="cblSuffered" runat="server" RepeatColumns="4" RepeatDirection="Horizontal">
                    </asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblDidyoueverundergopsychiatrictreatment" runat="server" Text="Did you ever undergo psychiatric treatment"></telerik:RadLabel>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblPsychiatric" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">Yes</asp:ListItem>
                        <asp:ListItem Value="0">No</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblHaveyoueversignedofffromashipduetoMedicalreasons" runat="server"
                        Text="Have you ever signed off from a ship due to Medical reasons">
                    </telerik:RadLabel>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblSignedOffReason" runat="server" RepeatDirection="Horizontal"
                        AutoPostBack="true" OnSelectedIndexChanged="rblSignedOffReason_SelectedIndexChanged">
                        <asp:ListItem Value="1">Yes</asp:ListItem>
                        <asp:ListItem Value="0">No</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>

        <eluc:TabStrip ID="MenuCrewMedical" runat="server" OnTabStripCommand="CrewMedical_TabStripCommand"></eluc:TabStrip>

        <%-- <asp:GridView ID="gvMedical" runat="server" AutoGenerateColumns="False" Font-Size="11px"
            Width="100%" CellPadding="3" ShowFooter="true" ShowHeader="true" EnableViewState="false"
            OnRowCommand="gvMedical_RowCommand" OnRowCancelingEdit="gvMedical_RowCancelingEdit"
            OnRowEditing="gvMedical_RowEditing" OnRowUpdating="gvMedical_RowUpdating" OnRowDeleting="gvMedical_RowDeleting"
            OnRowDataBound="gvMedical_RowDataBound">
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
                AutoGenerateColumns="false" TableLayout="Fixed" ShowFooter="true">
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

                    <telerik:GridTemplateColumn HeaderText="Date of Incident">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%#  SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATEOFOCCURRENCE"))%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Date ID="txtOccurrenceDateEdit" runat="server" CssClass="input_mandatory" Text=' <%#DataBinder.Eval(Container, "DataItem.FLDDATEOFOCCURRENCE", "{0:dd/MMM/yyyy}") %>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <eluc:Date ID="txtOccurrenceDateAdd" runat="server" CssClass="input_mandatory" />
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDVESSELNAME") %>
                            <telerik:RadLabel ID="lblMedicalHistoryId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMEDICALHISTORYID") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lblMedicalHistoryIdEdit" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMEDICALHISTORYID") %>'></telerik:RadLabel>
                            <telerik:RadTextBox ID="txtVesselNameEdit" runat="server" CssClass="gridinput_mandatory"
                                MaxLength="100" Text='<%#DataBinder.Eval(Container, "DataItem.FLDVESSELNAME") %>'>
                            </telerik:RadTextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <telerik:RadTextBox ID="txtVesselNameAdd" runat="server" CssClass="gridinput_mandatory"
                                ToolTip="Vessel">
                            </telerik:RadTextBox>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Description">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="txtDescriptionEdit" runat="server" CssClass="gridinput_mandatory"
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION")%>' MaxLength="300"
                                ToolTip="Brief Description of Illness / Injury/ Accident">
                            </telerik:RadTextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <telerik:RadTextBox ID="txtDescriptionAdd" runat="server" CssClass="gridinput_mandatory"
                                MaxLength="300" ToolTip="Brief Description of Illness / Injury/ Accident">
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
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>

        <br />
        <b>Vessel Medical Reports</b>

        <eluc:TabStrip ID="MenuMedicalCase" runat="server" OnTabStripCommand="MenuMedicalCase_TabStripCommand"></eluc:TabStrip>

        <%-- <asp:GridView ID="gvDeficiency" runat="server" AutoGenerateColumns="False" Font-Size="11px" OnRowDataBound="gvDeficiency_ItemDataBound"
            Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" DataKeyNames="FLDPNIMEDICALCASEID">
            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
            <RowStyle Height="10px" />--%>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvDeficiency" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvDeficiency_NeedDataSource"
            OnItemCommand="gvDeficiency_ItemCommand"
            OnItemDataBound="gvDeficiency_ItemDataBound1"
            GroupingEnabled="false" EnableHeaderContextMenu="true"
            AutoGenerateColumns="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed"  DataKeyNames="FLDPNIMEDICALCASEID">
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
                    <telerik:GridTemplateColumn HeaderText="Illness Date">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Illness Date
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#  SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATEOFILLNESS"))%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Vessel
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText="Illness Description">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblDescriptionHeader" runat="server">Description</telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <eluc:ToolTip ID="ucToolTipAddress" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDILLNESSDESCRIPTION") %>' />
                            <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDILLNESSDESCRIPTION") %>'
                                Visible="false">
                            </telerik:RadLabel>
                            <asp:LinkButton id="imgRemarks" runat="server" 
                                 >
                                <span class="icon"><i class="fas fa-glasses"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Type">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDTYPEOFMEDICALCASENAME")%>
                        </ItemTemplate>
                        <FooterTemplate>
                            <eluc:Hard ID="ucTypeofcaseAdd" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                HardTypeCode="174" AutoPostBack="true" Width="50%" />
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText="User">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDCASEOPENEDBY")%>
                        </ItemTemplate>
                        <FooterTemplate>
                            <telerik:RadTextBox ID="txtUserEditAdd" runat="server" CssClass="gridinput_mandatory"></telerik:RadTextBox>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Ref Number">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Reference Number
                        </HeaderTemplate>
                        <%-- <ItemTemplate>--%>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPNIMedicalCaseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPNIMEDICALCASEID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                            <asp:LinkButton ID="lnkRefNumber" runat="server" CommandName="Select" CommandArgument='<%# Container.DataSetIndex %>'
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENO") %>'></asp:LinkButton>
                            <telerik:RadLabel ID="lblRefNumber" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENO") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <%--                                <%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENO") %>
                            </ItemTemplate>
                        --%>
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
                            <%--                                    <asp:ImageButton runat="server" AlternateText="Sickness Report" ImageUrl="<%$ PhoenixTheme:images/incident-report.png%>"
                                        CommandName="SICKNESSREPORT" CommandArgument='<%# Container.DataSetIndex %>'
                                        ID="cmdSicknessReport" ToolTip="Sickness Report"></asp:ImageButton>
                                    <asp:ImageButton runat="server" AlternateText="PNI Checklist" CommandName="CHECKLIST"
                                        ImageUrl="<%$ PhoenixTheme:images/checklist.png %>" ID="cmdChkList" ToolTip="PNI Checklist">
                                    </asp:ImageButton>
                            --%>
                            <asp:LinkButton runat="server" AlternateText="Attachment"
                                CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAttachment"
                                ToolTip="Medical Unfit Report">
                                <span class="icon"><i class="fas fa-paperclip"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>

        <eluc:Status ID="ucStatus" runat="server" />
    </form>
</body>
</html>
