<%@ Page Language="C#" %>
<script language="c#" runat="server">
    protected void Page_Load(object o, EventArgs e)
    {
        Response.ContentType = "application/javascript";
    }    
</script>

function GetBrowser()
{
	return 'IE6';
}

var SitePath = '<% =Session["sitepath"] %>'+'/';
var ajxLoadingText = "";

function KeepComma(str)
{
	return str.replace(/,/g,'¿');
}