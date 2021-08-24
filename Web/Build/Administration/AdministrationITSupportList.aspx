<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdministrationITSupportList.aspx.cs"
    Inherits="AdministrationITSupportList" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SUPDepartment" Src="~/usercontrols/UserControlDepartmentList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ADMITCategory" Src="~/usercontrols/UserControlITCategoryList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ADMITStatus" Src="~/UserControls/UserControITStatusList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ITTeam" Src="~/UserControls/UserControlITTeam.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPStatus" Src="~/UserControls/UserControlSEPStatus.ascx" %>
<%--<%@ Register TagPrefix="eluc" TagName="ITStatus" Src="~/UserControls/UserControlITStatusList.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ITSupportSearch</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<form id="form1" runat="server">
<div class="subHeader" style="position: relative">
    <div id="divHeading" style="vertical-align: top">
        <eluc:Title runat="server" ID="ucTitle" Text="Support Request"></eluc:Title>
        <asp:button id="cmdHiddenSubmit" runat="server" text="cmdHiddenSubmit" onclick="cmdHiddenSubmit_Click" />
    </div>
</div>
<ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="True">
</ajaxToolkit:ToolkitScriptManager>
<asp:updatepanel runat="server" id="pnlITSupportSearch">
        <ContentTemplate>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
         <div id="divFind" style="margin-top:0px;">
          <table width="100%">
                    <tr>
                        <td colspan="4">
                            <font color="blue" size="0">
                            <b>Status Change</b>
                            <li>Open >> In-Progress >> Closed </li>
                            <li>Open >> In-Progress >> Closed >> Re-Open</li>
                            <li>Re-Open >> Assigned >> In-Progress >> Closed</li>
                            </font>
                        </td>                      
                    </tr>
                </table>          
                 <table width="100%" cellpadding="0" cellspacing="0" border="1">
                    <tr>                        
                        <td width="30%">
                            <table>
                                                          
                                <tr>
                                    <td>
                                        S.No
                                    </td>
                                    <td>
                                        <asp:TextBox id="txtIDSearch" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>                                
                                <tr>
                                    <td>
                                        Call Type
                                    </td>
                                    <td>
                                        <asp:TextBox id="txtCallTypeSearch" runat="server" MaxLength="100" CssClass="input" Width="120px"></asp:TextBox>
                                    </td>
                                </tr>                                
                                <tr>
                                    <td>
                                        System Name
                                    </td>
                                    <td>
                                        <asp:TextBox id="txtSystemNameSearch" runat="server" MaxLength="100" CssClass="input" Width="120px"></asp:TextBox>
                                    </td>
                                </tr>                       
                                <tr>
                                    <td>
                                        Logged Between
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucFromDate" runat="server" CssClass="input"/>
                                        <eluc:Date ID="ucToDate" runat="server" CssClass="input"/>
                                    </td>
                                </tr>    
                            <tr>
                                <td>
                                Attend By
                                </td>
                                <td>
                                <eluc:ITTeam ID="ddlITTeam" runat="server" MaxLength="100" Width="150px" AppendDataBoundItems="true"
                                    CssClass="input" />
                                </td>
                            </tr>
                               <tr>
                                     <td>
                                        Action Taken
                                    </td>
                                    <td>
                                        <asp:TextBox id="txtActionTakenSearch" runat="server" MaxLength="100" CssClass="input" Width="120px"></asp:TextBox>
                                    </td>
                                </tr> 
                                    
                                <tr>
                                    <td>
                                       Request User Name
                                    </td>
                                    <td>
                                        <asp:TextBox id="txtLoggedBySearch" runat="server" MaxLength="100" CssClass="input"></asp:TextBox>&nbsp;
                                        <asp:ImageButton runat="server" AlternateText="This Login" ImageUrl="<%$ PhoenixTheme:images/on-signer.png %>"
                                                        CommandName="THISUSER" ID="cmdThisUser" ToolTip="This Login" OnClick="cmdThisUser_Click"></asp:ImageButton>
                                    </td>
                                </tr>                                                                                           
                            </table>
                        </td>
                       <td width="30%">
                            Department<br />
                            <eluc:SUPDepartment  id="ucSUPDepartment" runat="server" AutoPostBack="true" OnTextChangedEvent="Filter_Changed"/>
                        </td>
                      <td width="20%">
                            Category<br />
                            <eluc:ADMITCategory  id="ucITCategory" runat="server" AutoPostBack="true" OnTextChangedEvent="Filter_Changed"/>
                        </td>
                     <td width="20%">
                            Status<br />
                            <eluc:ADMITStatus  id="ucITStatus" runat="server" AutoPostBack="true" OnTextChangedEvent="Filter_Changed"/>
                        </td>
                    </tr> 
                </table>
            </div>
            <div class="navSelect" style="position: relative; width: 15px">
                <eluc:TabStrip ID="MenuITSupportTracker" runat="server" OnTabStripCommand="MenuITSupportTracker_TabStripCommand">
                </eluc:TabStrip>
            </div>
            <asp:GridView ID="ITSupportSearchGrid" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                 Width="100%" CellPadding="3" OnRowDataBound="ITSupportSearchGrid_RowDataBound" OnRowEditing="ITSupportSearchGrid_RowEditing"
                 OnRowCommand="ITSupportSearchGrid_RowCommand" OnSorting="ITSupportSearchGrid_Sorting"  OnRowDeleting="ITSupportSearchGrid_RowDeleting" 
                  ShowFooter="True" EnableViewState="false" AllowSorting="True">
               <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" Wrap="False" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <Columns>
                     <asp:TemplateField HeaderText="Bug Id">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkBugIDHeader" runat="server" CommandName="Sort" CommandArgument="FLDBUGID"
                                    ForeColor="White">S.No&nbsp;</asp:LinkButton>
                            <img id="FLDBUGID" runat="server" visible="false" />
                            
                        </HeaderTemplate>                                               
                            
                        <ItemTemplate>
                             <asp:Label id="lblUniqueID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>' Visible="false"></asp:Label>
                             <asp:LinkButton ID="lnkBugId" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUGID") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Date">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Call Rec.Date
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDDATE", "{0:dd/MMM/yyyy}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="LoggedBy">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                           Request User Name
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDLOGGEDBY")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SystemName">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                           System Name
                        </HeaderTemplate>
                        <ItemTemplate>
                           
                            <%# DataBinder.Eval(Container, "DataItem.FLDSYSTEMNAME")%>                                      
                        </ItemTemplate>
                    </asp:TemplateField>
               <asp:TemplateField HeaderText="Department">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                         Department
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDDEPARTMENTNAME")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                   <asp:TemplateField HeaderText="Category">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                         Category
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDNAME")%>
                        </ItemTemplate>
                    </asp:TemplateField>                       
                    <asp:TemplateField HeaderText="CallType">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                         Call Type
                        </HeaderTemplate>
                        <ItemTemplate>
                         <asp:LinkButton id="lnkCallType" runat="server" CommandName="EDIT"
                                Text='<%# DataBinder.Eval(Container, "DataItem.FLDCALLTYPE").ToString().Length>20 ? DataBinder.Eval(Container, "DataItem.FLDCALLTYPE").ToString().Substring(0, 20)+ "..." : DataBinder.Eval(Container, "DataItem.FLDCALLTYPE").ToString() %>'></asp:LinkButton>
                            <eluc:ToolTip ID="ucCallType" runat="server"  Text='<%# DataBinder.Eval(Container, "DataItem.FLDCALLTYPE")%>'  />   
                            
                        </ItemTemplate>
                    </asp:TemplateField>                            
                    <asp:TemplateField HeaderText="Action Taken">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                           Action Taken
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDACTIONTAKEN")%>
                        </ItemTemplate>
                    </asp:TemplateField> 
                    <asp:TemplateField HeaderText="Attend By">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                           Attended By
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDATTENDBY")%>
                        </ItemTemplate>
                    </asp:TemplateField> 
                    <asp:TemplateField HeaderText="Status">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                         Status
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDSTATUSNAME")%>
                        </ItemTemplate>
                    </asp:TemplateField>                           
                    <asp:TemplateField HeaderText="Closed On">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Closed On
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDCLOSEDON"))%>
                        </ItemTemplate>
                    </asp:TemplateField>  
                  <asp:TemplateField HeaderText="Remarks">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Remarks
                        </HeaderTemplate>
                        <ItemTemplate>
                       <asp:Label ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Length>20 ? DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Substring(0, 20)+ "..." : DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString() %>'></asp:Label>                            
                            <eluc:ToolTip ID="ucRemarksTooltip" runat="server"  Text='<%# DataBinder.Eval(Container, "DataItem.FLDREMARKS")%>'  />   
                        </ItemTemplate>
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
                                ToolTip="Edit"></asp:ImageButton>
                            <img id="Img3" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                                 
                           <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                ToolTip="Delete"></asp:ImageButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                ToolTip="Save"></asp:ImageButton>
                            <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                ToolTip="Cancel"></asp:ImageButton>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>                
                </asp:GridView>
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
                    <eluc:Status runat="server" ID="ucStatus" />                
        </ContentTemplate>
    </asp:updatepanel>
</form>
</html>
