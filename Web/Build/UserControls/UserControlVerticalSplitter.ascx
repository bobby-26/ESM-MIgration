<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlVerticalSplitter.ascx.cs" Inherits="UserControls_UserControlVerticalSplitter" %>
<style type="text/css">
    html{  
        margin: 0;  
        padding: 0; 
        overflow: hidden;
    } 
    #dragSplit
        {            
            background-color: #ffffff;
            position: absolute;
            top: 0;
            left: 0;            
            z-index: 98;
            opacity: .10;
            filter: alpha(opacity=10);
            -moz-opacity: .10;
            height:100%;
            width:100%;
            display:none;
        }
    </style>
    <div id="dragSplit"></div>
<div style="background-position:center; background-repeat:no-repeat; position:relative; float:left; margin:0px; height:680px; width:3px; z-index:99; border-color:black; border-style:solid; border-width:1px;  cursor:e-resize" id="divSplit" runat="server">
<span id="spnTargetControlID" runat="server" title="" style="visibility:hidden"></span><asp:TextBox ID="txtTargetWidth" runat="server" Width="10px" />
</div>
