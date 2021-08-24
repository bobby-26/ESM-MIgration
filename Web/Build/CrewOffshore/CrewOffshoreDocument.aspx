<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreDocument.aspx.cs" Inherits="CrewOffshoreDocument" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Documents" Src="~/UserControls/UserControlDocuments.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserGroup" Src="~/UserControls/UserControlUserGroup.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DocumentType" Src="~/UserControls/UserControlDocumentType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DocumentCategory" Src="~/UserControls/UserControlDocumentCategory.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="InspectionCompany" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmOffshoreDocument" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlOffshoreDocument">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Document Configuration"></eluc:Title>
                    </div>
                </div>  
                 <br />
                <b>Travel Documents</b>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuOffshoreDocumentTravel" runat="server" OnTabStripCommand="MenuOffshoreDocumentTravel_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGridT" style="position: relative; z-index: 1">
                    <asp:GridView ID="gvOffshoreDocumentTravel" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3"  ShowFooter="false" OnRowCommand ="gvOffshoreDocumentTravel_RowCommand" 
                        ShowHeader="true" EnableViewState="true" DataKeyNames ="FLDOFFSHOREDOCUMENTID" 
                        OnRowDataBound="gvOffshoreDocumentTravel_RowDataBound"  >
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblStageHeader" runat="server" Text="Stage"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStageName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlStageEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"></asp:DropDownList>
                                    <asp:Label ID="lblDocumentType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>' >
                                    </asp:Label>
                                </EditItemTemplate>                          
                            </asp:TemplateField>
                             <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCategoryHeader" runat="server" Text="Document Category"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCategoryName" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME"))%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:DocumentCategory ID="ucCategoryEdit" runat="server" CssClass="input" RankList="<%# PhoenixRegistersDocumentCategory.ListDocumentCategory(1,null,null,null) %>" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:DocumentCategory ID="ucCategoryAdd" runat="server" CssClass="input" RankList="<%# PhoenixRegistersDocumentCategory.ListDocumentCategory(1,null,null,null) %>" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTravelHeader" runat="server" Text="Experience"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                                                        
                                    <asp:Label ID="lblTravelDocName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate> 
                                  <asp:Label ID="lblTravelDocNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></asp:Label>
                             
                                </EditItemTemplate>                            
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblMandatoryHeader" runat="server" Text="Mandatory Y/N"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:CheckBox ID="chkMandatoryYN" runat="server" Enabled="false" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDMANDATORYYN")).ToString().Equals("1")?true:false %>' />
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <asp:CheckBox ID="chkMandatoryYNEdit" runat="server" AutoPostBack="true" OnCheckedChanged="chkMandatoryYNEdit_CheckedChanged"
                                        Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDMANDATORYYN").ToString().Equals("1"))?true:false %>'>
                                    </asp:CheckBox>
                                </EditItemTemplate>                             
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblWaiverHeader" runat="server" Text="Waiver Y/N"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:CheckBox ID="chkWaiverYN" runat="server" Enabled="false" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDWAIVERYN").ToString().Equals("1"))?true:false %>' />
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <asp:CheckBox ID="chkWaiverYNEdit" Enabled="false" runat="server" AutoPostBack="true" OnCheckedChanged="chkWaiverYNEdit_CheckedChanged"
                                        Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDWAIVERYN").ToString().Equals("1"))?true:false %>'>
                                    </asp:CheckBox>
                                </EditItemTemplate>                          
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblUsergroupHeader" runat="server" Text="User Group to allow Waiver"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblUserGroup" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPNAME") %>'></asp:Label>
                                    <asp:ImageButton id="ImgUserGroup" runat="server" ImageUrl="<%$ PhoenixTheme:images/te_view.png%>" CommandArgument='<%# Container.DataItemIndex %>'></asp:ImageButton>
                                    <eluc:Tooltip ID="ucUserGroup" Width="200px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPNAME") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <%--<eluc:UserGroup runat="server" ID="ucUserGroupEdit" CssClass="input" Enabled="false" AppendDataBoundItems="true" />--%>
                                    <div style="height: 100px; overflow: auto;" class="input">
                                        <asp:CheckBoxList ID="chkUserGroupEdit" RepeatDirection="Vertical" Enabled="false" runat="server">
                                        </asp:CheckBoxList>
                                    </div>
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
                                        CommandName="EDITT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETET" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete" Visible="false"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="UPDATET" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="CancelT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />                            
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                 <div id="div2" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap="nowrap" align="center">
                                <asp:Label ID="lblPagenumberT" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPagesT" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecordsT" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap="nowrap" align="left" width="50px">
                                <asp:LinkButton ID="cmdPreviousT" runat="server" OnCommand="PagerButtonClickT" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap="nowrap" align="right" width="50px">
                                <asp:LinkButton ID="cmdNextT" OnCommand="PagerButtonClickT" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap="nowrap" align="center">
                                <asp:TextBox ID="txtnopageT" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGoT" runat="server" Text="Go" OnClick="cmdGoT_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
              <%--  <br />--%>
                <%--<b>COC</b>--%>
                <div class="navSelect" style="position: relative; width: 15px" visible="false" runat="server">
                    <eluc:TabStrip ID="MenuOffshoreDocumentLicence" runat="server" OnTabStripCommand="MenuOffshoreDocumentLicence_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 1" visible="false" runat="server">
                    <asp:GridView ID="gvOffshoreDocumentLicence" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3"  ShowFooter="true" OnRowCommand ="gvOffshoreDocumentLicence_RowCommand" 
                        OnRowEditing ="gvOffshoreDocumentLicence_RowEditing" OnRowCancelingEdit ="gvOffshoreDocumentLicence_RowCancelingEdit" 
                        OnRowUpdating ="gvOffshoreDocumentLicence_RowUpdating" ShowHeader="true" EnableViewState="true" DataKeyNames ="FLDOFFSHOREDOCUMENTID" 
                        OnRowDataBound="gvOffshoreDocumentLicence_RowDataBound"  OnRowDeleting ="gvOffshoreDocumentLicence_OnRowDeleting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblStageHeader" runat="server" Text="Stage"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStageName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlStageEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"></asp:DropDownList>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlStageAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"></asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblLicenceHeader" runat="server" Text="COC"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                                                        
                                    <asp:Label ID="lblLicenceName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate> 
                                    <eluc:Documents ID="ddlLicenceEdit" runat="server" DocumentList="<%# PhoenixRegistersDocumentLicence.ListDocumentLicence(null,null,1,null,null) %>"
                                        SelectedDocument='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>' AppendDataBoundItems="true" 
                                        CssClass="dropdown_mandatory" DocumentType="LICENCE" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Documents ID="ddlLicenceAdd" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" DocumentType="LICENCE"
                                        DocumentList="<%# PhoenixRegistersDocumentLicence.ListDocumentLicence(null,null,1,null,null) %>" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblMandatoryHeader" runat="server" Text="Mandatory Y/N"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:CheckBox ID="chkMandatoryYN" runat="server" Enabled="false" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDMANDATORYYN")).ToString().Equals("1")?true:false %>' />
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <asp:CheckBox ID="chkMandatoryYNEdit" runat="server" AutoPostBack="true" OnCheckedChanged="chkMandatoryYNEdit_CheckedChanged"
                                        Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDMANDATORYYN").ToString().Equals("1"))?true:false %>'>
                                    </asp:CheckBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="chkMandatoryYNAdd" runat="server" AutoPostBack="true" OnCheckedChanged="chkMandatoryYNAdd_CheckedChanged"></asp:CheckBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblWaiverHeader" runat="server" Text="Waiver Y/N"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:CheckBox ID="chkWaiverYN" runat="server" Enabled="false" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDWAIVERYN").ToString().Equals("1"))?true:false %>' />
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <asp:CheckBox ID="chkWaiverYNEdit" Enabled="false" runat="server" AutoPostBack="true" OnCheckedChanged="chkWaiverYNEdit_CheckedChanged"
                                        Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDWAIVERYN").ToString().Equals("1"))?true:false %>'>
                                    </asp:CheckBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="chkWaiverYNAdd" runat="server" Enabled="false" AutoPostBack="true" OnCheckedChanged="chkWaiverYNAdd_CheckedChanged"></asp:CheckBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblUsergroupHeader" runat="server" Text="User Group to allow Waiver"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblUserGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <%--<eluc:UserGroup runat="server" ID="ucUserGroupEdit" CssClass="input" Enabled="false" AppendDataBoundItems="true" />--%>
                                    <div style="height: 50px; overflow: auto;" class="input">
                                        <asp:CheckBoxList ID="chkUserGroupEdit" RepeatDirection="Vertical" Enabled="false" runat="server">
                                        </asp:CheckBoxList>
                                    </div>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <%--<eluc:UserGroup runat="server" ID="ucUserGroupAdd" Enabled="false" CssClass="input" AppendDataBoundItems="true" />--%>
                                    <div style="height: 50px; overflow: auto;" class="input">
                                        <asp:CheckBoxList ID="chkUserGroupAdd" RepeatDirection="Vertical" Enabled="false" runat="server">
                                        </asp:CheckBoxList>
                                    </div>
                                </FooterTemplate>
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
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
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
                <div id="divPage" style="position: relative;" visible="false" runat="server">
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
               <%-- <br />
                <b>STCW</b>--%>
                <div class="navSelect" style="position: relative; width: 15px" visible="false" runat="server">
                    <eluc:TabStrip ID="MenuOffshoreDocumentCourse" runat="server" OnTabStripCommand="MenuOffshoreDocumentCourse_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGridC" style="position: relative; z-index: 1" visible="false" runat="server">
                    <asp:GridView ID="gvOffshoreDocumentCourse" runat="server" AutoGenerateColumns="False" 
                        Font-Size="11px" Width="100%" CellPadding="3" ShowFooter="true" OnRowCommand="gvOffshoreDocumentCourse_RowCommand"
                        ShowHeader="true" EnableViewState="true"
                        DataKeyNames="FLDOFFSHOREDOCUMENTID" OnRowDataBound="gvOffshoreDocumentCourse_RowDataBound">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblStageHeader" runat="server" Text="Stage"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStageName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlStageEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlStageAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCourseHeader" runat="server" Text="STCW"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCourseName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlCourseEdit" Width="220px" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"></asp:DropDownList>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlCourseAdd" Width="220px" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"></asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblMandatoryHeader" runat="server" Text="Mandatory Y/N"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkMandatoryYN" runat="server" Enabled="false" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDMANDATORYYN")).ToString().Equals("1")?true:false %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkMandatoryYNEdit" runat="server" AutoPostBack="true" OnCheckedChanged="chkMandatoryYNEdit_CheckedChanged"
                                        Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDMANDATORYYN").ToString().Equals("1"))?true:false %>'>
                                    </asp:CheckBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="chkMandatoryYNAdd" runat="server" AutoPostBack="true" OnCheckedChanged="chkMandatoryYNAdd_CheckedChanged">
                                    </asp:CheckBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblWaiverHeader" runat="server" Text="Waiver Y/N"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkWaiverYN" runat="server" Enabled="false" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDWAIVERYN").ToString().Equals("1"))?true:false %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkWaiverYNEdit" Enabled="false" runat="server" AutoPostBack="true"
                                        OnCheckedChanged="chkWaiverYNEdit_CheckedChanged" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDWAIVERYN").ToString().Equals("1"))?true:false %>'>
                                    </asp:CheckBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="chkWaiverYNAdd" runat="server" Enabled="false" AutoPostBack="true"
                                        OnCheckedChanged="chkWaiverYNAdd_CheckedChanged"></asp:CheckBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblUsergroupHeader" runat="server" Text="User Group to allow Waiver"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblUserGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <%--<eluc:UserGroup runat="server" ID="ucUserGroupEdit" CssClass="input" Enabled="false"
                                        AppendDataBoundItems="true" />--%>
                                    <div style="height: 50px; overflow: auto;" class="input">
                                        <asp:CheckBoxList ID="chkUserGroupEdit" RepeatDirection="Vertical" Enabled="false" runat="server">
                                        </asp:CheckBoxList>
                                    </div>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <%--<eluc:UserGroup runat="server" ID="ucUserGroupAdd" Enabled="false" CssClass="input"
                                        AppendDataBoundItems="true" />--%>
                                    <div style="height: 50px; overflow: auto;" class="input">
                                        <asp:CheckBoxList ID="chkUserGroupAdd" RepeatDirection="Vertical" Enabled="false" runat="server">
                                        </asp:CheckBoxList>
                                    </div>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">Action</asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDITC" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETEC" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="UPDATEC" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="CancelC" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="AddC" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPageC" style="position: relative;" visible="false" runat="server">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap="nowrap" align="center">
                                <asp:Label ID="lblPagenumberC" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPagesC" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecordsC" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap="nowrap" align="left" width="50px">
                                <asp:LinkButton ID="cmdPreviousC" runat="server" OnCommand="PagerButtonClickC" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap="nowrap" align="right" width="50px">
                                <asp:LinkButton ID="cmdNextC" OnCommand="PagerButtonClickC" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap="nowrap" align="center">
                                <asp:TextBox ID="txtnopageC" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGoC" runat="server" Text="Go" OnClick="cmdGoC_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
                <%--<br />
                <b>Other Documents</b>--%>
                <div class="navSelect" style="position: relative; width: 15px" visible="false" runat="server">
                    <eluc:TabStrip ID="MenuOffshoreDocumentOther" runat="server" OnTabStripCommand="MenuOffshoreDocumentOther_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGridO" style="position: relative; z-index: 1" visible="false" runat="server">
                    <asp:GridView ID="gvOffshoreDocumentOther" runat="server" AutoGenerateColumns="False" Visible="false"
                        Font-Size="11px" Width="100%" CellPadding="3" ShowFooter="true" OnRowCommand="gvOffshoreDocumentOther_RowCommand"
                        ShowHeader="true" EnableViewState="true"
                        DataKeyNames="FLDOFFSHOREDOCUMENTID" OnRowDataBound="gvOffshoreDocumentOther_RowDataBound">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblStageHeader" runat="server" Text="Stage"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStageName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlStageEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlStageAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblOtherHeader" runat="server" Text="Other Docs"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOtherName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:DocumentType ID="ucDocumentOtherEdit" runat="server" CssClass="dropdown_mandatory"
                                        DocumentTypeList='<%#PhoenixRegistersDocumentOther.ListDocumentOther(((int)PhoenixDocumentType.OTHER_DOC_NONE).ToString())%>'
                                        SelectedDocumentType='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:DocumentType ID="ucDocumentOtherAdd" runat="server" DocumentType='<%# ((int)PhoenixDocumentType.OTHER_DOC_NONE).ToString()%>'
                                        DocumentTypeList='<%#PhoenixRegistersDocumentOther.ListDocumentOther(((int)PhoenixDocumentType.OTHER_DOC_NONE).ToString())%>'
                                        AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblMandatoryHeader" runat="server" Text="Mandatory Y/N"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkMandatoryYN" runat="server" Enabled="false" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDMANDATORYYN")).ToString().Equals("1")?true:false %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkMandatoryYNEdit" runat="server" AutoPostBack="true" OnCheckedChanged="chkMandatoryYNEdit_CheckedChanged"
                                        Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDMANDATORYYN").ToString().Equals("1"))?true:false %>'>
                                    </asp:CheckBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="chkMandatoryYNAdd" runat="server" AutoPostBack="true" OnCheckedChanged="chkMandatoryYNAdd_CheckedChanged">
                                    </asp:CheckBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblWaiverHeader" runat="server" Text="Waiver Y/N"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkWaiverYN" runat="server" Enabled="false" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDWAIVERYN").ToString().Equals("1"))?true:false %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkWaiverYNEdit" Enabled="false" runat="server" AutoPostBack="true"
                                        OnCheckedChanged="chkWaiverYNEdit_CheckedChanged" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDWAIVERYN").ToString().Equals("1"))?true:false %>'>
                                    </asp:CheckBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="chkWaiverYNAdd" runat="server" Enabled="false" AutoPostBack="true"
                                        OnCheckedChanged="chkWaiverYNAdd_CheckedChanged"></asp:CheckBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblUsergroupHeader" runat="server" Text="User Group to allow Waiver"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblUserGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:UserGroup runat="server" ID="ucUserGroupEdit" CssClass="input" Enabled="false"
                                        AppendDataBoundItems="true" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:UserGroup runat="server" ID="ucUserGroupAdd" Enabled="false" CssClass="input"
                                        AppendDataBoundItems="true" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">Action</asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDITO" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETEO" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="UPDATEO" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="CancelO" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="AddO" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPageO" style="position: relative;" visible="false" runat="server">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap="nowrap" align="center">
                                <asp:Label ID="lblPagenumberO" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPagesO" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecordsO" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap="nowrap" align="left" width="50px">
                                <asp:LinkButton ID="cmdPreviousO" runat="server" OnCommand="PagerButtonClickO" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap="nowrap" align="right" width="50px">
                                <asp:LinkButton ID="cmdNextO" OnCommand="PagerButtonClickO" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap="nowrap" align="center">
                                <asp:TextBox ID="txtnopageO" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGoO" runat="server" Text="Go" OnClick="cmdGoO_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
              <%--  <br />--%>
               <%-- <b>User Defined Documents</b>--%>
                <div class="navSelect" style="position: relative; width: 15px" visible="false">
                    <eluc:TabStrip ID="MenuOffshoreUserDocument" runat="server" OnTabStripCommand="MenuOffshoreUserDocument_TabStripCommand" Visible="false">
                    </eluc:TabStrip>
                </div>
                <div id="divGridU" style="position: relative; z-index: 1" visible="false">
                    <asp:GridView ID="gvOffshoreDocumentUser" runat="server" AutoGenerateColumns="False" Visible="false"
                        Font-Size="11px" Width="100%" CellPadding="3" ShowFooter="true" OnRowCommand="gvOffshoreDocumentUser_RowCommand"
                        ShowHeader="true" EnableViewState="true"
                        DataKeyNames="FLDOFFSHOREDOCUMENTID" OnRowDataBound="gvOffshoreDocumentUser_RowDataBound">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblStageHeader" runat="server" Text="Stage"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStageName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlStageEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlStageAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblUserDocHeader" runat="server" Text="User defined Docs"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblUserdocName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Quick ID="ddlUserDocEdit" runat="server" CssClass="input_mandatory" QuickTypeCode="109" Width="200px"
                                            QuickList='<%#PhoenixRegistersQuick.ListQuick(1,109)%>' AppendDataBoundItems="true" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Quick ID="ddlUserDocAdd" runat="server" QuickTypeCode="109" CssClass="input_mandatory" Width="200px"
                                            QuickList='<%#PhoenixRegistersQuick.ListQuick(1,109)%>' AppendDataBoundItems="true" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblMandatoryHeader" runat="server" Text="Mandatory Y/N"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkMandatoryYN" runat="server" Enabled="false" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDMANDATORYYN")).ToString().Equals("1")?true:false %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkMandatoryYNEdit" runat="server" AutoPostBack="true" OnCheckedChanged="chkMandatoryYNEdit_CheckedChanged"
                                        Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDMANDATORYYN").ToString().Equals("1"))?true:false %>'>
                                    </asp:CheckBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="chkMandatoryYNAdd" runat="server" AutoPostBack="true" OnCheckedChanged="chkMandatoryYNAdd_CheckedChanged">
                                    </asp:CheckBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblWaiverHeader" runat="server" Text="Waiver Y/N"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkWaiverYN" runat="server" Enabled="false" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDWAIVERYN").ToString().Equals("1"))?true:false %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkWaiverYNEdit" Enabled="false" runat="server" AutoPostBack="true"
                                        OnCheckedChanged="chkWaiverYNEdit_CheckedChanged" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDWAIVERYN").ToString().Equals("1"))?true:false %>'>
                                    </asp:CheckBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="chkWaiverYNAdd" runat="server" Enabled="false" AutoPostBack="true"
                                        OnCheckedChanged="chkWaiverYNAdd_CheckedChanged"></asp:CheckBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblUsergroupHeader" runat="server" Text="User Group to allow Waiver"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblUserGroup" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPNAME") %>'></asp:Label>
                                    <asp:ImageButton id="ImgUserGroup" runat="server" ImageUrl="<%$ PhoenixTheme:images/te_view.png%>" CommandArgument='<%# Container.DataItemIndex %>'></asp:ImageButton>
                                    <eluc:Tooltip ID="ucUserGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPNAME") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <%--<eluc:UserGroup runat="server" ID="ucUserGroupEdit" CssClass="input" Enabled="false"
                                        AppendDataBoundItems="true" />--%>
                                    <div style="height: 50px; overflow: auto;" class="input">
                                        <asp:CheckBoxList ID="chkUserGroupEdit" RepeatDirection="Vertical" Enabled="false" runat="server">
                                        </asp:CheckBoxList>
                                    </div>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <%--<eluc:UserGroup runat="server" ID="ucUserGroupAdd" Enabled="false" CssClass="input"
                                        AppendDataBoundItems="true" />--%>
                                    <div style="height: 50px; overflow: auto;" class="input">
                                        <asp:CheckBoxList ID="chkUserGroupAdd" RepeatDirection="Vertical" Enabled="false" runat="server">
                                        </asp:CheckBoxList>
                                    </div>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">Action</asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDITU" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETEU" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="UPDATEU" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="CancelU" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="AddU" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPageU" style="position: relative;visibility:hidden">
                    <table width="100%" border="0" class="datagrid_pagestyle" style="visibility:hidden">
                        <tr>
                            <td nowrap="nowrap" align="center">
                                <asp:Label ID="lblPagenumberU" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPagesU" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecordsU" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap="nowrap" align="left" width="50px">
                                <asp:LinkButton ID="cmdPreviousU" runat="server" OnCommand="PagerButtonClickU" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap="nowrap" align="right" width="50px">
                                <asp:LinkButton ID="cmdNextU" OnCommand="PagerButtonClickU" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap="nowrap" align="center">
                                <asp:TextBox ID="txtnopageU" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGoU" runat="server" Text="Go" OnClick="cmdGoU_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
               <%-- <br />--%>
                <b>Experience</b>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuOffshoreDocumentExp" runat="server" OnTabStripCommand="MenuOffshoreDocumentExp_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGridE" style="position: relative; z-index: 1">
                    <asp:GridView ID="gvOffshoreDocumentExp" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3"  ShowFooter="false" OnRowCommand ="gvOffshoreDocumentExp_RowCommand" 
                        ShowHeader="true" EnableViewState="true" DataKeyNames ="FLDOFFSHOREDOCUMENTID" 
                        OnRowDataBound="gvOffshoreDocumentExp_RowDataBound"  >
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblStageHeader" runat="server" Text="Stage"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStageName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlStageEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"></asp:DropDownList>
                                    <asp:Label ID="lblDocumentType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>' >
                                    </asp:Label>
                                </EditItemTemplate>                          
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblExpHeader" runat="server" Text="Experience"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                                                        
                                    <asp:Label ID="lblExpName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate> 
                                  <asp:Label ID="lblExpNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></asp:Label>
                             
                                </EditItemTemplate>                            
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblMandatoryHeader" runat="server" Text="Mandatory Y/N"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:CheckBox ID="chkMandatoryYN" runat="server" Enabled="false" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDMANDATORYYN")).ToString().Equals("1")?true:false %>' />
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <asp:CheckBox ID="chkMandatoryYNEdit" runat="server" AutoPostBack="true" OnCheckedChanged="chkMandatoryYNEdit_CheckedChanged"
                                        Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDMANDATORYYN").ToString().Equals("1"))?true:false %>'>
                                    </asp:CheckBox>
                                </EditItemTemplate>                             
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblWaiverHeader" runat="server" Text="Waiver Y/N"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:CheckBox ID="chkWaiverYN" runat="server" Enabled="false" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDWAIVERYN").ToString().Equals("1"))?true:false %>' />
                                </ItemTemplate>
                                <EditItemTemplate>                                  
                                    <asp:CheckBox ID="chkWaiverYNEdit" Enabled="false" runat="server" AutoPostBack="true" OnCheckedChanged="chkWaiverYNEdit_CheckedChanged"
                                        Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDWAIVERYN").ToString().Equals("1"))?true:false %>'>
                                    </asp:CheckBox>
                                </EditItemTemplate>                          
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblUsergroupHeader" runat="server" Text="User Group to allow Waiver"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblUserGroup" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPNAME") %>'></asp:Label>
                                    <asp:ImageButton id="ImgUserGroup" runat="server" ImageUrl="<%$ PhoenixTheme:images/te_view.png%>" CommandArgument='<%# Container.DataItemIndex %>'></asp:ImageButton>
                                    <eluc:Tooltip ID="ucUserGroup" Width="200px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPNAME") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <%--<eluc:UserGroup runat="server" ID="ucUserGroupEdit" CssClass="input" Enabled="false" AppendDataBoundItems="true" />--%>
                                    <div style="height: 100px; overflow: auto;" class="input">
                                        <asp:CheckBoxList ID="chkUserGroupEdit" RepeatDirection="Vertical" Enabled="false" runat="server">
                                        </asp:CheckBoxList>
                                    </div>
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
                                        CommandName="EDITE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETEE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete" Visible="false"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="UPDATEE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="CancelE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />                            
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                 <div id="divPageE" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap="nowrap" align="center">
                                <asp:Label ID="lblPagenumberE" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPagesE" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecordsE" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap="nowrap" align="left" width="50px">
                                <asp:LinkButton ID="cmdPreviousE" runat="server" OnCommand="PagerButtonClickE" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap="nowrap" align="right" width="50px">
                                <asp:LinkButton ID="cmdNextE" OnCommand="PagerButtonClickE" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap="nowrap" align="center">
                                <asp:TextBox ID="txtnopageE" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGoE" runat="server" Text="Go" OnClick="cmdGoE_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
