<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsUserNameCorrection.aspx.cs"
    Inherits="VesselAccountsUserNameCorrection" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>User Correction</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="frmRegistersRank" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRankName" runat="server" Text="Rank Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox DropDownPosition="Static" ID="ddlRank" runat="server" EnableLoadOnDemand="True"
                            EmptyMessage="Type to select Rank" AutoPostBack="true" Filter="Contains" MarkFirstMatch="true" Width="240px">
                        </telerik:RadComboBox>
                    </td>
                    <td><span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                        <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="400px" ShowEvent="onmouseover"
                            RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true"
                            HideEvent="ManualClose" Position="TopCenter" EnableRoundedCorners="true" ContentScrolling="Auto"
                            Text="">
                            <table cellpadding="1" cellspacing="1" width="100%" class="input" style="border-style: none; color: blue;">
                                <tr>
                                    <td>
                                        <b>
                                            <telerik:RadLabel ID="lblnote" runat="server" EnableViewState="false" Text="Notes :" CssClass="input"
                                                BorderStyle="None" ForeColor="Blue">
                                            </telerik:RadLabel>
                                        </b>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lbl1" runat="server" Text="1."></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblReviewthecrewusernameassignmentsinthelistbelow" runat="server"
                                            Text="Review the crew - user name assignments in the list below.">
                                        </telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lbl2" runat="server" Text="2."></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblReportthediscrepanciestoofficeandchangesrequired" runat="server"
                                            Text="Report the discrepancies to office and changes required.">
                                        </telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lbl3" runat="server" Text="3."></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblOnconfirmationfromofficereviewtheassignments" runat="server"
                                            Text="On confirmation from office, review the assignments.">
                                        </telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lbl4" runat="server" Text="4."></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblIftheassignmentsarecorrectclickontherefreshbuttoninthetopofthescrolllisttoacceptthecorrectedchanges"
                                            runat="server" Text="If the assignments are correct, click on the refresh button in the top of the scroll list to accept the corrected changes.">
                                        </telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </telerik:RadToolTip>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuVesselUserCorrection" runat="server" OnTabStripCommand="VesselUserCorrection_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvVesselUserCorrection" Height="92%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" OnItemCommand="gvVesselUserCorrection_ItemCommand" OnItemDataBound="gvVesselUserCorrection_ItemDataBound" EnableViewState="false"
                ShowFooter="false" ShowHeader="true" OnNeedDataSource="gvVesselUserCorrection_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="User Name">
                            <HeaderStyle Width="6%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUserNameCorrectionId" runat="server" Visible="false"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELUSERNAMEID") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblUserName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKUSERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblUserNameCorrectionIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELUSERNAMEID") %>' Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblUserNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKUSERNAME") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRankName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRankCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblRankNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRankCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="RestHour Employee">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWRHEmployeeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListRHEmployee">
                                    <telerik:RadTextBox ID="txtRHEmployeeName" runat="server" CssClass="input readonlytextbox"
                                        MaxLength="20" Width="180px" ReadOnly="true">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtRHRankName" runat="server" CssClass="input readonlytextbox" MaxLength="20"
                                        Width="150px" ReadOnly="true">
                                    </telerik:RadTextBox>
                                  
                                     <asp:LinkButton runat="server" ID="imgRHEmployee" ToolTip="Cancel" ForeColor="#4C93CC">
                                    <span class="icon"><i class="fas fa-list-alt"></i></span>
                                </asp:LinkButton>
                                   
                                    <telerik:RadTextBox ID="txtRHEmployeeId" runat="server" CssClass="input" Width="10px"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="VAC Employee">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVACEmployeeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVACEMPLOYEENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListVACEmployee">
                                    <telerik:RadTextBox ID="txtVACEmployeeName" runat="server" CssClass="input readonlytextbox"
                                        MaxLength="20" Width="180px" ReadOnly="true">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtVACRankName" runat="server" CssClass="input readonlytextbox"
                                        MaxLength="20" Width="150px" ReadOnly="true">
                                    </telerik:RadTextBox>
                                 <asp:LinkButton runat="server" ID="imgVACEmployee" ToolTip="Cancel" ForeColor="#4C93CC">
                                    <span class="icon"><i class="fas fa-list-alt"></i></span>
                                </asp:LinkButton>
                                   
                                    <telerik:RadTextBox ID="txtVACEmployeeId" runat="server" CssClass="input" Width="10px"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Save" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                          
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
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
