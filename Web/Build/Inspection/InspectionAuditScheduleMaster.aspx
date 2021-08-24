<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionAuditScheduleMaster.aspx.cs" Inherits="InspectionAuditScheduleMaster" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Inspection" Src="~/UserControls/UserControlInspection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="IChapter" Src="~/UserControls/UserControlInspectionChapter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ITopic" Src="~/UserControls/UserControlInspectionTopic.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Checklist" Src="~/UserControls/UserControlInspectionChecklist.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddrType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title></title>

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <%--<script type="text/javascript">
            function resizeDiv() {
                var obj = document.getElementById("divScroll");
                var iframe = document.getElementById("ifMoreInfo");
                var rect = iframe.getBoundingClientRect();
                var x = rect.left;
                var y = rect.top;
                var w = rect.right - rect.left;
                var h = rect.bottom - rect.top;
                obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - (h + 70) + "px";
            }
        </script>--%>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionScheduleMaster" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlInspectionScheduleSearchEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="frmTitle" Text="Audit / Inspection Schedule"></eluc:Title>
                    <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                    <eluc:TabStrip ID="MenuInspectionSchedulemaster" runat="server" TabStrip="true" OnTabStripCommand="MenuInspectionSchedulemaster_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="min-height: 350px;
                    width: 100%"></iframe>
                <asp:HiddenField ID="hdnScroll" runat="server" />
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuInspectionScheduleSearch" runat="server" OnTabStripCommand="InspectionScheduleSearch_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divScroll" style="position: relative; z-index: 1; width: 100%; >
                    <div id="divGrid" style="position: relative; z-index: 1; width: 100%;">
                        <asp:GridView ID="gvAuditSchedule" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowCommand="gvAuditSchedule_RowCommand" OnRowDataBound="gvAuditSchedule_ItemDataBound"
                            OnRowDeleting="gvAuditSchedule_RowDeleting" OnSorting="gvAuditSchedule_Sorting"
                            AllowSorting="true" OnRowEditing="gvAuditSchedule_RowEditing" OnSelectedIndexChanging="gvAuditSchedule_SelectedIndexChanging"
                            ShowFooter="false" ShowHeader="true" EnableViewState="false" DataKeyNames="FLDREVIEWSCHEDULEID">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Serial Number">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="50px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkInspectionSerialNoHeader" runat="server" CommandName="Sort"
                                            CommandArgument="FLDSERIALNUMBER" ForeColor="White">Serial Number&nbsp;</asp:LinkButton>
                                        <img id="FLDSERIALNUMBER" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblInspectionSerialNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Inspection Type">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="50px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblTypeHeader" runat="server">Type</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblInspectionTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVIEWTYPENAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Inspection Category">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="60px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblCategoryHeader" runat="server">Category</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblInspectionCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVIEWCATEGORY") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Inspection Name">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkInspectionHeader" runat="server" CommandName="Sort" CommandArgument="FLDREVIEWNAME"
                                            ForeColor="White">Audit / Inspection Name&nbsp;</asp:LinkButton>
                                        <img id="FLDREVIEWNAME" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblInspectionScheduleId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVIEWSCHEDULEID") %>'></asp:Label>
                                        <asp:Label ID="lblInspectionDtKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                        <asp:LinkButton ID="lnkInspection" runat="server" CommandName="Edit" CommandArgument="<%# Container.DataItemIndex %>"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVIEWNAME") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Vessel Code">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblVesselCompanyCodeHeader" runat="server">Vessel/Company Code</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblVesselCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELSHORTCODE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Last Done Date">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                                    <HeaderTemplate>
                                       <asp:Label ID="lblLastDoneDateHeader" runat="server"> Last <br />Done Date</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblLastDoneDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTDONEDATE")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Inspection Date">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkInspectionDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDREVIEWDATE"
                                            ForeColor="White">Due Date&nbsp;</asp:LinkButton>
                                        <img id="FLDREVIEWDATE" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblInspectionDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDREVIEWDATE")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Planned Date">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkPlannedDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDRANGEFROMDATE"
                                            ForeColor="White">Planned Date&nbsp;</asp:LinkButton>
                                        <img id="FLDRANGEFROMDATE" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblPlannedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDRANGEFROMDATE")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="60px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkStatusHeader" runat="server" CommandName="Sort" CommandArgument="FLDSTATUSNAME"
                                            ForeColor="White">Status &nbsp;</asp:LinkButton>
                                        <img id="FLDSTATUSNAME" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblActionHeader" runat="server">
                                                                    Action
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Edit Inspection" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                            ToolTip="Edit Inspection Schedule"></asp:ImageButton>
                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                            ToolTip="Delete Inspection Schedule"></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="divPage" style="position: relative;">
                        <table width="100%" border="0" class="datagrid_pagestyle">
                            <tr>
                                <td nowrap align="center">
                                    <asp:Label ID="lblPagenumber" runat="server">
                                    </asp:Label>
                                    <asp:Label ID="lblPages" runat="server">
                                    </asp:Label>
                                    <asp:Label ID="lblRecords" runat="server">
                                    </asp:Label>&nbsp;&nbsp;
                                </td>
                                <td nowrap align="left" width="50px">
                                    <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                                </td>
                                <td width="20px">
                                    &nbsp;
                                </td>
                                <td nowrap align="right" width="50px">
                                    <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                                </td>
                                <td nowrap align="center">
                                    <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                    </asp:TextBox>
                                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                        Width="40px"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <table cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <img id="Img1" src="<%$ PhoenixTheme:images/red-symbol.png%>" runat="server" />
                            </td>
                            <td>
                                <asp:Literal ID="lblOverDue" runat="server" Text="* Overdue"></asp:Literal>
                            </td>
                            <td>
                                <img id="Img2" src="<%$ PhoenixTheme:images/yellow-symbol.png%>" runat="server" />
                            </td>
                            <td>
                               <asp:Literal ID="lblDue" runat="server" Text="* Due"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
