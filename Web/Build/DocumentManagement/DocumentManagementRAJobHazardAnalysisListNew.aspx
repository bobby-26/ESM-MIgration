<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementRAJobHazardAnalysisListNew.aspx.cs" Inherits="DocumentManagementRAJobHazardAnalysisListNew" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselByOwner.ascx" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Job Hazard Analysis</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">


        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvRiskAssessmentJobHazardAnalysis.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
                fade('statusmessage');
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmJobHazardAnalysis" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblConfigure" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>                    
                    <td>Hazard Number
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtHazardNo" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>Job
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtjob" runat="server" Width="270px" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>Category
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlCategory" runat="server" CssClass="input" AppendDataBoundItems="true"
                            Width="250px" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory_Changed" Filter="Contains">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlStatus" runat="server" AppendDataBoundItems="true"
                            Width="250px" OnSelectedIndexChanged="ddlStatus_Changed" CssClass="input" Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuJobHazardAnalysis" runat="server" OnTabStripCommand="MenuJobHazardAnalysis_TabStripCommand"></eluc:TabStrip>
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvRiskAssessmentJobHazardAnalysis" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvRiskAssessmentJobHazardAnalysis" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableHeaderContextMenu="true" GroupingEnabled="false" ShowHeader="true" EnableViewState="false" AllowMultiRowSelection="true" EnableLinqExpressions="false"
                OnItemDataBound="gvRiskAssessmentJobHazardAnalysis_ItemDataBound" OnDeleteCommand="gvRiskAssessmentJobHazardAnalysis_DeleteCommand" Width="100%"
                OnItemCommand="gvRiskAssessmentJobHazardAnalysis_ItemCommand" OnNeedDataSource="gvRiskAssessmentJobHazardAnalysis_NeedDataSource" >
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDJOBHAZARDID" ClientDataKeyNames="FLDJOBHAZARDID" >
                    <ColumnGroups>
                        <telerik:GridColumnGroup Name="Level of Residual Risk" HeaderText="Level of Residual Risk" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Hazard Number" HeaderStyle-Width="7%" >
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="7%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobHazardid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBHAZARDID")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblReferencid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCEID")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblActiveyn" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEYN")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNUMBER")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblInstallcode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTALLCODE")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Process" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Process" HeaderStyle-Width="11%" >
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="11%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSubCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORY")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job" HeaderStyle-Width="21%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="21%"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkJob" runat="server" CommandName="EDITROW" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOB")  %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="9%" Visible="false">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="9%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatusID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSID")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rev No" HeaderStyle-Width="4%" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRevNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONNO")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issued Date" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDISSUEDDATE"])%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="H&S" HeaderStyle-Width="5%" ColumnGroupName="Level of Residual Risk" HeaderStyle-Font-Size="X-Small" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="5%" Font-Size="X-Small"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHS" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHSRR") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ENV" HeaderStyle-Width="5%" ColumnGroupName="Level of Residual Risk" HeaderStyle-Font-Size="X-Small" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="5%" Font-Size="X-Small"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblENV" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENVRR") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ECO" HeaderStyle-Width="5%" ColumnGroupName="Level of Residual Risk" HeaderStyle-Font-Size="X-Small" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="5%" Font-Size="X-Small"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblECO" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDECORR") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="WCS" HeaderStyle-Width="5%" ColumnGroupName="Level of Residual Risk" HeaderStyle-Font-Size="X-Small" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="5%" Font-Size="X-Small"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWCS" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWCRR") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblmaxvalue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXVALUE") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblminvalue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMINVALUE") %>' Visible="false"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Hazards" AllowFiltering="false" HeaderStyle-Width="4%">
                            <HeaderStyle HorizontalAlign="Center" Width="4%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Health and Safety" CommandName="HEALTH" ID="lnkHealth" ToolTip="Health and Safety Hazard" Width="16px">
                                    <span class="icon"><i class="fa-Health"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Enviromental" CommandName="ENV" ID="lnkenv" ToolTip="Enviromental Hazard" Width="16px">
                                    <span class="icon"><i class="fa-Environmental"></i></span>
                                </asp:LinkButton>                                                          
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Controls" AllowFiltering="false" HeaderTooltip="Controls" HeaderStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Equipment" CommandName="COMPONENTS" ID="lnkcomponent" ToolTip="Equipment" Width="16px">
                                    <span class="icon"><i class="fa-PMS"></i></span>
                                </asp:LinkButton>
                                 <asp:LinkButton runat="server" AlternateText="Procedure" CommandName="PROCDURE" ID="lnkprocedure" ToolTip="Procedures" Width="16px">
                                    <span class="icon"><i class="fa-process"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Forms and Checklist" CommandName="FORMS" ID="lnkforms" ToolTip="Forms and Checklist" Width="16px">
                                    <span class="icon"><i class="fa-file-contract-af"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Work Permits" CommandName="WORKPERMIT" ID="lnkWorkPermits" ToolTip="Work Permits" Width="16px">
                                    <span class="icon"><i class="fa-administration"></i></span>
                                </asp:LinkButton> 
                                 <asp:LinkButton runat="server" AlternateText="PPE" CommandName="PPE" ID="lnkPPE" ToolTip="PPE" Width="16px">
                                    <span class="icon"><i class="fa-PPE"></i></span>
                                </asp:LinkButton>                                                               
                                <asp:LinkButton runat="server" AlternateText="EPSS" CommandName="EPSS" ID="lnkEPSS" ToolTip="EPSS" Width="16px">
                                    <span class="icon"><i class="fa-Elog"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Report" HeaderStyle-Width="5%">
                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Job Hazard" CommandName="REPORT" ID="cmdJobHazard" ToolTip="Show PDF">
                                    <span class="icon"><i class="fas fa-file-pdf"></i></span>
                                </asp:LinkButton>                               
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    ColumnsReorderMethod="Reorder" EnablePostBackOnRowClick="true">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

