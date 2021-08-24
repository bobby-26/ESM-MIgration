<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDrillReport.aspx.cs" Inherits="Registers_DrillReport" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="vessellist" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Drill Log</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvMandatoryDrillReport.ClientID %>"));
               }, 200);
                setTimeout(function () {
                   TelerikGridResize($find("<%= gvCompanySpecifiedDrillReport.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
   
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                    <telerik:AjaxUpdatedControl ControlID="RadWindowManager1" />
                    <telerik:AjaxUpdatedControl ControlID="RadInputManager1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
    <eluc:TabStrip ID="Tabstripdrillreport" runat="server" OnTabStripCommand="drillreport_TabStripCommand"
        TabStrip="true"></eluc:TabStrip>
    <table id="table1">
        <tbody>
            <tr>
                <th>
                    <telerik:RadLabel ID="lblvesselname" runat="server" Text="Vessel Name" />
                </th>
                <th>
                    :
                </th>
                <td>
                    <eluc:vessellist ID="ddlvessellist" runat="server" Width="250px" CssClass="input" SyncActiveVesselsOnly="true"  ManagementType="FUL" 
                      Entitytype="VSL"  AutoPostBack="false" ActiveVesselsOnly="true"  VesselsOnly="true" AssignedVessels="true"/>
                </td>
                <th>
                
                <telerik:RadLabel ID="lblyear" runat="server" Text ="Year" />
                </th>
                <th>
                :
                </th>
                <td>
                <telerik:RadComboBox ID="radcomboyear" runat="server" AllowCustomText="true" EmptyMessage="Type to Select" />
                </td>
            </tr>
        </tbody>
    </table>
    <eluc:TabStrip ID="Tabstripdrillreportmenu" runat="server" OnTabStripCommand="drillreportmenu_TabStripCommand"
        TabStrip="true"></eluc:TabStrip>
    <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvMandatoryDrillReport"
        AutoGenerateColumns="false" OnNeedDataSource="gvMandatoryDrillReport_NeedDataSource" OnItemDataBound="gvMandatoryDrillReport_ItemDataBound">
        <MasterTableView AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="None"
            ShowHeadersWhenNoRecords="true" EnableColumnsViewState="false">
            <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true" ></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
            <Columns>
                <telerik:GridTemplateColumn HeaderText="Mandatory Drills/ Exercise">
                    <HeaderStyle HorizontalAlign="Left" Width="150px" Font-Bold="true" />
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <telerik:RadLabel ID="RadlblDrillName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDRILLNAME")%>'>
                        </telerik:RadLabel>
                          <telerik:RadLabel ID="Radlbldrillid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDRILLID")%>'>
                        </telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Scenario" >
                    <HeaderStyle HorizontalAlign="Center"  Width="120px" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <telerik:RadLabel ID="RadlblScenario" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSCENARIO")%>'>
                        </telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Interval">
                    <HeaderStyle HorizontalAlign="Center" width ="100px" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <telerik:RadLabel ID="RadlblInterval" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDINTERVAL")%>'>
                        </telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Jan">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <a id="Jananchor" runat="server" style="text-decoration: none; color: black">
                            <%# DataBinder.Eval(Container, "DataItem.FLDJAN")%></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Feb">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <a id="Febanchor" runat="server" style="text-decoration: none; color: black">
                            <%# DataBinder.Eval(Container, "DataItem.FLDFEB")%></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Mar">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                         <a id="Maranchor" runat="server" style="text-decoration: none; color: black">
                            <%# DataBinder.Eval(Container, "DataItem.FLDMAR")%></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Apr">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <a id="Apranchor" runat="server" style="text-decoration: none; color: black">
                            <%# DataBinder.Eval(Container, "DataItem.FLDAPR")%></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="May">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <a id="Mayanchor" runat="server" style="text-decoration: none; color: black">
                            <%# DataBinder.Eval(Container, "DataItem.FLDMAY")%></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Jun">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <a id="Junanchor" runat="server" style="text-decoration: none; color: black">
                            <%# DataBinder.Eval(Container, "DataItem.FLDJUN")%></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="July">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <a id="Julanchor" runat="server" style="text-decoration: none; color: black">
                            <%# DataBinder.Eval(Container, "DataItem.FLDJUL")%></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Aug">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <a id="Auganchor" runat="server" style="text-decoration: none; color: black">
                            <%# DataBinder.Eval(Container, "DataItem.FLDAUG")%></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Sept">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <a id="Sepanchor" runat="server" style="text-decoration: none; color: black">
                            <%# DataBinder.Eval(Container, "DataItem.FLDSEP")%></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Oct">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <a id="Octanchor" runat="server" style="text-decoration: none; color: black">
                            <%# DataBinder.Eval(Container, "DataItem.FLDOCT")%></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Nov">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                       <a id="Novanchor" runat="server" style="text-decoration: none; color: black">
                            <%# DataBinder.Eval(Container, "DataItem.FLDNOV")%></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Dec">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <a id="Decanchor" runat="server" style="text-decoration: none; color: black">
                            <%# DataBinder.Eval(Container, "DataItem.FLDDEC")%></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <div>
     <eluc:TabStrip ID="Tabstripdrillreportmenu_1" runat="server" OnTabStripCommand="drillreportmenu1_TabStripCommand"
        TabStrip="true"></eluc:TabStrip>
    <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvCompanySpecifiedDrillReport"
        AutoGenerateColumns="false" OnNeedDataSource="gvCompanySpecifiedDrillReport_NeedDataSource" OnItemDataBound="gvCompanySpecifiedDrillReport_ItemDataBound" >
        <MasterTableView AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="None"
            ShowHeadersWhenNoRecords="true" EnableColumnsViewState="false" >
            <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true" ></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
            <Columns>
                <telerik:GridTemplateColumn HeaderText="Company Specified Drills/Exercise">
                    <HeaderStyle HorizontalAlign="Left" Width="150px" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <telerik:RadLabel ID="RadlblDrillName1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDRILLNAME")%>'>
                        </telerik:RadLabel>
                         <telerik:RadLabel ID="Radlbldrillid1" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDRILLID")%>'>
                        </telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Scenario">
                    <HeaderStyle HorizontalAlign="Center"  Width="120px" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <telerik:RadLabel ID="RadlblScenario1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSCENARIO")%>'>
                        </telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Interval">
                    <HeaderStyle HorizontalAlign="Center" width ="100px" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <telerik:RadLabel ID="RadlblInterval1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDINTERVAL")%>'>
                        </telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Jan">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <a id="Jananchor1" runat="server" style="text-decoration: none; color: black">
                            <%# DataBinder.Eval(Container, "DataItem.FLDJAN")%></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Feb">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <a id="Febanchor1" runat="server" style="text-decoration: none; color: black">
                            <%# DataBinder.Eval(Container, "DataItem.FLDFEB")%></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Mar">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <a id="Maranchor1" runat="server" style="text-decoration: none; color: black">
                            <%# DataBinder.Eval(Container, "DataItem.FLDMAR")%></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Apr">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <a id="Apranchor1" runat="server" style="text-decoration: none; color: black">
                            <%# DataBinder.Eval(Container, "DataItem.FLDAPR")%></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="May">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <a id="Mayanchor1" runat="server" style="text-decoration: none; color: black">
                            <%# DataBinder.Eval(Container, "DataItem.FLDMAY")%></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Jun">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <a id="Junanchor1" runat="server" style="text-decoration: none; color: black">
                            <%# DataBinder.Eval(Container, "DataItem.FLDJUN")%></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="July">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <a id="Julanchor1" runat="server" style="text-decoration: none; color: black">
                            <%# DataBinder.Eval(Container, "DataItem.FLDJUL")%></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Aug">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                       <a id="Auganchor1" runat="server" style="text-decoration: none; color: black">
                            <%# DataBinder.Eval(Container, "DataItem.FLDAUG")%></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Sept">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <a id="Sepanchor1" runat="server" style="text-decoration: none; color: black">
                            <%# DataBinder.Eval(Container, "DataItem.FLDSEP")%></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Oct">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <a id="Octanchor1" runat="server" style="text-decoration: none; color: black">
                            <%# DataBinder.Eval(Container, "DataItem.FLDOCT")%></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Nov">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                       <a id="Novanchor1" runat="server" style="text-decoration: none; color: black">
                            <%# DataBinder.Eval(Container, "DataItem.FLDNOV")%></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Dec">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <a id="Decanchor1" runat="server" style="text-decoration: none; color: black">
                            <%# DataBinder.Eval(Container, "DataItem.FLDDEC")%></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    </div>
    </form>
</body>
</html>
