using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI;
using Telerik.Web.UI;

public partial class UserControlTabsTelerik : System.Web.UI.UserControl
{
    public event EventHandler TabStripCommand;
    //int _MenuIndex;
    bool _TabStrip = false;
    StateBag _viewstate = null;
    private string _title = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    private void SetMenu(List<MenuLink> _MenuList)
    {
        dlstTabs.Items.Clear();
        foreach (MenuLink lnk in _MenuList)
        {
            bool access = SessionUtil.CanAccess(_viewstate, lnk.CommandName);
            if (!access) continue;
            if (lnk.SubToolBar != null)
            {
                RadToolBarDropDown drpdwn = new RadToolBarDropDown(lnk.DisplayName);
                drpdwn.Font.Bold = true;
                drpdwn.ForeColor = lnk.Fontcolor;
                if (lnk.Direction == ToolBarDirection.Right)
                    drpdwn.OuterCssClass = "rightButton";
                if (lnk.ImageUrl.Trim() != string.Empty)
                {
                    drpdwn.ImageUrl = Session["images"] + "/" + lnk.ImageUrl;
                }
                if (lnk.DisplayType == DisplayType.Text || lnk.DisplayType == DisplayType.TextandImageUrl)
                {
                    drpdwn.Text = lnk.DisplayName;
                }
                else if (lnk.DisplayType == DisplayType.ImageUrl)
                {
                    drpdwn.Text = "";
                }
                dlstTabs.Items.Add(drpdwn);
                List<MenuLink> _lnk = lnk.SubToolBar.Show();
                foreach (MenuLink temp in _lnk)
                {
                    if (!SessionUtil.CanAccess(_viewstate, temp.CommandName)) continue;
                    drpdwn.Buttons.Add(CreateToolbar(temp));
                }
            }
            else
            {
                dlstTabs.Items.Add(CreateToolbar(lnk));
            }
        }
        if (!string.IsNullOrEmpty(Title))
        {
            RadToolBarButton item = new RadToolBarButton(Title);
            item.Enabled = false;
            item.Checked = false;
            item.DisabledCssClass = "CustomDisabledItem";
            dlstTabs.Items.Insert(0, item);
        }
        //dlstTabs.DataSource = _MenuList;
        //dlstTabs.DataBind();
    }
    private RadToolBarButton CreateToolbar(MenuLink lnk)
    {
        RadToolBarButton button = new RadToolBarButton();
        if (lnk.Direction == ToolBarDirection.Right)
            button.OuterCssClass = "rightButton";

        string tt = lnk.ToolTip.Trim() == null || lnk.ToolTip.Trim() == string.Empty?lnk.DisplayName:lnk.ToolTip;

        button.ToolTip = tt;

        button.CommandName = lnk.CommandName;
        button.Font.Bold = true;
        button.ForeColor = lnk.Fontcolor;

        if (_TabStrip)
        {
            button.Checked = false;
            button.Group = "TabStrip";
            button.CheckOnClick = true;
        }
        if (lnk.DisplayType == DisplayType.Text || lnk.DisplayType == DisplayType.TextandImageUrl)
        {
            button.Text = lnk.DisplayName;
        }
        else if (lnk.DisplayType == DisplayType.ImageUrl)
        {
            button.Text = "";
        }
        if (lnk.ImageUrl.Trim() != string.Empty)
        {
            button.ImageUrl = Session["images"] + "/" + lnk.ImageUrl;
        }
        if (lnk.URL.Trim() != string.Empty && lnk.URL.Trim().ToLower().IndexOf("javascript") < 0)
        {
            button.PostBackUrl = lnk.URL;
        }
        if (lnk.URL.Trim() != string.Empty && lnk.URL.Trim().ToLower().IndexOf("javascript") >= 0)
        {
            button.Attributes["onclick"] = lnk.URL + ";removeToolbardDisabled(this,'" + lnk.DisplayName + "');";
            button.PostBack = false;
        }
        if (!string.IsNullOrEmpty(lnk.FontAwesomeIconHTML))
        {
            RadToolBarFontAwesomeTemplate fontawesome = new RadToolBarFontAwesomeTemplate();
            fontawesome.IconHTML = lnk.FontAwesomeIconHTML;
            fontawesome.RadButton = button;
            fontawesome.InstantiateIn(button);
            //button.CssClass = lnk.CssClass;
        }
        return button;
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

    public List<MenuLink> MenuList
    {
        set
        {
            SetMenu(value);
        }
    }

    public void ClearSelection()
    {

    }
    public string Title
    {
        get
        {
            return _title;
        }
        set
        {
            _title = value;
            if (dlstTabs.Items.Count > 0)
            {
                RadToolBarButton btn = (RadToolBarButton)dlstTabs.Items[0];
                if (btn.DisabledCssClass == "CustomDisabledItem")
                {
                    btn.Text = _title;
                }
            }
        }
    }
    public bool TabStrip
    {
        set
        {
            _TabStrip = value;
        }
    }
    public int SelectedMenuIndex
    {
        set
        {
            if (_TabStrip)
            {
                int val = (string.IsNullOrEmpty(Title) ? value : value + 1);
                if (value < dlstTabs.Items.Count)
                {
                    if (dlstTabs.Items[val].GetType().Equals(typeof(RadToolBarButton)))
                    {
                        ((RadToolBarButton)dlstTabs.Items[val]).CheckOnClick = true;
                        ((RadToolBarButton)dlstTabs.Items[val]).Checked = true;
                        //((RadToolBarButton)dlstTabs.Items[value]).Enabled= false;

                    }
                }
            }
        }
    }
    protected void dlstTabs_ButtonDataBound(object sender, RadToolBarButtonEventArgs e)
    {

    }

    protected void OnTabStripCommand(RadToolBarEventArgs dce)
    {
        if (TabStripCommand != null)
            TabStripCommand(this, dce);
    }

    protected void dlstTabs_ButtonClick(object sender, RadToolBarEventArgs e)
    {
        OnTabStripCommand(e);
    }
}
class RadToolBarFontAwesomeTemplate : ITemplate
{
    public string IconHTML;
    public RadToolBarButton RadButton;
    public void InstantiateIn(Control container)
    {
        string onclick = string.IsNullOrEmpty(RadButton.Attributes["onclick"]) ? "" : "onclick=\"" + RadButton.Attributes["onclick"] + "\" ";
        string button = "<span title=\"" + RadButton.ToolTip + "\" class=\"pfas rtbButton\" tabindex=\"0\" " + onclick + ">";
        button += IconHTML;
        if (!string.IsNullOrEmpty(RadButton.Text))
        {
            button += "<span class=\"rtbText\">" + RadButton.Text + "</span>";
        }
        button += "</span>";
        LiteralControl ltrl = new LiteralControl(button);
        container.Controls.Add(ltrl);
    }
}