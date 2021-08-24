<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCourseParticipantList.aspx.cs"
    Inherits="CrewCourseParticipantList" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Batch" Src="~/UserControls/UserControlBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Participant List</title>
   
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewParticipantList" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlParticipantList">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
             <eluc:Status runat="server" ID="ucStatus" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <asp:Literal ID="lblParticipantList" runat="server" Text="Participant List"></asp:Literal>
                    </div>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblConfigureParticipantList" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblCourse" runat="server" Text="Course"></asp:Literal>
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
                                <asp:Literal ID="lblMinStrength" runat="server" Text="Min Strength"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMinStrength" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblMaxStrength" runat="server" Text="Max Strength"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMaxStrength" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                      
                        <tr>
                            <td>
                                <asp:Literal ID="lblBatch" runat="server" Text="Batch"></asp:Literal>
                            </td>
                            <td >
                                <eluc:Batch ID="ucBatch" runat="server"  AppendDataBoundItems="true" CssClass="dropdown_mandatory" AutoPostBack="true" IsOutside="true" OnTextChangedEvent="SetDetails" />
                            </td>
                             <td>
                                <asp:Literal ID="lblLastEditedbyDate" runat="server" Text="Last Edited by/Date"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLastEditedby" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="260px"></asp:TextBox>
                                <eluc:Date ID="txtLastEditedDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"></eluc:Date>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b><asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal></b>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblNominationList" runat="server" Text="Nomination List"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNominationList" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                             <td>
                                <asp:Literal ID="lblStatusParticipantList" runat="server" Text="Participant List"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtParticipantList" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                <asp:Literal ID="lblWaitList" runat="server" Text="Wait List"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWaitList" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                             <td>
                                <asp:Literal ID="lblFree" runat="server" Text="Free"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFreeList" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                          <tr>
                             <td>
                              <asp:Label ID="lblRegForm" runat="server" Text="Reg formalities Completed" Visible="false" ></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkRF" runat="server" AutoPostBack="true" OnCheckedChanged="CheckRF" Visible="false" />
                            </td>
                             <td> 
                                <asp:Label ID="lblRegFormCom" runat="server" Text="Reg formalities Completed By/Date" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRegEditedby" runat="server" CssClass="readonlytextbox" ReadOnly="true" Visible="false" ></asp:TextBox>
                                <eluc:Date ID="txtRegDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" Visible="false"></eluc:Date>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuCrewParticipantList" runat="server" OnTabStripCommand="CrewParticipantList_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvParticipantList" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvParticipantList_RowCommand" OnRowDataBound="gvParticipantList_ItemDataBound"
                        OnSorting="gvParticipantList_Sorting" AllowSorting="true" OnRowEditing="gvParticipantList_RowEditing" OnRowDeleting="gvParticipantList_RowDeleting"
                        ShowFooter="false" ShowHeader="true" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                         <asp:TemplateField>
                            <ItemStyle Width="20px" />
                            <HeaderTemplate>
                                <asp:Literal ID="lblSrNo" runat="server" Text="Sr.No"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate><center>
                                <asp:Label ID="lblSrNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>' ></asp:Label>
                                </center>
                            </ItemTemplate>
                        </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                        ForeColor="White">Employee Name&nbsp;</asp:LinkButton>
                                    <img id="FLDNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                <asp:Label ID="lblEmployeeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></asp:Label>
                                    <asp:Label ID="lblEnrollmentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENROLLMENTID") %>'></asp:Label>
                                    <asp:Label ID="lblEmpName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblRankNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDRANKNAME"
                                        ForeColor="White">Rank &nbsp;</asp:LinkButton>
                                    <img id="FLDRANKNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblPriority" runat="server" Text="Pool"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPool" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOOLNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblPriority" runat="server" Text="Priority"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPriority" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRIORITY") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                <asp:LinkButton ID="lblQueueDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDQUEUEDATE"
                                        ForeColor="White">Enrolled On&nbsp;</asp:LinkButton>
                                    <img id="FLDQUEUEDATE" runat="server" visible="false" />
                                    
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblApprovedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVEDDATE", "{0:dd/MMM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblBatchNo" runat="server" Text="Batch No"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBatchNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHNAME") %>'></asp:Label>
                                </ItemTemplate>
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
                                  <asp:ImageButton runat="server" AlternateText="Add to Participant List" ImageUrl="<%$ PhoenixTheme:images/yellow-symbol.png %>"
                                     CommandName="WAITLIST" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdWL"
                                     ToolTip="Move To Wait List"></asp:ImageButton>
                                     <img id="Img5" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Move to Participant List" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Move to Nomination List"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
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
                <asp:Label ID="lblnote1" runat="server" CssClass="guideline_text">Note: If the List of seafarer exceeds the maximum size of the course ,seafarer will automatically move to Wait List</asp:Label><br />
                <asp:Label ID="lblnote" runat="server" Visible="false" CssClass="guideline_text">Note : Registration Formalities -<br /> 1) Checked eligibility criteria as per SIMS Work Instruction <br />2) Collect supporting docs <br />3) Collect fees</asp:Label>
                <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="btn_Click" OKText="Yes"
                    CancelText="No" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="gvParticipantList" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
