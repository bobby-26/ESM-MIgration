<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsCrewListRHNonComplianceList.aspx.cs" Inherits="VesselAccountsCrewListRHNonComplianceList" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlCommonVessel.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Records of Hours of Work & Rest</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">

            function pageLoad() {
                PaneResized();
            }

            function PaneResized(sender, args) {
                var browserHeight = $telerik.$(window).height();
                var grid = $find("gvWorkCalender");
                var grid1 = $find("gvNC");
                //grid._gridDataDiv.style.height = ((browserHeight - 400) / 2) + "px";
                //grid1._gridDataDiv.style.height = ((browserHeight - 400) / 2) + "px";
            }
            function GridCreated(sender, args) {
                var scrollArea = sender.GridDataDiv;
                var dataHeight = sender.get_masterTableView().get_element().clientHeight;
                if (dataHeight < 200) {
                    scrollArea.style.height = dataHeight + "px";
                }
            }
            function GridCreated1(sender, args) {
                var scrollArea = sender.GridDataDiv;
                var dataHeight = sender.get_masterTableView().get_element().clientHeight;
                if (dataHeight < 400) {
                    scrollArea.style.height = dataHeight + "px";
                }
            }
        </script>
        <%--<style>
            .rbToggleCheckbox {
                float: right !important;
                }
            .rbToggleCheckboxChecked {
                float: right !important;
            }
            /*.rbVerticalList button {
                text-aligh: right;
            }*/
        </style>--%>
    </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmcr6breport" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager runat="server" ID="RadSkinManager1"></telerik:RadSkinManager>
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>
        
        <%--<telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1" LoadingPanelID="RadAjaxLoadingPanel1" Height="95%">--%>

            <telerik:RadToolTipManager ID="RadToolTipManager1" runat="server" Position="BottomCenter"
                Animation="Fade" AutoTooltipify="false" Width="300px" RenderInPageRoot="true" AutoCloseDelay="8000">
                <TargetControls>
                </TargetControls>
            </telerik:RadToolTipManager>

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table runat="server" width="100%" id="tblReport">
                <tr>
                    <td width="5%">
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel : "></telerik:RadLabel>
                    </td>
                    <td width="35%">
                        <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input_mandatory" AutoPostBack="true" EntityType="VSL" ActiveVessels="true" Width="60%" />
                    </td>
                    <td width="13%">
                        <telerik:RadLabel ID="lblReportfortheMonthof" runat="server" Text="Report for the Month of :"></telerik:RadLabel>
                    </td>
                    <td width="32%">
                        <telerik:RadComboBox ID="ddlMonth" runat="server" CssClass="input_mandatory" AutoPostBack="true" Filter="Contains" Width="50%" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                            <%--<Items>
                                <telerik:RadComboBoxItem Text="January" Value="1" />
                                <telerik:RadComboBoxItem Text="February" Value="2" />
                                <telerik:RadComboBoxItem Text="March" Value="3" />
                                <telerik:RadComboBoxItem Text="April" Value="4" />
                                <telerik:RadComboBoxItem Text="May" Value="5" />
                                <telerik:RadComboBoxItem Text="June" Value="6" />
                                <telerik:RadComboBoxItem Text="July" Value="7" />
                                <telerik:RadComboBoxItem Text="August" Value="8" />
                                <telerik:RadComboBoxItem Text="September" Value="9" />
                                <telerik:RadComboBoxItem Text="October" Value="10" />
                                <telerik:RadComboBoxItem Text="November" Value="11" />
                                <telerik:RadComboBoxItem Text="December" Value="12" />
                            </Items>--%>
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuWorkHour" runat="server" OnTabStripCommand="MenuWorkHour_TabStripCommand"></eluc:TabStrip>
            <h2 style="text-align:center"><u>Records of Hours of Work & Rest</u></h2>
            <table style="width:100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShipName" runat="server" Text="Ship's Name" Font-Bold="true"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblShipNameValue" runat="server" Text=""></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIMO" runat="server" Text="IMO NO" Font-Bold="true"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIMOValue" runat="server" Text=""></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFlag" runat="server" Text="Flag" Font-Bold="true"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFlagValue" runat="server" Text=""></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMonth" runat="server" Text="Month & Year" Font-Bold="true"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMonthValue" runat="server" Text=""></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name" Font-Bold="true"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblNameValue" runat="server" Text=""></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank" Font-Bold="true"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRankValue" runat="server" Text=""></telerik:RadLabel>
                    </td>
                    <td></td>
                    <td></td>
                    <td>
                        <telerik:RadLabel ID="lblWatch" runat="server" Text="Watch Keeper" Font-Bold="true" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblWatchValue" runat="server" Text="" Visible="false"></telerik:RadLabel>
                    </td>
                </tr>
            </table>

            <telerik:RadGrid ID="gvWorkCalender" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None" Width="100%" ShowFooter="true"
                AllowCustomPaging="false" AllowSorting="false" AllowPaging="false" CellSpacing="0" GroupingEnabled="false"
                EnableViewState="true" OnItemDataBound="gvWorkCalender_ItemDataBound" OnNeedDataSource="gvWorkCalender_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Rest Hour Requirements" Name="REQUIREMENTS">
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Date" >
                            <HeaderStyle HorizontalAlign="Center" Width="45px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="45px" />
                            <ItemTemplate>
                               <%# ((DataRowView)Container.DataItem)["FLDDATE"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Day">
                            <HeaderStyle HorizontalAlign="Center" Width="45px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="45px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDREPORTINGDAY"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="00" UniqueName="FLD00">
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="30px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLD00"].ToString()=="0.00" ?"" :  ((DataRowView)Container.DataItem)["FLD00"] %>
                                <asp:ImageButton runat="server" ID="ImgFlag00" ImageUrl="<%$ PhoenixTheme:images/Red.png %>" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="01" UniqueName="FLD01">
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="30px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLD01"].ToString()=="0.00" ?"" :  ((DataRowView)Container.DataItem)["FLD01"] %>
                                <asp:ImageButton runat="server" ID="ImgFlag01" ImageUrl="<%$ PhoenixTheme:images/Red.png %>" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="02" UniqueName="FLD02">
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="30px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLD02"].ToString()=="0.00" ?"" :  ((DataRowView)Container.DataItem)["FLD02"] %>
                                <asp:ImageButton runat="server" ID="ImgFlag02" ImageUrl="<%$ PhoenixTheme:images/Red.png %>" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="03" UniqueName="FLD03">
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="30px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLD03"].ToString()=="0.00" ?"" :  ((DataRowView)Container.DataItem)["FLD03"] %>
                                <asp:ImageButton runat="server" ID="ImgFlag03" ImageUrl="<%$ PhoenixTheme:images/Red.png %>" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="04" UniqueName="FLD04">
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="30px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLD04"].ToString()=="0.00" ?"" :  ((DataRowView)Container.DataItem)["FLD04"] %>
                                <asp:ImageButton runat="server" ID="ImgFlag04" ImageUrl="<%$ PhoenixTheme:images/Red.png %>" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="05" UniqueName="FLD05">
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="30px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLD05"].ToString()=="0.00" ?"" :  ((DataRowView)Container.DataItem)["FLD05"] %>
                                <asp:ImageButton runat="server" ID="ImgFlag05" ImageUrl="<%$ PhoenixTheme:images/Red.png %>" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="06" UniqueName="FLD06">
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="30px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLD06"].ToString()=="0.00" ?"" :  ((DataRowView)Container.DataItem)["FLD06"] %>
                                <asp:ImageButton runat="server" ID="ImgFlag06" ImageUrl="<%$ PhoenixTheme:images/Red.png %>" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="07" UniqueName="FLD07">
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="30px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLD07"].ToString()=="0.00" ?"" :  ((DataRowView)Container.DataItem)["FLD07"] %>
                                <asp:ImageButton runat="server" ID="ImgFlag07" ImageUrl="<%$ PhoenixTheme:images/Red.png %>" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="08" UniqueName="FLD08">
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="30px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLD08"].ToString()=="0.00" ?"" :  ((DataRowView)Container.DataItem)["FLD08"] %>
                                <asp:ImageButton runat="server" ID="ImgFlag08" ImageUrl="<%$ PhoenixTheme:images/Red.png %>" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="09" UniqueName="FLD09">
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="30px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLD09"].ToString()=="0.00" ?"" :  ((DataRowView)Container.DataItem)["FLD09"] %>
                                <asp:ImageButton runat="server" ID="ImgFlag09" ImageUrl="<%$ PhoenixTheme:images/Red.png %>" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="10" UniqueName="FLD10">
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="30px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLD10"].ToString()=="0.00" ?"" :  ((DataRowView)Container.DataItem)["FLD10"] %>
                                <asp:ImageButton runat="server" ID="ImgFlag10" ImageUrl="<%$ PhoenixTheme:images/Red.png %>" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="11" UniqueName="FLD11">
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="30px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLD11"].ToString()=="0.00" ?"" :  ((DataRowView)Container.DataItem)["FLD11"] %>
                                <asp:ImageButton runat="server" ID="ImgFlag11" ImageUrl="<%$ PhoenixTheme:images/Red.png %>" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="12" UniqueName="FLD12">
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="30px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLD12"].ToString()=="0.00" ?"" :  ((DataRowView)Container.DataItem)["FLD12"] %>
                                <asp:ImageButton runat="server" ID="ImgFlag12" ImageUrl="<%$ PhoenixTheme:images/Red.png %>" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="13" UniqueName="FLD13">
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="30px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLD13"].ToString()=="0.00" ?"" :  ((DataRowView)Container.DataItem)["FLD13"] %>
                                <asp:ImageButton runat="server" ID="ImgFlag13" ImageUrl="<%$ PhoenixTheme:images/Red.png %>" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="14" UniqueName="FLD14">
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="30px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLD14"].ToString()=="0.00" ?"" :  ((DataRowView)Container.DataItem)["FLD14"] %>
                                <asp:ImageButton runat="server" ID="ImgFlag14" ImageUrl="<%$ PhoenixTheme:images/Red.png %>" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="15" UniqueName="FLD15">
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="30px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLD15"].ToString()=="0.00" ?"" :  ((DataRowView)Container.DataItem)["FLD15"] %>
                                <asp:ImageButton runat="server" ID="ImgFlag15" ImageUrl="<%$ PhoenixTheme:images/Red.png %>" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="16" UniqueName="FLD16">
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="30px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLD16"].ToString()=="0.00" ?"" :  ((DataRowView)Container.DataItem)["FLD16"] %>
                                <asp:ImageButton runat="server" ID="ImgFlag16" ImageUrl="<%$ PhoenixTheme:images/Red.png %>" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="17" UniqueName="FLD17">
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="30px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLD17"].ToString()=="0.00" ?"" :  ((DataRowView)Container.DataItem)["FLD17"] %>
                                <asp:ImageButton runat="server" ID="ImgFlag17" ImageUrl="<%$ PhoenixTheme:images/Red.png %>" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="18" UniqueName="FLD18">
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="30px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLD18"].ToString()=="0.00" ?"" :  ((DataRowView)Container.DataItem)["FLD18"] %>
                                <asp:ImageButton runat="server" ID="ImgFlag18" ImageUrl="<%$ PhoenixTheme:images/Red.png %>" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="19" UniqueName="FLD19">
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="30px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLD19"].ToString()=="0.00" ?"" :  ((DataRowView)Container.DataItem)["FLD19"] %>
                                <asp:ImageButton runat="server" ID="ImgFlag19" ImageUrl="<%$ PhoenixTheme:images/Red.png %>" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="20" UniqueName="FLD20">
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="30px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLD20"].ToString()=="0.00" ?"" :  ((DataRowView)Container.DataItem)["FLD20"] %>
                                <asp:ImageButton runat="server" ID="ImgFlag20" ImageUrl="<%$ PhoenixTheme:images/Red.png %>" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="21" UniqueName="FLD21">
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="30px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLD21"].ToString()=="0.00" ?"" :  ((DataRowView)Container.DataItem)["FLD21"] %>
                                <asp:ImageButton runat="server" ID="ImgFlag21" ImageUrl="<%$ PhoenixTheme:images/Red.png %>" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="22" UniqueName="FLD22">
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="30px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLD22"].ToString()=="0.00" ?"" :  ((DataRowView)Container.DataItem)["FLD22"] %>
                                <asp:ImageButton runat="server" ID="ImgFlag22" ImageUrl="<%$ PhoenixTheme:images/Red.png %>" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="23" UniqueName="FLD23">
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="30px" />
                            <FooterStyle Wrap="true" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLD23"].ToString()=="0.00" ?"" :  ((DataRowView)Container.DataItem)["FLD23"] %>
                                <asp:ImageButton runat="server" ID="ImgFlag23" ImageUrl="<%$ PhoenixTheme:images/Red.png %>" Visible="false" />
                            </ItemTemplate>
                            <FooterTemplate>
                                Total hrs
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Daily hrs of Work">
                            <HeaderStyle HorizontalAlign="Center" Wrap="true" Width="60px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="60px" />
                            <FooterStyle Wrap="true" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDWORKHOURS"]%>
                            </ItemTemplate>
                            <FooterTemplate>
                                <%= String.Format("{0:0.00}",strTotalWork.ToString()) %>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Daily hrs of Rest">
                            <HeaderStyle HorizontalAlign="Center" Wrap="true" Width="60px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="60px" />
                            <FooterStyle Wrap="true" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDRESTHOURS"]%>
                            </ItemTemplate>
                            <FooterTemplate>
                                <%= String.Format("{0:0.00}",strTotalRest.ToString()) %>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nature of Work" UniqueName="NATUREOFWORK">
                            <HeaderStyle HorizontalAlign="Center" Wrap="true" Width="100px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="100px"/>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDNATUREOFWORK"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="OPA-90 Apply Y/N">
                            <HeaderStyle HorizontalAlign="Center" Wrap="true" Width="60px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="60px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDOPAAPPLYYN"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sea / Port">
                            <HeaderStyle HorizontalAlign="Center" Wrap="true" Width="60px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="60px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDSEAPORT"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Clocks Adv/Retd">
                            <HeaderStyle HorizontalAlign="Center" Wrap="true" Width="60px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="60px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDADVANCERETARD"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="IDL E-W/W-E">
                            <HeaderStyle HorizontalAlign="Center" Wrap="true" Width="60px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="60px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDIDL"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Hours of Rest in any 24h" ColumnGroupName="REQUIREMENTS">
                            <HeaderStyle HorizontalAlign="Center" Wrap="true" Width="80px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="80px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLD24HREST"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Hours of Rest in any 7d period" ColumnGroupName="REQUIREMENTS">
                            <HeaderStyle HorizontalAlign="Center" Wrap="true" Width="80px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="80px" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLD7DREST"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <ClientEvents OnGridCreated="GridCreated1" />
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <telerik:RadLabel runat="server" id="lblNote1" Text="IDL: International Date Line crossed in the direction indicated." Font-Italic="true"></telerik:RadLabel>
            <br />
            <telerik:RadLabel runat="server" id="lblNote2" Text="Clocks Adv/Retd: Ship's clocks Advanced or Retarded this date as indicated." Font-Italic="true"></telerik:RadLabel>
            <br />
            <br />
            <telerik:RadLabel runat="server" ID="lblNote3" Text="During the reporting period, crew member was in compliance with the rest hour requirements as regulated in STCW 2010, MLC 2006 & OPA 90 except as listed below:"></telerik:RadLabel>
            <br />
            <h2 style="text-align:center"><u>Rest Hours Non Compliance Report</u></h2>
        <telerik:RadGrid ID="gvNC" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None" Width="100%" 
                AllowCustomPaging="false" AllowSorting="false" AllowPaging="false" CellSpacing="0" GroupingEnabled="false"
                EnableViewState="true" OnItemDataBound="gvNC_ItemDataBound" OnNeedDataSource="gvNC_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <ColumnGroups>
                        
                        <telerik:GridColumnGroup HeaderText="STCW/ILO/MLC Requirements" Name="STCREQUIREMENTS">
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="OPA 90 Requirements" Name="OPAREQUIREMENTS">
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="No" >
                            <HeaderStyle HorizontalAlign="Center" Width="25px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="25px" />
                            <ItemTemplate>
                                <%# Container.ItemIndex+1 %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date">
                            <HeaderStyle HorizontalAlign="Center" Width="60px" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="60px" />
                            <ItemTemplate>
                               <%#((DataRowView)Container.DataItem)["FLDDATE"] %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="< 10 hrs rest in any 24 hrs period" UniqueName="FLDS1" ColumnGroupName="STCREQUIREMENTS">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%# int.Parse(((DataRowView)Container.DataItem)["FLDS1"].ToString())> 0 ?"1" :  "0" %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="10hrs rest split in more than 2 periods" UniqueName="FLDS2" ColumnGroupName="STCREQUIREMENTS">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%# int.Parse(((DataRowView)Container.DataItem)["FLDS2"].ToString())> 0 ?"1" :  "0" %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Longests period of rest is < 6 hrs continuous rest" UniqueName="FLDS3" ColumnGroupName="STCREQUIREMENTS">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%# int.Parse(((DataRowView)Container.DataItem)["FLDS3"].ToString())> 0 ?"1" :  "0" %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Interval between consecutive periods of rest is exceeding 14 hrs" UniqueName="FLDS4" ColumnGroupName="STCREQUIREMENTS">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%# int.Parse(((DataRowView)Container.DataItem)["FLDS4"].ToString())> 0 ?"1" :  "0" %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total rest in any 7 days period is < 77 hrs" UniqueName="FLDS5" ColumnGroupName="STCREQUIREMENTS">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%# int.Parse(((DataRowView)Container.DataItem)["FLDS5"].ToString())> 0 ?"1" :  "0" %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total hrs of work exceeding 15 hrs of work in last 24 hrs" UniqueName="FLDO1" ColumnGroupName="OPAREQUIREMENTS">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%# int.Parse(((DataRowView)Container.DataItem)["FLDO1"].ToString())> 0 ?"1" :  "0" %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total hrs of work exceeding 36 hrs of work in last 72 hrs" UniqueName="FLDO2" ColumnGroupName="OPAREQUIREMENTS">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%# int.Parse(((DataRowView)Container.DataItem)["FLDO2"].ToString())> 0 ?"1" :  "0" %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reason for Non Compliance" UniqueName="FLDREASONFORNC">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" />
                            <ItemTemplate>
                               <%# ((DataRowView)Container.DataItem)["FLDREASONFORNC"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="System Cause" UniqueName="FLDSYSTEMCAUSE">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDSYSTEMCAUSE"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Corrective Action" UniqueName="FLDCORRECTIVEACTION">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDCORRECTIVEACTION"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <ClientEvents OnGridCreated="GridCreated" />
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

        <table style="width:100%;">
            <tr>
                <td style="width:50%;">
                    <telerik:RadCheckBoxList ID="rdLevel" runat="server" RenderMode="Lightweight" Direction="Vertical" Width="100%" >
                            <DataBindings DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE" />
                        </telerik:RadCheckBoxList>
                </td>
                <td style="width:50%;">
                    <telerik:RadTextBox ID="txtRemarks" runat="server" RenderMode="Lightweight" TextMode="MultiLine" Rows="3" EmptyMessage="Remarks" Width="100%">
                    </telerik:RadTextBox>
                </td>
            </tr>
        </table>
        <br />
        <table style="width:100%">
            <tr>
                <td>
                    <telerik:RadLabel runat="server" ID="lblSigned" Text="Signed:"></telerik:RadLabel>
                </td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel runat="server" ID="lblSeafarer" Text="Seafarer:"></telerik:RadLabel>
                    <telerik:RadLabel runat="server" ID="lblSeafarerName" Text="" Font-Underline="true"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel runat="server" ID="lblChiefOffice" Text="Chief Officer / Chief Engineer:"></telerik:RadLabel>
                    <telerik:RadLabel runat="server" ID="lblCO" Text="" Font-Underline="true"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel runat="server" ID="lblMaster" Text="Master:"></telerik:RadLabel>
                    <telerik:RadLabel runat="server" ID="lblMasterName" Text="" Font-Underline="true"></telerik:RadLabel>
                </td>
            </tr>
        </table>
            <%--<iframe runat="server" id="ifMoreInfo" scrolling="yes" frameborder="0" style="min-height:96%; width: 100%"></iframe> --%>
        <%--</telerik:RadAjaxPanel>--%>
    </form>
</body>
</html>
