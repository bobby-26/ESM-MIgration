<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterVisa.aspx.cs" Inherits="Registers_RegisterVisa" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>

<%@ Register Src="~/UserControls/UserControlTabsTelerik.ascx" TagPrefix="eluc" TagName="TabStrip" %>
<%@ Register Src="~/UserControls/UserControlTitle.ascx" TagPrefix="eluc" TagName="Title" %>
<%@ Register Src="~/UserControls/UserControlErrorMessage.ascx" TagPrefix="eluc" TagName="Error" %>
<%@ Register Src="~/UserControls/UserControlStatus.ascx" TagName="UserControlStatus"
    TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Country Visa</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVisa" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="Div1" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">


                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

                <eluc:TabStrip ID="MenuVisaList" runat="server" OnTabStripCommand="MenuVisaList_TabStripCommand"></eluc:TabStrip>

                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <%-- <asp:GridView ID="gvVisa" runat="server" AutoGenerateColumns="False" GridLines="None"
                        Font-Size="11px" Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false"
                        OnRowCommand="gvVisa_RowCommand" OnRowDataBound="gvVisa_ItemDataBound" OnRowUpdating="gvVisa_RowUpdating"
                        OnRowEditing="gvVisa_RowEditing" OnRowDeleting="gvVisa_RowDeleting" OnRowCancelingEdit="gvVisa_RowCancelingEdit"
                        AllowSorting="true" ShowFooter="true">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvVisa" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvVisa_NeedDataSource"
                        OnItemCommand="gvVisa_ItemCommand"
                        OnItemDataBound="gvVisa_ItemDataBound1"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false" ShowFooter="true">
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
                                <telerik:GridTemplateColumn HeaderText="Seafarer" HeaderStyle-Width="80%">
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />

                                    <ItemTemplate>
                                        <table width="100%" cellpadding="1" cellspacing="1">
                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltCountryName" runat="server" Text="Country:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <telerik:RadLabel ID="lblVisaID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFARERVISAID") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblCountryName" runat="server" Text='<%#Bind("FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblCountryID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYCODE") %>'></telerik:RadLabel>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltVisaType" runat="server" Text="Visa Type:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="20%">
                                                    <telerik:RadLabel ID="lblVisaType" runat="server" Text='<%#Bind("FLDSEAFARERVISANAME") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblVisaTypeID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFARERVISATYPE") %>'></telerik:RadLabel>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltLocSubmission" runat="server" Text="Location for Submission:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="21%">
                                                    <telerik:RadLabel ID="lblLocSubmission" runat="server" Text='<%#Bind("FLDSEAFARERLOCSUBMISSION") %>'></telerik:RadLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltOnArrival" runat="server" Text="On Arrival:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <telerik:RadLabel ID="lblOnArrival" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFARERONARRIVALYESNO") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblOnArrivalID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFARERONARRIVAL") %>'></telerik:RadLabel>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltDaysRequired" runat="server" Text="Days Required:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="20%">
                                                    <telerik:RadLabel ID="lblDaysRequired" runat="server" Text='<%#Bind("FLDSEAFARERDAYSREQUIREDFORVISA") %>'></telerik:RadLabel>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltPhysicalPresenceYN" runat="server" Text="Physical Presence Y/N:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="21%">
                                                    <telerik:RadLabel ID="chkPhysicalPresenceYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFARERPHYSICALPRESENCEYESNO") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblPhysicalPresenceYNID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFARERPHYSICALPRESENCEYN") %>'></telerik:RadLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltPysicalPresenceSpecification" runat="server" Text="Physical Presence Specification:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <telerik:RadLabel ID="lblPhyPresenceTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFARERPHYSICALPRESENCESPECIFICATION") %>'
                                                        Visible="false">
                                                    </telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblPhysicalPresenceSpecification" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFARERPHYSICALPRESENCESPECIFICATION") %>'></telerik:RadLabel>
                                                    <eluc:ToolTip ID="ucPhyPresenceTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFARERPHYSICALPRESENCESPECIFICATION") %>' />
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblUrgentProcedureHeader" runat="server" Text="Urgent Procedure:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="20%">
                                                    <telerik:RadLabel ID="lblUrgentProcedureText" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFARERURGENTPROCEDURE") %>'
                                                        Visible="false">
                                                    </telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblUrgentProcedure" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSEAFARERURGENTPROCEDURE").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDSEAFARERURGENTPROCEDURE").ToString().Substring(0, 10) + "..." : DataBinder.Eval(Container, "DataItem.FLDSEAFARERURGENTPROCEDURE").ToString() %>'></telerik:RadLabel>
                                                    <eluc:ToolTip ID="ucUrgentProcTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFARERURGENTPROCEDURE") %>' />
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblPassportHeader" runat="server" Text="Valid with old PP no:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="21%">
                                                    <telerik:RadLabel ID="lblPassport" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSEAFARERNOTVALIDONOLDPASSPORTYN") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblPassportID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFERENOTVALIDONOLDPASSPORT") %>'></telerik:RadLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblOrdinaryAmountUSD" runat="server" Text="Ordinary Amount(USD):"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <telerik:RadLabel ID="lblOrdinaryAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFEREORDINARYAMOUNT") %>'></telerik:RadLabel>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblUrgentAmountUSD" runat="server" Text="Urgent Amount(USD):"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="20%">
                                                    <telerik:RadLabel ID="lblUrgentAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFEREURGENTAMOUNT") %>'></telerik:RadLabel>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblRemarksHeader" runat="server" Text="Remarks:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="21%">
                                                    <asp:LinkButton   id="imgRemarks" runat="server" style="cursor: hand" 
                                                        onmousedown="javascript:closeMoreInformation()">
                                                        <span class="icon"><i class="fas fa-glasses"></i></span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton   id="imgNoRemarks" runat="server" style="cursor: hand" 
                                                        onmousedown="javascript:closeMoreInformation()" >
                                                        <span class="icon" style="color:gray"><i class="fas fa-glasses"></i></span>
                                                    </asp:LinkButton>
                                                    <telerik:RadLabel runat="server" ID="lblRemarks" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFERERREMARKS") %>'></telerik:RadLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltModifiedBy" runat="server" Text="Last Modified By:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <telerik:RadLabel ID="lblModifiedBy" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFARERMODIFIEDBYNAME") %>'></telerik:RadLabel>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblModifiedDateHeader" runat="server" Text="Modified Date:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="20%">
                                                    <telerik:RadLabel ID="lblModifiedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFERERMODIFIEDDATE") %>'></telerik:RadLabel>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <table width="100%" cellpadding="1" cellspacing="1">
                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltEditCountryName" runat="server" Text="Country:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <telerik:RadLabel ID="lblEditVisaID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFARERVISAID") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblEditCountryName" runat="server" Text='<%#Bind("FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblEditCountryID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYCODE") %>'></telerik:RadLabel>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltEditVisaType" runat="server" Text="Visa Type:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="20%">
                                                    <telerik:RadLabel ID="lblEditVisaType" runat="server" Text='<%#Bind("FLDSEAFARERVISANAME") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblEditVisaTypeID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFARERVISATYPE") %>'></telerik:RadLabel>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltEditLocSubmission" runat="server" Text="Location for Submission:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="21%">
                                                    <telerik:RadTextBox ID="txtEditLocSubmission" runat="server" CssClass="gridinput_mandatory"
                                                        MaxLength="100" ToolTip="Enter Time Taken" Text='<%#Bind("FLDSEAFARERLOCSUBMISSION") %>'>
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltEditOnArrival" runat="server" Text="On Arrival:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <asp:CheckBox ID="chkEditOnArrival" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDSEAFARERONARRIVAL").ToString() == "1" ? true : false%>' />
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltEditDaysRequired" runat="server" Text="Days Required:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="20%">
                                                    <telerik:RadTextBox ID="txtEditDaysRequired" runat="server" CssClass="gridinput_mandatory"
                                                        MaxLength="100" ToolTip="Enter Days Required For Visa Processing" Text='<%#Bind("FLDSEAFARERDAYSREQUIREDFORVISA") %>'>
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltEditPhysicalPresenceYN" runat="server" Text="Physical Presence Y/N:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="21%">
                                                    <asp:CheckBox ID="chkEditPhysicalPresenceYN" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDSEAFARERPHYSICALPRESENCEYN").ToString() == "1" ? true : false%>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltEditPysicalPresenceSpecification" runat="server" Text="Physical Presence Specification:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <telerik:RadTextBox ID="txtPhysicalPresenceSpecificationEdit" runat="server" CssClass="input"
                                                        MaxLength="500" ToolTip="Enter Physical Presence Specification" Width="100%"
                                                        Text='<%#Bind("FLDSEAFARERPHYSICALPRESENCESPECIFICATION") %>'>
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblEditUrgentProcedureHeader" runat="server" Text="Urgent Procedure:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="20%">
                                                    <telerik:RadTextBox ID="txtUrgentProcedureEdit" runat="server" CssClass="gridinput_mandatory"
                                                        Width="100%" ToolTip="Enter Urgent Procedure" MaxLength="500" Text='<%#Bind("FLDSEAFARERURGENTPROCEDURE") %>'>
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblEditPassportHeader" runat="server" Text="Valid with old PP no:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="21%">
                                                    <asp:CheckBox ID="chkPassportYNEdit" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDSEAFERENOTVALIDONOLDPASSPORT").ToString() == "1" ? true : false%>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblEditOrdinaryAmountUSD" runat="server" Text="Ordinary Amount(USD):"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <eluc:Number ID="txtOrdinaryAmountEdit" runat="server" CssClass="input" Width="90px"
                                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFEREORDINARYAMOUNT") %>' />
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblEditUrgentAmountUSD" runat="server" Text="Urgent Amount(USD):"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="20%">
                                                    <eluc:Number ID="txtUrgentAmonutEdit" runat="server" CssClass="input" Width="90px"
                                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFEREURGENTAMOUNT") %>' />
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblEditRemarksHeader" runat="server" Text="Remarks:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="21%">
                                                    <asp:LinkButton   id="imgEditRemarks" runat="server" style="cursor: hand" 
                                                        onmousedown="javascript:closeMoreInformation()" >
                                                        <span class="icon"><i class="fas fa-glasses"></i></span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton   id="imgEditNoRemarks" runat="server" style="cursor: hand" 
                                                        onmousedown="javascript:closeMoreInformation()" >
                                                        <span class="icon" style="color:gray"><i class="fas fa-glasses"></i></span>
                                                    </asp:LinkButton>
                                                    <telerik:RadLabel runat="server" ID="txtRemarksEdit" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFERERREMARKS") %>'></telerik:RadLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltEditModifiedBy" runat="server" Text="Last Modified By:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <telerik:RadLabel ID="lblEditModifiedBy" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFARERMODIFIEDBYNAME") %>'></telerik:RadLabel>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblEditModifiedDateHeader" runat="server" Text="Modified Date:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="20%">
                                                    <telerik:RadLabel ID="lblEditModifiedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFERERMODIFIEDDATE") %>'></telerik:RadLabel>
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <table width="100%" cellpadding="4" cellspacing="1">
                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <%--   <telerik:RadLabel ID="ltVisaTypeAdd" runat="server" Visible="false" Text="Visa Type:"></telerik:RadLabel>--%>
                                                        <telerik:RadLabel ID="ltLocSubmissionAdd" runat="server" Text="Location for Submission:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <%--   <eluc:Hard ID="ucVisaTypeAdd" runat="server" Visible="false" CssClass="input" AppendDataBoundItems="false"
                                            HardTypeCode="107" ShortNameFilter="OFF" HardList='<%# PhoenixRegistersHard.ListHard(SouthNests.Phoenix.Framework.PhoenixSecurityContext.CurrentSecurityContext.UserCode, 107) %>' />--%>
                                                    <telerik:RadTextBox ID="txtLocSubmissionAdd" runat="server" CssClass="gridinput_mandatory"
                                                        MaxLength="100">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltDaysRequiredAdd" runat="server" Text="Days Required:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="20%">
                                                    <telerik:RadTextBox ID="txtDaysRequiredAdd" runat="server" CssClass="gridinput_mandatory"
                                                        MaxLength="100" ToolTip="Enter Days Required For Visa Processing">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltOnArrivalAdd" runat="server" Text="On Arrival:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="21%">
                                                    <asp:CheckBox ID="chkOnArrivalAdd" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblUrgentProcedureHeaderAdd" runat="server" Text="Urgent Procedure:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <telerik:RadTextBox ID="txtUrgentProcedureAdd" runat="server" CssClass="gridinput_mandatory"
                                                        Width="100%" ToolTip="Enter Urgent Procedure" MaxLength="500">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltPysicalPresenceSpecificationAdd" runat="server" Text="Physical Presence Specification:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="20%">
                                                    <telerik:RadTextBox ID="txtPhysicalPresenceSpecificationAdd" runat="server" CssClass="input"
                                                        MaxLength="500" ToolTip="Enter Physical Presence Specification" Width="100%">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltPhysicalPresenceYNAdd" runat="server" Text="Physical Presence Y/N:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="21%">
                                                    <asp:CheckBox ID="chkPhysicalPresenceYNAdd" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblUrgentAmountUSDAdd" runat="server" Text="Urgent Amount(USD):"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <eluc:Number ID="txtUrgentAmonutAdd" runat="server" CssClass="input" Width="90px" />
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblOrdinaryAmountUSDAdd" runat="server" Text="Ordinary Amount(USD):"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="20%">
                                                    <eluc:Number ID="txtOrdinaryAmountAdd" runat="server" CssClass="input" Width="90px" />
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblPassportAdd" runat="server" Text="Valid with old PP no:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="21%">
                                                    <asp:CheckBox ID="chkPassportYNAdd" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblRemarksHeaderAdd" runat="server" Text="Remarks:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <asp:LinkButton   id="imgRemarksAdd" runat="server" style="cursor: hand" 
                                                        onmousedown="javascript:closeMoreInformation()" >
                                                        <span class="icon"><i class="fas fa-glasses"></i></span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton   id="imgNoRemarksAdd" runat="server" style="cursor: hand" 
                                                        onmousedown="javascript:closeMoreInformation()" >
                                                        <span class="icon" style="color:gray"><i class="fas fa-glasses"></i></span>
                                                    </asp:LinkButton>
                                                    <telerik:RadLabel runat="server" ID="txtRemarksAdd" Visible="false"></telerik:RadLabel>
                                                </td>
                                                <%--  <td width="10%">
                                        <b></b>
                                    </td>
                                    <td width="20%">
                                    </td>--%>
                                            </tr>
                                        </table>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="20%">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Edit"
                                            CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                            ToolTip="Edit">
                                             <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Delete"
                                            CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                            ToolTip="Delete">
                                            <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Visa Documents"
                                            CommandName="VISADOCUMENTS" CommandArgument='<%# Container.DataSetIndex %>'
                                            ID="cmdVisaDocuments" ToolTip="Visa Documents">
                                             <span class="icon"><i class="fab fa-cc-visa"></i></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Send Mail"
                                            CommandName="SENDMAIL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSendMail"
                                            ToolTip="Send Mail">
                                            <span class="icon"><i class="fas fa-envelope"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Excel Export"
                                            CommandName="EXCEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdExcel"
                                            ToolTip="Excel Export">
                                             <span class="icon"><i class="fas fa-file-excel"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Attachment"
                                            CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAttachment"
                                            ToolTip="Attachment">
                                            <span class="icon"><i class="fas fa-paperclip"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="No Attachment"
                                            CommandName="NOATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdNoAttachment"
                                            ToolTip="No Attachment">
                                            <span class="icon" style="color:gray"><i class="fas fa-paperclip"></i></span>
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
                                            <span class="icon"><i class="fas fa-times-circle"></i></i></span>
                                        </asp:LinkButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <FooterTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save"
                                            CommandName="Add" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                            ToolTip="Add New">
                                            <span class="icon"><i class="fa fa-plus-circle"></i></i></span>
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
                </div>
                <br />
                <div id="div2" style="position: relative; z-index: 0; width: 100%;">
                    <%-- <asp:GridView ID="gvFVisa" runat="server" AutoGenerateColumns="False" GridLines="None"
                        Font-Size="11px" Width="100%" CellPadding="3" EnableViewState="false" OnRowCommand="gvFVisa_RowCommand"
                        OnRowUpdating="gvFVisa_RowUpdating" OnRowEditing="gvFVisa_RowEditing" OnRowDataBound="gvFVisa_ItemDataBound"
                        OnRowDeleting="gvFVisa_RowDeleting" OnRowCancelingEdit="gvFVisa_RowCancelingEdit"
                        AllowSorting="true" ShowFooter="true">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvFVisa" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvFVisa_NeedDataSource"
                        OnItemCommand="gvFVisa_ItemCommand"
                        OnItemDataBound="gvFVisa_ItemDataBound"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false" ShowFooter="true">
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
                                <telerik:GridTemplateColumn HeaderText="Family" HeaderStyle-Width="80%">
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />

                                    <ItemTemplate>

                                        <table width="100%" cellpadding="1" cellspacing="1">

                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblFamilyCountry" runat="server" Text="Country:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <telerik:RadLabel ID="lblFVisaId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYVISAID") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblFCountry" runat="server" Text='<%#Bind("FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblFCountryCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYCODE") %>'></telerik:RadLabel>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltFVisaType" runat="server" Text="Visa Type:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="20%">
                                                    <telerik:RadLabel ID="lblFVisaName" runat="server" Text='<%#Bind("FLDFAMILYVISANAME") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblFVisaType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYVISATYPE") %>'></telerik:RadLabel>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltFLocSub" runat="server" Text="Location for Submission:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="21%">
                                                    <telerik:RadLabel ID="lblFLocSub" runat="server" Text='<%#Bind("FLDFAMILYLOCSUBMISSION") %>'></telerik:RadLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltFOnArrival" runat="server" Text="On Arrival:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <telerik:RadLabel ID="lblFOnArrival" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYONARRIVALYESNO") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblFOnArrivalID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYONARRIVAL") %>'></telerik:RadLabel>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltFDaysRequired" runat="server" Text="Days Required:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="20%">
                                                    <telerik:RadLabel ID="lblFDaysRequired" runat="server" Text='<%#Bind("FLDFAMILYDAYSREQUIREDFORVISA") %>'></telerik:RadLabel>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltFPhysicalPresenceYN" runat="server" Text="Physical Presence Y/N:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="21%">
                                                    <telerik:RadLabel ID="chkFPhysicalPresenceYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYPHYSICALPRESENCEYESNO") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblFPhysicalPresenceYNID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYPHYSICALPRESENCEYN") %>'></telerik:RadLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltFPysicalPresenceSpecification" runat="server" Text="Physical Presence Specification:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <telerik:RadLabel ID="lblFPhyPresenceTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYPHYSICALPRESENCESPECIFICATION") %>'
                                                        Visible="false">
                                                    </telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblFPhysicalPresenceSpecification" runat="server" Visible="false"
                                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYPHYSICALPRESENCESPECIFICATION") %>'>
                                                    </telerik:RadLabel>
                                                    <eluc:ToolTip ID="ucFPhyPresenceTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYPHYSICALPRESENCESPECIFICATION") %>' />
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblFUrgentProcedureHeader" runat="server" Text="Urgent Procedure:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="20%">
                                                    <telerik:RadLabel ID="lblFUrgentProcedureText" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYURGENTPROCEDURE") %>'
                                                        Visible="false">
                                                    </telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblFUrgentProcedure" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFAMILYURGENTPROCEDURE").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDFAMILYURGENTPROCEDURE").ToString().Substring(0, 10) + "..." : DataBinder.Eval(Container, "DataItem.FLDFAMILYURGENTPROCEDURE").ToString() %>'></telerik:RadLabel>
                                                    <eluc:ToolTip ID="ucFUrgentProcTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYURGENTPROCEDURE") %>' />
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblFPassportHeader" runat="server" Text="Valid with old PP no:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="21%">
                                                    <telerik:RadLabel ID="lblFPassport" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFAMILYNOTVALIDONOLDPASSPORTYN") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblFPassportID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYNOTVALIDONOLDPASSPORT") %>'></telerik:RadLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblFOrdinaryAmountUSD" runat="server" Text="Ordinary Amount(USD):"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <telerik:RadLabel ID="lblFOrdinaryAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYORDINARYAMOUNT") %>'></telerik:RadLabel>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblFUrgentAmountUSD" runat="server" Text="Urgent Amount(USD):"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="20%">
                                                    <telerik:RadLabel ID="lblFUrgentAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYURGENTAMOUNT") %>'></telerik:RadLabel>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblFRemarksHeader" runat="server" Text="Remarks:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="21%">
                                                    <asp:LinkButton   id="imgFRemarks" runat="server" style="cursor: hand;"
                                                        onmousedown="javascript:closeMoreInformation()" >
                                                        <span class="icon"><i class="fas fa-glasses"></i></span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton   id="imgFNoRemarks" runat="server" style="cursor: hand" 
                                                        onmousedown="javascript:closeMoreInformation()" >
                                                         <span class="icon" style="color: gray;"><i class="fas fa-glasses"></i></span>
                                                    </asp:LinkButton>
                                                    <telerik:RadLabel runat="server" ID="Label14" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYREMARKS") %>'></telerik:RadLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltFModifiedBy" runat="server" Text="Last Modified By:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <telerik:RadLabel ID="lblFModifiedBy" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYMODIFIEDBYNAME") %>'></telerik:RadLabel>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblFModifiedDateHeader" runat="server" Text="Modified Date:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="20%">
                                                    <telerik:RadLabel ID="lblFModifiedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYMODIFIEDDATE") %>'></telerik:RadLabel>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <table width="100%" cellpadding="2" cellspacing="1">
                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltEditCountryName" runat="server" Text="Country:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <telerik:RadLabel ID="lblEditVisaID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYVISAID") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblEditCountryName" runat="server" Text='<%#Bind("FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblEditCountryID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYCODE") %>'></telerik:RadLabel>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltEditVisaType" runat="server" Text="Visa Type:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="20%">
                                                    <telerik:RadLabel ID="lblEditVisaType" runat="server" Text='<%#Bind("FLDFAMILYVISANAME") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblEditVisaTypeID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYVISATYPE") %>'></telerik:RadLabel>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltEditLocSubmission" runat="server" Text="Location for Submission:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="21%">
                                                    <telerik:RadTextBox ID="txtEditLocSubmission" runat="server" CssClass="gridinput_mandatory"
                                                        MaxLength="100" ToolTip="Enter Time Taken" Text='<%#Bind("FLDFAMILYLOCSUBMISSION") %>'>
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltEditOnArrival" runat="server" Text="On Arrival:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <asp:CheckBox ID="chkEditOnArrival" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDFAMILYONARRIVAL").ToString() == "1" ? true : false%>' />
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltEditDaysRequired" runat="server" Text="Days Required:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="20%">
                                                    <telerik:RadTextBox ID="txtEditDaysRequired" runat="server" CssClass="gridinput_mandatory"
                                                        MaxLength="100" ToolTip="Enter Days Required For Visa Processing" Text='<%#Bind("FLDFAMILYDAYSREQUIREDFORVISA") %>'>
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltEditPhysicalPresenceYN" runat="server" Text="Physical Presence Y/N:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="21%">
                                                    <asp:CheckBox ID="chkEditPhysicalPresenceYN" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDFAMILYPHYSICALPRESENCEYN").ToString() == "1" ? true : false%>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltEditPysicalPresenceSpecification" runat="server" Text="Physical Presence Specification:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <telerik:RadTextBox ID="txtPhysicalPresenceSpecificationEdit" runat="server" CssClass="input"
                                                        MaxLength="500" ToolTip="Enter Physical Presence Specification" Width="100%"
                                                        Text='<%#Bind("FLDFAMILYPHYSICALPRESENCESPECIFICATION") %>'>
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblEditUrgentProcedureHeader" runat="server" Text="Urgent Procedure:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="20%">
                                                    <telerik:RadTextBox ID="txtUrgentProcedureEdit" runat="server" CssClass="gridinput_mandatory"
                                                        Width="100%" ToolTip="Enter Urgent Procedure" MaxLength="500" Text='<%#Bind("FLDFAMILYURGENTPROCEDURE") %>'>
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblEditPassportHeader" runat="server" Text="Valid with old PP no:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="21%">
                                                    <asp:CheckBox ID="chkPassportYNEdit" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDFAMILYNOTVALIDONOLDPASSPORT").ToString() == "1" ? true : false%>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblEditOrdinaryAmountUSD" runat="server" Text="Ordinary Amount(USD):"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <eluc:Number ID="txtOrdinaryAmountEdit" runat="server" CssClass="input" Width="90px"
                                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYORDINARYAMOUNT") %>' />
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblEditUrgentAmountUSD" runat="server" Text="Urgent Amount(USD):"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="20%">
                                                    <eluc:Number ID="txtUrgentAmonutEdit" runat="server" CssClass="input" Width="90px"
                                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYURGENTAMOUNT") %>' />
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblEditRemarksHeader" runat="server" Text="Remarks:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="21%">
                                                    <asp:LinkButton   id="imgEditRemarks" runat="server" style="cursor: hand" 
                                                        onmousedown="javascript:closeMoreInformation()" >
                                                        <span class="icon"><i class="fas fa-glasses"></i></span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton   id="imgEditNoRemarks" runat="server" style="cursor: hand" 
                                                        onmousedown="javascript:closeMoreInformation()" >
                                                        <span class="icon" style="color:gray;"><i class="fas fa-glasses"></i></span>
                                                    </asp:LinkButton>
                                                    <telerik:RadLabel runat="server" ID="txtRemarksEdit" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYREMARKS") %>'></telerik:RadLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltEditModifiedBy" runat="server" Text="Last Modified By:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <telerik:RadLabel ID="lblEditModifiedBy" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYMODIFIEDBYNAME") %>'></telerik:RadLabel>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblEditModifiedDateHeader" runat="server" Text="Modified Date:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="20%">
                                                    <telerik:RadLabel ID="lblEditModifiedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYMODIFIEDDATE") %>'></telerik:RadLabel>
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <table width="100%" cellpadding="3" cellspacing="1">
                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <%--     <telerik:RadLabel ID="ltVisaTypeAdd" Visible="false" runat="server" Text="Visa Type:"></telerik:RadLabel>--%>
                                                        <telerik:RadLabel ID="ltLocSubmissionAdd" runat="server" Text="Location for Submission:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <%--  <eluc:Hard ID="ucVisaTypeAdd" runat="server" Visible="false" AppendDataBoundItems="false"
                                            CssClass="input" HardTypeCode="107" ShortNameFilter="FMY" HardList='<%# PhoenixRegistersHard.ListHard(SouthNests.Phoenix.Framework.PhoenixSecurityContext.CurrentSecurityContext.UserCode, 107) %>' />--%>
                                                    <telerik:RadTextBox ID="txtLocSubmissionAdd" runat="server" CssClass="gridinput_mandatory"
                                                        MaxLength="100">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltDaysRequiredAdd" runat="server" Text="Days Required:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="20%">
                                                    <telerik:RadTextBox ID="txtDaysRequiredAdd" runat="server" CssClass="gridinput_mandatory"
                                                        MaxLength="100" ToolTip="Enter Days Required For Visa Processing">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltOnArrivalAdd" runat="server" Text="On Arrival:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="21%">
                                                    <asp:CheckBox ID="chkOnArrivalAdd" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblUrgentProcedureHeaderAdd" runat="server" Text="Urgent Procedure:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <telerik:RadTextBox ID="txtUrgentProcedureAdd" runat="server" CssClass="gridinput_mandatory"
                                                        Width="100%" ToolTip="Enter Urgent Procedure" MaxLength="500">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltPysicalPresenceSpecificationAdd" runat="server" Text="Physical Presence Specification:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="20%">
                                                    <telerik:RadTextBox ID="txtPhysicalPresenceSpecificationAdd" runat="server" CssClass="input"
                                                        MaxLength="500" ToolTip="Enter Physical Presence Specification" Width="100%">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="ltPhysicalPresenceYNAdd" runat="server" Text="Physical Presence Y/N:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="21%">
                                                    <asp:CheckBox ID="chkPhysicalPresenceYNAdd" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblUrgentAmountUSDAdd" runat="server" Text="Urgent Amount(USD):"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <eluc:Number ID="txtUrgentAmonutAdd" runat="server" CssClass="input" Width="90px" />
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblOrdinaryAmountUSDAdd" runat="server" Text="Ordinary Amount(USD):"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="20%">
                                                    <eluc:Number ID="txtOrdinaryAmountAdd" runat="server" CssClass="input" Width="90px" />
                                                </td>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblPassportAdd" runat="server" Text="Valid with old PP no:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="21%">
                                                    <asp:CheckBox ID="chkPassportYNAdd" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%">
                                                    <b>
                                                        <telerik:RadLabel ID="lblRemarksHeaderAdd" runat="server" Text="Remarks:"></telerik:RadLabel>
                                                    </b>
                                                </td>
                                                <td width="19%">
                                                    <asp:LinkButton   id="imgRemarksAdd" runat="server" style="cursor: hand" 
                                                        onmousedown="javascript:closeMoreInformation()" >
                                                        <span class="icon"><i class="fas fa-glasses"></i></span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton   id="imgNoRemarksAdd" runat="server" style="cursor: hand" 
                                                        onmousedown="javascript:closeMoreInformation()" >
                                                        <span class="icon" style="color:gray"><i class="fas fa-glasses"></i></span>
                                                    </asp:LinkButton>
                                                    <telerik:RadLabel runat="server" ID="txtRemarksAdd" Visible="false"></telerik:RadLabel>
                                                </td>
                                                <%--  <td width="10%">
                                        <b></b>
                                    </td>
                                    <td width="20%">
                                    </td>--%>
                                            </tr>
                                        </table>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="20%" HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Edit" 
                                            CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                            ToolTip="Edit">
                                             <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>
                                       
                                        <asp:LinkButton runat="server" AlternateText="Delete" 
                                            CommandName="DELETEFAMILY" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                            ToolTip="Delete"><span class="icon"><i class="fas fa-trash-alt"></i></span></asp:LinkButton>
                                    
                                        <asp:LinkButton runat="server" AlternateText="Visa Documents" 
                                            CommandName="VISADOCUMENTS" CommandArgument='<%# Container.DataSetIndex %>'
                                            ID="cmdVisaDocuments" ToolTip="Visa Documents">
                                            <span class="icon"><i class="fab fa-cc-visa"></i></i></span>
                                        </asp:LinkButton>
                                   
                                        <asp:LinkButton runat="server" AlternateText="Send Mail" 
                                            CommandName="SENDMAIL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSendMail"
                                            ToolTip="Send Mail">
                                             <span class="icon"><i class="fas fa-envelope"></i></span>
                                        </asp:LinkButton>
                                       
                                        <asp:LinkButton runat="server" AlternateText="Excel Export" 
                                            CommandName="EXCEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdExcel"
                                            ToolTip="Excel Export">
                                             <span class="icon"><i class="fas fa-file-excel"></i></span>
                                        </asp:LinkButton>
                                      
                                        <asp:LinkButton runat="server" AlternateText="Attachment" 
                                            CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAttachment"
                                            ToolTip="Attachment">
                                             <span class="icon"><i class="fas fa-paperclip"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="No Attachment" 
                                            CommandName="NOATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdNoAttachment"
                                            ToolTip="No Attachment">
                                             <span class="icon" style="color:gray"><i class="fas fa-paperclip"></i></span>
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
                                            <span class="icon"><i class="fas fa-times-circle"></i></i></span>
                                        </asp:LinkButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <FooterTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save"
                                            CommandName="Add" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                            ToolTip="Add New">
                                            <span class="icon"><i class="fa fa-plus-circle"></i></i></span>
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
                </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
