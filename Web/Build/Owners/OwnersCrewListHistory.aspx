<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersCrewListHistory.aspx.cs"
    Inherits="OwnersCrewListHistory" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlOwnersVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Report for a Period</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixCrew.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
     <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts ="false" >
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCrewReportEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Crew List History"></eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                        TabStrip="false"></eluc:TabStrip>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <div>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                                </td>
                                <td colspan="2">
                                    <asp:Panel ID="pnlPeriod" runat="server" GroupingText="Period" Width="60%">
                                    <table>
                                    <tr>
                                    <td>
                                        <asp:Literal ID="lblFromDate" runat="server" Text="From Date"></asp:Literal> </td>
                                        <td>
                                        <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" /></td>
                                        <td><asp:Literal ID="lblToDate" runat="server" Text="To Date"></asp:Literal></td>
                                        <td>
                                        <eluc:Date ID="ucDate1" runat="server" CssClass="input_mandatory" /></td>
                                        </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>                           
                        </table>
                    </div>
                    <div class="navSelect" id="divTab1" runat="server" style="position: relative; width: 15px">
                        <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative; overflow: auto; z-index: 0">
                        <asp:GridView ID="gvCrew" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            OnRowDataBound="gvCrew_RowDataBound" Width="100%" CellPadding="3" ShowHeader="true"
                            EnableViewState="false" AllowSorting="true" RowStyle-Wrap="false" OnSorting="gvCrew_Sorting">
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <Columns>
                                <asp:TemplateField>                                 
                                 <HeaderTemplate>
                                    <asp:Literal ID="lblSrNo" runat="server" Text="Sr.No"></asp:Literal>
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Center" Wrap="False" />                              
                                <ItemTemplate>
                                    <asp:Label ID="lblSrno" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDROW")%>'></asp:Label>                                    
                                </ItemTemplate>
                             </asp:TemplateField>
                             <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                     ForeColor="White">Name&nbsp;</asp:LinkButton>
                                    <img id="FLDNAME" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                                        <asp:Label ID="lblName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblEmpNo" runat="server" Text="Emp.No"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmpFileNo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRankid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></asp:Label>
                                        <asp:Label ID="lblRank" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblDOB" runat="server" Text="DOB"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblBirthDate" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFBIRTH") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblPassportNo" runat="server" Text="Passport No"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblPassportNo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSPORTNO") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblDOI" runat="server" Text="DOI"></asp:Literal>
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>                                                                       
                                    <%# DataBinder.Eval(Container, "DataItem.FLDDOI","{0:dd/MMM/yyyy}")%>
                                </ItemTemplate> 
                            </asp:TemplateField> 
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblPOI" runat="server" Text="POI"></asp:Literal>
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>                                                                       
                                    <%# DataBinder.Eval(Container, "DataItem.FLDPLACEOFISSUE")%>
                                </ItemTemplate> 
                            </asp:TemplateField>  
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblDOE" runat="server" Text="DOE"></asp:Literal>
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>                                                                       
                                    <%# DataBinder.Eval(Container, "DataItem.FLDDOE", "{0:dd/MMM/yyyy}")%>
                                </ItemTemplate> 
                            </asp:TemplateField>  
                             <asp:TemplateField>
                                 <HeaderTemplate>
                                    <asp:LinkButton ID="lblJoinDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDSIGNONDATE"
                                                        ForeColor="White">Sign On Date&nbsp;</asp:LinkButton>
                                    <img id="FLDSIGNONDATE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />                              
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDSIGNONDATE", "{0:dd/MMM/yyyy}") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblReliefDue" runat="server" Text="Relief Due"></asp:Literal>
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>                                                                       
                                    <%# DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE", "{0:dd/MMM/yyyy}")%>
                                </ItemTemplate> 
                            </asp:TemplateField>   
                            <asp:TemplateField>
                                 <HeaderTemplate>
                                    <asp:Literal ID="lblJoiningPort" runat="server" Text="Joining Port"></asp:Literal>
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />                              
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDSEAPORTNAME", "{0:dd/MMM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField> 
                            
<%--                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblNationality" runat="server" Text="Nationality"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblNationality" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNATIONALITY") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>                          
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblFrom" runat="server" Text="From"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblFrom" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblTo" runat="server" Text="To"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblTo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNOFFDATE") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblDtFirstJoin" runat="server" Text="Dt.FirstJoin"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDtFirstJoin" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFJOINING") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblBToD" runat="server" Text="BToD"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblBtod" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBTOD") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblEToD" runat="server" Text="EToD"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblEtod" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDETOD") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblSMBNKNo" runat="server" Text="SMBNK No"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSmbnkNo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAMANBOOKNO") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblTfrPomStatus" runat="server" Text="Tfr./Pom Status"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblTfrPomStatus" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROMOTIONYN") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="divPage" runat="server" style="position: relative;">
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
                            <eluc:Status runat="server" ID="ucStatus" />
                        </table>
                    </div>
                    <asp:Label ID="lblNote" runat="server" ForeColor="Blue" Font-Bold="true" Text=" Note: Access is denied to see the CrewList History"></asp:Label>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
