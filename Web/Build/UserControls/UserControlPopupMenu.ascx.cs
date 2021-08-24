using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;

public partial class UserControlPopupMenu : System.Web.UI.UserControl
{
    public event EventHandler MenuStripCommand;
    int _MenuIndex;


    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private void SetMenu(List<MenuLink> _MenuList)
    {
        dlstMenu.DataSource = _MenuList;
        dlstMenu.DataBind();
        dlstMenu.ItemStyle.BackColor = System.Drawing.Color.LightSteelBlue;
        dlstMenu.ItemStyle.ForeColor = System.Drawing.Color.White;
        dlstMenu.SelectedIndex = _MenuIndex;
        dlstMenu.SelectedItemStyle.BackColor = System.Drawing.Color.DeepSkyBlue;

        DataListItem lstItem = dlstMenu.SelectedItem;
    }


    public void SetTrigger(UpdatePanel up)
    {
        foreach (DataListItem dle in dlstMenu.Items)
        {
            LinkButton lb = (LinkButton)dle.FindControl("btnMenu");
            if (lb.PostBackUrl != null && !lb.PostBackUrl.Equals(""))
            {
                PostBackTrigger pbt = new PostBackTrigger();
                pbt.ControlID = lb.UniqueID;
                up.Triggers.Add(pbt);
            }
        }
    }

    public List<MenuLink> MenuList
    {
        set
        {
            SetMenu(value);
        }
    }

    public int MenuIndex
    {
        set
        {
            _MenuIndex = value;
        }
        get
        {
            return dlstMenu.SelectedIndex;
        }
    }

    protected void dlstMenu_ItemDataBound(object sender, DataListItemEventArgs de)
    {
        LinkButton lb = (LinkButton)de.Item.FindControl("btnMenu");

        MenuLink ml = (MenuLink)de.Item.DataItem;

        lb.Visible = false;
        lb.ToolTip = ml.DisplayName;      

        string url = String.Format("{0}?idx={1}&pidx={2}", ml.URL, ml.ItemIndex, ml.ParentItemIndex);
        lb.Visible = true;

        if (!ml.URL.Equals("#") && ml.URL.Contains("javascript:"))
            lb.Attributes.Add("onclick", ml.URL);
        else
        {
            if (!ml.URL.Equals(""))
            {
                lb.PostBackUrl = ml.URL;
            }
        }

        if (ml.ImageUrl.Equals(""))
        {
            lb.Text = ml.DisplayName;
            lb.Width = Unit.Pixel(180);
        }
        else
        {
            lb.Attributes.Add("style", "background-image:url('../images/" + ml.ImageUrl + "'); background-repeat:no-repeat; background-position:center;");
            lb.Width = Unit.Pixel(180);
        }

        if (!ml.CommandName.Equals(""))
            lb.CommandName = ml.CommandName;
    }

    protected void OnMenuStripCommand(EventArgs dce)
    {
        if (MenuStripCommand != null)
            MenuStripCommand(this, dce);
    }

    protected void dlstMenu_ItemCommand(object sender, DataListCommandEventArgs dce)
    {
        OnMenuStripCommand(dce);
    }
}
