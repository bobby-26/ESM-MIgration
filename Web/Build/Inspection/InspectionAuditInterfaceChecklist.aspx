<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionAuditInterfaceChecklist.aspx.cs" Inherits="InspectionAuditInterfaceChecklist" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Audit Inspection
    </title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function PaneResized() {
                var sender = $find('RadSplitter1');
                var browserHeight = $telerik.$(window).height();
                sender.set_height(browserHeight - 70);
                $telerik.$(".rdTreeScroll").height($telerik.$("#navigationPane").height() - 30);
            }
        </script>
        <style>
            .yellow {
                color: #dfed6c !important;
                font-family: "Helvetica Neue",Helvetica,Arial,sans-serif !important;
            }

            .white {
                color: #fffff7 !important;
                font-family: "Helvetica Neue",Helvetica,Arial,sans-serif !important;
                text-align-last: right !important;
            }

            .btn {
                justify-content: center;
                align-content: center;
                text-align: center;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmOrderForm" runat="server">
        <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="form1" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="92%" Width="99.9%">
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
            <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" Visible="false" />
            <table style="width: 100%; background-color: #0070c0;">
                <tr>
                    <td style="width: 7%;">
                        <telerik:RadLabel ID="RadLabel1" runat="server" Font-Bold="true" Text="Vessel" CssClass="white"></telerik:RadLabel>
                    </td>
                    <td style="width: 15%;">
                        <telerik:RadLabel ID="txtVessel" runat="server" Text="" CssClass="yellow"></telerik:RadLabel>
                    </td>
                    <td style="width: 7%;">
                        <telerik:RadLabel ID="RadLabel7" runat="server" Font-Bold="true" Text="Inspection" CssClass="white"></telerik:RadLabel>
                    </td>
                    <td style="width: 15%;">
                        <telerik:RadLabel ID="txtInspection" runat="server" Text="" CssClass="yellow"></telerik:RadLabel>
                    </td>
                    <td style="width: 7%;">
                        <telerik:RadLabel ID="RadLabel2" runat="server" Font-Bold="true" Text="Flag" CssClass="white"></telerik:RadLabel>
                    </td>
                    <td style="width: 15%;">
                        <telerik:RadLabel ID="txtFlag" runat="server" Text="" CssClass="yellow"></telerik:RadLabel>
                    </td>
                    <td style="width: 7%;">
                        <telerik:RadLabel ID="RadLabel3" runat="server" Font-Bold="true" Text="Type" CssClass="white"></telerik:RadLabel>
                    </td>
                    <td style="width: 15%;">
                        <telerik:RadLabel ID="txtType" runat="server" Text="" CssClass="yellow"></telerik:RadLabel>
                    </td>
                    <td style="width: 12%;">
                        <telerik:RadButton ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" Width="60%" Height="10px" CssClass="btn"></telerik:RadButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel4" runat="server" Font-Bold="true" Text="Auditor" CssClass="white"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtAuditor" runat="server" Text="" CssClass="yellow"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel5" runat="server" Font-Bold="true" Text="Master" CssClass="white"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtMaster" runat="server" Text="" CssClass="yellow"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel6" runat="server" Font-Bold="true" Text="CE" CssClass="white"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtChiefEngineer" runat="server" Text="" CssClass="yellow"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel8" runat="server" Font-Bold="true" Text="Audit Date" CssClass="white"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtAuditDate" runat="server" Text="" CssClass="yellow"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadButton ID="btnbulk" runat="server" Text="Upload" OnClick="btnBulk_Click" Width="60%" Height="10px" CssClass="btn"></telerik:RadButton>
                    </td>
                </tr>
            </table>
            <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="99.8%" Height="89%">
                <telerik:RadPane ID="navigationPane" runat="server" Width="23%">
                    <table>
                        <tr>
                            <td colspan="2">
                                <eluc:TabStrip ID="MenuInterfaceSearch" runat="server" OnTabStripCommand="MenuInterfaceSearch_TabStripCommand"></eluc:TabStrip>
                                <telerik:RadGrid ID="gvDeficiency" runat="server" EnableViewState="true"
                                    AllowSorting="true" RenderMode="Lightweight" ShowHeader="true" Height="320px"
                                    ShowFooter="false" OnNeedDataSource="gvDeficiency_NeedDataSource" OnItemCommand="gvDeficiency_ItemCommand">
                                    <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>

                                    <MasterTableView EditMode="InPlace" TableLayout="Fixed" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                        AutoGenerateColumns="false">
                                        <NoRecordsTemplate>
                                            <table width="100%" border="0">
                                                <tr>
                                                    <td align="center">
                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </NoRecordsTemplate>
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="40px" HeaderText="Chapter">
                                                <ItemStyle Wrap="true" Width="40px" HorizontalAlign="left" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lblDeficiencyName" runat="server" CommandName="CATEGORY" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHAPTERNAME") %>'></asp:LinkButton>
                                                    <telerik:RadLabel ID="lblChapterId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHAPTERID") %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="15px" HeaderText="Total">
                                                <ItemStyle Wrap="true" Width="15px" HorizontalAlign="Right" />
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTAL") %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="15px" HeaderText="Comp">
                                                <ItemStyle Wrap="true" Width="15px" HorizontalAlign="Right" />
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblComp" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLETED") %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="15px" HeaderText="Pending">
                                                <ItemStyle Wrap="true" Width="15px" HorizontalAlign="Right" />
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblPending" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPENDING") %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                                        ColumnsReorderMethod="Reorder">

                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                    </ClientSettings>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblResponsibility" runat="server" Text="Responsibility" Width="70px" Font-Bold="true"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlResponsible" runat="server" Width="160px" Filter="Contains" EmptyMessage="Select Responsible">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lbllocation" runat="server" Text="Location" Width="70px" Font-Bold="true"></telerik:RadLabel>
                            </td>
                            <td>

                                <telerik:RadComboBox ID="ddlLocation" runat="server" Width="160px" Filter="Contains" EmptyMessage="Select Location">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblItem" runat="server" Text="Item" Width="70px" Font-Bold="true"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtItem" runat="server" Width="160px"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPendingComplete" runat="server" Text="Pend/Comp" Width="70px" Font-Bold="true"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadRadioButtonList runat="server" ID="btnPendingComplete" Direction="Horizontal" Width="150px" OnSelectedIndexChanged="btnPendingComplete_SelectedIndexChanged">
                                    <Items>
                                        <telerik:ButtonListItem Text="Pending" Value="0" />
                                        <telerik:ButtonListItem Text="Completed" Value="1" />
                                    </Items>
                                </telerik:RadRadioButtonList>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblSelect" runat="server" Text="Select" Width="70px" Font-Bold="true"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadRadioButtonList runat="server" ID="btnAnsSearch" Direction="Horizontal" CssClass="content" DataBindings-DataTextField="FLDQUICKNAME"
                                    DataBindings-DataValueField="FLDQUICKCODE" OnSelectedIndexChanged="btnAnsSearch_SelectedIndexChanged">
                                </telerik:RadRadioButtonList>
                            </td>
                        </tr>
                    </table>
                </telerik:RadPane>

                <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
                </telerik:RadSplitBar>
                <telerik:RadPane ID="contentPane" runat="server" Width="77%">
                    <telerik:RadGrid ID="gvInspectionCheckItems" runat="server" AllowPaging="true" AllowCustomPaging="true" AllowSorting="true"
                        OnNeedDataSource="gvInspectionCheckItems_NeedDataSource" RenderMode="Lightweight" Height="97%" Width="99.8%"
                        OnItemDataBound="gvInspectionCheckItems_ItemDataBound" ShowHeader="true" ShowFooter="false" EnableViewState="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client"
                            AutoGenerateColumns="false" InsertItemPageIndexAction="ShowItemOnCurrentPage" DataKeyNames="FLDINSPECTIONCHECKITEMID">
                            <GroupByExpressions>
                                <telerik:GridGroupByExpression>
                                   <SelectFields>
                            <telerik:GridGroupByField FieldAlias="Chapter" FieldName="Chapter" FormatString="{0:D}"
                                HeaderValueSeparator=" : "></telerik:GridGroupByField>
                        </SelectFields>
                        <GroupByFields>
                            <telerik:GridGroupByField FieldName="Chapter" SortOrder="Ascending"></telerik:GridGroupByField>
                        </GroupByFields>
                                </telerik:GridGroupByExpression>
                                <telerik:GridGroupByExpression>
                                   <SelectFields>
                            <telerik:GridGroupByField FieldAlias="Parent" FieldName="Parent" FormatString="{0:D}"
                                HeaderValueSeparator=" : "></telerik:GridGroupByField>
                        </SelectFields>
                        <GroupByFields>
                            <telerik:GridGroupByField FieldName="Parent" SortOrder="Ascending"></telerik:GridGroupByField>
                        </GroupByFields>
                                </telerik:GridGroupByExpression>
                            </GroupByExpressions>
                            <NoRecordsTemplate>
                                <table width="100%" border="0">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Item Code" HeaderStyle-Width="7%">
                                    <ItemStyle Wrap="true" HorizontalAlign="left" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblPscViqid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER")%>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblCheckItemid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"Dataitem.FLDINSPECTIONCHECKITEMID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblMappingIId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAPPINGID") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Item" HeaderStyle-Width="20%">
                                    <ItemStyle Wrap="true" HorizontalAlign="left" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblitem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITEM") %>' />
                                        <telerik:RadLabel ID="lblGuidenceNote" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGUIDENCENOTE") %>' />
                                        <telerik:RadLabel ID="lblMappingid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAPPINGID") %>' />
                                        <telerik:RadLabel ID="lblFormCheckItem" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMSCHECKLISTID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblJobs" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBMAPPING") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDeficiency" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"Dataitem.FLDDEFICIENCYCATEGORY") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblQuestionType" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"Dataitem.FLDQUESTIONTYPE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Description" HeaderStyle-Width="35%">
                                    <ItemStyle Wrap="true" HorizontalAlign="left" />
                                    <ItemTemplate>

                                        <telerik:RadLabel ID="lblChapter" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHAPTERID") %>'></telerik:RadLabel>

                                        <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderStyle-Width="3%">
                                    <ItemStyle Wrap="False" HorizontalAlign="left" />
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Note" ID="cmdNote" CommandName="NOTE" ToolTip="Note">
                                                 <span class="icon"><i class="fas fa-Info"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderStyle-Width="15%">
                                    <ItemStyle Wrap="False" HorizontalAlign="left" />

                                    <ItemTemplate>
                                        <telerik:RadRadioButtonList runat="server" ID="rblAnswer" Direction="Horizontal" Columns="3" CssClass="content" OnSelectedIndexChanged="rblAnswer_SelectedIndexChanged" AutoPostBack="true">
                                            <DataBindings DataTextField="FLDQUICKNAME" DataValueField="FLDQUICKCODE" />
                                        </telerik:RadRadioButtonList>

                                        <telerik:RadLabel ID="lblRemarks" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>

                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <HeaderStyle HorizontalAlign="Center" Width="12%" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblActionHeader" runat="server">
                                            Action
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>

                                        <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" ID="cmdAtt" ToolTip="Attachment" Visible="false">
                                                    <span class="icon"> <i class="fa fa-paperclip"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Job" ID="Cmdjob" ToolTip="Job"
                                            CommandName="JOB" Width="20px" Height="20px">
                                                   <span class="icon"><i class="fas fa-Inspection-Job"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Form" ID="CmdFile" ToolTip="Form"
                                            CommandName="FILE" Width="20px" Height="20px">
                                                   <span class="icon"><i class="fas fa-40"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="History"
                                            CommandName="HISTORY" ID="cmdHistory" ToolTip="History"><span class="icon"><i class="fas fa-history"></i></span></asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Create"
                                            CommandName="REMARKS" ID="cmdRemarks" ToolTip="Remarks">
                                                    <span class="icon"><i class="fas fa-receipt"></i></span></asp:LinkButton>

                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                            ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                    <table cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <table>
                                    <tr style="background-color: red">
                                        <td width="5px" height="10px"></td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <b>
                                    <telerik:RadLabel ID="lblOverdue" runat="server" Text=" - Component Job Overdue"></telerik:RadLabel>
                                </b>
                            </td>
                        </tr>
                    </table>
                </telerik:RadPane>
            </telerik:RadSplitter>
        </telerik:RadAjaxPanel>
    </form>

</body>
</html>
