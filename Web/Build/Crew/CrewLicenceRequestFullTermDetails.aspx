<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewLicenceRequestFullTermDetails.aspx.cs" Inherits="CrewLicenceRequestFullTermDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Full Term Details</title>  
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewFullTermDetails" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlFullTerm">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status ID="ucStatus" runat="server" />
                <div class="subHeader" style="position: relative">
                    <div id="div2" style="vertical-align: top">
                        <eluc:Title runat="server" ID="Title1" Text="Full Term Details" ShowMenu="false">
                        </eluc:Title>
                    </div>
                </div>
                 <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:tabstrip id="CrewLicReq" runat="server" ontabstripcommand="CrewLicReq_TabStripCommand"
                        tabstrip="true">
                    </eluc:tabstrip>
                </div>
                <div>
                    <table width="100%">
                        <tr>
                            <td style="width:20px">                            
                                <asp:Literal ID="lblEmployeeName" runat="server" Text="Name"></asp:Literal>
                            </td>
                            <td style="width:30px">
                                <asp:TextBox runat="server" ID="txtEmployeeName" Width="220px"  CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td style="width:20px">
                                <asp:Literal ID="lblEmployeeNumber" runat="server" Text="File No" ></asp:Literal>
                            </td>
                            <td style="width:30px">
                                <asp:TextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>  
                         <tr>
                            <td>
                                <asp:Literal ID="lblLicence" runat="server" Text="Licence"></asp:Literal>
                            </td>
                            <td colspan="2">
                                <asp:TextBox runat="server" ID="txtLicence" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="220px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>   
                <div class="navSelect" id="divTab1" runat="server" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
                    </eluc:TabStrip>
                </div>      
                <asp:GridView runat="server" ID="gvFullTerm" AutoGenerateColumns="false" Width="100%" OnRowCommand="gvFullTerm_RowCommand"
                    ShowFooter="true" ShowHeader="true" EnableViewState="false" OnRowCancelingEdit="gvFullTerm_RowCancelingEdit"
                    OnRowEditing="gvFullTerm_RowEditing" OnRowUpdating="gvFullTerm_RowUpdating"
                    OnRowDataBound="gvFullTerm_RowDataBound">
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblFullTermDocsRecd" runat="server" Text="Received"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDRECDFULLTERMDATE")%>
                            </ItemTemplate> 
                             <FooterTemplate>
                                    <eluc:Date runat="server" ID="txtFulltermsRecievedDateAdd" CssClass="input_mandatory" />
                             </FooterTemplate>                           
                        </asp:TemplateField>  
                         <asp:TemplateField>
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblFullTermHandedOverby" runat="server" Text="Handed Over by"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDFULLTERMHANDEDOVERBYNAME")%>
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="300px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblComments" runat="server" Text="Comments"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>                                
                                <asp:Label ID="lblHandOverComments" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFULLTERMHANDEDOVERBYCOMMENTS") %>'></asp:Label>
                                <eluc:ToolTip ID="ucHandOverCommentsTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFULLTERMHANDEDOVERBYCOMMENTS") %>' />
                            </ItemTemplate> 
                            <FooterTemplate>
                            <asp:TextBox ID="txtHandOverCommentsAdd" runat="server" Width="300px" Height="30px" CssClass="gridinput_mandatory"
                                    TextMode="MultiLine"/>
                                  
                                </FooterTemplate>                           
                        </asp:TemplateField>
                         
                        <asp:TemplateField>
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblDate" runat="server" Text="Date"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDFULLTERMHANDOVERDATE")%>
                            </ItemTemplate>              
                        </asp:TemplateField>                        
                        <asp:TemplateField>
                            <ItemStyle Wrap="true" HorizontalAlign="center" Width="50px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblReceived" runat="server" Text="Received"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDISFULLTERMRECEIVEDNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkReceivedYN" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDISFULLTERMRECEIVED").ToString() == "1" ? true : false%>'  />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="350px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblComments" runat="server" Text="Comments"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblReceivedComments" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFULLTERMRECEIVEDCOMMENTS") %>'></asp:Label>
                                <eluc:ToolTip ID="ucReceivedCommentsTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFULLTERMRECEIVEDCOMMENTS") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtReceivedComments" runat="server" Width="350px" Height="30px" CssClass="gridinput_mandatory"
                                    TextMode="MultiLine" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFULLTERMRECEIVEDCOMMENTS") %>'>
                                </asp:TextBox>
                                <asp:Label ID="lblFullTermId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFULLTERMID") %>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblFullTermReceivedby" runat="server" Text="Received by"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDFULLTERMRECEIVEDBYNAME")%>
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblDate" runat="server" Text="Date"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDFULLTERMRECEIVEDDATE")%>
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="70px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblAction" runat="server" Text="Action"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                     <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                             <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="Add" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>  
                <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />          
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
