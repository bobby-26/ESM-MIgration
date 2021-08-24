<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceJobParameterOptions.aspx.cs"
    Inherits="PlannedMaintenanceJobParameterOptions" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Parameter Options</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersFBQOptions" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="50%">
                <tr>
                   
                    <td>
                        <asp:Literal ID="lblParametername" runat="server" Text="Parameter Name"></asp:Literal>
                    </td>
                    <td ></td>
                    <td>
                        <telerik:RadTextBox ID="txtParameterName" runat="server" Width="300px" CssClass="readonlytextbox"
                            ReadOnly="true" >
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuRegistersFBQOptions" runat="server" OnTabStripCommand="MenuRegistersFBQOptions_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvJobParameterOptions" runat="server" Height="83%" OnItemDataBound="gvJobParameterOptions_ItemDataBound"
                OnNeedDataSource="gvJobParameterOptions_NeedDataSource" OnItemCommand="gvJobParameterOptions_ItemCommand"
                EnableViewState="false" ShowFooter="true" AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true">
                <MasterTableView EditMode="InPlace" AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" DataKeyNames="FLDJOBPARAMETEROPTIONSID">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Order No.">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDLEVEL"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtParameterLevelEdit" runat="server" CssClass="input_mandatory" IsPositive="true"
                                    IsInteger="true" Width="60px" MaxLength="3" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVEL") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtParameterLevel" runat="server" CssClass="input_mandatory" IsPositive="true"
                                    IsInteger="true" Width="60px" MaxLength="3" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <HeaderStyle Width="70%" />
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDOPTIONNAME"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtParameterOptionEdit" ToolTip="Name" runat="server" Width="100%" CssClass="gridinput_mandatory"
                                    MaxLength="100" Text='<%#((DataRowView)Container.DataItem)["FLDOPTIONNAME"]%>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtParameterOption" ToolTip="Name" runat="server" Width="100%" MaxLength="100" CssClass="gridinput_mandatory">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expected Ans.?" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <telerik:RadLabel lbl="lblexpectedAns" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.ISEXPECTEDANSWER").ToString() == "1" ? "Yes" : "No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkExpectedAns" runat="server" Checked='<%#((DataRowView)Container.DataItem)["ISEXPECTEDANSWER"].ToString() =="1"? true : false%>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
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
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" EnableNextPrevFrozenColumns="true" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
