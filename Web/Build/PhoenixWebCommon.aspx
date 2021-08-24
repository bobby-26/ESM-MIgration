<%@ Page Language="C#"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Frameset//EN" "http://www.w3.org/TR/html4/frameset.dtd">
<script runat="server" language="C#">
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["phoenixinuse"] != null)
        {
            Response.Redirect("~/PhoenixBrowsingRestriction.aspx");
        }
        Session["phoenixinuse"] = "yes";
    }
</script>
<html>
<head>
<meta http-equiv="Content-Type" content="text/html;charset=utf-8">
<title></title>
<script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
<script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js" />

<script type="text/javascript" language="javascript">

        window.onresize = function() 
        {
            resizeWindow();
        }

        function Maximize() 
        {
            window.innerWidth = screen.width;
            window.innerHeight = screen.height;
            window.screenX = 0;
            window.screenY = 0;
            alwaysLowered = false;
        }
            
        function resizeWindow() 
        {       
            window.innerWidth = screen.width;
            window.innerHeight = screen.height + 50;
            document.getElementById('taskpane').clientHeight='100%';
        }

        function refresh()
        {
            location.reload();            
        }
        function myTest()
        {
            alert('testing...');
        }
</script>
</head>
<div>
<frameset rows="60px, *" bordercolor="#efefef" framespacing="1">
<frame id="applicationtitle" name="applicationtitle" src="PhoenixApplicationTitle.aspx" scrolling="no"></frame>
<frameset id="content" cols="15%,*">    
    <frame id="taskpane" name="taskpane" src="PhoenixTaskPane.aspx" scrolling="no"></frame>
    <frame id="filterandsearch" name="filterandsearch" src="Dashboard/Dashboard.aspx"></frame>
</frameset>
</frameset>
</div>
<script>
    window.onresize();
</script>