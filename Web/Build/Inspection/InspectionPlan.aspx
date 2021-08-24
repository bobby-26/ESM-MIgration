<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPlan.aspx.cs" Inherits="InspectionPlan" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register Src="../UserControls/UserControlQuick.ascx" TagName="Quick" TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Audit Plan</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style>
            .heading {
                text-decoration: underline;
            }
            .textsize {
                font-size:11.5px;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" class="textsize">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" />
            <eluc:TabStrip ID="MenuInspectionPlan" runat="server" OnTabStripCommand="InspectionPlan_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table cellspacing="2" cellpadding="2" width="100%">
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblAuditplan" runat="server" Text="1. AUDIT TEAM" CssClass="heading"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAuditteam" runat="server" Text="The audit team consisted of following:"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvAuditteam" runat="server" AllowSorting="true" ShowHeader="true"
                            CellSpacing="0" GridLines="None" OnItemCommand="gvAuditteam_ItemCommand" OnNeedDataSource="gvAuditteam_NeedDataSource"
                            EnableViewState="true" GroupingEnabled="false" ShowFooter="true" Width="99%">
                            <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
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
                                    <telerik:GridTemplateColumn HeaderText="S.No" HeaderStyle-Width="5%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="5%"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblSno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="50%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="50%"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblmeetingid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONPLANMEETINGID") %>' Visible="false"></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblAuditName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEETINGPERSONNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadLabel ID="lblmeetingidedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONPLANMEETINGID") %>' Visible="false"></telerik:RadLabel>
                                            <telerik:RadTextBox ID="txtNameEdit" runat="server" Width="98%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEETINGPERSONNAME") %>'></telerik:RadTextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadTextBox ID="txtNameAdd" runat="server" Text="" Width="98%"></telerik:RadTextBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Rank" HeaderStyle-Width="35%">
                                        <ItemStyle Width="35%" HorizontalAlign="Left" ></ItemStyle>
                                        <ItemTemplate >
                                            <telerik:RadLabel ID="lbltype" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>' Visible="false"></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEETINGPERSONRANK") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadTextBox ID="txtRankEdit" runat="server" Width="98%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEETINGPERSONRANK") %>'></telerik:RadTextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadTextBox ID="txtRankAdd" runat="server" Text="" Width="98%"></telerik:RadTextBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="10%">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                                CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit">
                                <span class="icon"><i class="fas fa-edit"></i></span></asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Delete" ToolTip="Delete" Width="20PX" Height="20PX"
                                                CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Save" Width="20PX" Height="20PX"
                                                CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Cancel" ToolTip="Cancel" Width="20PX" Height="20PX"
                                                CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                            </asp:LinkButton>
                                        </EditItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <FooterTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Add" ToolTip="Add New" Width="20PX" Height="20PX"
                                                CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"> <span class="icon"><i class="fa fa-plus-circle"></i></span></asp:LinkButton>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblOpntitle" runat="server" Text="2. OPENING MEETING" CssClass="heading"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblopncontent" runat="server" Text="Prior to commencement of the audit a pre-audit meeting was held between the Master,Senior Officers and the auditor."></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblopn" runat="server" Text="The purpose and terms of audit was defined and the arrangement of the audit were reviewed and agreed, including the audit schedule."></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblopnflg" runat="server" Text="The following persons attended the opening meeting:"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvOpeningMeeting" runat="server" AllowSorting="true" ShowHeader="true"
                            CellSpacing="0" GridLines="None" OnItemCommand="gvOpeningMeeting_ItemCommand" OnNeedDataSource="gvOpeningMeeting_NeedDataSource"
                            EnableViewState="true" GroupingEnabled="false" ShowFooter="true" Width="99%">
                            <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
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
                                    <telerik:GridTemplateColumn HeaderText="S.No" HeaderStyle-Width="5%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="5%"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblSno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="50%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="50%"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblOpeningmeetingid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONPLANMEETINGID") %>' Visible="false"></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblOpenName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEETINGPERSONNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadLabel ID="lblOpeningmeetingidedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONPLANMEETINGID") %>' Visible="false"></telerik:RadLabel>
                                            <telerik:RadTextBox ID="txtOpenNameEdit" runat="server" Width="98%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEETINGPERSONNAME") %>'></telerik:RadTextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadTextBox ID="txtOpenNameAdd" runat="server" Text="" Width="98%"></telerik:RadTextBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Rank" HeaderStyle-Width="35%">
                                        <ItemStyle HorizontalAlign="Left" Width="35%" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblOpentype" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>' Visible="false"></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblOpenRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEETINGPERSONRANK") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadTextBox ID="txtOpenRankEdit" Width="98%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEETINGPERSONRANK") %>'></telerik:RadTextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadTextBox ID="txtOpenRankAdd" runat="server" Width="98%" Text=""></telerik:RadTextBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="10%">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                                CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdOpnEdit">
                                                <span class="icon"><i class="fas fa-edit"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Delete" ToolTip="Delete" Width="20PX" Height="20PX"
                                                CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                            
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Save" Width="20PX" Height="20PX"
                                                CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Cancel" ToolTip="Cancel" Width="20PX" Height="20PX"
                                                CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                            </asp:LinkButton>
                                        </EditItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <FooterTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Add" ToolTip="Add New" Width="20PX" Height="20PX"
                                                CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd">
                                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                            </asp:LinkButton>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblAssessment" runat="server" Text="3. ASSESSMENT STANDARDES" CssClass="heading"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%--<telerik:RadLabel ID="lblassessmentstds" runat="server" Text="" ></telerik:RadLabel>--%>
                        <telerik:RadTextBox ID="lblassessmentstds" runat="server" Text="" ReadOnly="true" Width ="98%" Height="110px" BorderColor="White" TextMode="MultiLine"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblResults" runat="server" Text="4. RESULTS OF PREVIOUS AUDIT" CssClass="heading"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td width="70%">
                                    <telerik:RadLabel ID="lbllstncobscount" runat="server" Text="No. of NC/OBS Notes issued during last audit:"></telerik:RadLabel>
                                </td>
                                <td width="30%">
                                    <eluc:Number ID="uclastncobscount" runat="server"
                                        MaxLength="3" IsPositive="true" Width="60px"></eluc:Number>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblClosedncobs" runat="server" Text="No. of NC's/OBS's closed out as correctiveaction sacifactory:"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Number ID="ucClosedncobs" runat="server"
                                        MaxLength="3" IsPositive="true" Width="60px"></eluc:Number>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblnotclosedncobd" runat="server" Text="No. of NC's/OBS's not closed out, new NCN issued in lieu:"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Number ID="ucnotclosedncobs" runat="server"
                                        MaxLength="3" IsPositive="true" Width="60px"></eluc:Number>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblContact" runat="server" Text="5. CONTACTS" CssClass="heading"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblContactdetail" runat="server" Text="(Mention ranks of ship's staffs, interviewed during the audit process. This includes the bridge team members whose performances have been checked as per Checklist Code number 4.28)"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadTextBox ID="txtContact" runat="server" Text="" TextMode="MultiLine" Resize="Both" Width="98%" Height="70px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblClose" runat="server" Text="6. CLOSING MEETING" CssClass="heading"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblClosedetails" runat="server" Text="After completion of the audit, following staffs attended the closing meeting with the auditor"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvCloseMeeting" runat="server" AllowSorting="true" ShowHeader="true"
                            CellSpacing="0" GridLines="None" OnItemCommand="gvCloseMeeting_ItemCommand" OnNeedDataSource="gvCloseMeeting_NeedDataSource"
                            EnableViewState="true" GroupingEnabled="false" ShowFooter="true" Width="99%">
                            <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
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
                                    <telerik:GridTemplateColumn HeaderText="S.No" HeaderStyle-Width="5%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="5%"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblSno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="50%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="50%"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblClosegmeetingid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONPLANMEETINGID") %>' Visible="false"></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblCloseName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEETINGPERSONNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadLabel ID="lblClosegmeetingidedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONPLANMEETINGID") %>' Visible="false"></telerik:RadLabel>
                                            <telerik:RadTextBox ID="txtCloseNameEdit" runat="server" Width="98%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEETINGPERSONNAME") %>'></telerik:RadTextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadTextBox ID="txtCloseNameAdd" runat="server" Width="98%" Text=""></telerik:RadTextBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Rank" HeaderStyle-Width="35%">
                                        <ItemStyle HorizontalAlign="Left" Width="35%" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblClosetype" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>' Visible="false"></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblCloseRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEETINGPERSONRANK") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadTextBox ID="txtCloseRankEdit" runat="server" Width="98%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEETINGPERSONRANK") %>'></telerik:RadTextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadTextBox ID="txtCloseRankAdd" runat="server" Width="98%" Text=""></telerik:RadTextBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="10%">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="30PX" Height="20PX"
                                                CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCloseEdit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Delete" ToolTip="Delete" Width="20PX" Height="20PX"
                                                CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Save" Width="20PX" Height="20PX"
                                                CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Cancel" ToolTip="Cancel" Width="20PX" Height="20PX"
                                                CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                            </asp:LinkButton>
                                        </EditItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <FooterTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Add" ToolTip="Add New" Width="20PX" Height="20PX"
                                                CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd">
                                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                            </asp:LinkButton>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblpreview" runat="server" Text="7. AUDITOR'S PERFORMENCE PREVIEW" CssClass="heading"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblpreviewtext" runat="server" Text="(This section is Applicable only when auditor's performance has been reviewed by supervising auditor from office)"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblSupervisorcomments" runat="server" Text="Supervising auditor's Comments:"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtSupervisorcomments" runat="server" Text="" Width="98%" Height="70px" TextMode="MultiLine" Resize="Both"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblAuditordetails" runat="server" Text="Name / Designation of the supervising auditor:"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtsupervisorName" runat="server" Text="" Width="180px"></telerik:RadTextBox> / 
                                    <telerik:RadTextBox ID="txtSupervisorRank" runat="server" Text="" Width="145px"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblauditorcomments" runat="server" Text="Comments by Auditor:"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtauditorcomments" runat="server" Text="" Width="98%" Height="70px" TextMode="MultiLine" Resize="Both"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblSign" runat="server" Text="Signature of Auditor:"></telerik:RadLabel>
                                </td>
                                <td>

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
