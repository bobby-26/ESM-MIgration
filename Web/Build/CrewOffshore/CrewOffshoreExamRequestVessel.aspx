<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreExamRequestVessel.aspx.cs" Inherits="CrewOffshoreExamRequestVessel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Exam Request</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlExamRequest">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="Pending Test Request" ShowMenu="<%# Title1.ShowMenu %>">
                    </eluc:Title>
                    <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="CrewQuery" runat="server" TabStrip="true" OnTabStripCommand="CrewQuery_TabStripCommand">
                    </eluc:TabStrip>
                </div>                
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="CrewQueryMenu" runat="server" OnTabStripCommand="CrewQueryMenu_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvExamRequest" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvExamRequest_RowDataBound" OnRowEditing="gvExamRequest_RowEditing"
                        ShowHeader="true" EnableViewState="false" AllowSorting="true" OnSorting="gvExamRequest_Sorting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>                            
                            <asp:TemplateField HeaderText="Vessel">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                     <asp:LinkButton ID="lnkVesselHeader" runat="server" CommandName="Sort" CommandArgument="FLDVESSELNAME"
                                        ForeColor="White" Text="Vessel"></asp:LinkButton>
                                     <img id="FLDVESSELNAME" runat="server" visible="false" />                                
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Employee Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                        ForeColor="White" Text="Employee Name"></asp:LinkButton>
                                     <img id="FLDNAME" runat="server" visible="false" />    
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblEmployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkEmployeeName" runat="server" CommandArgument="<%#Container.DataItemIndex%>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME")%>' CommandName="GETEMPLOYEE"></asp:LinkButton>                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="File No">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                            
                            <asp:TemplateField HeaderText="Rank">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblRankHeader" runat="server" Text="Rank"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Course">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkCourseHeader" runat="server" CommandName="Sort" CommandArgument="FLDCOURSENAME"
                                        ForeColor="White" Text="Course"></asp:LinkButton>
                                     <img id="FLDCOURSENAME" runat="server" visible="false" />  
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCourse" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Exam">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblExamHeader" runat="server" Text="Exam"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblExam" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Target Date" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTargetDateHeader" runat="server" Text="Target Date"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTargetDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTARGETDATE")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>  
                            <asp:TemplateField HeaderText="Status" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Image ID="ImgFlagP" runat="server" alt="" ImageUrl="<%$ PhoenixTheme:images/Red.png %>"
                                        Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Score">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblScoreHeader" runat="server" Text="Score"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>    
                            <asp:TemplateField HeaderText="Exam Result">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblExamResultHeader" runat="server" Text="Exam Result"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblExamResult" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMRESULTNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField HeaderText="Date Attended">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDateAttendedHeader" runat="server" Text="Date Attended"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDateAttended" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEATTENDED")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>  
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">Action</asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Exam" ImageUrl="<%$ PhoenixTheme:images/course-request.png %>"
                                        CommandName="EXAMREQ" CommandArgument="<%# Container.DataItemIndex %>"
                                        ID="cmdExamReq" ToolTip="Exam"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>                       
                        </Columns>
                    </asp:GridView>
                </div>
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
                <table cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <img id="lblRed" runat="server" src="<%$ PhoenixTheme:images/Red.png %>"  visible="false"/>
                        </td>
                        <td>
                            <asp:Literal ID="lblOverDue" runat="server" Text=" * Overdue"  visible="false"></asp:Literal>
                        </td>
                        <td>
                            <img id="lblYellow" runat="server" src="<%$ PhoenixTheme:images/Yellow.png %>"  visible="false" />
                        </td>
                        <td>
                            <asp:Literal ID="lblDueWithin30days" runat="server" Text=" * Due within 30 days"  visible="false"></asp:Literal>
                        </td>
                        <td>
                            <img id="lblGreen" runat="server" src="<%$ PhoenixTheme:images/Green.png %>"  visible="false"/>
                        </td>
                        <td>
                            <asp:Literal ID="lblDueWithin60Days" runat="server" Text=" * Due within 60 days"  visible="false"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
