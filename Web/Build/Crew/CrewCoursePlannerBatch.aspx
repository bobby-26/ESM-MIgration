<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCoursePlannerBatch.aspx.cs"
    Inherits="CrewCoursePlannerBatch" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="../UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Institution" Src="~/UserControls/UserControlInstitution.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Batch" Src="~/UserControls/UserControlBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmExtraApproval" Src="~/UserControls/UserControlCourseExtraApproval.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Batch Planner</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewBatchPlanner" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlBatchPlanner">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <asp:Literal ID="lblBatchPlanner" runat="server" Text="Batch Planner"></asp:Literal>
                    </div>
                </div>
                 <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuHeader" runat="server" OnTabStripCommand="MenuHeader_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                 <div class="subHeader" style="position: relative;">
                    <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                        <eluc:TabStrip ID="MenuBatch" runat="server" OnTabStripCommand="CrewBatch_TabStripCommand">
                        </eluc:TabStrip>
                    </span>
                </div>
               <%-- <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                    <eluc:TabStrip ID="MenuBatch" runat="server" OnTabStripCommand="CrewBatch_TabStripCommand">
                    </eluc:TabStrip>
                </div>--%>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblConfigureBatchList" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblBatchPlannerCourse" runat="server" Text="Course"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Course ID="ucCourse" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox"
                                    Enabled="false" />
                            </td>
                            <td>
                                <asp:Literal ID="lblCourseType" runat="server" Text="Course Type"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Hard runat="server" ID="ucCourseType" CssClass="readonlytextbox" AppendDataBoundItems="true"
                                    Enabled="false" HardTypeCode="103" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblBatchHistory" runat="server" Text="Batch History"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Batch ID="ucBatch" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    Enabled="true" IsOutside="true" />
                            </td>
                            <td>
                                <asp:Literal ID="lblCourseDurationindays" runat="server" Text="Course Duration(in days)"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCourseDuration" runat="server" CssClass="readonlytextbox" MaxLength="50"
                                    Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
               
                <br />
                <table cellpadding="1" cellspacing="1" width="100%">
                    
                    <tr>
                        <td>
                            <asp:Literal ID="lblInstitution" runat="server" Text="Institution"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Institution runat="server" ID="ucInstitution" AppendDataBoundItems="true" CssClass="input_mandatory"  />
                        </td>
                        <td>
                            <asp:Literal ID="lblCourse" runat="server" Text="Course"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Course ID="ddlCourse" runat="server" CourseList="<%#PhoenixRegistersDocumentCourse.ListDocumentCourse(null)%>"
                                ListCBTCourse="true" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblBatchNo" runat="server" Text="Batch No"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBatch" runat="server" CssClass="input_mandatory" MaxLength="50"
                                Width="60%"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblFromDate" runat="server" Text="From Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date runat="server" ID="txtFromDate" CssClass="input_mandatory" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblToDate" runat="server" Text="To Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date runat="server" ID="txtToDate" CssClass="input_mandatory"  />
                        </td>
                        <td>
                            <asp:Literal ID="lblDurationInDays" runat="server" Text="Duration(In Days)"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDuration" runat="server" CssClass="readonlytextbox" MaxLength="50"
                                Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard ID="ucStatus" runat="server" AppendDataBoundItems="true" Enabled="false"
                                CssClass="input" HardTypeCode="152" />
                        </td>
                        <td>
                            <asp:Literal ID="lblClosedYN" runat="server" Text="Closed Y/N"></asp:Literal>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkClosedYN" runat="server"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblPublishedYN" runat="server" Text="Published Y/N"></asp:Literal>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkPublishYN" runat="server" Enabled="false"></asp:CheckBox>
                        </td>
                        <td>
                            <asp:Label ID="lblNoofSeats" runat="server" Text="No of Seats"></asp:Label>
                        </td>
                        <td>
                            <eluc:Number ID="txtNoOfSeats" runat="server" CssClass="input txtNumber" MaxLength="5"
                                IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                    <td colspan="6">
                      <asp:Label ID="lblBatch" runat="server" ForeColor="Blue" Text="Note: To edit further ,please go to Batch Details "></asp:Label>
                    </td>
                    </tr>
                </table>
                <br />
                 <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuCrewBatchList" runat="server" OnTabStripCommand="CrewBatchList_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                <asp:GridView ID="gvBatchList" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvBatchList_RowCommand" OnRowDataBound="gvBatchList_ItemDataBound"
                        OnSorting="gvBatchList_Sorting" AllowSorting="true" OnRowEditing="gvBatchList_RowEditing"
                        ShowFooter="false" ShowHeader="true" EnableViewState="true" DataKeyNames="FLDBATCHID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblItemCodeHeader" runat="server" CommandName="Sort" CommandArgument="FLDBATCHNO"
                                        ForeColor="White">Batch No &nbsp;</asp:LinkButton>
                                    <img id="FLDBATCHNO" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBatchId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHID") %>'></asp:Label>
                                    <asp:Label ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkBatchNo" runat="server" CommandName="EDIT" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHID") %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCH") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblFromDate" runat="server" Text="From Date"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFromDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMDATE", "{0:dd/MMM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblToDate" runat="server" Text="To Date"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblToDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTODATE", "{0:dd/MMM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="180px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblBatchLocation" runat="server" Text="Batch Location"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblLocation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHLOCATION") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblPublishYNHeader" runat="server">
                                    Published Y/N
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPublishYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDPUBLISHEDYN").ToString().Equals("1"))?"Yes":"No" %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkPublishYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDPUBLISHEDYN").ToString().Equals("1"))?true:false %>'>
                                    </asp:CheckBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblPagenumber" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPages" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecords" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap align="center">
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
                 <eluc:ConfirmExtraApproval ID="ucConfirmExtraApproval" runat="server" OnConfirmMesage="btnCrewApprove_Click" OKText="Proceed" CancelText="Cancel" Visible="false" />
                      
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
