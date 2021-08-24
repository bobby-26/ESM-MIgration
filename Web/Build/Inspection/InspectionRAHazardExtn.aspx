<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAHazardExtn.aspx.cs" Inherits="InspectionRAHazardExtn" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="HazardType" Src="~/UserControls/UserControlRAMiscellaneousExtn.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Severity" Src="~/UserControls/UserControlRASeverityExtn.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Hazard</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            $(document).ready(function () {
                var browserHeight = $telerik.$(window).height();
                $("#gvRAHazard").height(browserHeight - 40);
            });
        </script>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvRAHazard.ClientID %>"));
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
    <form id="frmRAHazard" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager" runat="server"></telerik:RadSkinManager>
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="99.9%"></telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" >
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Title runat="server" ID="ucTitle" Text="RA Hazard" Visible="false"></eluc:Title>
            <table id="tblConfigure" width="100%">
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" MaxLength="100" Width="360px" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlHazardType" runat="server" CssClass="input" Visible="false">
                            <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Health and Safety"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Environmental"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Operational"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Other Consequence"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRAHazard" runat="server" OnTabStripCommand="RAHazard_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvRAHazard" runat="server" AutoGenerateColumns="False" AllowPaging="true" AllowCustomPaging="true"
                Font-Size="11px" Width="100%" CellPadding="3" EnableHeaderContextMenu="true" GroupingEnabled="false" OnNeedDataSource="gvRAHazard_NeedDataSource"
                OnItemCommand="gvRAHazard_ItemCommand" OnItemDataBound="gvRAHazard_ItemDataBound" AllowSorting="true" ShowFooter="true" ShowHeader="true" EnableViewState="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table width="99.9%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="30%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHazardId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblName"  runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblHazardIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtNameEdit" Width="98%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtNameAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200" Width="98%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Impact">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="30%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblImpact" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMPACTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                             <EditItemTemplate>
                               <telerik:RadLabel ID="lblImpactIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMPACTID") %>'></telerik:RadLabel>
                                <telerik:RadComboBox ID="ddlImpactEdit"  Width="98%" runat="server" AppendDataBoundItems="true" CssClass="input"
                                    AllowCustomText="true" EmptyMessage="Type to Select" DataTextField="FLDIMPACTNAME" DataValueField="FLDRISKASSESSMENTIMPACTID">
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlImpactAdd" runat="server" CssClass="input" AppendDataBoundItems="true"  Width="98%"
                                    AllowCustomText="true" EmptyMessage="Type to Select" DataTextField="FLDIMPACTNAME" DataValueField="FLDRISKASSESSMENTIMPACTID">
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Severity">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSeverity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEVERITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                           <EditItemTemplate>
                                <eluc:Severity ID="ucSeverityEdit" runat="server" AppendDataBoundItems="true"  Width="98%"
                                    Type="2" SeverityList='<%# PhoenixInspectionRiskAssessmentSeverityExtn.ListRiskAssessmentSeverity() %>'
                                    SelectedSeverity='<%# DataBinder.Eval(Container,"DataItem.FLDSEVERITYID") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Severity ID="ucSeverityAdd" runat="server" AppendDataBoundItems="true"  Width="98%"
                                    Type="2" SeverityList='<%# PhoenixInspectionRiskAssessmentSeverityExtn.ListRiskAssessmentSeverity() %>' />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Score" SortExpression="FLDSCORE" ShowSortIcon="true" AllowSorting="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Incident Consequence Classification">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIncidentConsequence" runat="server"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCIDENTCONSEQUENCENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Hard ID="ucIncidentConsequenceEdit"  runat="server" HardTypeCode="204"  Width="98%" AppendDataBoundItems="true" SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDINCIDENTCONSEQUENCE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Hard ID="ucIncidentConsequenceAdd" runat="server" HardTypeCode="204"  Width="98%" AppendDataBoundItems="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Consequence" SortExpression="FLDCONSCATEGORY" ShowSortIcon="true" AllowSorting="true" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblConsequence" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONSCATEGORY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Icon">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="imgPhoto" runat="server" Height="49px" 
                                Width="60px" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="IMAGEEDIT" ID="cmdimgedit" ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                        <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" ID="cmdSave" ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="CANCEL" ID="cmdCancel" ToolTip="Cancel">
                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="ADD" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
        <eluc:Status runat="server" ID="ucStatus" />
    </form>
</body>
</html>
