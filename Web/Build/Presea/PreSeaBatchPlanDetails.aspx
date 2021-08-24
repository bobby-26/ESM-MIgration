<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaBatchPlanDetails.aspx.cs"
    Inherits="PreSeaBatchPlanDetails" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlPreSeaCourse.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Batch - Details</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
      

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewDateOfAvailability" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlDOA">
        <ContentTemplate>
         <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">               
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <eluc:Title runat="server" ID="ucTitle" Text="Batch" ShowMenu="false" />
                    </div>
                    <div style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuBatchPlanner" runat="server" OnTabStripCommand="BatchPlanner_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </div>
                </div>
                <div class="subHeader" style="position: relative">
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                        <eluc:TabStrip ID="MenuPreSea" runat="server" OnTabStripCommand="MenuPreSea_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                </div>
                <table cellpadding="1" cellspacing="1" width="90%">
                    <tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblCourse" runat="server" Text="Course" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtCourse" runat="server" CssClass="readonlytextbox" Enabled="false" Width="120px"></asp:TextBox>                                
                            </td>
                            <td>
                                <asp:Literal ID="lblBatch" runat="server" Text="Batch" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtBatchName" runat="server" CssClass="readonlytextbox" Enabled="false" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblIMU" runat="server" Text=" IMU Entrance Applicable" />
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsIMU" TextAlign="Left" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblMinLimit" runat="server" Text="  No of Students (Min)" />
                            </td>
                            <td>
                                <eluc:Number runat="server" ID="ucMinLimit" CssClass="input_mandatory" IsInteger="true" Width="120px" />
                            </td>
                            <td>
                                <asp:Literal ID="lblMaxLimit" runat="server" Text="  No of Students (Max)" />
                            </td>
                            <td>
                                <eluc:Number runat="server" ID="ucMaxLimit" CssClass="input_mandatory" IsInteger="true" Width="120px" />
                            </td>
                            <td valign="top">
                                <asp:Literal ID="lblNoofSem" runat="server" Text="   No of Semesters"  />
                            </td>
                            <td valign="top">
                                <eluc:Number runat="server" ID="txtNoOfSem" CssClass="readonlytextbox"  IsInteger="true" Width="120px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblAppIssStrt" runat="server" Text="  Application Issue start" />
                            </td>
                            <td>
                                <eluc:Date ID="txtAppIssStrt" runat="server" CssClass="input_mandatory" Width="120px"/>
                            </td>
                            <td>
                                <asp:Literal ID="lblLstDtAppl" runat="server" Text="  Last date for Application" />
                            </td>
                            <td>
                                <eluc:Date ID="txtLstDtAppl" runat="server" CssClass="input_mandatory"  Width="120px"/>
                            </td>
                            <td valign="top">
                                <asp:Literal ID="lblScoreCard" runat="server" Text="   Score card Template" />
                            </td>
                            <td valign="top">
                                <asp:DropDownList ID="ddlScoreCard" runat="server" DataTextField="FLDSCORECARDNAME"
                                    CssClass="input_mandatory" DataValueField="FLDSCORECARDID">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Literal ID="lblTDCInCharge" runat="server" Text="TDC In-Charge" />
                            </td>
                            <td valign="baseline">
                                <span id="spnTDCIncharge">
                                    <asp:TextBox ID="txtTDCInchargeName" runat="server" CssClass="input" Enabled="false"
                                        MaxLength="50" Width="120px"></asp:TextBox>                                                                     
                                    <asp:ImageButton runat="server" ID="imgTDCIncharge" Style="cursor: pointer;
                                        vertical-align: top" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" />
                                     <asp:TextBox ID="txtTDCInchargeDesignation" runat="server" CssClass="input" Width="0px"
                                        Enabled="false" MaxLength="50"  ></asp:TextBox>   
                                    <asp:TextBox runat="server" ID="txtTDCInchargeId" CssClass="input" Width="0px" MaxLength="20"></asp:TextBox>
                                    <asp:TextBox runat="server" ID="txtTDCInchargeEmail" CssClass="input" Width="0px" MaxLength="20"></asp:TextBox>
                                </span>
                            </td>             
                            <td valign="top">
                                <asp:Literal ID="lblCourseInCharge" runat="server" Text=" Course In-Charge & Acting Dean" />
                            </td>
                             <td valign="baseline">
                                <span id="spnFacultyCourseIncharge">
                                    <asp:TextBox ID="txtFacultyCourseName" runat="server" CssClass="input" Enabled="false"
                                        MaxLength="50" Width="120px"></asp:TextBox>                                    
                                    <asp:ImageButton runat="server" ID="imgFacultyCourseIncharge" Style="cursor: pointer;
                                        vertical-align: top" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" />
                                     <asp:TextBox ID="txtFacultyCourseInchargeDesignation" runat="server" CssClass="input"
                                        Enabled="false" MaxLength="50" Width="0px" ></asp:TextBox>
                                    <asp:TextBox runat="server" ID="txtFacultyCourseInchargeId" CssClass="input" Width="0px" MaxLength="20"></asp:TextBox>
                                    <asp:TextBox runat="server" ID="txtFacultyCourseInchargeEmail" CssClass="input" Width="0px" MaxLength="20"></asp:TextBox>
                                </span>
                            </td> 
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr> <td colspan="6"></td></tr>
                </table>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuPreSeaBatch" runat="server" OnTabStripCommand="PreSeaBatch_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvPreSea" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvPreSea_RowCommand" OnRowDataBound="gvPreSea_RowDataBound"
                        OnRowDeleting="gvPreSea_RowDeleting" OnRowEditing="gvPreSea_RowEditing" OnRowCancelingEdit="gvPreSea_RowCancelingEdit"
                        ShowHeader="true" ShowFooter="false" Style="margin-bottom: 0px" EnableViewState="true">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblExamVenueInformation" runat="server" Text="Entrance Exam Venue Details" />
                                </HeaderTemplate>
                                <ItemTemplate>                                 
                                    <table width="100%" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td width="10%">
                                                <b>
                                                    <asp:Literal ID="lblExamVenue" runat="server" Text="Venue:"></asp:Literal></b>
                                                    <asp:Label ID="lblExamVenueID" runat="server" visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDEXAMVENUE") %>'></asp:Label>
                                                    <asp:Label ID="lblEntranceExamPlanID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENTRANCEEXAMPLANID") %>'></asp:Label>
                                            </td>
                                            <td width="20%">
                                                <asp:Label ID="lblVenue" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEXAMVENUENAME")%>'></asp:Label>                                                
                                            </td>
                                            <td width="10%">
                                                <b>
                                                    <asp:Literal ID="lblzone" runat="server" Text="Zone:"></asp:Literal></b>
                                            </td>
                                            <td width="20%">
                                                <asp:Label ID="lblZoneName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZONE") %>'></asp:Label>
                                            </td>
                                            <td width="10%">
                                                <b>
                                                    <asp:Literal ID="lblVenueAddr" runat="server" Text="Address:"></asp:Literal></b>
                                            </td>
                                            <td width="20%">
                                                <asp:Label ID="lblVenueAddress" runat="server" Text='  <%# DataBinder.Eval(Container, "DataItem.FLDVENUEADDRESS")%>'></asp:Label>
                                            </td>
                                            <td wisth="10%"></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>
                                                    <asp:Label ID="lblExamStartDate" runat="server" Text="Start Date:"></asp:Label></b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblStartDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSTARTDATE")%>'></asp:Label>
                                                <%-- <eluc:Date ID="txtStartDate" runat="server" CssClass="input_mandatory" />
                                                &nbsp;
                                                <asp:ImageButton ID="imgbtnTiming" BorderWidth="0" Style="cursor: pointer; vertical-align: top"
                                                    runat="server" ImageUrl="<%$ PhoenixTheme:images/time-schedule.png %>" ToolTip="Exam day time Schedule" />--%>
                                            </td>
                                            <td>
                                                <b>
                                                    <asp:Literal ID="lblNoofDay" runat="server" Text="No of days:"></asp:Literal>
                                                </b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblNoofdays" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDNOFDAYS")%>'></asp:Label>
                                                <%--<eluc:Number runat="server" ID="txtNoofdays" CssClass="input_mandatory" IsInteger="true" />--%>
                                            </td>
                                            <td>
                                                <b>
                                                    <asp:Literal ID="lblMedicalfee" runat="server" Text="Medical fee:"></asp:Literal></b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMedicalfees" runat="server" Text='  <%# DataBinder.Eval(Container, "DataItem.FLDMEDICALFEES")%>'></asp:Label>                                                
                                            </td>
                                            <td wisth="10%"></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>
                                                    <asp:Literal ID="lblPerson" runat="server" Text="Contact Person:"></asp:Literal></b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblContactPerson" runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDCONTACTPESON")%>'></asp:Label>                                                
                                            </td>
                                            <td>
                                                <b>
                                                    <asp:Literal ID="lblPhoneNumber" runat="server" Text="Phone Numbers:"></asp:Literal>
                                                </b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPhoneNumbers" runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDCONTACTNUMBERS")%>'></asp:Label>
                                            </td>
                                            <td>
                                                <b>
                                                    <asp:Literal ID="lblEMail" runat="server" Text="E-Mail:"></asp:Literal>
                                                </b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblContactMail" runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDCONTACTMAIL")%>'></asp:Label>                                                
                                            </td>
                                            <td wisth="10%"></td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
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
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                </ItemTemplate>                            
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap="nowrap" align="center">
                                <asp:Label ID="lblPagenumber" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPages" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecords" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap="nowrap" align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap="nowrap" align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap="nowrap" align="center">
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
                <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
