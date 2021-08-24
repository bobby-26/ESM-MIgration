<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewNewApplicantReference.aspx.cs" Inherits="CrewNewApplicantReference" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="~/UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Licence</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewLicence" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuTitle" runat="server" Title="Reference"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="88%">
             <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeFirstName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeMiddleName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeLastName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAppliedRank" runat="server" Text="Applied Rank"></telerik:RadLabel>
                    </td>
                    <td colspan="5">
                        <telerik:RadTextBox ID="txtRank" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <hr />
            <eluc:TabStrip ID="MenuReference" runat="server" OnTabStripCommand="MenuReference_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewReference" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCrewReference_ItemCommand" OnNeedDataSource="gvCrewReference_NeedDataSource"
                EnableViewState="false" OnItemDataBound="gvCrewReference_ItemDataBound" Height="80%" GroupingEnabled="false" EnableHeaderContextMenu="true" ShowFooter="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblCompanyNameHeader" runat="server">Company Name</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReferenceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEREFERENCEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCompanyName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblReferenceEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEREFERENCEID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtCompanyName" runat="server" CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYNAME") %>' 
                                    MaxLength="200" Width="120px"></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtCompanyNameAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200" Width="120px"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPersoninChargeHeader" runat="server">Person in Charge</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPICHeader" runat="server">
                                    <%# DataBinder.Eval(Container, "DataItem.FLDPIC") %>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtPic" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPIC") %>'
                                    CssClass="gridinput" MaxLength="200" Width="120px">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtPicAdd" runat="server" CssClass="gridinput"
                                    MaxLength="200" ToolTip="" Width="120px">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDesignationHeader" runat="server">Designation</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATION") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtDesignation" runat="server" CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATION")%>' 
                                    MaxLength="200" Width="120px"></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtDesignationAdd" runat="server" CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATION")%>' 
                                    MaxLength="200" Width="120px"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPhoneNumberHeader" runat="server">Phone Number</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDPHONENUMBER") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:PhoneNumber ID="txtPhoneNumber" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHONENUMBER")%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:PhoneNumber ID="txtPhoneNumberAdd" runat="server" CssClass="input" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDateoReferenceHeader" runat="server">Date of Reference</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDDATEOFREFERENCE", "{0:dd/MMM/yyyy}")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtReferenceDateEdit" runat="server" CssClass="input"  Width="120px"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFREFERENCE")%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date ID="txtReferenceDateAdd" runat="server" CssClass="input" Width="120px"/>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Reffered By
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRefferredBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFFERREDBYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Quick ID="ucRefferredByEdit" runat="server" AppendDataBoundItems="true"
                                    Visible="true" CssClass="input" QuickTypeCode="127" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Quick ID="ucRefferredByAdd" runat="server" AppendDataBoundItems="true"
                                    Visible="true" CssClass="input" QuickTypeCode="127" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Comments">
                            <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMENTS") %>' Visible="false"></telerik:RadLabel>
                                <asp:LinkButton runat="server" AlternateText="Remarks"
                                    CommandName="REMARKS" CommandArgument="<%# Container.DataSetIndex %>" ID="imgRemarks"
                                    ToolTip="Remarks">
                                <span class="icon"><i class="fas fa-glasses"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblRemarksEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMENTS") %>' Visible="false"></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtCommentsAdd" runat="server" CssClass="gridinput" MaxLength="500" Width="130px"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment"
                                    CommandName="Attachment" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAtt"
                                    ToolTip="Attachment" Width="20PX" Height="20PX">
                               <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add"
                                    CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                    ToolTip="Add New" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
