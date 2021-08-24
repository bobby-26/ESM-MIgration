<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaBatchSemesterExamResults.aspx.cs"
    Inherits="PreSeaBatchSemesterExamResults" %>

<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaBatch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlPreSeaCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Semester" Src="~/UserControls/UserControlPreSeaCourseSemester.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Exam Results</title>
   <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
</telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPreSeaExam" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPreSeaEntranceExam">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="EntranceExamInterview" Text="Semester Results" ShowMenu="true">
                    </eluc:Title>
                    <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuBatchPlanner" runat="server" OnTabStripCommand="BatchPlanner_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>               
               <%-- <table style="width: 100%">
                    <tr>
                        <td colspan="6" style="color: Blue;">
                            Note : Please initiate plan  in Semester Planner before enter marks. </br>
                        </td>
                    </tr>
                </table>--%>
                <table cellpadding="2" cellspacing="2" width="80%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblCourse" runat="server" Text="Course"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Course ID="ddlCourse" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox" Width="200px" />
                        </td>
                        <td>
                            <asp:Literal ID="lblBatch" runat="server" Text="Batch"></asp:Literal>
                        </td>
                        <td>
                            <eluc:PreSeaBatch ID="ucBatch" runat="server" CssClass="readonlytextbox" AppendDataBoundItems="true" Width="240px" />
                        </td>   
                          <%--  <td>
                            <asp:Literal ID="lblPlan" runat="server" Text="Plan"></asp:Literal>
                        </td>  
                        <td>
                            <asp:DropDownList ID="ddlPlan" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                CssClass="input" Width="120px">                                
                            </asp:DropDownList>
                        </td>               --%>  
                    </tr>
                    <tr>
                    <td>
                    <asp:Literal ID="lblyear" runat="server" Text="Year"></asp:Literal>
                    </td>
                     <td>
                          <eluc:Quick ID="ddlYear" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                QuickTypeCode="55" AutoPostBack ="true" Width="200px"/>
                        </td>
                        <td>
                            <asp:Literal ID="lblPlan" runat="server" Text="Semester"></asp:Literal>
                        </td>
                        <td>
                             <eluc:Semester ID="ddlSemester" runat="server" Width="120px"  
                                 CssClass="dropdown_mandatory" AppendDataBoundItems="true" AutoPostBack="true"/>   
                            <%--<asp:DropDownList ID="ddlPlan" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                CssClass="input_mandatory" Width="120px">
                                <asp:ListItem Value="" Text="--Select--">--Select--</asp:ListItem>
                                <asp:ListItem Value="1" Text="1">1</asp:ListItem>
                                <asp:ListItem Value="2" Text="2">2</asp:ListItem>
                            </asp:DropDownList>--%>
                        </td>
                    </tr>
                </table>
                <br />
                <div class="navSelect" style="position: relative; clear: both; width: 15px">
                    <eluc:TabStrip ID="MenuPreSea" runat="server" OnTabStripCommand="MenuPreSea_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvPreSea" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvPreSea_RowDataBound" ShowHeader="true"
                        OnRowEditing="gvPreSea_RowEditing" OnRowCancelingEdit="gvPreSea_RowCancelingEdit"
                        OnRowUpdating = "gvPreSea_RowUpdating"
                        OnRowCommand="gvPreSea_RowCommand" AllowSorting="true" OnSorting="gvPreSea_Sorting"
                        OnSelectedIndexChanging="gvPreSea_SelectedIndexChanging" ShowFooter="false" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                          <asp:TemplateField HeaderText="Date Of Birth">
                                <HeaderTemplate>
                                    Batch Roll No
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRollNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRBATCHROLLNUMBER") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblRollNoEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRBATCHROLLNUMBER") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTraineeExamid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAINEEEXAMID")%>'></asp:Label>
                                    <asp:Label ID="lblEmployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID")%>'></asp:Label>
                                    <asp:Label ID="lblEployeeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTNAME").ToString()+ " "+ DataBinder.Eval(Container,"DataItem.FLDMIDDLENAME").ToString()+" "+ DataBinder.Eval(Container,"DataItem.FLDLASTNAME").ToString()%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblTraineeExamidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAINEEEXAMID")%>'></asp:Label>
                                    <asp:Label ID="lblEmployeeidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID")%>'></asp:Label>
                                    <asp:Label ID="lblEployeeNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTNAME").ToString()+ " "+ DataBinder.Eval(Container,"DataItem.FLDMIDDLENAME").ToString()+" "+ DataBinder.Eval(Container,"DataItem.FLDLASTNAME").ToString()%>'></asp:Label>
                                    </ItemTemplate>
                                </EditItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSubjectId" runat="server" Text="Subject"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSemesterPlanId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEMESTERPLANID") %>'></asp:Label>
                                    <asp:Label ID="lblPlanId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANID") %>'></asp:Label>
                                    <asp:Label ID="lblSubjectId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTID") %>'></asp:Label>                                  
                                    <asp:Label ID="lblSubjectName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblSemesterPlanIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEMESTERPLANID") %>'></asp:Label>
                                    <asp:Label ID="lblPlanIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANID") %>'></asp:Label>
                                    <asp:Label ID="lblSubjectIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTID") %>'></asp:Label>
                                    <asp:Label ID="lblSubjectNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTNAME") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                          
                            <asp:TemplateField HeaderText="">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    Mark
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblMark" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMARKS") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtMarkEdit" runat="server" CssClass="input_mandatory" Mask="999.99"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDMARKS") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    Pass/Fail
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPassFail" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSFAIL") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                  <asp:Label ID="lblPassFailEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSFAIL") %>'></asp:Label>
                                </EditItemTemplate>
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
                                        ToolTip="Edit" Visible="false"></asp:ImageButton>
                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" visible="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="UPDATE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save" Visible="false"></asp:ImageButton>
                                    <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="CANCEL" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel" Visible="false"></asp:ImageButton>
                                </EditItemTemplate>
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
