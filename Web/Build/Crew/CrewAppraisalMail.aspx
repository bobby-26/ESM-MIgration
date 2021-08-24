<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewAppraisalMail.aspx.cs" Inherits="CrewAppraisalMail" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew In-Active</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewList" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCrewList">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">                
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <eluc:Title runat="server" ID="ucTitle" Text="Crew Appraisal Mail" ShowMenu="false" />                        
                    </div>
                </div>                               
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuAppraisal" runat="server" OnTabStripCommand="MenuAppraisal_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table cellpadding="1" cellspacing="1" width="50%">
                    <tr>
                        <td><asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal></td>
                        <td>
                            <asp:TextBox ID="txtVessel" runat="server" ReadOnly="true" 
                                CssClass="readonlytextbox" Width="200px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblOnDate" runat="server" Text="On Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtOnDate" runat="server" CssClass="input" />
                        </td>                        
                    </tr>
                </table>
                 <br /> 
                 <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuCrewList" runat="server" OnTabStripCommand="CrewList_TabStripCommand">
                    </eluc:TabStrip>
                </div> 
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvCrewList" runat="server" AutoGenerateColumns="False" Width="100%"
                        OnRowDataBound="gvCrewList_RowDataBound" AllowSorting="true" OnSorting="gvCrewList_Sorting"
                        CellPadding="3" ShowFooter="false" ShowHeader="true" EnableViewState="false">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                 <HeaderTemplate>
                                    <asp:LinkButton ID="lblNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDEMPLOYEENAME"
                                                        ForeColor="White">Name&nbsp;</asp:LinkButton>
                                    <img id="FLDEMPLOYEENAME" runat="server" visible="false" />                                   
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <asp:Label ID="lblCrewId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></asp:Label>
                                    <asp:Label ID="lblSGId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONOFFID") %>'></asp:Label> 
                                     <asp:Label ID="lblSignOff" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNOFFDATE") %>'></asp:Label>
                                     <asp:Label ID="lblSeaPort" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNOFFSEAPORT") %>'></asp:Label>                                                                  
                                    <asp:LinkButton ID="lnkCrew" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="Employee No">
                                 <HeaderTemplate>
                                    <asp:LinkButton ID="lblEmpCodeHeader" runat="server" CommandName="Sort" CommandArgument="FLDEMPLOYEECODE"
                                                        ForeColor="White">Employee Code&nbsp;</asp:LinkButton>
                                    <img id="FLDEMPLOYEECODE" runat="server" visible="false" />                                   
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />                               
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDEMPLOYEECODE")%>
                                </ItemTemplate>
                            </asp:TemplateField> --%>
                            <asp:TemplateField>
                                 <HeaderTemplate>
                                    <asp:LinkButton ID="lblRankHeader" runat="server" CommandName="Sort" CommandArgument="FLDRANKNAME"
                                                        ForeColor="White">Rank&nbsp;</asp:LinkButton>
                                    <img id="FLDRANKNAME" runat="server" visible="false" />                                   
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />                              
                                <ItemTemplate>
                                    <asp:Label ID="lblRank" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                 <HeaderTemplate>
                                    <asp:LinkButton ID="lblNationalityHeader" runat="server" CommandName="Sort" CommandArgument="FLDNATIONALITYNAME"
                                                        ForeColor="White">Nationality&nbsp;</asp:LinkButton>
                                    <img id="FLDNATIONALITYNAME" runat="server" visible="false" />                                   
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />                              
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDNATIONALITYNAME")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">
                                 <ItemStyle HorizontalAlign="Left" Wrap="False" />   
                                <ItemTemplate>
                                    <asp:Label ID="lblType" runat="server" Visible="false" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDTYPE")%>'></asp:Label>
                                     <%# DataBinder.Eval(Container, "DataItem.FLDSTATUS")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblPPTNoHeader" runat="server" CommandName="Sort" CommandArgument="FLDPASSPORTNO"
                                                        ForeColor="White">Passport No&nbsp;</asp:LinkButton>
                                    <img id="FLDPASSPORTNO" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <asp:Label ID="lblOverDue" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOVERDUE")%>'></asp:Label>
                                    <asp:Label ID="lblEmployeeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEMPLOYEEID")%>'></asp:Label>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDPASSPORTNO")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblBirthDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDDATEOFBIRTH"
                                                        ForeColor="White">Birth Date&nbsp;</asp:LinkButton>
                                    <img id="FLDDATEOFBIRTH" runat="server" visible="false" />                                   
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />                              
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDDATEOFBIRTH", "{0:dd/MMM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                 <HeaderTemplate>
                                    <asp:LinkButton ID="lblJoinDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDSIGNONDATE"
                                                        ForeColor="White">(Exp.) Join&nbsp;</asp:LinkButton>
                                    <img id="FLDSIGNONDATE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />                              
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDSIGNONDATE", "{0:dd/MMM/yyyy}") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                 <HeaderTemplate>
                                    <asp:LinkButton ID="lblReliefDueHeader" runat="server" CommandName="Sort" CommandArgument="FLDRELIEFDUEDATE"
                                                        ForeColor="White">Relief Due&nbsp;</asp:LinkButton>
                                    <img id="FLDRELIEFDUEDATE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />                              
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE", "{0:dd/MMM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>  
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblSMBKNOHeader" runat="server" CommandName="Sort" CommandArgument="FLDSEAMANBOOKNO"
                                                        ForeColor="White">CDC No&nbsp;</asp:LinkButton>
                                    <img id="FLDSEAMANBOOKNO" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />                              
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDSEAMANBOOKNO")%>
                                </ItemTemplate>
                            </asp:TemplateField>                            
                        </Columns>
                    </asp:GridView>
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
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                    <table cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <table>
                                    <tr class="rowred">
                                        <td width="5px" height="10px">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <asp:Literal ID="lblOverdueRelief" runat="server" Text="* Overdue Relief"></asp:Literal>
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
