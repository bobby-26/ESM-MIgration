<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreRecommendedTraining.aspx.cs" Inherits="CrewOffshoreRecommendedTraining" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Training Summary</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="CrewRecommendedCourseslink" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmCrewRecommendedCourses" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCrewRecommendedCourses">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
                position: absolute;">
                <div class="subHeader" style="position: relative">
                    <div class="subHeader" id="divHeading">
                        <span style="float: left">
                            <eluc:Title runat="server" ID="ucTitle" Text="Training Summary" ShowMenu="false" />
                        </span>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="RecommendedCourses" runat="server"></eluc:TabStrip>
                </div>
                <div>
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                            <asp:Literal ID="lblFirstName" runat="server" Text="First Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                            <asp:Literal ID="lblMiddleName" runat="server" Text="Middle Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                            <asp:Literal ID="lblLastName" runat="server" Text="Last Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            <asp:Literal ID="lblEmployeeNumber" runat="server" Text="Employee Number"></asp:Literal>
                                
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                            <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <asp:TextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <hr />
                &nbsp;<b> Recommended Training Needs</b>
                <br />
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuOffshoreTraining" runat="server" OnTabStripCommand="MenuOffshoreTraining_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 1">
                    <asp:GridView ID="gvOffshoreTraining" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3"  ShowFooter="false" OnRowCommand ="gvOffshoreTraining_RowCommand" OnRowCreated="gvOffshoreTraining_RowCreated" 
                        OnRowEditing ="gvOffshoreTraining_RowEditing" OnRowCancelingEdit ="gvOffshoreTraining_RowCancelingEdit" 
                        OnRowUpdating ="gvOffshoreTraining_RowUpdating" ShowHeader="true" EnableViewState="false" DataKeyNames ="FLDTRAININGNEEDID" 
                        OnRowDataBound="gvOffshoreTraining_RowDataBound"  OnRowDeleting ="gvOffshoreTraining_OnRowDeleting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                             <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                             <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSerialHeader" runat="server" Text="No"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblSno" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROW") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblVesselHeader" runat="server" Text="Vessel"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblVessel" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCategoryHeader" runat="server" Text="Category"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <asp:DropDownList ID="ddlCategoryEdit" Enabled="false" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                         AppendDataBoundItems ="true" OnSelectedIndexChanged="ddlCategoryEdit_SelectedIndexChanged">                                        
                                    </asp:DropDownList> 
                                </EditItemTemplate>
                                <FooterTemplate> 
                                    <asp:DropDownList ID="ddlCategoryAdd" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                        AppendDataBoundItems ="true" OnSelectedIndexChanged="ddlCategoryAdd_SelectedIndexChanged">                                        
                                    </asp:DropDownList>                                     
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSubCategoryHeader" runat="server" Text="Sub Category"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblSubCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                           
                                    <asp:DropDownList ID="ddlSubCategoryEdit" Enabled="false" runat="server" CssClass="input_mandatory" AppendDataBoundItems ="true">                                        
                                    </asp:DropDownList> 
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlSubCategoryAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems ="true">                                        
                                    </asp:DropDownList>                                     
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTraningHeader" runat="server" Text="Identified Training Need"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblTrainingNeed" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEED") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <asp:TextBox ID="txtTrainingNeedEdit" Enabled="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEED") %>'
                                        CssClass="gridinput_mandatory"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtTrainingNeedAdd" runat="server" CssClass="gridinput_mandatory"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblImprovementHeader" runat="server" Text="Level of Improvement"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblLevelOfImprovement" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELOFIMPROVEMENTNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Quick ID="ucImprovementEdit" Enabled="false" runat="server" CssClass="input_mandatory" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELOFIMPROVEMENT") %>'
                                        AppendDataBoundItems="true" QuickTypeCode="128" />                                    
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Quick ID="ucImprovementAdd" runat="server" CssClass="input_mandatory"
                                        AppendDataBoundItems="true" QuickTypeCode="128" />                                   
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTypeofTrainingHeader" runat="server" Text="Type of Training"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblTypeofTraining" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAININGNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Quick ID="ucTypeofTrainingEdit" runat="server" CssClass="input" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAINING") %>'
                                        AppendDataBoundItems="true" QuickTypeCode="129" Enabled="false" />                                    
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Course/CBT">
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblCourseName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTEDCOURSES").ToString() %>'></asp:Label>                                    
                                    <asp:ImageButton ID="imgCourseName" runat="server" ImageUrl="<%$ PhoenixTheme:images/te_view.png%>" CommandArgument='<%# Container.DataItemIndex %>'></asp:ImageButton>
                                    <eluc:ToolTip ID="ucToolTipCourseName" Width="450px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTEDCOURSES") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="200px"></ItemStyle>                                
                                <HeaderTemplate>
                                    <asp:Label ID="lblDonebyHeader" runat="server" Text="To be done by"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblDoneby" Width="200px" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOBEDONEBYNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID = "ddlDoneByEdit" Width="200px" runat="server" CssClass ="input" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                               <%--     <eluc:Quick ID="ucDonebyEdit" Width="200px" runat="server" CssClass="input" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDTOBEDONEBY") %>'
                                        AppendDataBoundItems="true" QuickTypeCode="130" />  --%>                                  
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDetailsHeader" runat="server" Text="Details of Training Imparted / Course Attended"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblDetails" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILSOFTRAINING") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <asp:TextBox ID="txtDetailsEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILSOFTRAINING") %>'
                                        CssClass="input"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTrainerHeader" runat="server" Text="Name of Trainer / Institute"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblTrainer" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFTRAINERINSTITUTE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <asp:TextBox ID="txtTrainerEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFTRAINERINSTITUTE") %>'
                                        CssClass="input"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDateHeader" runat="server" Text="Date Completed"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblCompletionDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="ucCompletionDate" runat="server" CssClass="input" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">Action</asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                        CommandName="ATTACHMENT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAtt"
                                        ToolTip="Upload Attachment"></asp:ImageButton>
                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Initiate Course Request" ImageUrl="<%$ PhoenixTheme:images/task-list.png %>"
                                        CommandName="Initiate Course Request" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCourseReq" Visible="false"
                                        ToolTip="View Requested Course"></asp:ImageButton>
                                    <%--<img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>--%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="UPDATE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>                                    
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="Add" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
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
                
                 <br />
                  &nbsp;<b> Pending CBT Training Needs</b>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuOffshorePendingCBTTraining" runat="server" OnTabStripCommand="MenuOffshorePendingCBTTraining_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="div1" style="position: relative; z-index: 1">
                    <asp:GridView ID="gvOffshorePCBTTraining" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3"  ShowFooter="false" OnRowCommand ="gvOffshorePCBTTraining_RowCommand" OnRowCreated="gvOffshorePCBTTraining_RowCreated" 
                        OnRowEditing ="gvOffshorePCBTTraining_RowEditing" OnRowCancelingEdit ="gvOffshorePCBTTraining_RowCancelingEdit" 
                        OnRowUpdating ="gvOffshorePCBTTraining_RowUpdating" ShowHeader="true" EnableViewState="false" DataKeyNames ="FLDTRAININGNEEDID" 
                        OnRowDataBound="gvOffshorePCBTTraining_RowDataBound"  OnRowDeleting ="gvOffshorePCBTTraining_OnRowDeleting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                             <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />                             
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblVesselHeader" runat="server" Text="Vessel"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblVessel" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCategoryHeader" runat="server" Text="Category"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <asp:DropDownList ID="ddlCategoryEdit" Enabled="false" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                         AppendDataBoundItems ="true" OnSelectedIndexChanged="ddlCategoryEdit_SelectedIndexChanged">                                        
                                    </asp:DropDownList> 
                                </EditItemTemplate>
                                <FooterTemplate> 
                                    <asp:DropDownList ID="ddlCategoryAdd" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                        AppendDataBoundItems ="true" OnSelectedIndexChanged="ddlCategoryAdd_SelectedIndexChanged">                                        
                                    </asp:DropDownList>                                     
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSubCategoryHeader" runat="server" Text="Sub Category"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblSubCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                           
                                    <asp:DropDownList ID="ddlSubCategoryEdit" Enabled="false" runat="server" CssClass="input_mandatory" AppendDataBoundItems ="true">                                        
                                    </asp:DropDownList> 
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlSubCategoryAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems ="true">                                        
                                    </asp:DropDownList>                                     
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTraningHeader" runat="server" Text="Identified Training Need"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblTrainingNeed" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEED") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <asp:TextBox ID="txtTrainingNeedEdit" Enabled="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEED") %>'
                                        CssClass="gridinput_mandatory"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtTrainingNeedAdd" runat="server" CssClass="gridinput_mandatory"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblImprovementHeader" runat="server" Text="Level of Improvement"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblLevelOfImprovement" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELOFIMPROVEMENTNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Quick ID="ucImprovementEdit" Enabled="false" runat="server" CssClass="input_mandatory" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELOFIMPROVEMENT") %>'
                                        AppendDataBoundItems="true" QuickTypeCode="128" />                                    
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Quick ID="ucImprovementAdd" runat="server" CssClass="input_mandatory"
                                        AppendDataBoundItems="true" QuickTypeCode="128" />                                   
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTypeofTrainingHeader" runat="server" Text="Type of Training"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblTypeofTraining" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAININGNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Quick ID="ucTypeofTrainingEdit" runat="server" CssClass="input" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAINING") %>'
                                        AppendDataBoundItems="true" QuickTypeCode="129" Enabled="false" />                                    
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Course/CBT">
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblCourseName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTEDCOURSES").ToString() %>'></asp:Label>                                    
                                    <asp:ImageButton ID="imgCourseName" runat="server" ImageUrl="<%$ PhoenixTheme:images/te_view.png%>" CommandArgument='<%# Container.DataItemIndex %>'></asp:ImageButton>
                                    <eluc:ToolTip ID="ucToolTipCourseName" Width="450px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTEDCOURSES") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="200px"></ItemStyle>                                
                                <HeaderTemplate>
                                    <asp:Label ID="lblDonebyHeader" runat="server" Text="To be done by"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblDoneby" Width="200px" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOBEDONEBYNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID = "ddlDoneByEdit" Width="200px" runat="server" CssClass ="input" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                   <%-- <eluc:Quick ID="ucDonebyEdit" Width="200px" runat="server" CssClass="input" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDTOBEDONEBY") %>'
                                        AppendDataBoundItems="true" QuickTypeCode="130" /> --%>                                   
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDetailsHeader" runat="server" Text="Details of Training Imparted / Course Attended"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblDetails" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILSOFTRAINING") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <asp:TextBox ID="txtDetailsEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILSOFTRAINING") %>'
                                        CssClass="input"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTrainerHeader" runat="server" Text="Name of Trainer / Institute"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblTrainer" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFTRAINERINSTITUTE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <asp:TextBox ID="txtTrainerEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFTRAINERINSTITUTE") %>'
                                        CssClass="input"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDateHeader" runat="server" Text="Date Completed"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblCompletionDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="ucCompletionDate" runat="server" CssClass="input" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Score %">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblScoreHeader" runat="server" Text="Score %"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Result">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblResultHeader" runat="server" Text="Result"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblResult" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMRESULT") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">Action</asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                        CommandName="ATTACHMENT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAtt"
                                        ToolTip="Upload Attachment"></asp:ImageButton>
                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Initiate Course Request" ImageUrl="<%$ PhoenixTheme:images/task-list.png %>"
                                        CommandName="Initiate Course Request" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCourseReq" Visible="false"
                                        ToolTip="View Requested Course"></asp:ImageButton>
                                    <%--<img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>--%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="UPDATE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>                                    
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="Add" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPagePendingCBT" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap="nowrap" align="center">
                                <asp:Label ID="lblPagenumberPCBT" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPagesPCBT" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecordsPCBT" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap="nowrap" align="left" width="50px">
                                <asp:LinkButton ID="cmdPreviousPCBT" runat="server" OnCommand="PagerButtonClick" CommandName="prevPCBT">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap="nowrap" align="right" width="50px">
                                <asp:LinkButton ID="cmdNextPCBT" OnCommand="PagerButtonClick" runat="server" CommandName="nextPCBT">Next >></asp:LinkButton>
                            </td>
                            <td nowrap="nowrap" align="center">
                                <asp:TextBox ID="txtnopagePCBT" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGoPCBT" runat="server" Text="Go" OnClick="cmdGoPCBT_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>   
                
                 <br />
                  &nbsp;<b> Pending Training Course</b>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuOffshorePendingCourseTraining" runat="server" OnTabStripCommand="MenuOffshorePendingCourseTraining_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="div2" style="position: relative; z-index: 1">
                    <asp:GridView ID="gvOffshorePCourseTraining" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3"  ShowFooter="false" OnRowCommand ="gvOffshorePCourseTraining_RowCommand" OnRowCreated="gvOffshorePCourseTraining_RowCreated" 
                        OnRowEditing ="gvOffshorePCourseTraining_RowEditing" OnRowCancelingEdit ="gvOffshorePCourseTraining_RowCancelingEdit" 
                        OnRowUpdating ="gvOffshorePCourseTraining_RowUpdating" ShowHeader="true" EnableViewState="false" DataKeyNames ="FLDTRAININGNEEDID" 
                        OnRowDataBound="gvOffshorePCourseTraining_RowDataBound"  OnRowDeleting ="gvOffshorePCourseTraining_OnRowDeleting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                             <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />                             
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblVesselHeader" runat="server" Text="Vessel"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblVessel" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCategoryHeader" runat="server" Text="Category"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <asp:DropDownList ID="ddlCategoryEdit" Enabled="false" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                         AppendDataBoundItems ="true" OnSelectedIndexChanged="ddlCategoryEdit_SelectedIndexChanged">                                        
                                    </asp:DropDownList> 
                                </EditItemTemplate>
                                <FooterTemplate> 
                                    <asp:DropDownList ID="ddlCategoryAdd" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                        AppendDataBoundItems ="true" OnSelectedIndexChanged="ddlCategoryAdd_SelectedIndexChanged">                                        
                                    </asp:DropDownList>                                     
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSubCategoryHeader" runat="server" Text="Sub Category"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblSubCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                           
                                    <asp:DropDownList ID="ddlSubCategoryEdit" Enabled="false" runat="server" CssClass="input_mandatory" AppendDataBoundItems ="true">                                        
                                    </asp:DropDownList> 
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlSubCategoryAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems ="true">                                        
                                    </asp:DropDownList>                                     
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTraningHeader" runat="server" Text="Identified Training Need"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblTrainingNeed" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEED") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <asp:TextBox ID="txtTrainingNeedEdit" Enabled="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEED") %>'
                                        CssClass="gridinput_mandatory"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtTrainingNeedAdd" runat="server" CssClass="gridinput_mandatory"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblImprovementHeader" runat="server" Text="Level of Improvement"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblLevelOfImprovement" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELOFIMPROVEMENTNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Quick ID="ucImprovementEdit" Enabled="false" runat="server" CssClass="input_mandatory" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELOFIMPROVEMENT") %>'
                                        AppendDataBoundItems="true" QuickTypeCode="128" />                                    
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Quick ID="ucImprovementAdd" runat="server" CssClass="input_mandatory"
                                        AppendDataBoundItems="true" QuickTypeCode="128" />                                   
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTypeofTrainingHeader" runat="server" Text="Type of Training"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblTypeofTraining" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAININGNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Quick ID="ucTypeofTrainingEdit" runat="server" CssClass="input" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAINING") %>'
                                        AppendDataBoundItems="true" QuickTypeCode="129" Enabled="false" />                                    
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Course/CBT">
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblCourseName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTEDCOURSES").ToString() %>'></asp:Label>                                    
                                    <asp:ImageButton ID="imgCourseName" runat="server" ImageUrl="<%$ PhoenixTheme:images/te_view.png%>" CommandArgument='<%# Container.DataItemIndex %>'></asp:ImageButton>
                                    <eluc:ToolTip ID="ucToolTipCourseName" Width="450px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTEDCOURSES") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="200px"></ItemStyle>                                
                                <HeaderTemplate>
                                    <asp:Label ID="lblDonebyHeader" runat="server" Text="To be done by"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblDoneby" Width="200px" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOBEDONEBYNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID = "ddlDoneByEdit" Width="200px" runat="server" CssClass ="input" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                    <%--<eluc:Quick ID="ucDonebyEdit" Width="200px" runat="server" CssClass="input" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDTOBEDONEBY") %>'
                                        AppendDataBoundItems="true" QuickTypeCode="130" /> --%>                                   
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDetailsHeader" runat="server" Text="Details of Training Imparted / Course Attended"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblDetails" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILSOFTRAINING") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <asp:TextBox ID="txtDetailsEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILSOFTRAINING") %>'
                                        CssClass="input"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTrainerHeader" runat="server" Text="Name of Trainer / Institute"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblTrainer" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFTRAINERINSTITUTE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <asp:TextBox ID="txtTrainerEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFTRAINERINSTITUTE") %>'
                                        CssClass="input"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDateHeader" runat="server" Text="Date Completed"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblCompletionDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="ucCompletionDate" runat="server" CssClass="input" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Score %">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblScoreHeader" runat="server" Text="Score %"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Result">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblResultHeader" runat="server" Text="Result"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblResult" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMRESULT") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">Action</asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                        CommandName="ATTACHMENT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAtt"
                                        ToolTip="Upload Attachment"></asp:ImageButton>
                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Initiate Course Request" ImageUrl="<%$ PhoenixTheme:images/task-list.png %>"
                                        CommandName="Initiate Course Request" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCourseReq" Visible="false"
                                        ToolTip="View Requested Course"></asp:ImageButton>
                                    <%--<img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>--%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="UPDATE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>                                    
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="Add" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPagePendingTC" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap="nowrap" align="center">
                                <asp:Label ID="lblPagenumberPTC" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPagesPTC" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecordsPTC" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap="nowrap" align="left" width="50px">
                                <asp:LinkButton ID="cmdPreviousPTC" runat="server" OnCommand="PagerButtonClick" CommandName="prevPTC">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap="nowrap" align="right" width="50px">
                                <asp:LinkButton ID="cmdNextPTC" OnCommand="PagerButtonClick" runat="server" CommandName="nextPTC">Next >></asp:LinkButton>
                            </td>
                            <td nowrap="nowrap" align="center">
                                <asp:TextBox ID="txtnopagePTC" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGoPTC" runat="server" Text="Go" OnClick="cmdGoPTC_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>  
                <br />
                 &nbsp;<b> Completed CBT Training</b>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuOffshoreCompletedCBTTraining" runat="server" OnTabStripCommand="MenuOffshoreCompletedCBTTraining_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="div3" style="position: relative; z-index: 1">
                    <asp:GridView ID="gvOffshoreCCBTTraining" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3"  ShowFooter="false" OnRowCommand ="gvOffshoreCCBTTraining_RowCommand" OnRowCreated="gvOffshoreCCBTTraining_RowCreated" 
                        OnRowEditing ="gvOffshoreCCBTTraining_RowEditing" OnRowCancelingEdit ="gvOffshoreCCBTTraining_RowCancelingEdit" 
                        OnRowUpdating ="gvOffshoreCCBTTraining_RowUpdating" ShowHeader="true" EnableViewState="false" DataKeyNames ="FLDTRAININGNEEDID" 
                        OnRowDataBound="gvOffshoreCCBTTraining_RowDataBound"  OnRowDeleting ="gvOffshoreCCBTTraining_OnRowDeleting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                             <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />                             
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblVesselHeader" runat="server" Text="Vessel"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblVessel" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCategoryHeader" runat="server" Text="Category"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <asp:DropDownList ID="ddlCategoryEdit" Enabled="false" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                         AppendDataBoundItems ="true" OnSelectedIndexChanged="ddlCategoryEdit_SelectedIndexChanged">                                        
                                    </asp:DropDownList> 
                                </EditItemTemplate>
                                <FooterTemplate> 
                                    <asp:DropDownList ID="ddlCategoryAdd" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                        AppendDataBoundItems ="true" OnSelectedIndexChanged="ddlCategoryAdd_SelectedIndexChanged">                                        
                                    </asp:DropDownList>                                     
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSubCategoryHeader" runat="server" Text="Sub Category"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblSubCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                           
                                    <asp:DropDownList ID="ddlSubCategoryEdit" Enabled="false" runat="server" CssClass="input_mandatory" AppendDataBoundItems ="true">                                        
                                    </asp:DropDownList> 
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlSubCategoryAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems ="true">                                        
                                    </asp:DropDownList>                                     
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTraningHeader" runat="server" Text="Identified Training Need"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblTrainingNeed" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEED") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <asp:TextBox ID="txtTrainingNeedEdit" Enabled="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEED") %>'
                                        CssClass="gridinput_mandatory"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtTrainingNeedAdd" runat="server" CssClass="gridinput_mandatory"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblImprovementHeader" runat="server" Text="Level of Improvement"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblLevelOfImprovement" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELOFIMPROVEMENTNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Quick ID="ucImprovementEdit" Enabled="false" runat="server" CssClass="input_mandatory" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELOFIMPROVEMENT") %>'
                                        AppendDataBoundItems="true" QuickTypeCode="128" />                                    
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Quick ID="ucImprovementAdd" runat="server" CssClass="input_mandatory"
                                        AppendDataBoundItems="true" QuickTypeCode="128" />                                   
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTypeofTrainingHeader" runat="server" Text="Type of Training"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblTypeofTraining" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAININGNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Quick ID="ucTypeofTrainingEdit" runat="server" CssClass="input" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAINING") %>'
                                        AppendDataBoundItems="true" QuickTypeCode="129" Enabled="false" />                                    
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Course/CBT">
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblCourseName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTEDCOURSES").ToString() %>'></asp:Label>                                    
                                    <asp:ImageButton ID="imgCourseName" runat="server" ImageUrl="<%$ PhoenixTheme:images/te_view.png%>" CommandArgument='<%# Container.DataItemIndex %>'></asp:ImageButton>
                                    <eluc:ToolTip ID="ucToolTipCourseName" Width="450px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTEDCOURSES") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="200px"></ItemStyle>                                
                                <HeaderTemplate>
                                    <asp:Label ID="lblDonebyHeader" runat="server" Text="To be done by"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblDoneby" Width="200px" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOBEDONEBYNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID = "ddlDoneByEdit" Width="200px" runat="server" CssClass ="input" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                  <%--  <eluc:Quick ID="ucDonebyEdit" Width="200px" runat="server" CssClass="input" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDTOBEDONEBY") %>'
                                        AppendDataBoundItems="true" QuickTypeCode="130" />--%>                                    
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDetailsHeader" runat="server" Text="Details of Training Imparted / Course Attended"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblDetails" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILSOFTRAINING") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <asp:TextBox ID="txtDetailsEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILSOFTRAINING") %>'
                                        CssClass="input"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTrainerHeader" runat="server" Text="Name of Trainer / Institute"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblTrainer" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFTRAINERINSTITUTE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <asp:TextBox ID="txtTrainerEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFTRAINERINSTITUTE") %>'
                                        CssClass="input"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDateHeader" runat="server" Text="Date Completed"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblCompletionDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="ucCompletionDate" runat="server" CssClass="input" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Score %">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblScoreHeader" runat="server" Text="Score %"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Result">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblResultHeader" runat="server" Text="Result"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblResult" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMRESULT") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">Action</asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                        CommandName="ATTACHMENT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAtt"
                                        ToolTip="Upload Attachment"></asp:ImageButton>
                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Initiate Course Request" ImageUrl="<%$ PhoenixTheme:images/task-list.png %>"
                                        CommandName="Initiate Course Request" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCourseReq" Visible="false"
                                        ToolTip="View Requested Course"></asp:ImageButton>
                                    <%--<img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>--%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="UPDATE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>                                    
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="Add" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPageCompletedCBT" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap="nowrap" align="center">
                                <asp:Label ID="lblPagenumberCCBT" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPagesCCBT" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecordsCCBT" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap="nowrap" align="left" width="50px">
                                <asp:LinkButton ID="cmdPreviousCCBT" runat="server" OnCommand="PagerButtonClick" CommandName="prevCCBT">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap="nowrap" align="right" width="50px">
                                <asp:LinkButton ID="cmdNextCCBT" OnCommand="PagerButtonClick" runat="server" CommandName="nextCCBT">Next >></asp:LinkButton>
                            </td>
                            <td nowrap="nowrap" align="center">
                                <asp:TextBox ID="txtnopageCCBT" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGoCCBT" runat="server" Text="Go" OnClick="cmdGoCCBT_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>    
                <br />
                 &nbsp;<b> Completed Training Course</b>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuOffshoreCompletedCourseTraining" runat="server" OnTabStripCommand="MenuOffshoreCompletedCourseTraining_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="div4" style="position: relative; z-index: 1">
                    <asp:GridView ID="gvOffshoreCCourseTraining" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3"  ShowFooter="false" OnRowCommand ="gvOffshoreCCourseTraining_RowCommand" OnRowCreated="gvOffshoreCCourseTraining_RowCreated" 
                        OnRowEditing ="gvOffshoreCCourseTraining_RowEditing" OnRowCancelingEdit ="gvOffshoreCCourseTraining_RowCancelingEdit" 
                        OnRowUpdating ="gvOffshoreCCourseTraining_RowUpdating" ShowHeader="true" EnableViewState="false" DataKeyNames ="FLDTRAININGNEEDID" 
                        OnRowDataBound="gvOffshoreCCourseTraining_RowDataBound"  OnRowDeleting ="gvOffshoreCCourseTraining_OnRowDeleting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                             <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />                             
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblVesselHeader" runat="server" Text="Vessel"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblVessel" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCategoryHeader" runat="server" Text="Category"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <asp:DropDownList ID="ddlCategoryEdit" Enabled="false" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                         AppendDataBoundItems ="true" OnSelectedIndexChanged="ddlCategoryEdit_SelectedIndexChanged">                                        
                                    </asp:DropDownList> 
                                </EditItemTemplate>
                                <FooterTemplate> 
                                    <asp:DropDownList ID="ddlCategoryAdd" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                        AppendDataBoundItems ="true" OnSelectedIndexChanged="ddlCategoryAdd_SelectedIndexChanged">                                        
                                    </asp:DropDownList>                                     
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSubCategoryHeader" runat="server" Text="Sub Category"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblSubCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                           
                                    <asp:DropDownList ID="ddlSubCategoryEdit" Enabled="false" runat="server" CssClass="input_mandatory" AppendDataBoundItems ="true">                                        
                                    </asp:DropDownList> 
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlSubCategoryAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems ="true">                                        
                                    </asp:DropDownList>                                     
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTraningHeader" runat="server" Text="Identified Training Need"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblTrainingNeed" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEED") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <asp:TextBox ID="txtTrainingNeedEdit" Enabled="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEED") %>'
                                        CssClass="gridinput_mandatory"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtTrainingNeedAdd" runat="server" CssClass="gridinput_mandatory"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblImprovementHeader" runat="server" Text="Level of Improvement"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblLevelOfImprovement" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELOFIMPROVEMENTNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Quick ID="ucImprovementEdit" Enabled="false" runat="server" CssClass="input_mandatory" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELOFIMPROVEMENT") %>'
                                        AppendDataBoundItems="true" QuickTypeCode="128" />                                    
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Quick ID="ucImprovementAdd" runat="server" CssClass="input_mandatory"
                                        AppendDataBoundItems="true" QuickTypeCode="128" />                                   
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTypeofTrainingHeader" runat="server" Text="Type of Training"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblTypeofTraining" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAININGNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Quick ID="ucTypeofTrainingEdit" runat="server" CssClass="input" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAINING") %>'
                                        AppendDataBoundItems="true" QuickTypeCode="129" Enabled="false" />                                    
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Course/CBT">
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblCourseName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTEDCOURSES").ToString() %>'></asp:Label>                                    
                                    <asp:ImageButton ID="imgCourseName" runat="server" ImageUrl="<%$ PhoenixTheme:images/te_view.png%>" CommandArgument='<%# Container.DataItemIndex %>'></asp:ImageButton>
                                    <eluc:ToolTip ID="ucToolTipCourseName" Width="450px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTEDCOURSES") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="200px"></ItemStyle>                                
                                <HeaderTemplate>
                                    <asp:Label ID="lblDonebyHeader" runat="server" Text="To be done by"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblDoneby" Width="200px" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOBEDONEBYNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID = "ddlDoneByEdit" Width="200px" runat="server" CssClass ="input" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                   <%-- <eluc:Quick ID="ucDonebyEdit" Width="200px" runat="server" CssClass="input" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDTOBEDONEBY") %>'
                                        AppendDataBoundItems="true" QuickTypeCode="130" />     --%>                               
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDetailsHeader" runat="server" Text="Details of Training Imparted / Course Attended"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblDetails" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILSOFTRAINING") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <asp:TextBox ID="txtDetailsEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILSOFTRAINING") %>'
                                        CssClass="input"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTrainerHeader" runat="server" Text="Name of Trainer / Institute"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblTrainer" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFTRAINERINSTITUTE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <asp:TextBox ID="txtTrainerEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFTRAINERINSTITUTE") %>'
                                        CssClass="input"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDateHeader" runat="server" Text="Date Completed"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Label ID="lblCompletionDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="ucCompletionDate" runat="server" CssClass="input" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Score %">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblScoreHeader" runat="server" Text="Score %"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Result">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblResultHeader" runat="server" Text="Result"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblResult" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMRESULT") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">Action</asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                        CommandName="ATTACHMENT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAtt"
                                        ToolTip="Upload Attachment"></asp:ImageButton>
                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Initiate Course Request" ImageUrl="<%$ PhoenixTheme:images/task-list.png %>"
                                        CommandName="Initiate Course Request" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCourseReq" Visible ="false"
                                        ToolTip="View Requested Course"></asp:ImageButton>
                                    <%--<img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>--%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="UPDATE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>                                    
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="Add" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="div5" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap="nowrap" align="center">
                                <asp:Label ID="lblPagenumberCTC" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPagesCTC" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecordsCTC" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap="nowrap" align="left" width="50px">
                                <asp:LinkButton ID="cmdPreviousCTC" runat="server" OnCommand="PagerButtonClick" CommandName="prevPTC">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap="nowrap" align="right" width="50px">
                                <asp:LinkButton ID="cmdNextCTC" OnCommand="PagerButtonClick" runat="server" CommandName="nextCTC">Next >></asp:LinkButton>
                            </td>
                            <td nowrap="nowrap" align="center">
                                <asp:TextBox ID="txtnopageCTC" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGoCTC" runat="server" Text="Go" OnClick="cmdGoCTC_Click" CssClass="input"
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
