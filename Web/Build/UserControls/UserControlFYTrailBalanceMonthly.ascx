<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlFYTrailBalanceMonthly.ascx.cs"
    Inherits="UserControls_UserControlFYTrailBalanceMonthly" %>
<div class="navAppSelect" style="position: relative">
    <asp:Repeater ID="rptTrailBalance" runat="server" OnItemDataBound="rptTrailBalance_OnItemDataBound">
        <HeaderTemplate>
            <table class="Grid" cellspacing="0" rules="all" border="1">
                <tr>
                    <th scope="col">
                        &nbsp;
                    </th>
                    <th scope="col" align="center" style="width: 150px">
                        Account
                    </th>
                    <th scope="col" align="center" style="width: 150px">
                        Opening Balance
                    </th>
                    <th scope="col" align="center" style="width: 150px">
                        January
                    </th>
                    <th scope="col" align="center" style="width: 150px">
                        February
                    </th>
                    <th scope="col" align="center" style="width: 150px">
                        March
                    </th>
                    <th scope="col" align="center" style="width: 150px">
                        April
                    </th>
                    <th scope="col" align="center" style="width: 150px">
                        May
                    </th>
                    <th scope="col" align="center" style="width: 150px">
                        June
                    </th>
                    <th scope="col" align="center" style="width: 150px">
                        July
                    </th>
                    <th scope="col" align="center" style="width: 150px">
                        August
                    </th>
                    <th scope="col" align="center" style="width: 150px">
                        September
                    </th>
                    <th scope="col" align="center" style="width: 150px">
                        October
                    </th>
                    <th scope="col" align="center" style="width: 150px">
                        November
                    </th>
                    <th scope="col" align="center" style="width: 150px">
                        December
                    </th>
                    <th scope="col" align="center" style="width: 150px">
                        Closing Balance
                    </th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <img alt="" style="cursor: pointer" src="../css/Theme2/images/collapsed.gif" />
                    <asp:Panel ID="pnlLevel2" runat="server" Style="display: none">
                        <asp:Repeater ID="rptTrailBalance2nd" runat="server" OnItemDataBound="rptTrailBalance2nd_OnItemDataBound">
                            <HeaderTemplate>
                                <table class="ChildGrid" cellspacing="0" rules="all" border="1">
                                    <tr>
                                        <th scope="col">
                                            &nbsp;
                                        </th>
                                        <th scope="col" align="center" style="width: 150px">
                                            Account
                                        </th>
                                        <th scope="col" align="center" style="width: 150px">
                                            Opening Balance
                                        </th>
                                        <th scope="col" align="center" style="width: 150px">
                                            January
                                        </th>
                                        <th scope="col" align="center" style="width: 150px">
                                            February
                                        </th>
                                        <th scope="col" align="center" style="width: 150px">
                                            March
                                        </th>
                                        <th scope="col" align="center" style="width: 150px">
                                            April
                                        </th>
                                        <th scope="col" align="center" style="width: 150px">
                                            May
                                        </th>
                                        <th scope="col" align="center" style="width: 150px">
                                            June
                                        </th>
                                        <th scope="col" align="center" style="width: 150px">
                                            July
                                        </th>
                                        <th scope="col" align="center" style="width: 150px">
                                            August
                                        </th>
                                        <th scope="col" align="center" style="width: 150px">
                                            September
                                        </th>
                                        <th scope="col" align="center" style="width: 150px">
                                            October
                                        </th>
                                        <th scope="col" align="center" style="width: 150px">
                                            November
                                        </th>
                                        <th scope="col" align="center" style="width: 150px">
                                            December
                                        </th>
                                        <th scope="col" align="center" style="width: 150px">
                                            Closing Balance
                                        </th>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <img alt="" style="cursor: pointer" src="../css/Theme2/images/collapsed.gif" />
                                        <asp:Panel ID="pnlLevel3" runat="server" Style="display: none">
                                            <asp:Repeater ID="rptTrailBalance3rd" runat="server">
                                                <HeaderTemplate>
                                                    <table class="ChildGrid" cellspacing="0" rules="all" border="1">
                                                        <tr>
                                                            <th scope="col" align="center" style="width: 150px">
                                                                Account
                                                            </th>
                                                            <th scope="col" align="center" style="width: 150px">
                                                                Opening Balance
                                                            </th>
                                                            <th scope="col" align="center" style="width: 150px">
                                                                January
                                                            </th>
                                                            <th scope="col" align="center" style="width: 150px">
                                                                February
                                                            </th>
                                                            <th scope="col" align="center" style="width: 150px">
                                                                March
                                                            </th>
                                                            <th scope="col" align="center" style="width: 150px">
                                                                April
                                                            </th>
                                                            <th scope="col" align="center" style="width: 150px">
                                                                May
                                                            </th>
                                                            <th scope="col" align="center" style="width: 150px">
                                                                June
                                                            </th>
                                                            <th scope="col" align="center" style="width: 150px">
                                                                July
                                                            </th>
                                                            <th scope="col" align="center" style="width: 150px">
                                                                August
                                                            </th>
                                                            <th scope="col" align="center" style="width: 150px">
                                                                September
                                                            </th>
                                                            <th scope="col" align="center" style="width: 150px">
                                                                October
                                                            </th>
                                                            <th scope="col" align="center" style="width: 150px">
                                                                November
                                                            </th>
                                                            <th scope="col" align="center" style="width: 150px">
                                                                December
                                                            </th>
                                                            <th scope="col" align="center" style="width: 150px">
                                                                Closing Balance
                                                            </th>
                                                        </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblOrderId" runat="server" Text='<%# Eval("FLDDESCRIPTION") %>' />
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("FLDOPENINGBALANCE") %>' />
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="lblOrderDate" runat="server" Text='<%# Eval("FLDJANUARY") %>' />
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="Label6" runat="server" Text='<%# Eval("FLDFEBRUARY") %>' />
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="Label7" runat="server" Text='<%# Eval("FLDMARCH") %>' />
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="Label8" runat="server" Text='<%# Eval("FLDAPRIL") %>' />
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="Label9" runat="server" Text='<%# Eval("FLDMAY") %>' />
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="Label10" runat="server" Text='<%# Eval("FLDJUNE") %>' />
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="Label11" runat="server" Text='<%# Eval("FLDJULY") %>' />
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="Label12" runat="server" Text='<%# Eval("FLDAUGUST") %>' />
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="Label13" runat="server" Text='<%# Eval("FLDSEPTEMBER") %>' />
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="Label14" runat="server" Text='<%# Eval("FLDOCTOBER") %>' />
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="Label15" runat="server" Text='<%# Eval("FLDNOVEMBER") %>' />
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="Label16" runat="server" Text='<%# Eval("FLDDECEMBER") %>' />
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("FLDCLOSINGBALANCE") %>' />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </asp:Panel>
                                        <asp:HiddenField ID="hfCustomerId" runat="server" Value='<%# Eval("FLDACCOUNTID") %>' />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOrderId" runat="server" Text='<%# Eval("FLDDESCRIPTION") %>' />
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("FLDOPENINGBALANCE") %>' />
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblOrderDate" runat="server" Text='<%# Eval("FLDJANUARY") %>' />
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label6" runat="server" Text='<%# Eval("FLDFEBRUARY") %>' />
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label7" runat="server" Text='<%# Eval("FLDMARCH") %>' />
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label8" runat="server" Text='<%# Eval("FLDAPRIL") %>' />
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label9" runat="server" Text='<%# Eval("FLDMAY") %>' />
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label10" runat="server" Text='<%# Eval("FLDJUNE") %>' />
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label11" runat="server" Text='<%# Eval("FLDJULY") %>' />
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label12" runat="server" Text='<%# Eval("FLDAUGUST") %>' />
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label13" runat="server" Text='<%# Eval("FLDSEPTEMBER") %>' />
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label14" runat="server" Text='<%# Eval("FLDOCTOBER") %>' />
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label15" runat="server" Text='<%# Eval("FLDNOVEMBER") %>' />
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label16" runat="server" Text='<%# Eval("FLDDECEMBER") %>' />
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label4" runat="server" Text='<%# Eval("FLDCLOSINGBALANCE") %>' />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </asp:Panel>
                    <asp:HiddenField ID="hfCustomerId" runat="server" Value='<%# Eval("FLDACCOUNTID") %>' />
                </td>
                <td>
                    <asp:Label ID="lblContactName" runat="server" Text='<%# Eval("FLDDESCRIPTION") %>' />
                </td>
                <td align="right">
                    <asp:Label ID="lblOrderDate" runat="server" Text='<%# Eval("FLDOPENINGBALANCE") %>' />
                </td>
                <td align="right">
                    <asp:Label ID="Label17" runat="server" Text='<%# Eval("FLDJANUARY") %>' />
                </td>
                <td align="right">
                    <asp:Label ID="Label6" runat="server" Text='<%# Eval("FLDFEBRUARY") %>' />
                </td>
                <td align="right">
                    <asp:Label ID="Label7" runat="server" Text='<%# Eval("FLDMARCH") %>' />
                </td>
                <td align="right">
                    <asp:Label ID="Label8" runat="server" Text='<%# Eval("FLDAPRIL") %>' />
                </td>
                <td align="right">
                    <asp:Label ID="Label9" runat="server" Text='<%# Eval("FLDMAY") %>' />
                </td>
                <td align="right">
                    <asp:Label ID="Label10" runat="server" Text='<%# Eval("FLDJUNE") %>' />
                </td>
                <td align="right">
                    <asp:Label ID="Label11" runat="server" Text='<%# Eval("FLDJULY") %>' />
                </td>
                <td align="right">
                    <asp:Label ID="Label12" runat="server" Text='<%# Eval("FLDAUGUST") %>' />
                </td>
                <td align="right">
                    <asp:Label ID="Label13" runat="server" Text='<%# Eval("FLDSEPTEMBER") %>' />
                </td>
                <td align="right">
                    <asp:Label ID="Label14" runat="server" Text='<%# Eval("FLDOCTOBER") %>' />
                </td>
                <td align="right">
                    <asp:Label ID="Label15" runat="server" Text='<%# Eval("FLDNOVEMBER") %>' />
                </td>
                <td align="right">
                    <asp:Label ID="Label16" runat="server" Text='<%# Eval("FLDDECEMBER") %>' />
                </td>
                <td align="right">
                    <asp:Label ID="Label5" runat="server" Text='<%# Eval("FLDCLOSINGBALANCE") %>' />
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $("body").on("click", "[src*=collapsed]", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../css/Theme2/images/expanded.gif");
        });
        $("body").on("click", "[src*=expanded]", function () {
            $(this).attr("src", "../css/Theme2/images/collapsed.gif");
            $(this).closest("tr").next().remove();
        });
    </script>
