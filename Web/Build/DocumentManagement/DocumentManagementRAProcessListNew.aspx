<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementRAProcessListNew.aspx.cs" Inherits="DocumentManagementRAProcessListNew" %>


<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlRACategoryExtn.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Process RA</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvRiskAssessmentProcess.ClientID %>"));
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
    <form id="form1" runat="server">
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <asp:RadioButtonList ID="rblRAType" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" RepeatLayout="Table" OnSelectedIndexChanged="rblRAType_SelectedIndexChanged">
                            <asp:ListItem Text="Process RA" Value="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Standard Template" Value="1"></asp:ListItem>
                        </asp:RadioButtonList>

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Activity"></telerik:RadLabel>
                    </td>
                    <td>
                         <telerik:RadComboBox ID="ddlCategory" runat="server" CssClass="input" AppendDataBoundItems="true"
                            AutoPostBack="True" DataTextField="FLDNAME" DataValueField="FLDACTIVITYID" Width="270px" EmptyMessage="Type to select Activity" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRefNumber" runat="server" Text='Ref Number'>
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRefNo" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbltypes" runat="server" Text='Type'>
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"
                            AppendDataBoundItems="true" Visible="false">
                        </telerik:RadComboBox>

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRevNos" runat="server" Text='Vessel'>
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                            CssClass="input" OnTextChangedEvent="ucVessel_SelectedIndexChanged" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblactivityconditions" runat="server" Text='Activity/Condition'>
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtActivity" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuProcess" runat="server" OnTabStripCommand="MenuProcess_TabStripCommand"></eluc:TabStrip>
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvRiskAssessmentProcess" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvRiskAssessmentProcess" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableHeaderContextMenu="true" GroupingEnabled="false" ShowHeader="true" EnableViewState="false"
                OnItemDataBound="gvRiskAssessmentProcess_ItemDataBound" AllowMultiRowSelection="true"
                OnItemCommand="gvRiskAssessmentProcess_ItemCommand" OnNeedDataSource="gvRiskAssessmentProcess_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDRISKASSESSMENTPROCESSID" ClientDataKeyNames="FLDRISKASSESSMENTPROCESSID">
                    <ColumnGroups>
                        <telerik:GridColumnGroup Name="Level of Residual Risk" HeaderText="Level of Residual Risk" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
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
                        <telerik:GridTemplateColumn HeaderText="Ref. No" HeaderStyle-Width="7%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblInstallcode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTALLCODE")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRiskAssessmentProcessID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTPROCESSID")  %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Prepared" HeaderStyle-Width="7%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                            <ItemTemplate>
                                <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDDATE"]) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Intended <br> Work" HeaderStyle-Width="7%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                            <ItemTemplate>
                                <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDINTENDEDWORKDATE"]) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Process" HeaderStyle-Width="15%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="15%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbltypeid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEID")  %>' Visible="false"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Activity / Conditions" HeaderStyle-Width="17%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="17%"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkJobActivity" runat="server" CommandName="EDITROW" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCESSNAME")  %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Target Date <br> for completion" HeaderStyle-Wrap="true" HeaderStyle-Width="11%" >
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTarget" runat="server" Text='<%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDCOMPLETIONDATE"])%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Task <br> completed YN" HeaderStyle-Width="10%" HeaderStyle-Wrap="true" >
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMitigating" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKSTATUS")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="10%" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rev No" HeaderStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" >
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="3%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRevNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONNO")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Approved" HeaderStyle-Width="7%" Visible="false" >
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                            <ItemTemplate>
                                <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDISSUEDDATE"])%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="H&S" HeaderStyle-Width="3%" ColumnGroupName="Level of Residual Risk" HeaderStyle-Font-Size="XX-Small" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="3%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHealth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHSSCORE")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ENV" HeaderStyle-Width="3%" ColumnGroupName="Level of Residual Risk" HeaderStyle-Font-Size="XX-Small" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="3%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEnvironmental" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENVSCORE")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ECO" HeaderStyle-Width="3%" ColumnGroupName="Level of Residual Risk" HeaderStyle-Font-Size="XX-Small" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="3%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEcononmic" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDECOSCORE")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="WCS" HeaderStyle-Width="3%" ColumnGroupName="Level of Residual Risk" HeaderStyle-Font-Size="XX-Small" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="3%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWorstCase" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWSSCORE")  %>'></telerik:RadLabel>
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
                                <asp:LinkButton runat="server" AlternateText="Process PDF" CommandName="REPORT" ID="cmdRAProcess" ToolTip="Show PDF">
                                    <span class="icon"><i class="fas fa-file-pdf"></i></span>
                                </asp:LinkButton>                                
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

