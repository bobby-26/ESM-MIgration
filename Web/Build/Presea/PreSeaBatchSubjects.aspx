<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaBatchSubjects.aspx.cs"
    Inherits="PreSeaBatchSubjects" %>

<%@ Import Namespace="SouthNests.Phoenix.PreSea" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaBatch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Exam" Src="~/UserControls/UserControlPreSeaExam.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Semester" Src="~/UserControls/UserControlPreSeaSemester.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Subject" Src="~/UserControls/UserControlPreSeaSubject.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlPreSeaCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Pre Sea Subjects</title>
  <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPreSeaExam" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPreSeaRoom">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Batch Subjects" ShowMenu="false"></eluc:Title>
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
               
                <br style="clear: both;" />
                <div id="divMain" runat="server" style="width: 100%;">
                    <table width="80%" cellpadding="1" cellspacing="1">
                        <tr valign="top">
                             <td>
                                Course Name
                            </td>
                            <td>
                                <eluc:Course ID="ddlCourse" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox" />
                            </td>

                            <td>
                                Batch
                            </td>
                            <td>
                                <eluc:PreSeaBatch ID="ucBatch" runat="server" CssClass="readonlytextbox" AppendDataBoundItems="true" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvBatchSubject" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCancelingEdit="gvBatchSubject_RowCancelingEdit"
                        OnRowCommand="gvBatchSubject_RowCommand" OnRowDataBound="gvBatchSubject_RowDataBound"
                        OnRowDeleting="gvBatchSubject_RowDeleting" OnRowEditing="gvBatchSubject_RowEditing"
                        OnRowUpdating="gvBatchSubject_RowUpdating" ShowFooter="true" EnableViewState="true">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField HeaderText="Exam Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Semester
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBatchSubjectId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHSUBJECTID") %>'></asp:Label>
                                    <asp:Label ID="lblSemesterName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEMESTERNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblBatchSubjectIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHSUBJECTID") %>'></asp:Label>
                                    <asp:Label ID="lblSemesterIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEMESTERID") %>'></asp:Label>
                                    <asp:Label ID="lblSemesterNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEMESTERNAME") %>'></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Semester ID="ucSemesterAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"  
                                        Course='<%# Filter.CurrentPreSeaCourseMasterSelection %>' />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Exam Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Subject
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSubjectId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTID") %>'></asp:Label>
                                    <asp:Label ID="lblSubjectName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblSubjectIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTID") %>'></asp:Label>
                                    <asp:Label ID="lblSubjectNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTNAME") %>'></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                      <asp:DropDownList ID="ddlBatchSubjectAdd" runat="server" CssClass="dropdown_mandatory"
                                    DataTextField="FLDSUBJECTNAME" DataValueField="FLDSUBJECTID">
                                </asp:DropDownList>                                      
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Subject Code
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSubjectCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTCODE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtSubjectCodeEdit" runat="server" MaxLength="20" CssClass="input_mandatory"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTCODE") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtSubjectCodeAdd" runat="server" MaxLength="20" CssClass="input_mandatory"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    Max Score
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblMaxMark" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXMARK") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtMaxMarkEdit" runat="server" CssClass="input_mandatory" Mask="999.99"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXMARK") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number ID="txtMaxMarkAdd" runat="server" CssClass="input_mandatory" Mask="999.99" />                                    
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    Pass Score
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPassMark" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSMARK") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtPassMarkEdit" runat="server" CssClass="input_mandatory" Mask="999.99"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSMARK") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number ID="txtPassMarkAdd" runat="server" CssClass="input_mandatory" Mask="999.99" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    Total Class(Hrs)
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalClass" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCLASSHOURS") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtTotalClassEdit" runat="server" CssClass="input_mandatory" MaxLength="5"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCLASSHOURS") %>'  Mask="999.99" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number ID="txtTotalClasssAdd" runat="server" CssClass="input_mandatory" MaxLength="5"  Mask="999.99" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Type
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSubType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTTYPE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblSubTypeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTTYPE") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Active
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblActive" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkAciveEdit" runat="server" Checked='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1")? true: false %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="chkAciveAdd" runat="server"  Checked="true"/>
                                </FooterTemplate>
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
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="SAVE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save" Visible="false"></asp:ImageButton>
                                    <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="CANCEL" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel" Visible="false"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="ADD" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
