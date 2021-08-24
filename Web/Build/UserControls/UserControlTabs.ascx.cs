using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;

public partial class UserControlTabs : System.Web.UI.UserControl
{
    public event EventHandler TabStripCommand;
    int _MenuIndex;
    bool _TabStrip = false;
    StateBag _viewstate = null;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    private void SetMenu(List<MenuLink> _MenuList)
    {
        dlstTabs.DataSource = _MenuList;
        dlstTabs.DataBind();
        dlstTabs.ItemStyle.BackColor = System.Drawing.Color.LightSteelBlue;
        dlstTabs.ItemStyle.ForeColor = System.Drawing.Color.White;
        dlstTabs.SelectedIndex = _MenuIndex;
        dlstTabs.SelectedItemStyle.BackColor = System.Drawing.Color.LightSteelBlue;

        DataListItem lstItem = dlstTabs.SelectedItem;
    }

    public StateBag AccessRights
    {
        get
        {
            return _viewstate;
        }
        set
        {
            _viewstate = value;
        }
    }

    public void SetTrigger(UpdatePanel up)
    {
        foreach (DataListItem dle in dlstTabs.Items)
        {
            LinkButton lb = (LinkButton)dle.FindControl("btnMenu");
            bool access = SessionUtil.CanAccess(_viewstate, lb.CommandName);
            if (lb.PostBackUrl != null && !lb.PostBackUrl.Equals(""))
            {
                PostBackTrigger pbt = new PostBackTrigger();
                pbt.ControlID = lb.UniqueID;
                up.Triggers.Add(pbt);
            }
            if (!access)
            {
                lb.Visible = false;
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

    public void ClearSelection()
    {
        foreach (DataListItem dli in dlstTabs.Items)
        {
            dli.FindControl("btnMenu").Visible = true;
            dli.FindControl("lblMenu").Visible = false;
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
            return dlstTabs.SelectedIndex;
        }
    }

    public string TabStrip
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                _TabStrip = true;            
        }
    }


    public int SelectedMenuIndex
    {
        set
        {
            if (_TabStrip)
            {

                foreach (DataListItem dli in dlstTabs.Items)
                {
                    LinkButton lb = (LinkButton)dli.FindControl("btnMenu");
                    Label lbl = (Label)dli.FindControl("lblMenu");
                    bool access = SessionUtil.CanAccess(_viewstate, lb.CommandName);

                    if (lb.Text.Length < 12)
                        lb.Width = Unit.Pixel(80);

                    if (lbl.Text.Length < 12)
                        lbl.Width = Unit.Pixel(80);

                    if (access)
                    {
                        dli.FindControl("btnMenu").Visible = true;
                        dli.FindControl("lblMenu").Visible = false;
                    }
                    else
                    {
                        lb.Visible = false;
                        lbl.Visible = false;
                        lbl.Width = Unit.Pixel(0);
                    }

                    
                }

                DataListItem selecteddli = dlstTabs.Items[value];
                selecteddli.FindControl("lblMenu").Visible = true;
                selecteddli.FindControl("btnMenu").Visible = false;
            }
        }
    }

    protected void dlstTabs_ItemDataBound(object sender, DataListItemEventArgs de)
    {
        Label lbl = (Label)de.Item.FindControl("lblMenu");
        LinkButton lb = (LinkButton)de.Item.FindControl("btnMenu");
        lbl.Width = Unit.Pixel(150);

        MenuLink ml = (MenuLink)de.Item.DataItem;
        
        lb.Visible = false;
        lbl.Visible = false;
        lb.ToolTip = ml.DisplayName;
        lbl.ToolTip = ml.DisplayName;

        ml.LinkVisible = (SessionUtil.CanAccess(_viewstate, ml.CommandName))?1:0;

        if (ml.LinkVisible == 1)
        {
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
                if (ml.CommandName.ToUpper() != "EXCEL")
                    lb.Attributes.Add("onclick", "javascript:this.onclick=function(){if (document.getElementById('lblProgress') != null) document.getElementById('lblProgress').value='Processing now. Please Wait!'; return false;};");// this will fix the multiple click problem
            }

            if (ml.ImageUrl.Equals(""))
            {
                lb.Text = ml.DisplayName;
                lbl.Text = ml.DisplayName;

                if(lb.Text.Length < 12)
                    lb.Width = Unit.Pixel(80);

                if (lbl.Text.Length < 12)
                    lbl.Width = Unit.Pixel(80);
            }
            else
            {
                lb.Attributes.Add("style", "background-image:url('" + Session["images"] + "/" + ml.ImageUrl + "'); background-repeat:no-repeat; background-position:center;");
                lb.Width = Unit.Pixel(35);
            }

            if (!ml.CommandName.Equals(""))
                lb.CommandName = ml.CommandName;

            lb.Visible = true;
        }
        else
        {
            lb.Visible = false;
            lb.Width = Unit.Pixel(0);
            lbl.Visible = false;
            lbl.Width = Unit.Pixel(0);            
            de.Item.CssClass = "hidden";
            de.Item.Attributes.Add("style", "width: 0px");
            //if (lb.Text.Length < 12)
            //    lb.Width = Unit.Pixel(80);

            //if (lbl.Text.Length < 12)
            //    lbl.Width = Unit.Pixel(80);

            //lb.CommandName = ml.CommandName;
            //if (ml.ImageUrl.Equals(""))
            //{
            //    lbl.Visible = _TabStrip;
            //    lbl.Text = ml.DisplayName;
            //    lbl.CssClass = "";
            //    lbl.ForeColor = System.Drawing.Color.LightGray;
            //    lbl.Attributes.Add("style", "height:24px;width:0px;");
            //}
            //else
            //{
            //    lb.Attributes.Add("style", "background-image:url('" + Session["images"] + "/" + ml.ImageUrl + "'); background-repeat:no-repeat; background-position:center; background-color:#CCCCCC;height:24px;");
            //    lb.Width = Unit.Pixel(0);
            //    lb.Visible = _TabStrip;
            //    lb.Enabled = false;

            //}


        }
    }

    protected void OnTabStripCommand(EventArgs dce)
    {
        if (TabStripCommand != null)
            TabStripCommand(this, dce);  
    }

    protected void dlstTabs_ItemCommand(object sender, DataListCommandEventArgs dce)
    {
        OnTabStripCommand(dce);

        DataList dl = (DataList)sender;
        foreach (DataListItem dli in dl.Items)
        {
            LinkButton lb = (LinkButton)dli.FindControl("btnMenu");
            Label lbl = (Label)dli.FindControl("lblMenu");

            if (lb.Text.Length < 12)
                lb.Width = Unit.Pixel(80);

            if (lbl.Text.Length < 12)
                lbl.Width = Unit.Pixel(80);

            if (lb.Text == "")
                lb.Width = Unit.Pixel(35);

            bool access = SessionUtil.CanAccess(_viewstate, lb.CommandName);
            if (access)
            {
                lb.Visible = true;
                lbl.Visible = false;
            }
            else
            {
                lb.Visible = false;
                lbl.Visible = false;
                lbl.Width = Unit.Pixel(0);
            }                        
        }

        if (_TabStrip)
        {
            dce.Item.FindControl("btnMenu").Visible = false;
            dce.Item.FindControl("lblMenu").Visible = true;
            Label lbl = (Label)dce.Item.FindControl("lblMenu");
            if (lbl.Text.Length < 12)
                lbl.Width = Unit.Pixel(80);

            if (lbl.Text.Length < 12)
                lbl.Width = Unit.Pixel(80);

            if (lbl.Text == "")
                lbl.Width = Unit.Pixel(35);
        }

    }
}
