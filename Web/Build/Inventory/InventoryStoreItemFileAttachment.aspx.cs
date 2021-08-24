using System;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Web.UI;
using System.IO;
using System.Web;

public partial class InventoryStoreItemFileAttachment : PhoenixBasePage
{
    string name = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            if (Request.QueryString["storenumber"] != null)
            {
                
                ViewState["STORENUMBER"] = Request.QueryString["storenumber"].ToString();
                if (Request.QueryString["name"] != null)
                    name = Request.QueryString["storenumber"].ToString() + " - " + Request.QueryString["name"].ToString();

                GetAttachmentDtkey();
            }


        }
    }
    protected void GetAttachmentDtkey()
    {
        char[] s = ViewState["STORENUMBER"].ToString().ToCharArray();

        string t = "";

        for (int i=0; i < s.Length; i++)
        {
            if (s[i] != '.')
                t = t + s[i];
        }

        //RadLightBoxItem item = new RadLightBoxItem();
        //item.ImageUrl = "../css/Theme1/images/stores/" + t + ".jpg";
        //item.Title = name;
        //RadLightBox1.Items.Add(item);

        if (File.Exists(HttpContext.Current.Server.MapPath("~/css/Theme1/images/stores/" + t + ".jpg")))
        {
            RadLightBoxItem item = new RadLightBoxItem();
            item.ImageUrl = "../css/Theme1/images/stores/" + t + ".jpg";
            item.Title = name;
            RadLightBox1.Items.Add(item);
        }
        else
        {
            RadLightBoxItem item = new RadLightBoxItem();
            item.ImageUrl = "../css/Theme1/images/stores/no-image-available.jpg";
            item.Title = name;
            RadLightBox1.Items.Add(item);
        }



    }
}