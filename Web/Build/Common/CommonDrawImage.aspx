<%@ Page Language="C#" ContentType="image/jpeg" %>
<%@ Import Namespace="System.Drawing" %>
<%@ Import Namespace="System.Drawing.Drawing2D" %>
<%@ Import Namespace="System.Drawing.Imaging" %>

<script language="C#" runat="server">
    void Page_Load (Object sender, EventArgs e)
    {
        // Draw the output

        Response.Clear();
        int height = 30;
        int width = 100;
        Bitmap bmp = new Bitmap(width, height);

        RectangleF rectf = new RectangleF(10, 5, 0, 0);

        Graphics g = Graphics.FromImage(bmp);
        g.Clear(Color.White);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        g.DrawString(Session["captcha"].ToString(), new Font("Thaoma", 12, FontStyle.Italic), Brushes.Green, rectf);
        g.DrawRectangle(new Pen(Color.Red), 1, 1, width - 2, height - 2);
        g.Flush();
        Response.ContentType = "image/jpeg";
        bmp.Save(Response.OutputStream, ImageFormat.Jpeg);
        g.Dispose();
        bmp.Dispose();
    }
</script>
