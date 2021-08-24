<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsCrewBirthdayAlert.aspx.cs"
    Inherits="OptionsCrewBirthdayAlert" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Birthday Alert</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

</telerik:RadCodeBlock></head>
<form id="frmBirthdayAlert" runat="server">
<div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:updatepanel id="UpdatePanel1" runat="server">
            <ContentTemplate>
            <div class="subHeader" style="position: relative">
                <eluc:Title runat="server" ID="Title" Text="" ShowMenu="false"></eluc:Title>
            </div> 
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuSendMail" runat="server" OnTabStripCommand="MenuSendMail_TabStripCommand"
                        TabStrip="false"></eluc:TabStrip>
                </div>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error> 
                <eluc:Status ID="ucStatus" runat="server" />         
                <br clear="all" />                
                <table width="80%">
                    <tr>
                        <td>
                            <asp:GridView ID="gvAlertsTask" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" OnRowCommand="gvAlertsTask_RowCommand" OnRowDataBound="gvAlertsTask_ItemDataBound"
                                EnableViewState="false" AllowSorting="true" OnSorting="gvAlertsTask_Sorting">
                                
                                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                <RowStyle Height="10px" />
                                
                                <Columns>
                                    
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lblNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                                ForeColor="White">Name &nbsp;</asp:LinkButton>
                                            <img id="FLDNAME" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTaskKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKKEY") %>'></asp:Label>
                                            <asp:Label ID="lblTaskType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'></asp:Label>
                                            <asp:Label ID="lblExpression" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPRESSION") %>'></asp:Label>
                                            <asp:LinkButton ID="lblDescriptionName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' CommandArgument="<%# Container.DataItemIndex %>"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                  
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lblRankHeader" runat="server" CommandName="Sort" CommandArgument="FLDRANKNAME"
                                                ForeColor="White">Rank &nbsp;</asp:LinkButton>
                                            <img id="FLDRANKNAME" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescriptionRank" runat="server" 
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal id="lblStatusHeader" runat="server" Text="Status"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" 
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                                                        <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal id="lblVesselHeader" runat="server" Text="Vessel"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblVessel" runat="server" 
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="View Date">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal id="lblBirthDate" runat="server" Text="Birth Date"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDateofBirth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFBIRTH") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="View Date">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblViewDateHeader" runat="server">Viewed Date&nbsp;
                                            </asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVIEWDATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="View By">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblViewByHeader" runat="server">Viewed By&nbsp;
                                            </asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVIEWBY") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="gvAlertsTask" />
            </Triggers>            
        </asp:updatepanel>
</div>
</form>
</html>
