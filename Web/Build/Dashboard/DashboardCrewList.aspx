<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardCrewList.aspx.cs" Inherits="Dashboard_DashboardCrewList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="DivHeader">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
            
            <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />

            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
    </div>
</telerik:RadCodeBlock></head>
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
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                    <eluc:TabStrip ID="MenuDdashboradVesselParticulrs" runat="server" OnTabStripCommand="MenuDdashboradVesselParticulrs_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuCrewList" runat="server" OnTabStripCommand="CrewList_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView GridLines="None" ID="gvCrewList" runat="server" AutoGenerateColumns="False" Width="100%"
                        AllowSorting="true" CellPadding="3" ShowFooter="false"
                        ShowHeader="true" EnableViewState="false">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSrNo" runat="server">Sr.No</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDROWNUMBER")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDEMPLOYEENAME"
                                        >Name&nbsp;</asp:LinkButton>
                                    <img id="FLDEMPLOYEENAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <asp:Label ID="lblCrewId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></asp:Label>
                                    <asp:Label ID="lblConId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTRACTID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblexhandyn" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXHAND") %>'></asp:Label>
                                    <asp:Label ID="lblNewApp" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWAPP") %>'></asp:Label>
                                    <asp:Label ID="lblFamilyId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYID") %>'></asp:Label>
                                    <asp:Label ID="lblVesselName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                    <asp:Label ID="lblCrewPlanYN" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWPLANYN") %>'></asp:Label>
                                    <asp:Label ID="lnkCrew" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></asp:Label>
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
                                        >Rank&nbsp;</asp:LinkButton>
                                    <img id="FLDRANKNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <asp:Label ID="lblrankid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></asp:Label>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblNationalityHeader" runat="server" CommandName="Sort" CommandArgument="FLDNATIONALITYNAME"
                                        >Nationality&nbsp;</asp:LinkButton>
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
                                        >PP No.&nbsp;</asp:LinkButton>
                                    <img id="FLDPASSPORTNO" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <asp:Label ID="lblDue" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDUE")%>'></asp:Label>
                                    <asp:Label ID="lblOverDue" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOVERDUE")%>'></asp:Label>
                                    <asp:Label ID="lblEmployeeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEMPLOYEEID")%>'></asp:Label>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDPASSPORTNO")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblBirthDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDDATEOFBIRTH"
                                        >Birth Date&nbsp;</asp:LinkButton>
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
                                        >(Exp.) Join&nbsp;</asp:LinkButton>
                                    <img id="FLDSIGNONDATE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDSIGNONDATE", "{0:dd/MMM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblExpDecimal" runat="server" Text="Exp(M)"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblExp" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDECIMALEXPERIENCE")%>'
                                        ToolTip='<%# "Total Exp(M): " + DataBinder.Eval(Container.DataItem,"FLDTOATLEXPERIENCE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PD Status">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblPDStatusid" runat="server" Visible="false" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDPDSTATUSID")%>'></asp:Label>
                                    <asp:Label ID="lblPDStatus" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPDSTATUS")%>'></asp:Label>
                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblReliefDueHeader" runat="server" CommandName="Sort" CommandArgument="FLDRELIEFDUEDATE"
                                        >Relief Due&nbsp;</asp:LinkButton>
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
                                        >CDC No&nbsp;</asp:LinkButton>
                                    <img id="FLDSEAMANBOOKNO" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDSEAMANBOOKNO")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>                 
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
