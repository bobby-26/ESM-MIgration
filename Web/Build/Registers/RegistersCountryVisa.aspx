<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersCountryVisa.aspx.cs"
    Inherits="RegisterCountryVisa" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Country Visa</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>        
       <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvCountryVisa.ClientID %>"));
                }, 200);
           }
           window.onresize = window.onload = Resize;
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCountryVisa" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuTitle" runat="server" Title="Country Visa"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <table style="width: 50%; padding: 2px;">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCountry" runat="server" Text="Country"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Country ID="ucCountry" runat="server" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVisaType" runat="server" Text="Visa Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucType" runat="server" AppendDataBoundItems="true"
                            HardTypeCode="107" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:HiddenField ID="hdnScrollgvCountryVisa" runat="server" />
            <eluc:TabStrip ID="MenuRegistersCountryVisa" runat="server" OnTabStripCommand="RegistersCountryVisa_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvCountryVisa" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCountryVisa_ItemCommand" OnNeedDataSource="gvCountryVisa_NeedDataSource"
                OnItemDataBound="gvCountryVisa_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true" ShowFooter="false"
                OnSortCommand="gvCountryVisa_SortCommand">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"
                    CommandItemDisplay="Top">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
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
                        <telerik:GridTemplateColumn HeaderText="Country Name" HeaderStyle-Width="110px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkCountryNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDCOUNTRYNAME">Country Name&nbsp;</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVisaID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVISAID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCountryName" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCountryID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Visa Type" HeaderStyle-Width="80px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblVisaTypeHeader" runat="server" Text="Visa Type"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVisaType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVISATYPENAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVisaTypeID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVISATYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="200px" HeaderText="Locations for Submission">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblTimeTakenHeader" runat="server" Text="Locations for Submission"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTimeTaken" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTIMETAKEN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="70px" HeaderText="On Arrival">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblOnArrival" runat="server" Text="On Arrival"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOnArrival" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONARRIVALYESNO") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOnArrivalID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONARRIVAL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="250px" HeaderText="Days Required">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDaysRequiredHeader" runat="server" Text="Days Required"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDaysRequried" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDAYSREQUIREDFORVISA") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="135px" HeaderText="Physical Presence Y/N">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPhysicalPresenceYNHeader" runat="server" Text="Physical Presence Y/N"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="chkPhysicalPresenceYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHYSICALPRESENCEYESNO") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPhysicalPresenceYNID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHYSICALPRESENCEYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="110px" HeaderText="Physical Presence Specification">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPysicalPresenceSpecificationHeader" runat="server" Text="Physical Presence Specification"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPhyPresenceTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHYSICALPRESENCESPECIFICATION") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblPhysicalPresenceSpecification" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPHYSICALPRESENCESPECIFICATION").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDPHYSICALPRESENCESPECIFICATION").ToString().Substring(0, 10) + "..." : DataBinder.Eval(Container, "DataItem.FLDPHYSICALPRESENCESPECIFICATION").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucPhyPresenceTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHYSICALPRESENCESPECIFICATION") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="130px" HeaderText="Urgent Procedure">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblUrgentProcedureHeader" runat="server" Text="Urgent Procedure"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUrgentProcedureText" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURGENTPROCEDURE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblUrgentProcedure" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDURGENTPROCEDURE").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDURGENTPROCEDURE").ToString().Substring(0, 10) + "..." : DataBinder.Eval(Container, "DataItem.FLDURGENTPROCEDURE").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucUrgentProcTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURGENTPROCEDURE") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="125px" HeaderText="Valid with old PP no">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPassportHeader" runat="server" Text="Valid with old PP no"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPassport" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDNOTVALIDONOLDPASSPORTYN") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPassportID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOTVALIDONOLDPASSPORT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="145px" HeaderText="Ordinary Amount(USD)">
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblOrdinaryAmountUSD" runat="server" Text="Ordinary Amount(USD)"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrdinaryAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDINARYAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="130px" HeaderText="Urgent Amount(USD)">
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblUrgentAmountUSD" runat="server" Text="Urgent Amount(USD)"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUrgentAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURGENTAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="65px" HeaderText="Remarks">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblRemarksHeader" runat="server" Text="Remarks"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Remarks" ID="imgRemarks" ToolTip="Remarks">                                    
                                <span class="icon"><i class="fas fa-glasses"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Remarks" ID="imgNoRemarks" ToolTip="Remarks">                                    
                                <span class="icon" style="color:gray;"><i class="fas fa-glasses"></i></span>
                                </asp:LinkButton>
                                <%--    <img id="imgRemarks" runat="server" style="cursor: hand" src="<%$ PhoenixTheme:images/te_view.png %>"
                                            onmousedown="javascript:closeMoreInformation()" />
                                        <img id="imgNoRemarks" runat="server" style="cursor: hand" src="<%$ PhoenixTheme:images/no-remarks.png %>"
                                            onmousedown="javascript:closeMoreInformation()" />--%>
                                <telerik:RadLabel runat="server" ID="lblRemarks" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="110px" HeaderText="Last Modified By">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblModifiedbyHeader" runat="server" Text="Last Modified By"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblModifiedBy" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMODIFIEDBYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="95px" HeaderText="Modified Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblModifiedDateHeader" runat="server" Text="Modified Date"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblModifiedDate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMODIFIEDDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="165px" HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                    CommandName="SELECT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ToolTip="Delete" Width="20PX" Height="20PX"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete">
                                <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Visa Documents" ToolTip="Visa Documents" Width="20PX" Height="20PX"
                                    CommandName="VISADOCUMENTS" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdVisaDocuments">
                                <span class="icon"><i class="fab fa-cc-visa"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Send Mail" ToolTip="Send Mail" Width="20PX" Height="20PX"
                                    CommandName="SENDMAIL" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSendMail">
                               <span class="icon"><i class="fa fa-envelope"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Excel Export" ToolTip="Excel Export" Width="20PX" Height="20PX"
                                    CommandName="EXCEL" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdExcel">
                               <span class="icon"><i class="fas fa-file-excel"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment" ToolTip="Attachment" Width="20PX" Height="20PX"
                                    CommandName="Attachment" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAttachment">
                               <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="No Attachment" ToolTip="No Attachment" Width="20PX" Height="20PX"
                                    CommandName="NOAttachment" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdNoAttachment">
                               <span class="icon" style="color:gray;"><i class="fas fa-paperclip"></i></span>
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
                            <%--<FooterStyle HorizontalAlign="Center" />--%>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
