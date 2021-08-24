<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockDiscussion.aspx.cs"
    Inherits="DryDockDiscussion" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Discussion forum</title>
     <telerik:RadCodeBlock ID="rad1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
     
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
   <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
             
                <div style="font-weight: 600; font-size: 12px;" runat="server">
                    <eluc:TabStrip ID="MenuHeader" runat="server" OnTabStripCommand="MenuHeader_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div style="font-weight: 600; font-size: 12px;" runat="server">
                    
                        <eluc:TabStrip ID="MenuDiscussion" runat="server" OnTabStripCommand="MenuDiscussion_TabStripCommand">
                        </eluc:TabStrip>
                  
                </div>
       
                <div>                                      
                    <table border="0" cellpadding="1" cellspacing="0"  style="padding: 1px;
                        margin: 1px; border-style: solid; border-width: 1px;" width="99%"> 
                        <tr>
                            <td align="left" colspan="2">
                              <telerik:RadLabel ID="lblPostYourCommentsHere" runat="server" Text="Post Your Comments Here"></telerik:RadLabel>
                            </td>
                            <td align="left" style="vertical-align: top;" colspan="2">
                                <telerik:RadTextBox RenderMode="Lightweight"  ID="txtNotesDescription" runat="server" 
                                    CssClass="gridinput_mandatory" Height="49px" TextMode="MultiLine" Width="692px"></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>                    
              
                    <telerik:RadListView ID="repDiscussion" runat="server" >
                       
                        <ItemTemplate>
                            <table cellpadding="1" cellspacing="0" style="padding: 1px; margin: 1px; border-style: solid;
                                border-width: 1px;" width="99%">
                                <tr>
                                    <td>
                              <telerik:RadLabel ID="lblPostedBy" runat="server" Text="Posted By"></telerik:RadLabel>
                                        
                                    </td>
                                    <td>
                                        <b>
                                            <%#DataBinder.Eval(Container, "DataItem.NAME")%>
                                        </b>
                                    </td>
                                   <td rowspan="2" align="center"  style="border-left: 1px solid">Comments -
                                        <div id="divComment" style="height: 40px; width: 600px;text-align: left; border-width: 1px;
                                            overflow: auto;white-space:normal;word-wrap:break-word";>
                                            <b>
                                                <%# Eval("DESCRIPTION")%></b>
                                        </div>
                                    </td>    
                                    <td align="left" valign="top"  style="border-left: 1px solid">
                              <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                                        
                                    </td>
                                    <td align="left" valign="top">
                                        <b>
                                            <%#DataBinder.Eval(Container, "DataItem.POSTEDDATE")%>
                                        </b>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                       
                    </telerik:RadListView>                                 
                </div>
                
            </div>
        
    </form>
</body>
</html>
