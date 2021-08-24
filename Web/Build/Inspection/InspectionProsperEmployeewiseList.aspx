<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionProsperEmployeewiseList.aspx.cs" Inherits="Inspection_InspectionProsperEmployeewiseList" %>

<!DOCTYPE html>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCRank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCVesseltype" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCPool" Src="~/UserControls/UserControlPool.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCLQuick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCOfficerlist" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Progress Report</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript" language="javascript">
            function checkTextAreaMaxLength(textBox, e, length) {

                var mLen = textBox["MaxLength"];
                if (null == mLen)
                    mLen = length;

                var maxLength = parseInt(mLen);
                if (!checkSpecialKeys(e)) {
                    if (textBox.value.length > maxLength - 1) {
                        if (window.event)//IE
                            e.returnValue = false;
                        else//Firefox
                            e.preventDefault();
                    }
                }
            }

            function checkSpecialKeys(e) {
                if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
                    return false;
                else
                    return true;
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Title runat="server" ID="ucTitle" Text="Individual History" Visible="false"
                ShowMenu="true" />
            <eluc:TabStrip ID="MenuProgress" runat="server" OnTabStripCommand="QualityProgress_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" width="75%">
                <tr align="left">
                    <td>
                        <telerik:RadLabel ID="lblfirstname" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="lblfname" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="220px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRANKname" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="lblrname" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="170px"></telerik:RadTextBox>
                    </td>
                    <%--  <td>
                        <telerik:RadLabel ID="lbllastname"  runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>                        
                        <asp:TextBox ID="lbllname" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                    </td>--%>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblfromdate" runat="server" Text="From"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtfromdate" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="78px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbltodate" runat="server" Text="To"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox ID="txttodate" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="78px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblcycle" runat="server" Text="Cycle Period"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlcycle" DataTextField="FLDCYCLEDATE" AutoPostBack="true"  DataValueField="FLDCYCLEID" Width="170px"
                            runat="server" OnSelectedIndexChanged="ddlcycle_SelectedIndexChanged"
                            Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="Label1" runat="server" Text="Tenure Period"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddltenurecycle" DataTextField="FLDCYCLEDATE" AutoPostBack="true"  DataValueField="FLDTENURECYCLEID" Width="170px"
                            runat="server" OnSelectedIndexChanged="ddltenurecycle_SelectedIndexChanged"
                            Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <h3><b>
                            <telerik:RadLabel ID="lbllastmodified" runat="server" Text=""></telerik:RadLabel>
                        </b></h3>
                    </td>

                </tr>

            </table>
            <eluc:TabStrip ID="ProsperMenu" runat="server" OnTabStripCommand="Prosper_TabStripCommand"></eluc:TabStrip>
            <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                <telerik:RadGrid RenderMode="Lightweight" ID="gvProsperemplist" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" ShowHeader="true" ShowFooter="false" AllowPaging="true" AllowCustomPaging="true" GroupingEnabled="false" EnableHeaderContextMenu="true"
                    OnItemCommand="gvProsperemplist_ItemCommand"
                    OnRowDataBound="gvProspercomplist_RowDataBound"
                    OnRowCreated="gvProspercomplist_RowCreated" OnNeedDataSource="gvProsperemplist_NeedDataSource"
                    EnableViewState="true" OnDataBound="OnDataBound">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false">
                        <ColumnGroups>
                            <telerik:GridColumnGroup Name="PSCGROUP" HeaderText="PSC" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="VETTINGGROUP" HeaderText="Vetting" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="INCIDENTGROUP" HeaderText="Incident" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="HFGROUP" HeaderText="Health & Safety" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="FBGROUP" HeaderText="Feedback" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="TPGROUP" HeaderText="Third Party" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="ENVGROUP" HeaderText="External Nav & Env" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        </ColumnGroups>
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
                            <telerik:GridTemplateColumn HeaderText="Date" HeaderStyle-Width="75px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbldate" CommandName="VIEWDETAILS" CommandArgument="<%# Container.DataItem %>" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATE")) %>'></asp:LinkButton>
                                    <telerik:RadLabel ID="lblsource" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSOURCEID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblEVENT" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEVENT") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%-- <telerik:GridTemplateColumn HeaderText="To Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTODATE"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                            <%-- <telerik:GridTemplateColumn HeaderText="Rank">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblHeaderName" runat="server" Text="Rank"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                            <telerik:GridTemplateColumn HeaderText="Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblinsname" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadLabel ID="lbltotal" runat="server" Visible="true" Text='Total:'></telerik:RadLabel>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Vessel Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblvesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblvesselname" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Vessel Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDTYPEDESCRIPTION")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="DEF" UniqueName="PSCDEF" ColumnGroupName="PSCGROUP" HeaderStyle-Width="40px">
                                <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDPSCDEF").ToString() == "0" ? "NIL" : DataBinder.Eval(Container, "DataItem.FLDPSCDEF")%>
                                    <%-- <%# DataBinder.Eval(Container, "DataItem.FLDPSCDEF")%>--%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="DET" UniqueName="PSCDET" ColumnGroupName="PSCGROUP" HeaderStyle-Width="40px">
                                <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDPSCDET").ToString() == "0" ? "NIL" : DataBinder.Eval(Container, "DataItem.FLDPSCDET")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="DEF" UniqueName="VETTINGDEF" ItemStyle-Wrap="true" ItemStyle-Width="4" ColumnGroupName="VETTINGGROUP" HeaderStyle-Width="40px">
                                <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDVETTINGDEF").ToString() == "0" ? "NIL" : DataBinder.Eval(Container, "DataItem.FLDVETTINGDEF")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="REJ" UniqueName="VETTINGREJ" ColumnGroupName="VETTINGGROUP" HeaderStyle-Width="40px">
                                <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDVETTINGREJ").ToString() == "0" ? "NIL" : DataBinder.Eval(Container, "DataItem.FLDVETTINGREJ")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="A" UniqueName="INCIDENTACATEGORY" ColumnGroupName="INCIDENTGROUP" HeaderStyle-Width="40px">
                                <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDINCIDENTACATEGORY")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="B" UniqueName="INCIDENTBCATEGORY" ColumnGroupName="INCIDENTGROUP" HeaderStyle-Width="40px">
                                <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDINCIDENTBCATEGORY")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="C" UniqueName="INCIDENTCCATEGORY" ColumnGroupName="INCIDENTGROUP" HeaderStyle-Width="40px">
                                <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDINCIDENTCCATEGORY")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="A" UniqueName="HNSACATEGORY" ColumnGroupName="HFGROUP" HeaderStyle-Width="40px">
                                <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDHNSACATEGORY")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="B" UniqueName="HNSBCATEGORY" ColumnGroupName="HFGROUP" HeaderStyle-Width="40px">
                                <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDHNSBCATEGORY")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="C" UniqueName="HNSCCATEGORY" ColumnGroupName="HFGROUP" HeaderStyle-Width="40px">
                                <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDHNSCCATEGORY")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="POS" UniqueName="FEEDBACKPOSITIVE" ColumnGroupName="FBGROUP" HeaderStyle-Width="45px">
                                <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDFEEDBACKPOSITIVE")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="NEG" UniqueName="FEEDBACKNEGATIVE" ColumnGroupName="FBGROUP" HeaderStyle-Width="45px">
                                <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDFEEDBACKNEGATIVE")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="WRN" UniqueName="FEEDBACKWARNING" ColumnGroupName="FBGROUP" HeaderStyle-Width="45px">
                                <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDFEEDBACKWARNING")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="DEF" UniqueName="TPIDEF" ColumnGroupName="TPGROUP" HeaderStyle-Width="45px">
                                <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDTPIDEF").ToString() == "0" ? "NIL" : DataBinder.Eval(Container, "DataItem.FLDTPIDEF")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="REJ" UniqueName="TPIDET" ColumnGroupName="TPGROUP" HeaderStyle-Width="45px">
                                <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDTPIDET").ToString() == "0" ? "NIL" : DataBinder.Eval(Container, "DataItem.FLDTPIDET")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="DEF" UniqueName="EXTDEF" ColumnGroupName="ENVGROUP" HeaderStyle-Width="54px">
                                <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDEXTDEF").ToString() == "0" ? "NIL" : DataBinder.Eval(Container, "DataItem.FLDEXTDEF")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="REJ" UniqueName="EXTDET" ColumnGroupName="ENVGROUP" HeaderStyle-Width="54px">
                                <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDEXTDET").ToString() == "0" ? "NIL" : DataBinder.Eval(Container, "DataItem.FLDEXTDET")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <%--     <telerik:GridTemplateColumn HeaderText="psc">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblHeaderReliefDue" runat="server" Text="PSC"></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                      <%# DataBinder.Eval(Container, "DataItem.FLDPSC")%> 
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="SIRE">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblHeaderReliefDue" runat="server" Text="SIRE"></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                      <%# DataBinder.Eval(Container, "DataItem.FLDSIRE")%> 
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="Incident">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblHeaderReliefDue" runat="server" Text="Incident"></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                     <%# DataBinder.Eval(Container, "DataItem.FLDINCIDENT")%> 
                                   
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="Feedback ">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblHeaderReliefDue" runat="server" Text="Feedback"></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                      <%# DataBinder.Eval(Container, "DataItem.FLDFEEDBACK")%> 
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>--%>
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
            </div>

            <%--         <div id="divPage" runat="server" style="position: relative;">
                <table width="100%" border="0" class="datagrid_pagestyle">
                    <tr>
                        <td nowrap="nowrap" align="center">
                            <telerik:RadLabel ID="lblPagenumber" runat="server">
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblPages" runat="server">
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblRecords" runat="server">
                            </telerik:RadLabel>&nbsp;&nbsp;
                        </td>
                        <td nowrap="nowrap" align="left" width="50px">
                            <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                        </td>
                        <td width="20px">&nbsp;
                        </td>
                        <td nowrap="nowrap" align="right" width="50px">
                            <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                        </td>
                        <td nowrap="nowrap" align="center">
                            <eluc:Number ID="txtnopage" MaxLength="3" Width="20px" runat="server"  />

                            <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" 
                                Width="40px"></asp:Button>
                        </td>
                    </tr>
                    <%-- <eluc:Status runat="server" ID="Status1" />--%>
            <%--      </table>
            </div>--%>
            <div>
                </br>
            </div>
            <div id="divGrid1" style="position: relative; z-index: 0; width: 100%;">

                <telerik:RadGrid RenderMode="Lightweight" ID="gvEmployeeProsper" Visible="false" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" AllowSorting="true"
                    OnRowCreated="gvEmployeeProsper_RowCreated"
                    OnRowDataBound="gvEmployeeProsper_RowDataBound" ShowFooter="true">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false">
                        <ColumnGroups>
                            <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="AuditInsp" HeaderText="Audit/Inspection" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="DeficiencyCount" HeaderText="Def Count" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        </ColumnGroups>
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
                            <telerik:GridTemplateColumn Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblFlagHeader" runat="server" Text=""></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                </ItemTemplate>
                                <FooterTemplate></FooterTemplate>
                            </telerik:GridTemplateColumn>


                            <telerik:GridTemplateColumn HeaderText="Vessel Type" Visible="false">
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblvesseltype" runat="server" Text='Vessel Type'></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%-- <asp:HiddenField runat="server" ID="hdnTYPEDESCRIPTION" Value='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEDESCRIPTION") %>' />
                                        <%# DataBinder.Eval(Container,"DataItem.FLDTYPEDESCRIPTION") %>--%>
                                    <%--  <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>--%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Rank" Visible="false">
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblRank" runat="server" Text='Rank'></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%-- <%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>--%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%--  <telerik:GridTemplateColumn HeaderText="Vessel Score" Visible="false">
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblRank" runat="server" Text='Vessel Score'></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDVESSELSCORE") %>
                                      
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            --%>
                            <telerik:GridTemplateColumn HeaderText="Category Type">
                                <ItemStyle Wrap="False" Width="25%"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblPCategory" runat="server" Text='Category'></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>

                                    <telerik:RadLabel ID="lblCategory" runat="server" CommandArgument="<%#Container.DataItem%>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'>
                                    </telerik:RadLabel>

                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Measure">
                                <ItemStyle Wrap="False" Width="25%"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblPMeasure" runat="server" Text='Measure'></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblMeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURENAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="No. of Inspection">
                                <ItemStyle Wrap="False" Width="10%" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblheadNOOFFINSPECTION" runat="server" Text='No. of Inspection'></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblNOOFFINSPECTION" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFFINSPECTION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Count">
                                <ItemStyle Wrap="False" Width="10%" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblPCount" runat="server" Text='Count'></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCount" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOUNT").ToString() == "0" ? "NIL" : DataBinder.Eval(Container, "DataItem.FLDCOUNT")%>'></telerik:RadLabel>

                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Average">
                                <ItemStyle Wrap="False" Width="10%" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblheadaverage" runat="server" Text='Average'></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblaverage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVERAGE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Actual">
                                <ItemStyle Wrap="False" Width="10%" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblPaCTUAL" runat="server" Text='Actual'></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblACTUAL" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUAL") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Count">
                                <ItemStyle Wrap="False" Width="10%" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblPCount" runat="server" Text='Final Score'></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCALCULATED") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <FooterTemplate>
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

            </div>
            <br />
            <%--<eluc:TabStrip ID="Prosperreportmenu" runat="server" OnTabStripCommand="Prosperreportmenu_TabStripCommand"></eluc:TabStrip>--%>
            <div id="divprosperreport" style="position: relative; z-index: 0; width: 100%;">

                <telerik:RadGrid RenderMode="Lightweight" ID="gdprosperreport" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" AllowSorting="true"
                    OnRowDataBound="gdprosperreport_RowDataBound" ShowFooter="true">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false">
                        <ColumnGroups>
                            <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="AuditInsp" HeaderText="Audit/Inspection" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="DeficiencyCount" HeaderText="Def Count" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        </ColumnGroups>
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
                            <telerik:GridTemplateColumn HeaderText="">
                                <ItemStyle Wrap="False" Width="25%"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblPCategory" runat="server" Text=''></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCategory" runat="server" CommandArgument="<%#Container.DataItem%>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'>
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="*Detention/Rejection/High Risk">
                                <ItemStyle Wrap="False" Width="25%"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblMeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREJDET") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="No.of Inspection">
                                <ItemStyle Wrap="False" Width="10%" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblNOOFFINSPECTION" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFINSPECTION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Total No.of Deficiency">
                                <ItemStyle Wrap="False" Width="10%" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCount" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOUNT").ToString() == "0" ? "NIL" : DataBinder.Eval(Container, "DataItem.FLDCOUNT")%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Average Deficiency" UniqueName="AVGDEFICIENCY">
                                <ItemStyle Wrap="False" Width="10%" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblaverage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVERAGE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Score" UniqueName="SCORE" AllowSorting="false">
                                <ItemStyle Wrap="False" Width="10%" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCALCULATED") %>'></telerik:RadLabel>
                                </ItemTemplate>
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

            </div>
            <%--    <div id="divPage" style="position: relative;">
                        <table width="100%" border="0" class="datagrid_pagestyle">
                            <tr>
                               
                                <td width="50%">Total:</td>
                                <td align="right">
                                    <telerik:RadLabel ID="lbltot" runat ="server" ></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>

                    </div>--%>
            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td></td>
                    <td>
                        <telerik:RadLabel ID="Literal1" Text="NOTE: DEF -- Deficiency, DET -- Detention, REJ -- Rejection, POS -- Positive, NEG -- Negative, WRN -- Warning" runat="server">
                        </telerik:RadLabel>
                    </td>
                </tr>
            </table>

        </div>

        <%-- <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="btnconfirm_Click" OKText="Yes"
                    CancelText="No" Visible="false" />--%>


        <%--  </ContentTemplate>
    </asp:UpdatePanel>--%>
        <%--<eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />--%>
    </form>
</body>
</html>
