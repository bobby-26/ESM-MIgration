using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;

public partial class CrewOffshoreBulkSignOnOff : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbartap = new PhoenixToolbar();
            toolbartap.AddButton("Save", "SAVE");
            MenuTab.AccessRights = this.ViewState;
            MenuTab.MenuList = toolbartap.Show();

            if (!IsPostBack)
            {
                if (Request.QueryString["type"] != null)
                {
                    if (Request.QueryString["type"].ToString() == "signon")
                    {
                        ucTitle.Text = "Sign On";
                        lblDate.Text = "Sign On Date";
                        lblSignOnPort.Text = "Sign On Port";
                    }
                    else
                    {
                        ucTitle.Text = "Sign Off";
                        lblDate.Text = "Sign Off Date";
                        lblSignOnPort.Text = "Sign Off Port";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuTab_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        NameValueCollection criteria = new NameValueCollection();
        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidSignOn(txtDate.Text, ddlSeaPort.SelectedSeaport))
            {
                ucError.Visible = true;
                return;
            }
            criteria.Clear();
            criteria.Add("txtDate", txtDate.Text);
            criteria.Add("ddlSeaPort", ddlSeaPort.SelectedSeaport);
            if (Request.QueryString["type"] != null)
                criteria.Add("type", Request.QueryString["type"].ToString());
            else
                criteria.Add("type", "");

            Filter.CurrentSignOnOffSelection = criteria;
        }
        else if (dce.CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
            Filter.CurrentSignOnOffSelection = criteria;
        }
        String script = "javascript:fnReloadList('codehelp1');";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    private bool IsValidSignOn(string date, string SeaPort)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Sign-On Date is required.";
        }
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Sign-On Date should be earlier than current date";
        }
        if (!General.GetNullableInteger(SeaPort).HasValue)
        {
            ucError.ErrorMessage = "Sign-On Sea Port is required.";
        }
        return (!ucError.IsError);
    }

    //public StateBag ReturnViewState()
    //{
    //    return ViewState;
    //}
}
