<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaBatchWeeklyPlannerActual.aspx.cs" Inherits="PreSeaBatchWeeklyPlannerActual" %>

<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaBatch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlPreSeaCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MCSubject" Src="~/UserControls/UserControlMultiColumnPreSeaSubject.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Semester" Src="~/UserControls/UserControlPreSeaCourseSemester.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Weekly Planner Actual</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
</telerik:RadCodeBlock>
</head>
<body>
    <form id="frmBondReq" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPreSeaEntranceExam">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="WeeklyPlannerTitle" Text="Weekly Planner Actual" ShowMenu="true">
                    </eluc:Title>
                    <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"  />
                </div>
                <div style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuBatchPlanner" runat="server" OnTabStripCommand="BatchPlanner_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblCourse" runat="server" Text="Course"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Course ID="ddlCourse" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox" />
                        </td>
                        <td>
                            <asp:Literal ID="lblBatch" runat="server" Text="Batch"></asp:Literal>
                        </td>
                        <td>
                            <eluc:PreSeaBatch ID="ucBatch" runat="server" CssClass="readonlytextbox" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblSemester" runat="server" Text="Semester"></asp:Literal>
                        </td>
                        <td>
                             <eluc:Semester ID="ddlSemester" runat="server" Width="120px"  
                                 CssClass="input" AppendDataBoundItems="true" OnTextChangedEvent="ddlSemester_TextChangedEvent"/>     
                          <%--  <asp:DropDownList ID="ddlSemester" runat="server" AppendDataBoundItems="true" Width="120px"
                                AutoPostBack="true" CssClass="input" OnTextChanged="ddlSemester_Changed">
                                <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                            </asp:DropDownList>--%>
                        </td>
                        <td>
                            <asp:Literal ID="lblSection" runat="server" Text="Section"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlSection" AppendDataBoundItems="true" CssClass="input"
                                Width="120px">
                                <asp:ListItem Text="A" Value="1">A</asp:ListItem>
                                <asp:ListItem Text="B" Value="2">B</asp:ListItem>
                                <asp:ListItem Text="C" Value="3">C</asp:ListItem>
                                <asp:ListItem Text="D" Value="4">D</asp:ListItem>
                                <asp:ListItem Text="E" Value="5">E</asp:ListItem>
                                <asp:ListItem Text="F" Value="6">F</asp:ListItem>
                                <asp:ListItem Text="G" Value="7">G</asp:ListItem>
                                <asp:ListItem Text="H" Value="8">H</asp:ListItem>
                                <asp:ListItem Text="I" Value="9">I</asp:ListItem>
                                <asp:ListItem Text="J" Value="10">J</asp:ListItem>
                                <asp:ListItem Text="K" Value="11">K</asp:ListItem>
                                <asp:ListItem Text="L" Value="12">L</asp:ListItem>
                                <asp:ListItem Text="M" Value="13">M</asp:ListItem>
                                <asp:ListItem Text="N" Value="14">N</asp:ListItem>
                                <asp:ListItem Text="O" Value="15">O</asp:ListItem>
                                <asp:ListItem Text="P" Value="16">P</asp:ListItem>
                                <asp:ListItem Text="Q" Value="17">Q</asp:ListItem>
                                <asp:ListItem Text="R" Value="18">R</asp:ListItem>
                                <asp:ListItem Text="S" Value="19">S</asp:ListItem>
                                <asp:ListItem Text="T" Value="20">T</asp:ListItem>
                                <asp:ListItem Text="U" Value="21">U</asp:ListItem>
                                <asp:ListItem Text="V" Value="22">V</asp:ListItem>
                                <asp:ListItem Text="W" Value="23">W</asp:ListItem>
                                <asp:ListItem Text="X" Value="24">X</asp:ListItem>
                                <asp:ListItem Text="Y" Value="25">Y</asp:ListItem>
                                <asp:ListItem Text="Z" Value="26">Z</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblDate" runat="server" Text="Date"></asp:Literal>
                        </td>
                        <td >
                            <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" Width="120px" AutoPostBack="true"  />
                        </td>
                         <td>
                            <asp:Literal ID="lblTimeSlot" runat="server" Text="Time slot"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTimeSlot" runat="server" AppendDataBoundItems="true" CssClass="input" AutoPostBack="true"
                            OnTextChanged="ddlTimeSlot_Changed" >
                            </asp:DropDownList>
                        </td>
                       
                    </tr>
                </table> 
                <br />
                <br /> 
                 <div class="navSelect" style="position: relative; clear: both; width: 15px">
                    <eluc:TabStrip ID="MenuPreSea" runat="server" OnTabStripCommand="MenuPreSea_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvPreSea" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvPreSea_RowDataBound" ShowHeader="true"
                        OnRowEditing = "gvPreSea_RowEditing" OnRowCancelingEdit="gvPreSea_RowCancelingEdit"
                        OnRowCommand="gvPreSea_RowCommand" AllowSorting="true" OnSorting="gvPreSea_Sorting"
                        OnSelectedIndexChanging="gvPreSea_SelectedIndexChanging" ShowFooter="false" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSubjectId" runat="server" Text="Subject"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblWeeklyPlanId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWEEKPLANID") %>'></asp:Label>
                                    <asp:Label ID="lblIsCompletedYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDYN") %>'></asp:Label>
                                    <asp:Label ID="lblSubjectId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTID") %>'></asp:Label>
                                    <asp:Label ID="lblSubjectName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblWeeklyPlanIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWEEKPLANID") %>'></asp:Label>
                                    <asp:Label ID="lblSubjectIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTID") %>'></asp:Label>
                                    <asp:Label ID="lblSubjectNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTNAME") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblStaff" runat="server" Text="Staff"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDSTAFFNAME"]%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblFaucltyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAFFID") %>'></asp:Label>                                    
                                    <asp:Label ID="lblFacultyEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAFFNAME") %>'></asp:Label>                                                                  
                                </EditItemTemplate>                        
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblStaff" runat="server" Text="Class Room"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDROOMNAME"]%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblClassRoomIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLASSROOM") %>'></asp:Label>                                    
                                    <asp:Label ID="lblClassRoomEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROOMNAME") %>'></asp:Label>                                    
                                  
                                </EditItemTemplate>                           
                            </asp:TemplateField>                           
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblPractical" runat="server" Text="Practical"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDPRACTICALNAME"]%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblPracticalIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRACTICALID") %>'></asp:Label>
                                    <asp:Label ID="lblPracticalEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRACTICALNAME") %>'></asp:Label>                                 
                                </EditItemTemplate>                           
                            </asp:TemplateField>  
                            <asp:TemplateField HeaderText="IsCompletedYN">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblIsActive" runat="server"  Text="IsCompletedYN">                                   
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblIsCompleted" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDYN").ToString().Equals("1"))?"Yes":"No" %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkIsCompletedYN" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDYN").ToString().Equals("1"))?true:false %>'>
                                    </asp:CheckBox>
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
                                    CommandName="PLANEDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                    ToolTip="Edit" Visible="false"></asp:ImageButton>
                                <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" visible="false" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="PLANDELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>                                
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="PLANUPDATE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                    ToolTip="Save" Visible="false"></asp:ImageButton>
                                <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="PLANCANCEL" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel" Visible="false"></asp:ImageButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />                        
                        </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                 <br />
                 <div class="navSelect" style="position: relative; clear: both; width: 15px">
                    <eluc:TabStrip ID="MenuPreSeaWeekPlanner" runat="server" OnTabStripCommand="MenuPreSeaWeekPlanner_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="div2" runat="server" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvPreseaWeeklyPlanner" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" Width="100%" CellPadding="3" ShowFooter="false" OnRowDataBound="gvPreseaWeeklyPlanner_RowDataBound"
                        EnableViewState="false" OnDataBound="gvPreseaWeeklyPlanner_DataBound" OnRowCommand="gvPreseaWeeklyPlanner_RowCommand"
                        OnRowDeleting="gvPreseaWeeklyPlanner_RowDeleting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" VerticalAlign="Middle" />
                        <Columns>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
