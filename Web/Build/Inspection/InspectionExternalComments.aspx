<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionExternalComments.aspx.cs" Inherits="Inspection_InspectionExternalComments" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inspector Remarks</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    
    <div id="Div1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmInspectionNCExtensionReason" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlDiscussion">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="div2" style="vertical-align: top">
                        <eluc:Title runat="server" ID="Title1" Text="Comments About Inspector" ShowMenu="false">
                        </eluc:Title>
                    </div>
                </div>
                <table>
                    <tr>
                        <td>
                            <asp:Literal ID="lblNameofInspecor" runat="server" Text="Name of Inspector"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNameOfInspector" runat="server" 
                                    CssClass="readonlytextbox" ReadOnly="true" Width="90%">
                                </asp:TextBox>
                        </td>
                    </tr>
                </table>
                <%--<div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuNCGeneral" runat="server" TabStrip="true" OnTabStripCommand="MenuNCRGeneral_TabStripCommand">
                    </eluc:TabStrip>
                </div> --%>               
                <br />
               <div id="divGrid" style="position: relative; z-index: 1; width: 100%; overflow:auto">                
                    <asp:GridView ID="gvExtensionReason" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3"
                        ShowHeader="true" EnableViewState="false" AllowSorting="true">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="VesselName">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:label ID="lblRefrenceIDHeader" Runat="server" >Refrence ID</asp:label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblReferenceId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="New Target Date">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                   <asp:Label ID="lblInspectiondateHeader" runat="server"> Inspection Date</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <asp:Label ID="lblNewTargetDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDINSPECTIONDATE")) %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Extension Reason">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                   <asp:Label ID="lblCompanyNameHeader" runat="server"> Company Name</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYNAME") %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Posted By">
                                <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblRemarksHeader" runat="server">Remarks</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPostedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKSABTINSPECTOR") %>' Width="75%"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <%--<div id="divPage" style="position: relative;">                  
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
                                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" CssClass="input"
                                        Width="40px"></asp:Button>
                                </td>
                            </tr>
                        </table>                   
                </div>--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
