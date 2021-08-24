<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionLongTermActionWorkOrderList.aspx.cs" Inherits="InspectionLongTermActionWorkOrderList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Long Term Action List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersBudgetBillingList" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlBudgetBillingListEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="Work Order" />
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuBulkPO" runat="server" OnTabStripCommand="MenuBulkPO_TabStripCommand" TabStrip="true"> 
                    </eluc:TabStrip>
                </div>
                <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="min-height: 350px; width: 100%">
                </iframe>             
                <div style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuLongTermAction" runat="server" OnTabStripCommand="MenuLongTermAction_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: +1">
                    <asp:GridView ID="gvLongTermActionWorkOrder" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        OnSorting="gvLongTermActionWorkOrder_Sorting" Width="100%" CellPadding="3" OnRowCommand="gvLongTermActionWorkOrder_RowCommand"
                        OnRowDataBound="gvLongTermActionWorkOrder_ItemDataBound" OnRowCancelingEdit="gvLongTermActionWorkOrder_RowCancelingEdit"
                        AllowSorting="true" OnRowEditing="gvLongTermActionWorkOrder_RowEditing" ShowFooter="false"
                        ShowHeader="true" EnableViewState="false" DataKeyNames="FLDWORKORDERID"
                        OnSelectedIndexChanging="gvLongTermActionWorkOrder_SelectedIndexChanging">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="Work Order Number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblWorkOrderNumberHeader" runat="server">Work Order <BR /> Number</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                     <asp:LinkButton ID="lnkWorkOrderNumber" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDWONUMBER") %>' Width="10px" ></asp:LinkButton>         
                                     <asp:Label ID="lblWorOrderId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERID") %>'></asp:Label>
                                     <asp:Label ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>                              
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Description">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" Width="50px" />
                                <HeaderTemplate>
                                   <asp:Label ID="lblDescriptionHeader" runat="server">Description</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWODESCRIPTION").ToString().Length>50 ? DataBinder.Eval(Container, "DataItem.FLDWODESCRIPTION").ToString().Substring(0, 50)+ "..." : DataBinder.Eval(Container, "DataItem.FLDWODESCRIPTION").ToString() %>'>
                                   </asp:Label>
                                    <eluc:ToolTip ID="ucToolTipDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWODESCRIPTION")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>                 
                            <asp:TemplateField HeaderText="Department">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" Width="20px" />
                                <HeaderTemplate>
                                   <asp:Label ID="lblDepartmentHeader" runat="server" > Department</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDepartment"   runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField HeaderText="Action Taken">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" Width="50px" />
                                <HeaderTemplate>
                                   <asp:Label ID="lblActionTakenHeader" runat="server">Action Taken </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblActionTaken" runat="server"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIONTAKEN").ToString().Length>50 ? DataBinder.Eval(Container, "DataItem.FLDACTIONTAKEN").ToString().Substring(0, 50)+ "..." : DataBinder.Eval(Container, "DataItem.FLDACTIONTAKEN").ToString() %>'>
                                    </asp:Label>
                                   <eluc:ToolTip ID="ucToolTipActionTaken" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIONTAKEN")%>' />
                                </ItemTemplate>
                            </asp:TemplateField> 
                                                                                                                                                                   
                            <asp:TemplateField HeaderText="Status">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblStatusHeader" runat="server">Status</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Width="30px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Closed Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="50px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblClosedDateHeader" runat="server">Closed Date</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblClosedDate" runat="server" Width="50px" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDDATE")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server"> Action </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="80px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                        CommandName="ATTACHMENT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAtt"
                                        ToolTip="Attachment"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblPagenumber" runat="server"> </asp:Label>
                                <asp:Label ID="lblPages" runat="server"> </asp:Label>
                                <asp:Label ID="lblRecords" runat="server"> </asp:Label>&nbsp;&nbsp;
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
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input"> </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="gvLongTermActionWorkOrder" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
