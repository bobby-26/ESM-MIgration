<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayRollGeneral.aspx.cs" Inherits="PayRoll_PayRollGeneral" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PayRoll General</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvPayRollGeneral.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>

        <%-- For Popup Relaod --%>
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Style="display: none;" />

        <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
        <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
        <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>

        <telerik:RadGrid RenderMode="Lightweight" ID="gvPayRollGeneral" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" ShowFooter="false" Style="margin-bottom: 0px" EnableViewState="true"
            OnNeedDataSource="gvPayRollGeneral_NeedDataSource"
            OnItemCommand="gvPayRollGeneral_ItemCommand"
            OnItemDataBound="gvPayRollGeneral_ItemDataBound">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDEMPLOYEEID">
                <HeaderStyle Width="102px" />
                <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                <Columns>

                    <telerik:GridTemplateColumn HeaderText='Name' AllowSorting='true' HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblemployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>

                             <asp:LinkButton ID="lnkEployeeName" runat="server" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTNAME").ToString()+ " "+ DataBinder.Eval(Container,"DataItem.FLDMIDDLENAME").ToString()+" "+ DataBinder.Eval(Container,"DataItem.FLDLASTNAME").ToString() %>'
                                    CommandName="GETEMPLOYEE"></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <%--                  
    
            <telerik:GridTemplateColumn HeaderText='First Name' AllowSorting='true' HeaderStyle-Width="70px">
                        <ItemTemplate>
                           <telerik:RadLabel ID="lblFIRSTNAME" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
    
            <telerik:GridTemplateColumn HeaderText='Middle Name' AllowSorting='true' HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblMIDDLENAME" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMIDDLENAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Last Name' AllowSorting='true' HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblLASTNAME" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>--%>

                    <telerik:GridTemplateColumn HeaderText='PAN No.' AllowSorting='true' HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPANNUMBER" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPANNUMBER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Door No.' AllowSorting='true' HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFLATDOORBLOCKNUMBER" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLATDOORBLOCKNUMBER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Premises' AllowSorting='true' HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblNAMEOFPREMISES" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFPREMISES") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Street' AllowSorting='true' HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSTREET" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTREET") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Locality' AllowSorting='true' HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAREALOCALITY" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAREALOCALITY") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Town' AllowSorting='true' HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblTOWNCITYDISTRICT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOWNCITYDISTRICT") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='State' AllowSorting='true' HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSTATE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATENAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Country' AllowSorting='true' HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCOUNTRY" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                <%--    <telerik:GridTemplateColumn HeaderText='Pin Code' AllowSorting='true' HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPINCODE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPINCODE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Email Add. 1' AllowSorting='true' HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblEMAILADDRESS1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMAILADDRESS1") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Email Add. 2' AllowSorting='true' HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblEMAILADDRESS2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMAILADDRESS2") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Mobile No. 1' AllowSorting='true' HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblMOBILENUMBER1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOBILENUMBER1") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Mobile No. 2' AllowSorting='true' HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblMOBILENUMBER2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOBILENUMBER2") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='STD/ISD Code' AllowSorting='true' HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSTDISDCODE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTDISDCODE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Office No.' AllowSorting='true' HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblOFFICENUMBER" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICENUMBER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Residence No.' AllowSorting='true' HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRESIDENCENUMBER" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESIDENCENUMBER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Aadhar No.' AllowSorting='true' HeaderTooltip="Aadhar No." HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAADHAARNUMBER" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAADHAARNUMBER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Enrol. Id' AllowSorting='true' HeaderTooltip="Enrollment Id" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAADHAARENROLLMENTID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAADHAARENROLLMENTID") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>--%>

                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="70px" AllowSorting='true' HeaderTooltip="Action">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit"
                                CommandName="EDIT" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' ID="cmdEdit"
                                ToolTip="Edit" Width="20PX">
                                  <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>

                            <asp:LinkButton runat="server" AlternateText="Delete"
                                CommandName="DELETE" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' ID="cmdDelete"
                                ToolTip="Delete" Width="20PX">
                                     <span class="icon"><i class="fas fa-trash"></i></span>
                            </asp:LinkButton>

                        </ItemTemplate>
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
                <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
