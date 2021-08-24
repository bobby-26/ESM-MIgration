<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerBugList.aspx.cs"
    Inherits="DefectTrackerBugList" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPModule" Src="~/UserControls/UserControlSEPBugModuleList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPVessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPStatus" Src="~/UserControls/UserControlSEPStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPType" Src="~/UserControls/UserControlSEPBugType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPSeverity" Src="~/UserControls/UserControlSEPBugSeverity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPPriority" Src="~/UserControls/UserControlSEPBugPriority.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPTeamMembers" Src="~/UserControls/UserControlSEPTeamMembers.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>SEPBugSearch</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript">
            function fnDefectTrackerBugProject(projectname) {
                location.href = 'DefectTrackerBugAdd.aspx?projectname=' + projectname;
            }
        </script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <eluc:Title runat="server" ID="ucTitle" Text="Issue List"></eluc:Title>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        </div>
    </div>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="True">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlSEPBugSearch">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="subHeader" id="div1" style="position: relative">
                <asp:Literal ID="lblsep" runat="server" Text=""></asp:Literal>
            </div>
            <div id="divFind" style="margin-top: 0px;">
                <table width="100%" cellpadding="0" cellspacing="0" border="1">
                    <tr>
                        <td width="25%">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblproject" runat="server">Project</asp:Literal>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlProject" runat="server" CssClass="input" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlProject_SelectedIndexChanged" MaxLength="100" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Vessel
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlvesselcode" runat="server" CssClass="input" MaxLength="100"
                                            Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        ID
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIDSearch" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Subject
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSubjectSearch" runat="server" MaxLength="100" CssClass="input"
                                            Width="120px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Description
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDescriptionSearch" runat="server" MaxLength="100" CssClass="input"
                                            Width="120px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Logged Between
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucFromDate" runat="server" CssClass="input" />
                                        <eluc:Date ID="ucToDate" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Assigned To
                                    </td>
                                    <td>
                                        <%--<asp:TextBox id="txtAssignedTo" runat="server" MaxLength="100" CssClass="input"></asp:TextBox>--%>
                                        <eluc:SEPTeamMembers ID="ucDeveloperName" AppendDataBoundItems="True" runat="server"
                                            OnTextChangedEvent="Filter_Changed" CssClass="input"></eluc:SEPTeamMembers>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Reported By
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtReportedBy" runat="server" MaxLength="100" CssClass="input"></asp:TextBox>&nbsp;
                                        <asp:ImageButton runat="server" AlternateText="This Login" ImageUrl="<%$ PhoenixTheme:images/on-signer.png %>"
                                            CommandName="THISUSER" ID="cmdThisUser" ToolTip="This Login" OnClick="cmdThisUser_Click">
                                        </asp:ImageButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblmilestone" runat="server" Text="Milestone"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlmilestone" runat="server" CssClass="input" AutoPostBack="true"
                                            MaxLength="100" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblissueflag" runat="server" Text="Issue Flag"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlissueflag" runat="server" CssClass="input" AutoPostBack="true"
                                            MaxLength="100" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="30%">
                            &nbsp; Module<br />
                            <eluc:SEPModule ID="ucSEPModule" runat="server" AutoPostBack="true" OnTextChangedEvent="Filter_Changed" />
                        </td>
                        <td width="15%">
                            &nbsp; Type<br />
                            <eluc:SEPStatus ID="ucSEPType" runat="server" AutoPostBack="true" OnTextChangedEvent="Filter_Changed" />
                        </td>
                        <td>
                            &nbsp; Status<br />
                            <eluc:SEPStatus ID="SEPStatus" runat="server" AutoPostBack="true" OnTextChangedEvent="Filter_Changed" />
                        </td>
                        <td align="left">
                            &nbsp; Severity<br />
                            <eluc:SEPStatus ID="SEPSeverity" runat="server" AutoPostBack="true" OnTextChangedEvent="Filter_Changed" />
                        </td>
                        <td align="left">
                            &nbsp; Priority<br />
                            <eluc:SEPStatus ID="SEPPriority" runat="server" AutoPostBack="true" OnTextChangedEvent="Filter_Changed" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="navSelect" style="position: relative; width: 15px">
                <eluc:TabStrip ID="MenuDefectTracker" runat="server" OnTabStripCommand="MenuDefectTracker_TabStripCommand">
                </eluc:TabStrip>
            </div>
            <asp:GridView ID="SEPBugSearchGrid" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnRowDataBound="SEPBugSearchGrid_ItemDataBound"
                OnRowEditing="SEPBugSearchGrid_RowEditing" OnRowCommand="SEPBugSearchGrid_RowCommand"
                OnSorting="SEPBugSearchGrid_Sorting" ShowFooter="True" EnableViewState="false"
                AllowSorting="True">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" Wrap="False" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <Columns>
                    <asp:TemplateField HeaderText="Bug Id">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkBugIDHeader" runat="server" CommandName="Sort" CommandArgument="FLDBUGID"
                                ForeColor="White">ID&nbsp;</asp:LinkButton>
                            <img id="FLDBUGID" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblUniqueID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                Visible="false"></asp:Label>
                            <asp:LinkButton ID="lnkBugId" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUGID") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Type">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Type
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDType")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Module">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Module
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDMODULENAME")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Subject">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Subject
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkSubject" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSUBJECT")%>'></asp:LinkButton>
                            <eluc:Tooltip ID="ucDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Priority">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Priority
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDPRIORITY")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Severity">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Severity
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDSEVERITYNAME")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Status
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDSTATUSNAME")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Reported By">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Reported By
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDREPORTEDBY")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Date">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Date
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDOPENDATE")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Date">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Vessel
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDVESSELSHORTCODE")%>
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
                                CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                ToolTip="Edit"></asp:ImageButton>
                            <img id="Img3" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                CommandName="ATTACHMENT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAttachment"
                                ToolTip="Attachment"></asp:ImageButton>
                            <asp:ImageButton runat="server" AlternateText="No Attachment" ImageUrl="<%$ PhoenixTheme:images/no-attachment.png %>"
                                CommandName="NOATTACHMENT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdNoAttachment"
                                ToolTip="No Attachment" Enabled="false"></asp:ImageButton>
                            <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <asp:ImageButton runat="server" AlternateText="Comment" ImageUrl="<%$ PhoenixTheme:images/crew-suitability-check.png %>"
                                CommandName="COMMENT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdComments"
                                ToolTip="Comments" onmousedown="javascript:closeMoreInformation()"></asp:ImageButton>
                            <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/annexure.png %>"
                                CommandName="ANALYSIS" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAnalysis"
                                ToolTip="Analysis"></asp:ImageButton>
                            <img id="Img4" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/45.png %>"
                                CommandName="DEFECTCLOSE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDefectclose"
                                ToolTip="Defect close"></asp:ImageButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                ToolTip="Save"></asp:ImageButton>
                            <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                ToolTip="Cancel"></asp:ImageButton>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
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
            <eluc:Status runat="server" ID="ucStatus" />
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
