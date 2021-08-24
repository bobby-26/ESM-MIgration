<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewAppraisalInCompetent.aspx.cs" Inherits="CrewAppraisalInCompetent" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="ajaxToolkit" Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit"  %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Not Competent for the job</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>        
    </telerik:RadCodeBlock>
</head>

<body>
    <form id="frmCrewAppraisalInCompetent" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    
    <asp:UpdatePanel runat="server" ID="pnlCrewApprasial">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="Div1" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            
                <div class="subHeader">
                    <eluc:Title runat="server" ID="Appraisalactivity" Text="InCompetent Questions" ShowMenu="false"></eluc:Title>                        
                </div>
                
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuPersonalInCompetent" runat="server">
                    </eluc:TabStrip>
                </div>
                
                <div id="divGrid" style="position: relative; z-index: +1">
                    <asp:GridView ID="gvCrewInCompetentAppraisal" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" Width="100%" CellPadding="3" OnRowDataBound="gvCrewInCompetentAppraisal_RowDataBound"
                        OnRowDeleting="gvCrewInCompetentAppraisal_RowDeleting" OnRowEditing="gvCrewInCompetentAppraisal_RowEditing"
                        Style="margin-bottom: 0px" EnableViewState="false" OnRowCommand="gvCrewInCompetentAppraisal_RowCommand"
                        OnRowCancelingEdit="gvCrewInCompetentAppraisal_RowCancelingEdit" OnRowUpdating="gvCrewInCompetentAppraisal_RowUpdating" >
                        
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        
                        <Columns>                            
                            <asp:TemplateField HeaderText="Evaluation Item">
                                <ItemTemplate>
                                    <asp:Label ID="lblAppraisalInCompetentId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAPPRAISALINCOMPETENTID")%>'></asp:Label>
                                    <asp:Label ID="lblQuestionId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDQUICKCODE")%>'></asp:Label>
                                    <asp:Label ID="lblevaluationitem" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDQUICKNAME")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <%--<asp:TemplateField HeaderText="Rating">
                                <ItemTemplate>
                                    <asp:Label ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRESPONSE")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number runat="server" ID="ucRatingEdit" CssClass="input_mandatory" MaxLength="2"
                                         Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESPONSE") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number runat="server" ID="ucRatingAdd" CssClass="input_mandatory" MaxLength="2" />
                                </FooterTemplate>
                            </asp:TemplateField>--%>
                            
                            <asp:TemplateField HeaderText="Remarks">
                                <ItemTemplate>
                                    <asp:Label ID="lblRemark" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDREMARKS")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtRemarksEdit" CssClass="gridinput" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDREMARKS")%>'></asp:TextBox>
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
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl='<%$ PhoenixTheme:images/te_edit.png%>'
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl='<%$ PhoenixTheme:images/te_del.png%>'
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl='<%$ PhoenixTheme:images/save.png%>'
                                        CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img11" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png%>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl='<%$ PhoenixTheme:images/te_del.png%>'
                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>                              
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>              
    
                <eluc:Status runat="server" ID="ucStatus" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
