<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PortalDefault.aspx.cs" Inherits="PortalDefault" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <title>
            <%=Application["softwarename"].ToString() %>
            -
            <%=ConfigurationManager.AppSettings.Get("companyname").ToString()%></title>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/Theme1/bootstrap/css/bootstrap.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/jquery-1.12.4.min.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/css/Theme1/bootstrap/js/bootstrap.min.js"></script>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />


        <style type="text/css">
            html, body {
                height: 100%;
            }

            body {
                font-family: "open sans", "Helvetica Neue", Helvetica, Arial, sans-serif;
                background-color: #2f4050;
                font-size: 13px;
                color: #676a6c;
                color: black;
                overflow-x: hidden;
            }

            .text-white {
                color: #ffffff;
            }

            h3, h4, h5 {
                margin-top: 5px;
                font-weight: 600;
            }

            h3 {
                font-size: 16px;
            }

            h1, h2, h3, h4, h5, h6 {
                font-weight: 100;
            }

            .h3, h3 {
                font-size: 24px;
            }

            .h1, .h2, .h3, h1, h2, h3 {
                margin-top: 20px;
                margin-bottom: 10px;
            }

            p {
                margin: 0 0 10px;
            }

            .m-t {
                margin-top: 15px;
            }

            .form-control, .single-line {
                background-color: #FFFFFF;
                background-image: none;
                border: 1px solid #e5e6e7;
                border-radius: 1px;
                color: inherit;
                display: block;
                padding: 6px 12px;
                transition: border-color 0.15s ease-in-out 0s, box-shadow 0.15s ease-in-out 0s;
                width: 100%;
            }

                .form-control, .form-control:focus, .has-error .form-control:focus, .has-success .form-control:focus, .has-warning .form-control:focus {
                    box-shadow: none;
                }

            .form-control {
                display: block;
                width: 100%;
                height: 34px;
                padding: 6px 12px;
                font-size: 14px;
                line-height: 1.42857143;
                color: #555;
                background-color: #fff;
                background-image: none;
                border: 1px solid #ccc;
                border-radius: 4px;
                -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
                box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
                -webkit-transition: border-color ease-in-out .15s,-webkit-box-shadow ease-in-out .15s;
                -o-transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
                transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
            }

            button, input, select, textarea {
                font-family: inherit;
                font-size: inherit;
                line-height: inherit;
            }

            input {
                line-height: normal;
            }

            button, input, optgroup, select, textarea {
                margin: 0;
                font: inherit;
                font-size: inherit;
                line-height: inherit;
                font-family: inherit;
                color: inherit;
            }

            .block {
                display: block !important;
            }

            .m-b {
                margin-bottom: 15px;
            }

            .full-width {
                width: 100% !important;
            }

            .block {
                display: block;
            }

            .btn-primary {
                background-color: #1ab394;
                border-color: #1ab394;
                color: #FFFFFF;
            }

            .btn {
                border-radius: 3px;
            }

            .btn {
                display: inline-block;
                padding: 6px 12px;
                margin-bottom: 0;
                font-size: 14px;
                font-weight: 400;
                line-height: 1.42857143;
                text-align: center;
                white-space: nowrap;
                vertical-align: middle;
                -ms-touch-action: manipulation;
                touch-action: manipulation;
                cursor: pointer;
                -webkit-user-select: none;
                -moz-user-select: none;
                -ms-user-select: none;
                user-select: none;
                background-image: none;
                border: 1px solid transparent;
                border-top-color: transparent;
                border-right-color: transparent;
                border-bottom-color: transparent;
                border-left-color: transparent;
                border-radius: 4px;
            }

            button, input, select, textarea {
                font-family: inherit;
                font-size: inherit;
                line-height: inherit;
            }

                button, html input[type="button"], input[type="reset"], input[type="submit"] {
                    -webkit-appearance: button;
                    cursor: pointer;
                }

            button, select {
                text-transform: none;
            }

            button {
                overflow: visible;
            }

            button, input, optgroup, select, textarea {
                margin: 0;
                margin-bottom: 0px;
                font: inherit;
                font-weight: inherit;
                font-size: inherit;
                line-height: inherit;
                font-family: inherit;
                color: inherit;
            }

            a {
                cursor: pointer;
            }

            a {
                background-color: transparent;
            }

            .loginscreen.middle-box {
                width: 300px;
            }

            .middle-box {
                max-width: 400px;
                z-index: 100;
                margin: 0 auto;
                padding-top: 40px;
            }

            .fadeInDown {
                -webkit-animation-name: fadeInDown;
                animation-name: fadeInDown;
            }

            .animated {
                -webkit-animation-duration: 1s;
                animation-duration: 1s;
                -webkit-animation-fill-mode: both;
                animation-fill-mode: both;
            }

            .text-center {
                text-align: center;
            }

            small, small {
                font-size: 85%;
            }

            .text-center {
                text-align: center;
            }

            p {
                margin: 0 0 10px;
            }

            .btn-primary:hover, .btn-primary:focus, .btn-primary:active, .btn-primary.active, .btn-primary:active:focus, .btn-primary:active:hover, .btn-primary.active:hover, .btn-primary.active:focus {
                background-color: #18a689;
                border-color: #18a689;
                color: #FFFFFF;
            }

            .btn.focus, .btn:focus {
                color: #333;
                text-decoration: none;
            }

            .loginbody {
                background-image: url('<%=Session["sitepath"]%>/css/Theme1/images/bg2.jpg');
                background-size: cover;
                margin: 0;
                overflow: hidden;
            }

            .icon-bar {
                position: relative;
                overflow: hidden;
                margin: 0;
                transform: translateY(-50%);
                float: right;
            }

                .icon-bar a {
                    display: block;
                    text-align: center;
                    padding: 16px;
                    transition: all 0.3s ease;
                    color: white;
                    font-size: 20px;
                }

                    .icon-bar a:hover {
                        background-color: #FFFFFF;
                    }

            .newapplicant {
                background: #3B5998;
                color: white;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body class="loginbody">
    <div class="middle-box text-center loginscreen  animated fadeInDown">
        <div>
            <h3 class="text-white">Welcome to
                <%=Application["softwarename"].ToString() %></h3>
            <p class="text-white">
                Login in. To see it in action.
            </p>
            <form class="m-t" role="form" action="#" runat="server" autocomplete="off">
                <div class="form-group">
                    <input id="txtUserName" type="text" class="form-control" placeholder="Username" required=""
                        runat="server">
                </div>
                <div class="form-group">
                    <input id="txtPassword" type="password" class="form-control" placeholder="Password"
                        required="" runat="server">
                </div>
                <div class="form-group">
                    <input id="txtCompanyCode" type="text" class="form-control" placeholder="Company Code" visible="false"
                        maxlength="20" required="" runat="server">
                </div>
                <button type="submit" class="btn btn-primary block full-width m-b" runat="server"
                    onserverclick="phoenixLogin_Authenticate">
                    Login</button>
                <div class="alert alert-danger alert-dismissible fade in" runat="server" id="divMessage"
                    visible="false" style="margin: 10px">
                    <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a> <span
                        id="spnmsg" runat="server"></span>
                </div>
                <a href="../Options/OptionsForgotPassword.aspx?loginfrom=AGENT" class="text-white"><small>Forgot password?</small></a>
            </form>
        </div>
    </div>
  
</body>
</html>
