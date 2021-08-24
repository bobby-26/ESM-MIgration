<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPSCMOUSRPCompanyPerformance.aspx.cs" Inherits="Inspection_InspectionPSCMOUSRPCompanyPerformance" %>

<!DOCTYPE html>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.DocumentManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="~/UserControls/UserControlFlag.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByOwner" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiAddress" Src="~/UserControls/UserControlMultiColumnAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>SRP Company Performance</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            table.Hazard {
                border-collapse: collapse;
            }

                table.Hazard td, table.Hazard th {
                    border: 1px solid;
                    border-color: rgb(30, 57, 91);
                    padding: 5px;
                }
        </style>
        <style type="text/css">
            .checkboxstyle tbody tr td {
                width: 550px;
                vertical-align: top;
            }
        </style>
        <%--      <style>
            /* Split the screen in half */
            .split {
                height: 90%;
                width: 50%;
                position: fixed;
                top: 0;
                padding-top: 20px;
            }

            /* Control the left side */
            .left {
                left: 0;
                background-color: none;
            }

            /* Control the right side */
            .right {
                right: 0;
                background-color: none;
            }

            /* If you want the content centered horizontally and vertically */
            .centered {
                position: absolute;
                top: 50%;
                left: 50%;
                transform: translate(-50%, -50%);
                text-align: center;
            }
        </style>--%>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblConfigure" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <eluc:TabStrip ID="MenuShipRiskProfile" runat="server" OnTabStripCommand="MenuShipRiskProfile_TabStripCommand" Title="Ship Risk Calculator" />
        <%--            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />--%>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadSplitter RenderMode="Lightweight" BorderColor="White" ID="RadSplitter1" runat="server" Height="68%" Width="100%">
            <telerik:RadPane ID="navigationPane" runat="server" Scrolling="None" Width="45%">
                <br />
                <table cellpadding="3" cellspacing="3" width="70%">
                    <tr>
                        <td width="40%">
                            <telerik:RadLabel ID="lblpscmou" runat="server" Text="PSC MOU"></telerik:RadLabel>
                        </td>
                        <td width="10%" align="Center">
                            <telerik:RadComboBox ID="ddlCompany" runat="server" Width="150px" OnTextChanged="ddlCompany_TextChanged" AutoPostBack="true" Filter="Contains" MarkFirstMatch="true" CssClass="input">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                </table>
                <br />
                <table cellpadding="5" cellspacing="5" width="100%" id="shipprofile">
                    <tr>
                        <td colspan="5">
                            <asp:Label ID="RadLabel1" runat="server" Font-Size="Larger" Font-Bold="true" ForeColor="#6699ff" Text="Company Inspection Histroy from the last 36 months"></asp:Label>
                            <hr />
                        </td>
                    </tr>

                    <tr>
                        <td width="40%">
                            <telerik:RadLabel ID="lblnoofinspections" runat="server" Text="How many PSC inspections has the company undergone in the MOU Region?"></telerik:RadLabel>
                        </td>
                        <td width="10%" align="Center">
                            <telerik:RadTextBox ID="txtnoofinsp" runat="server" Width="60px" Text=""></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="40%">
                            <telerik:RadLabel ID="lblnoofdetention" runat="server" Text="In how many detentions have these inspections resulted?"></telerik:RadLabel>
                        </td>
                        <td width="10%" align="Center">
                            <telerik:RadTextBox ID="txtnoofdetention" runat="server" Width="60px" Text=""></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="40%">
                            <telerik:RadLabel ID="lblnoofnonismdef" runat="server" Text="How many non ISM deficicencies have been recorded during these inspections?"></telerik:RadLabel>
                        </td>
                        <td width="10%" align="Center">
                            <telerik:RadTextBox ID="txtnoofnonismdef" runat="server" Width="60px" Text=""></telerik:RadTextBox>
                        </td>
                    </tr>

                    <tr>
                        <td width="40%">
                            <telerik:RadLabel ID="lblnoofismdef" runat="server" Text="How many ISM deficicencies have been recorded during these inspections?"></telerik:RadLabel>
                        </td>
                        <td width="10%" align="Center">
                            <telerik:RadTextBox ID="txtnoofismdef" runat="server" Width="60px" Text=""></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="40%">
                            <telerik:RadLabel ID="lblrefusal" runat="server" Text="Has a refusal of access order been issued to any ship of the company?"></telerik:RadLabel>
                        </td>

                        <td width="10%" align="Center">
                            <asp:RadioButtonList ID="rblrefusal" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Width="100px" AutoPostBack="true">
                                <asp:ListItem Text="Yes" Value="1" Selected="False"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0" Selected="False"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <br />
            </telerik:RadPane>
            <telerik:RadPane ID="RadPane1" runat="server" Scrolling="None" Width="45%">
                <table cellpadding="2" cellspacing="2" width="80%" align="center" id="shipprofile1">
                    <tr>
                        <td colspan="5">
                            <asp:Label ID="RadLabel2" runat="server" Font-Size="Larger" ForeColor="#6699ff" Font-Bold="true" Text="Performance Matrix"></asp:Label>
                            <hr />
                        </td>
                    </tr>
                    <tr height="50%">
                        <td>
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvMenuPSCMOU" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None"
                                AllowPaging="false" AllowCustomPaging="false" Width="100%" Height="100%" CellPadding="0" OnNeedDataSource="gvMenuPSCMOU_NeedDataSource" AllowSorting="true" ShowFooter="false" ShowHeader="true" EnableViewState="false"
                                OnItemDataBound="gvMenuPSCMOU_ItemDataBound">
                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" TableLayout="Fixed" AllowNaturalSort="false"
                                    AutoGenerateColumns="false" DataKeyNames="FLDPERFORMANCEID">
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
                                        <telerik:GridTemplateColumn HeaderText="PSC MOU" Visible="false">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="60%"></ItemStyle>
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblpscmouHeader" runat="server">PSC MOU&nbsp;</telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblpscmouId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERFORMANCEID") %>'></asp:Label>
                                                <asp:Label ID="lblpscmou" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPSCMOUREGION") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <telerik:RadLabel ID="lblpscmouEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERFORMANCEID") %>'></telerik:RadLabel>
                                                <telerik:RadComboBox ID="ddlpscmouregionNameedit" runat="server" Filter="Contains"></telerik:RadComboBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadComboBox ID="ddlpscmouregionNameAdd" runat="server" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true"></telerik:RadComboBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Deficiency Index">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="60%"></ItemStyle>
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblDefindexHeader" runat="server">Deficiency Index&nbsp;</telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDefindexId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERFORMANCEID") %>'></asp:Label>
                                                <asp:Label ID="lblDefindex" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEFICIENCYINDEX") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <telerik:RadLabel ID="lblDefindexEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERFORMANCEID") %>'></telerik:RadLabel>
                                                <telerik:RadComboBox ID="ddlDefindexedit" runat="server" Filter="Contains"></telerik:RadComboBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadComboBox ID="ddlDefindexAdd" runat="server" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true"></telerik:RadComboBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Detention Index">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="60%"></ItemStyle>
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblDetindexHeader" runat="server">Detention Index&nbsp;</telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDetindexId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERFORMANCEID") %>'></asp:Label>
                                                <asp:Label ID="lblDetindex" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETENTIONINDEX") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <telerik:RadLabel ID="lblDetindexEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERFORMANCEID") %>'></telerik:RadLabel>
                                                <telerik:RadComboBox ID="ddlDetindexedit" runat="server" Filter="Contains"></telerik:RadComboBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadComboBox ID="ddlDetindexAdd" runat="server" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true"></telerik:RadComboBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Company Performance">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="60%"></ItemStyle>
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblCompanyperfHeader" runat="server">Performance&nbsp;</telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompanyperfId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERFORMANCEID") %>'></asp:Label>
                                                <asp:Label ID="lblCompanygperf" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYPERFORMANCE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <telerik:RadLabel ID="lblCompanyperfEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERFORMANCEID") %>'></telerik:RadLabel>
                                                <telerik:RadComboBox ID="ddlCompanyperfedit" runat="server" Filter="Contains"></telerik:RadComboBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadComboBox ID="ddlCompanyperfAdd" runat="server" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true"></telerik:RadComboBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Weightage" Visible="false">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPSCMOUWEIGHTAGE") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <eluc:Number ID="ucScoreEdit" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                                    MaxLength="3" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPSCMOUWEIGHTAGE") %>' />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <eluc:Number ID="ucScoreAdd" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                                    MaxLength="3" />
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>


                                        <telerik:GridTemplateColumn HeaderText="Action" Visible="false">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
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
                                    <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <telerik:RadSplitter RenderMode="Lightweight" BorderColor="White" ID="RadSplitter2" runat="server" Height="35%" Width="100%">
            <telerik:RadPane ID="idRadPaneindex" runat="server" Scrolling="None" Width="45%">
                <table cellpadding="2" cellspacing="2" width="100%" id="shipprofile5">
                    <tr>
                        <td colspan="5">
                            <asp:Label ID="RadLabel3" runat="server" Font-Size="Larger" Font-Bold="true" ForeColor="#6699ff" Text="Company Detention Index"></asp:Label>
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lbldetmouavg" runat="server" Text="MoU Average detention ratio "></telerik:RadLabel>
                        </td>
                        <td align="left">
                            <telerik:RadTextBox ID="txtdetmouavg" runat="server" Enabled="false" Width="60px" Text=""></telerik:RadTextBox>
                            <telerik:RadLabel ID="RadLabel5" runat="server" Text="(detentions per inspection)"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblcompdetratio" runat="server" Text="Company Detention Ratio "></telerik:RadLabel>
                        </td>
                        <td align="left">
                            <telerik:RadTextBox ID="txtcompdetratio" runat="server" Enabled="false" Width="60px" Text=""></telerik:RadTextBox>
                            <telerik:RadLabel ID="RadLabel9" runat="server" Text="(Detentions per inspection)"></telerik:RadLabel>
                            <asp:Label runat="server" AlternateText="Edit" ID="idtooltip" ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-calculator-alt"></i></span>
                                                </asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <table cellpadding="0" cellspacing="0" align="center" width="70%" id="shipprofile4">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lbldetbelowlimit" runat="server" Style="text-align: right" Width="120px" Text=""></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="RadLabel17" runat="server" Style="text-align: center" Width="120px" Text=""></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lbldetabvelimit" runat="server" Style="text-align: left" Width="120px" Text=""></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="RadLabel12" runat="server" Style="text-align: center" Width="120px" BackColor="YellowGreen" Text="Below Average"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="RadLabel13" runat="server" Style="text-align: center" Width="120px" BackColor="Orange" Text="Average"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="RadLabel14" runat="server" Style="text-align: center" Width="120px" BackColor="Red" Text="Above Average"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <br />
                            <asp:Label ID="lbldettext" runat="server" Font-Size="" Text="">Detention Index is&nbsp;&nbsp;&nbsp;</asp:Label>
                            <asp:Label ID="lbldetresult" runat="server" Font-Size="" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="idRadPanedefindex" runat="server" Scrolling="None" Width="45%">
                <table align="center" cellpadding="2" cellspacing="2" width="80%" id="shipprofile2">
                    <tr>
                        <td colspan="5">
                            <asp:Label ID="RadLabel4" runat="server" Font-Size="Larger" ForeColor="#6699ff" Font-Bold="true" Text="Company Deficiency Index"></asp:Label>
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblavgdef" runat="server" Text="MoU Average deficiency ratio "></telerik:RadLabel>
                        </td>
                        <td align="left">
                            <telerik:RadTextBox ID="txtavgdef" runat="server" Enabled="false" Width="60px" Text=""></telerik:RadTextBox>
                            <telerik:RadLabel ID="RadLabel7" runat="server" Text="(deficiencies per inspection)"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblcompdefratio" runat="server" Text="Company Deficiency Ratio "></telerik:RadLabel>
                        </td>
                        <td align="left">
                            <telerik:RadTextBox ID="txtcompdefratio" runat="server" Enabled="false" Width="60px" Text=""></telerik:RadTextBox>
                            <telerik:RadLabel ID="RadLabel8" runat="server" Text="(deficiencies per inspection)"></telerik:RadLabel>
                            <asp:Label runat="server" AlternateText="Edit" ID="iddeftooltip" ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-calculator-alt"></i></span>
                                                </asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <table align="center" cellpadding="0" cellspacing="0" width="50%" id="shipprofile3">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lbldeflimitbelowrange" runat="server" Style="text-align: right" Width="120px" Text=""></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="RadLabel16" runat="server" Style="text-align: center" Width="120px" Text=""></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lbldeflimitaboverange" runat="server" Style="text-align: left" Width="120px" Text=""></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="RadLabel6" runat="server" Style="text-align: center" Width="120px" BackColor="YellowGreen" Text="Below Average"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="RadLabel10" runat="server" Style="text-align: center" Width="120px" BackColor="Orange" Text="Average"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="RadLabel11" runat="server" Style="text-align: center" Width="120px" BackColor="Red" Text="Above Average"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <br />
                            <asp:Label ID="lbldeftext" runat="server" Font-Size="" Text="">Deficiency Index is&nbsp;&nbsp;&nbsp;</asp:Label>
                            <asp:Label ID="lbldefindexresult" runat="server" Font-Size="" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <table cellpadding="0" cellspacing="0" width="50%" id="shipprofile6">
            <tr>
                <td align="right">
                    <asp:Label ID="lblcomp" Style="text-align: right" Font-Size="Larger" Font-Bold="true" ForeColor="#6699ff" runat="server">Company Performance is&nbsp;&nbsp;&nbsp;</asp:Label>
                </td>
                <td align="left">
                    <asp:Label ID="lblcompperf" Style="text-align: left" Font-Size="Larger" Font-Bold="true"  runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
