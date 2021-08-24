<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnerBudgetExpenseReport.aspx.cs" Inherits="OwnerBudgetExpenseReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Custom" Src="~/UserControls/UserControlEditor.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    <style type="text/css">
        p
        {
            margin: 0;
            padding: 0;
            text-align: left;
            max-width: 950px;
            max-width: 900px\9;
            display: block;
        }
        table
        {
            table-layout: fixed; /*border: 1px solid #f00;*/
            word-wrap: break-word;            
            max-width: 950px;
            max-width: 900px\9;
            margin: 0;
            padding: 0;
        }
        img
        {
            table-layout: fixed;
            max-width: 950px;
            max-width: 900px\9;
            margin: 0;
            padding: 0;
            display: block;
        }
        span
        {
            margin: 0;
            padding: 0;
            word-wrap: break-word;
            max-width: 950px;
            max-width: 900px\9;
            *zoom: 1;
            *display: inline;            
        }
        span span
        {
            margin: 0;
            padding: 0;
            word-wrap: break-word;
            max-width: 950px;
            max-width: 900px\9;
            *zoom: 1;
            *display: inline;             
        }
    </style>
</telerik:RadCodeBlock></head>
<body>
    <form runat="server" id="form1">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>

<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden"/>
                    <eluc:TabStrip ID="MenuClose" Title="Expense Report" runat="server" OnTabStripCommand="MenuClose_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                        <eluc:TabStrip ID="MenuWord" runat="server" OnTabStripCommand="MenuWord_TabStripCommand"/>
               <%-- <table align="center" title="PROPOSED OPERATING COST" visible="true">
                    <tr>
                        <td>
                            <font face="BernhardMod BT" size="5">Proposed Operating Cost</font>
                        </td>
                    </tr>
                </table>--%>
            <br />   
            <br />
            <span id="span2" runat="server" title="" style="display: block; margin-left: 20px;
                width: 780px;"></span>
            <br />
            <br />
            <span id="span1" runat="server" title="" style="display: inline-block; margin-left: 20px;
                width: 780px;"></span>
            <br />
            <br />
            <span id="span3" runat="server" title="" style="display: inline-block; margin-left: 20px;
                width: 780px;"></span>
            <br />
            <br />
            <span id="span4" runat="server" title="" style="display: inline-block; margin-left: 20px;
                width: 780px;"></span>
            <br />
            <br />
            <span id="span5" runat="server" title="" style="display: inline-block; margin-left: 20px;
                width: 780px;"></span>
            <br />
            <br />
            <span id="span6" runat="server" title="" style="display: inline-block; margin-left: 20px;
                width: 780px;"></span>
            <br />
            <br />
            <span id="span7" runat="server" title="" style="display: inline-block; margin-left: 20px;
                width: 780px;"></span>
            <br />
            <br />
            <span id="span8" runat="server" title="" style="display: inline-block; margin-left: 20px;
                width: 780px;"></span>
            <br />
            <br />
            <span id="span9" runat="server" title="" style="display: inline-block; margin-left: 20px;
                width: 780px;"></span>
            <br />
            <br />
            <span id="span10" runat="server" title="" style="display: inline-block; margin-left: 20px;
                width: 780px;"></span>    
            <%--<table><tr style="font-weight:bold"></tr></table>--%>
            <span id="spanWord" runat="server" title="" style="display: inline-block; margin-left: 20px;
                width: 780px;" visible="false" ></span>
              </div>   
    </telerik:RadAjaxPanel>     
    </form>
</body>
</html>
